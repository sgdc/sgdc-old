using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using SGDeContent.DataTypes;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace SGDeContent.Processors
{
    public class SpriteSheetProcessor
    {
        public enum SpriteType
        {
            Unknown,
            Bitmap,
            SVG
        }

        public static Dictionary<int, SpriteType> SpriteSheetTypes;

        public const string SPRITE_TYPE_FILE = "SpriteTypes.xml";

        public static SpriteSheet Process(Content input, ContentProcessorContext context)
        {
            return Process(input, context, true);
        }

        public static SpriteSheet Process(Content input, ContentProcessorContext context, bool compile)
        {
            SpriteSheet map = new SpriteSheet();
            foreach (XmlNode spriteMaps in input.document.DocumentElement)
            {
                if (ContentTagManager.TagMatches("IMPORT_SPRITESHEET_ELEMENT", spriteMaps.Name, input.Version))
                {
                    #region SpriteMaps

                    foreach (XmlNode smap in spriteMaps)
                    {
                        if (ContentTagManager.TagMatches("SPRITESHEET_MAP", smap.Name, input.Version))
                        {
                            #region SpriteMap

                            XmlAttribute at = ContentTagManager.GetXMLAtt("SPRITESHEET_MAP_NAME", input.Version, smap);
                            if (at != null)
                            {
                                map.Names.Add(at.Value);
                            }
                            else
                            {
                                map.Names.Add(null);
                            }
                            at = ContentTagManager.GetXMLAtt("GENERAL_ID", input.Version, smap);
                            if (at == null)
                            {
                                throw new InvalidContentException(Messages.SpriteSheet_RequiresID);
                            }
                            map.TextureIDs.Add(int.Parse(at.Value));
                            bool hasSource = false;
                            foreach (XmlNode mapComponent in smap)
                            {
                                if (ContentTagManager.TagMatches("SPRITESHEET_MAP_COMP_SOURCE", mapComponent.Name, input.Version))
                                {
                                    hasSource = true;
                                    if (compile)
                                    {
                                        if (SpriteSheetProcessor.SpriteSheetTypes == null)
                                        {
                                            //For one reason or another, SpriteSheetProcessor.SpriteSheetTypes doesn't always end up getting created. Do this to create it
                                            SpriteSheetProcessor.SpriteSheetTypes = new Dictionary<int, SpriteType>();
                                            Process(input, context, false);
                                        }
                                        switch (SpriteSheetProcessor.SpriteSheetTypes[map.TextureIDs[map.TextureIDs.Count - 1]])
                                        {
                                            case SpriteType.Bitmap:
                                                map.Textures.Add(Utils.CompileExternal<TextureContent, TextureContent, TextureContent, Microsoft.Xna.Framework.Content.Pipeline.TextureImporter, Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessor>(SGDEProcessor.GetInnerText(mapComponent), context));
                                                break;
                                            case SpriteType.SVG:
                                                map.Textures.Add(Utils.CompileExternal<Content, ProcessedContent, SGDeContent.DataTypes.Sprites.SVG.SVGfx, SVGImport, SVGProcessor>(SGDEProcessor.GetInnerText(mapComponent), context));
                                                break;
                                            default:
                                                throw new InvalidContentException(Messages.SpriteSheet_UnknownType);
                                        }
                                    }
                                    else
                                    {
                                        //This only happens when types are being looked up
                                        SpriteSheetProcessor.SpriteSheetTypes.Add(map.TextureIDs[map.TextureIDs.Count - 1], GetType(SGDEProcessor.GetInnerText(mapComponent)));
                                    }
                                }
                                else if (ContentTagManager.TagMatches("SPRITESHEET_MAP_COMP_ANIMATION", mapComponent.Name, input.Version))
                                {
                                    if (compile)
                                    {
                                        Animation animation = AnimationProcessor.Process(mapComponent, input.Version, context);
                                        if (!animation.BuiltIn)
                                        {
                                            throw new InvalidContentException(Messages.SpriteSheet_AnimationMustBeInternal);
                                        }
                                        map.AnimationSets.Add(animation.Sets);
                                    }
                                }
                            }
                            if (map.AnimationSets.Count != map.TextureIDs.Count)
                            {
                                map.AnimationSets.Add(null);
                            }
                            if (!hasSource)
                            {
                                throw new InvalidContentException(Messages.SpriteSheet_NeedsSource);
                            }

                            #endregion
                        }
                    }

                    //First check to see if out of order
                    bool process = false;
                    for (int i = 0; i < map.TextureIDs.Count; i++)
                    {
                        if (map.TextureIDs[i] != i)
                        {
                            process = true;
                            break;
                        }
                    }
                    if (process)
                    {
                        //Need to sort the textures.
                        List<int> ids = new List<int>();
                        List<object> texs = new List<object>();
                        for (int i = 0; i < map.TextureIDs.Count; i++)
                        {
                            ids.Add(i);
                            texs.Add(map.Textures[map.TextureIDs.IndexOf(i)]);
                        }
                        map.TextureIDs.Clear();
                        map.TextureIDs.AddRange(ids);
                        map.Textures.Clear();
                        map.Textures.AddRange(texs);
                    }

                    #endregion
                }
                else if (ContentTagManager.TagMatches("SPRITESHEET_FONTS", spriteMaps.Name, input.Version))
                {
                    #region SpriteFonts

                    if (compile)
                    {
                        foreach (XmlNode font in spriteMaps)
                        {
                            if (ContentTagManager.TagMatches("SPRITESHEET_FONT", font.Name, input.Version))
                            {
                                //Get the name
                                XmlAttribute at = ContentTagManager.GetXMLAtt("SPRITESHEET_FONT_NAME", input.Version, font);
                                string name = at == null ? null : at.Value;
                                if (string.IsNullOrWhiteSpace(name))
                                {
                                    throw new ArgumentException(Messages.SpriteSheet_FontRequiresName);
                                }

                                //Create the Font XML file and load it
                                XmlDocument doc = new XmlDocument();
                                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));

                                XmlElement el = doc.CreateElement("XnaContent");
                                doc.AppendChild(el);
                                at = doc.CreateAttribute("xmlns:Graphics");
                                el.Attributes.Append(at);
                                at.Value = "Microsoft.Xna.Framework.Content.Pipeline.Graphics";

                                el.AppendChild(doc.CreateElement("Asset"));
                                el = el["Asset"];
                                at = doc.CreateAttribute("Type");
                                el.Attributes.Append(at);
                                at.Value = "Graphics:FontDescription";

                                //Copy the current element over to the new element
                                if (!CopyElement(el, font, "FontName"))
                                {
                                    throw new ArgumentException(Messages.SpriteSheet_FontMissingFontName);
                                }
                                if (!CopyElement(el, font, "Size"))
                                {
                                    AddElement(el, "Size", "14");
                                }
                                if (!CopyElement(el, font, "Spacing"))
                                {
                                    AddElement(el, "Spacing", "0");
                                }
                                if (!CopyElement(el, font, "UseKerning"))
                                {
                                    AddElement(el, "UseKerning", "true");
                                }
                                if (!CopyElement(el, font, "Style"))
                                {
                                    AddElement(el, "Style", "Regular");
                                }
                                CopyElement(el, font, "DefaultCharacter");
                                if (!CopyElement(el, font, "CharacterRegions", true))
                                {
                                    XmlElement cr = doc.CreateElement("CharacterRegions");
                                    el.AppendChild(cr);
                                    el = doc.CreateElement("CharacterRegion");
                                    cr.AppendChild(el);
                                    cr = el;
                                    el = doc.CreateElement("Start");
                                    cr.AppendChild(el);
                                    el.InnerText = ((char)32).ToString();
                                    el = doc.CreateElement("End");
                                    cr.AppendChild(el);
                                    el.InnerText = ((char)126).ToString();
                                }

                                //Save and deserialize the description
                                FontDescription description = null;
                                using (MemoryStream spritefontStream = new MemoryStream())
                                {
                                    doc.Save(spritefontStream);
                                    spritefontStream.Position = 0;

                                    //Create the FontDescription
                                    using (XmlReader reader = XmlReader.Create(spritefontStream))
                                    {
                                        description = Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.IntermediateSerializer.Deserialize<FontDescription>(reader, input.Identity.SourceFilename);
                                    }
                                }
                                description.Identity = input.Identity;

                                //Add the font
                                map.Fonts.Add(name, Utils.Process<FontDescription, Microsoft.Xna.Framework.Content.Pipeline.Processors.SpriteFontContent, Microsoft.Xna.Framework.Content.Pipeline.Processors.FontDescriptionProcessor>(description, context));
                            }
                        }
                    }

                    #endregion
                }
            }
            if (!compile)
            {
                //Means that SpriteSheetTypes was set
                Utils.Serialize(SpriteSheetProcessor.SpriteSheetTypes, SPRITE_TYPE_FILE, context);
            }
            return map;
        }

        private static bool CopyElement(XmlElement parent, XmlNode src, string name)
        {
            return CopyElement(parent, src, name, false);
        }

        private static bool CopyElement(XmlElement parent, XmlNode src, string name, bool recursive)
        {
            XmlElement el = parent.OwnerDocument.CreateElement(name);
            XmlNode el2 = src[name];
            if (el2 != null)
            {
                if (recursive)
                {
                    foreach (XmlNode child in el2)
                    {
                        if (!CopyElement(el, el2, child.Name))
                        {
                            return false;
                        }
                    }
                }
                el.InnerText = el2.InnerText;
                parent.AppendChild(el);
                return true;
            }
            return false;
        }

        private static void AddElement(XmlElement parent, string name, string inner)
        {
            XmlElement el = parent.OwnerDocument.CreateElement(name);
            el.InnerText = inner;
            parent.AppendChild(el);
        }

        private static SpriteType GetType(string name)
        {
            string ext = Path.GetExtension(name).ToLower();
            switch (ext)
            {
                case ".bmp":
                case ".dds":
                case ".dib":
                case ".hdr":
                case ".jpg":
                case ".pfm":
                case ".png":
                case ".ppm":
                case ".tga":
                    return SpriteType.Bitmap;
                case ".svg":
                case ".svgz":
                    return SpriteType.SVG;
            }
            return SpriteType.Unknown;
        }
    }
}

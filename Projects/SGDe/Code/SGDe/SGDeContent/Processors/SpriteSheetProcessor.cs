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

        public static SpriteSheet Process(Content input, ContentProcessorContext context)
        {
            return Process(input, context, true);
        }

        public static SpriteSheet Process(Content input, ContentProcessorContext context, bool compile)
        {
            SpriteSheet map = new SpriteSheet();
            foreach (XmlElement spriteMaps in input.document.DocumentElement)
            {
                if (ContentTagManager.TagMatches("IMPORT_SPRITESHEET_ELEMENT", spriteMaps.Name, input.Version))
                {
                    #region SpriteMaps

                    foreach (XmlElement smap in spriteMaps)
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
                            foreach (XmlElement mapComponent in smap)
                            {
                                if (ContentTagManager.TagMatches("SPRITESHEET_MAP_COMP_SOURCE", mapComponent.Name, input.Version))
                                {
                                    hasSource = true;
                                    if (compile)
                                    {
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
                                for (int i = 0; i < map.TextureIDs.Count; i++)
                                {
                                    if (map.TextureIDs[i] != i)
                                    {
                                        int index = map.TextureIDs.IndexOf(i);
                                        object entity = map.Textures[index];
                                        map.Textures.RemoveAt(index);
                                        map.Textures.Insert(index, entity);
                                        map.TextureIDs.RemoveAt(i);
                                        map.TextureIDs.Insert(i, i);
                                    }
                                }
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
            }
            return map;
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

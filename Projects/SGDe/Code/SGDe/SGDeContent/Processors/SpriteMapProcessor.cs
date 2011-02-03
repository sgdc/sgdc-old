using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDeContent.DataTypes;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace SGDeContent.Processors
{
    public class SpriteMapProcessor
    {
        public static SpriteMap Process(Content input, ContentProcessorContext context)
        {
            SpriteMap map = new SpriteMap();
            foreach (XmlElement spriteMaps in input.document.DocumentElement)
            {
                if (ContentTagManager.TagMatches("IMPORT_SPRITEMAP_ELEMENT", spriteMaps.Name, input.Version))
                {
                    #region SpriteMaps

                    foreach (XmlElement smap in spriteMaps)
                    {
                        if (ContentTagManager.TagMatches("SPRITEMAP_MAP", smap.Name, input.Version))
                        {
                            #region SpriteMap

                            XmlAttribute at = ContentTagManager.GetXMLAtt("SPRITEMAP_MAP_NAME", input.Version, smap);
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
                                throw new InvalidContentException(Messages.SpriteMap_RequiresID);
                            }
                            map.TextureIDs.Add(int.Parse(at.Value));
                            bool hasSource = false;
                            foreach (XmlElement mapComponent in smap)
                            {
                                if (ContentTagManager.TagMatches("SPRITEMAP_MAP_COMP_SOURCE", mapComponent.Name, input.Version))
                                {
                                    hasSource = true;
                                    map.Textures.Add(Utils.CompileExternal<TextureContent, TextureContent, TextureContent, Microsoft.Xna.Framework.Content.Pipeline.TextureImporter, Microsoft.Xna.Framework.Content.Pipeline.Processors.TextureProcessor>(SGDEProcessor.GetInnerText(mapComponent), context));
                                }
                                else if (ContentTagManager.TagMatches("SPRITEMAP_MAP_COMP_ANIMATION", mapComponent.Name, input.Version))
                                {
                                    Animation animation = AnimationProcessor.Process(mapComponent, input.Version, context);
                                    if (!animation.BuiltIn)
                                    {
                                        throw new InvalidContentException(Messages.SpriteMap_AnimationMustBeInternal);
                                    }
                                    int sid = map.TextureIDs[map.TextureIDs.Count - 1];
                                    for (int i = 0; i < animation.Sets.Count; i++)
                                    {
                                        animation.Sets[i].SpriteID = sid;
                                    }
                                    map.AnimationSets.Add(animation.Sets);
                                }
                            }
                            if (map.AnimationSets.Count != map.TextureIDs.Count)
                            {
                                map.AnimationSets.Add(null);
                            }
                            if (!hasSource)
                            {
                                throw new InvalidContentException(Messages.SpriteMap_NeedsSource);
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
                                        ExternalReference<TextureContent> entity = map.Textures[index];
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
    }
}

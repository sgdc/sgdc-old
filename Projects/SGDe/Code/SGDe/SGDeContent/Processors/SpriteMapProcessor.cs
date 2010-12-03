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
                if (spriteMaps.Name.Equals("SpriteMaps"))
                {
                    #region SpriteMaps

                    foreach (XmlElement smap in spriteMaps)
                    {
                        if (smap.Name.Equals("SpriteMap"))
                        {
                            #region SpriteMap

                            XmlAttribute at = smap.Attributes["Name"];
                            if (at != null)
                            {
                                map.Names.Add(at.Value);
                            }
                            else
                            {
                                map.Names.Add(null);
                            }
                            at = smap.Attributes["ID"];
                            if (at == null)
                            {
                                throw new InvalidContentException("SpriteMap must contain an ID.");
                            }
                            map.TextureIDs.Add(int.Parse(at.Value));
                            bool hasSource = false;
                            foreach (XmlElement mapComponent in smap)
                            {
                                if (mapComponent.Name.Equals("Source"))
                                {
                                    hasSource = true;
                                    string innerText = SGDEProcessor.GetInnerText(mapComponent);
                                    map.Textures.Add(context.BuildAsset<TextureContent, TextureContent>(new ExternalReference<TextureContent>(innerText), "TextureProcessor", null, "TextureImporter", Path.GetFileNameWithoutExtension(innerText)));
                                }
                                else if (mapComponent.Name.Equals("Animation"))
                                {
                                    Animation animation = AnimationProcessor.Process(mapComponent, context);
                                    if (!animation.BuiltIn)
                                    {
                                        throw new InvalidContentException("Animation must be a built in type.");
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
                                throw new InvalidContentException("SpriteMap must contain a Source.");
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

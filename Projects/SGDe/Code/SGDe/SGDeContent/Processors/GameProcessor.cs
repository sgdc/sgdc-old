using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using SGDeContent.DataTypes;
using System.Xml;
using System.IO;

namespace SGDeContent.Processors
{
    public class GameProcessor
    {
        public static SGDeContent.DataTypes.Game Process(Content input, ContentProcessorContext context)
        {
            Game game = new Game();

            bool hasFirstRun = false;

            foreach (XmlElement component in input.document.DocumentElement)
            {
                //Handle the Game component (resources)
                if (component.Name.Equals("Game"))
                {
                    #region Game

                    foreach (XmlElement gameComponent in component)
                    {
                        if (gameComponent.Name.Equals("Maps"))
                        {
                            #region Maps

                            foreach (XmlElement map in gameComponent)
                            {
                                int id = int.Parse(map.Attributes["ID"].Value);
                                if (game.MapIDs.Contains(id))
                                {
                                    throw new InvalidContentException(string.Format(Messages.Game_MapIDExists, id));
                                }
                                game.MapIDs.Add(id);
                                string innerText = SGDEProcessor.GetInnerText(map.ChildNodes[0]);
                                if (string.IsNullOrWhiteSpace(innerText))
                                {
                                    //Actual map
                                    game.Maps.Add(MapProcessor.Process((XmlElement)map.ChildNodes[0], context));
                                }
                                else
                                {
                                    //Map reference
                                    string mapRef = innerText;
                                    if (!mapRef.EndsWith(".sgde", StringComparison.OrdinalIgnoreCase))
                                    {
                                        throw new InvalidContentException(Messages.Game_MapRefNotSGDE);
                                    }
                                    ExternalReference<ProcessedContent> ext = context.BuildAsset<Content, ProcessedContent>(new ExternalReference<Content>(mapRef), typeof(SGDEProcessor).Name, null, typeof(SGDEImport).Name, Path.GetFileNameWithoutExtension(mapRef));
                                    ExternalReference<Map> naoExt;
                                    if (ext.Identity == null)
                                    {
                                        naoExt = new ExternalReference<Map>(ext.Filename);
                                    }
                                    else
                                    {
                                        naoExt = new ExternalReference<Map>(ext.Filename, ext.Identity);
                                    }
                                    game.Maps.Add(naoExt);
                                }
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
                //Handle the Settings component (game description)
                else if (component.Name.Equals("Settings"))
                {
                    #region Setting

                    #region SpriteSheet

                    XmlAttribute at = component.Attributes["SpriteSheet"];
                    if (at != null)
                    {
                        //Specific SpriteSheet (though result is always "SpriteSheet")
                        context.BuildAsset<Content, ProcessedContent>(new ExternalReference<Content>(at.Value), typeof(SGDEProcessor).Name, null, typeof(SGDEImport).Name, "SpriteSheet");
                    }
                    else
                    {
                        //Default SpriteSheet
                        context.BuildAsset<Content, ProcessedContent>(new ExternalReference<Content>("SpriteSheet.sgde"), typeof(SGDEProcessor).Name, null, typeof(SGDEImport).Name, "SpriteSheet");
                    }

                    #endregion

                    foreach (XmlElement settingComponent in component)
                    {
                        if (settingComponent.Name.Equals("MapList"))
                        {
                            #region Map List

                            foreach (XmlElement map in settingComponent)
                            {
                                if (map.Name.Equals("Map"))
                                {
                                    int id = int.Parse(map.Attributes["ID"].Value);
                                    if (!game.MapIDs.Contains(id))
                                    {
                                        throw new InvalidContentException(string.Format(Messages.Game_MapIDNotExist, id));
                                    }
                                    at = map.Attributes["InitialMap"];
                                    if (at != null)
                                    {
                                        if (bool.Parse(at.Value))
                                        {
                                            if (hasFirstRun)
                                            {
                                                throw new InvalidContentException(Messages.Game_TooManyInitialMaps);
                                            }
                                            hasFirstRun = true;
                                            game.FirstRun = id;
                                        }
                                    }
                                    //Don't do any checks because maps can repeat
                                    game.MapOrderId.Add(id);
                                    at = map.Attributes["Name"];
                                    if (at != null)
                                    {
                                        game.MapOrderName.Add(at.Value);
                                    }
                                    else
                                    {
                                        game.MapOrderName.Add(null);
                                    }
                                }
                            }

                            #endregion
                        }
                        else if (settingComponent.Name.Equals("DefGameSettings"))
                        {
                            #region Default Game Settings

                            foreach (XmlElement gameSettingComponent in settingComponent)
                            {
                                if (gameSettingComponent.Name.Equals("Screen"))
                                {
                                    if (context.TargetPlatform == TargetPlatform.Windows)
                                    {
                                        at = gameSettingComponent.Attributes["Width"];
                                        if (at != null)
                                        {
                                            game.Width = int.Parse(at.Value);
                                        }
                                        at = gameSettingComponent.Attributes["Height"];
                                        if (at != null)
                                        {
                                            game.Height = int.Parse(at.Value);
                                        }
                                        at = gameSettingComponent.Attributes["Fullscreen"];
                                        if (at != null)
                                        {
                                            game.Fullscreen = bool.Parse(at.Value);
                                        }
                                    }
                                    else if (context.TargetPlatform == TargetPlatform.Xbox360)
                                    {
                                        game.Width = 1280;
                                        game.Height = 720;
                                        game.Fullscreen = true;
                                    }
                                    else if (context.TargetPlatform == TargetPlatform.WindowsPhone)
                                    {
                                        //TODO: Not sure what resoultion this should be built for or if this should be like Windows.
                                    }
                                    else
                                    {
                                        context.Logger.LogWarning(null, null, Messages.Game_UnknownTarget, context.TargetPlatform);
                                    }
                                }
                                //TODO: Stuff like fullscreen, resoultion, ...
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
            }

            game.Sort();

            return game;
        }
    }
}

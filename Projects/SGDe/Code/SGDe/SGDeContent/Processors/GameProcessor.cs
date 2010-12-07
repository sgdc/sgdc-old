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
                                    game.Maps.Add(Utils.CompileExternal<Map>(mapRef, context));
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
                        //Specific SpriteSheet
                        game.SpriteSheet = Utils.CompileExternal<SpriteMap>(at.Value, context);
                    }
                    else
                    {
                        //Default SpriteSheet
                        game.SpriteSheet = Utils.CompileExternal<SpriteMap>("SpriteSheet.sgde", context);
                    }

                    #endregion

                    foreach (XmlElement settingComponent in component)
                    {
                        if (settingComponent.Name.Equals("MapList"))
                        {
                            #region Map List

                            int index = 0;
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
                                            game.FirstRun = index;
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
                                    index++;
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
                                    #region Screen

                                    //Any platform
                                    at = gameSettingComponent.Attributes["Fullscreen"];
                                    if (at != null)
                                    {
                                        //Ignored on Xbox
                                        game.Fullscreen = bool.Parse(at.Value);
                                    }
                                    at = gameSettingComponent.Attributes["VSync"];
                                    if (at != null)
                                    {
                                        game.VSync = bool.Parse(at.Value);
                                    }
                                    at = gameSettingComponent.Attributes["Multisample"];
                                    if (at != null)
                                    {
                                        game.Multisample = bool.Parse(at.Value);
                                    }
                                    //Platform specific
                                    switch (context.TargetPlatform)
                                    {
                                        case TargetPlatform.WindowsPhone:
                                            //TODO: Not sure what resoultion this should be built for or if this should be like Windows.
                                        case TargetPlatform.Windows:
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
                                            break;
                                        case TargetPlatform.Xbox360:
                                            game.Width = 1280;
                                            game.Height = 720;
                                            game.Fullscreen = true;
                                            //game.VSync = false; //Default value
                                            //game.Multisample = true; //It's built into the chip, so why should you disable it?
                                            break;
                                        default:
                                            context.Logger.LogWarning(null, null, Messages.Game_UnknownTarget, context.TargetPlatform);
                                            break;
                                    }

                                    #endregion
                                }
                                else if (gameSettingComponent.Name.Equals("Game"))
                                {
                                    #region Game

                                    //Any platform
                                    at = gameSettingComponent.Attributes["FixedTime"];
                                    if (at != null)
                                    {
                                        game.FixedTime = bool.Parse(at.Value);
                                    }
                                    at = gameSettingComponent.Attributes["FrameTime"];
                                    if (at != null)
                                    {
                                        if (!TimeSpan.TryParse(at.Value, out game.FrameTime) && (game.FixedTime.HasValue && game.FixedTime.Value))
                                        {
                                            context.Logger.LogWarning(null, null, Messages.Game_BadFrameTime);
                                            game.FixedTime = false;
                                        }
                                    }

                                    //Platform specific
                                    switch (context.TargetPlatform)
                                    {
                                        case TargetPlatform.Windows:
                                            at = gameSettingComponent.Attributes["MouseVisible"];
                                            if (at != null)
                                            {
                                                game.MouseVisible = bool.Parse(at.Value);
                                            }
                                            break;
                                        case TargetPlatform.WindowsPhone:
                                            at = gameSettingComponent.Attributes["Orientation"];
                                            if (at != null)
                                            {
                                                game.Orientation = Utils.ParseEnum<Microsoft.Xna.Framework.DisplayOrientation>(at.Value, Microsoft.Xna.Framework.DisplayOrientation.Default, context.Logger);
                                            }
                                            break;
                                    }
                                    switch(context.TargetPlatform)
                                    {
                                        case TargetPlatform.Windows:
                                        case TargetPlatform.WindowsPhone:
                                            at = gameSettingComponent.Attributes["WindowResizeable"];
                                            if (at != null)
                                            {
                                                game.WindowResize = bool.Parse(at.Value);
                                                if (game.WindowResize)
                                                {
                                                    context.Logger.LogWarning(null, null, Messages.Game_WindowResizeable);
                                                }
                                            }
                                            at = gameSettingComponent.Attributes["Title"];
                                            if (at != null)
                                            {
                                                game.Title = at.Value;
                                            }
                                            break;
                                    }

                                    #endregion
                                }
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

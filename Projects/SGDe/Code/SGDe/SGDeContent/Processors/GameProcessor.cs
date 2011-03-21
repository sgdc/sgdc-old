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
        #region Reading

        public static SGDeContent.DataTypes.Game Process(Content input, ContentProcessorContext context)
        {
            Game game = new Game();

            bool hasFirstRun = false;

            foreach (XmlNode component in input.document.DocumentElement)
            {
                //Handle the Game component (resources)
                if (ContentTagManager.TagMatches("IMPORT_GAME_ELEMENT", component.Name, input.Version))
                {
                    #region Game

                    foreach (XmlNode gameComponent in component)
                    {
                        if (ContentTagManager.TagMatches("GAME_GAME_MAPS", gameComponent.Name, input.Version))
                        {
                            #region Maps

                            foreach (XmlNode map in gameComponent)
                            {
                                int id = int.Parse(ContentTagManager.GetXMLAtt("GENERAL_ID", input.Version, map).Value);
                                if (game.MapIDs.Contains(id))
                                {
                                    throw new InvalidContentException(string.Format(Messages.Game_MapIDExists, id));
                                }
                                game.MapIDs.Add(id);
                                string innerText = SGDEProcessor.GetInnerText(map.ChildNodes[0]);
                                if (string.IsNullOrWhiteSpace(innerText))
                                {
                                    //Actual map
                                    game.Maps.Add(MapProcessor.Process(map.ChildNodes[0], input.Version, context));
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
                else if (ContentTagManager.TagMatches("GAME_SETTINGS", component.Name, input.Version))
                {
                    #region Setting

                    #region SpriteSheet

                    XmlAttribute at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_SPRITESHEET", input.Version, component);
                    string sPath;
                    if (at != null)
                    {
                        //Specific SpriteSheet
                        sPath = at.Value;
                    }
                    else
                    {
                        //Default SpriteSheet
                        sPath = ContentTagManager.GetTagValue("GAME_SETTINGS_SPRITESHEET", input.Version) + ".sgde";
                    }
                    game.SpriteSheet = Utils.CompileExternal<SpriteSheet>(sPath, context);
                    LoadSpriteTypes(sPath, context);

                    #endregion

                    foreach (XmlNode settingComponent in component)
                    {
                        if (ContentTagManager.TagMatches("GAME_SETTINGS_MAPLIST", settingComponent.Name, input.Version))
                        {
                            #region Map List

                            int index = 0;
                            foreach (XmlNode map in settingComponent)
                            {
                                if (ContentTagManager.TagMatches("GAME_SETTINGS_MAPLIST_MAP", map.Name, input.Version))
                                {
                                    int id = int.Parse(ContentTagManager.GetXMLAtt("GENERAL_ID", input.Version, map).Value);
                                    if (!game.MapIDs.Contains(id))
                                    {
                                        throw new InvalidContentException(string.Format(Messages.Game_MapIDNotExist, id));
                                    }
                                    at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_MAPLIST_MAP_INITIALMAP", input.Version, map);
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
                                    at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_MAPLIST_MAP_NAME", input.Version, map);
                                    if (at != null)
                                    {
                                        game.MapOrderName.Add(at.Value);
                                    }
                                    else
                                    {
                                        game.MapOrderName.Add(null);
                                    }
                                    index++;

                                    #region Settings

                                    SGDE.Content.DataTypes.MapSettings settings = null;
                                    foreach (XmlNode mapSet in map)
                                    {
                                        if (ContentTagManager.TagMatches("GAME_SETTINGS_MAPLIST_MAP_CAMERA", mapSet.Name, input.Version))
                                        {
                                            #region Camera Position

                                            at = ContentTagManager.GetXMLAtt("GENERAL_X", input.Version, mapSet);
                                            if (at != null)
                                            {
                                                if (settings == null)
                                                {
                                                    settings = new SGDE.Content.DataTypes.MapSettings();
                                                }
                                                settings.CameraPosition = new Microsoft.Xna.Framework.Vector2(float.Parse(at.Value), 0);
                                            }
                                            at = ContentTagManager.GetXMLAtt("GENERAL_Y", input.Version, mapSet);
                                            if (at != null)
                                            {
                                                if (settings == null)
                                                {
                                                    settings = new SGDE.Content.DataTypes.MapSettings();
                                                }
                                                if (settings.CameraPosition.HasValue)
                                                {
                                                    settings.CameraPosition = new Microsoft.Xna.Framework.Vector2(settings.CameraPosition.Value.X, float.Parse(at.Value));
                                                }
                                                else
                                                {
                                                    settings.CameraPosition = new Microsoft.Xna.Framework.Vector2(0, float.Parse(at.Value));
                                                }
                                            }

                                            #endregion
                                        }
                                        else if (ContentTagManager.TagMatches("GAME_SETTINGS_MAPLIST_MAP_ORDER", mapSet.Name, input.Version))
                                        {
                                            #region Order

                                            at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_MAPLIST_MAP_ORDER_CENTRAL", input.Version, mapSet);
                                            if (at != null)
                                            {
                                                if (settings == null)
                                                {
                                                    settings = new SGDE.Content.DataTypes.MapSettings();
                                                }
                                                settings.CentralOrder = int.Parse(at.Value);
                                            }
                                            at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_MAPLIST_MAP_ORDER_SEPERATION", input.Version, mapSet);
                                            if (at != null)
                                            {
                                                if (settings == null)
                                                {
                                                    settings = new SGDE.Content.DataTypes.MapSettings();
                                                }
                                                settings.OrderSeperation = float.Parse(at.Value);
                                            }

                                            #endregion
                                        }
                                    }
                                    game.MapSettings.Add(settings);

                                    #endregion
                                }
                            }

                            #endregion
                        }
                        else if (ContentTagManager.TagMatches("GAME_SETTINGS_DEFSETTINGS", settingComponent.Name, input.Version))
                        {
                            #region Default Game Settings

                            foreach (XmlNode gameSettingComponent in settingComponent)
                            {
                                if (ContentTagManager.TagMatches("GAME_SETTINGS_DEFSETTINGS_SCREEN", gameSettingComponent.Name, input.Version))
                                {
                                    #region Screen

                                    //Any platform
                                    at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_DEFSETTINGS_SCREEN_FULLSCREEN", input.Version, gameSettingComponent);
                                    if (at != null)
                                    {
                                        //Ignored on Xbox
                                        game.Fullscreen = bool.Parse(at.Value);
                                    }
                                    at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_DEFSETTINGS_SCREEN_VSYNC", input.Version, gameSettingComponent);
                                    if (at != null)
                                    {
                                        game.VSync = bool.Parse(at.Value);
                                    }
                                    at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_DEFSETTINGS_SCREEN_MULTISAMPLE", input.Version, gameSettingComponent);
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
                                            at = ContentTagManager.GetXMLAtt("GENERAL_WIDTH", input.Version, gameSettingComponent);
                                            if (at != null)
                                            {
                                                game.Width = int.Parse(at.Value);
                                            }
                                            at = ContentTagManager.GetXMLAtt("GENERAL_HEIGHT", input.Version, gameSettingComponent);
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
                                else if (ContentTagManager.TagMatches("GAME_SETTINGS_DEFSETTINGS_GAME", gameSettingComponent.Name, input.Version))
                                {
                                    #region Game

                                    //Any platform
                                    at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_DEFSETTINGS_GAME_FIXEDTIME", input.Version, gameSettingComponent);
                                    if (at != null)
                                    {
                                        game.FixedTime = bool.Parse(at.Value);
                                    }
                                    at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_DEFSETTINGS_GAME_FRAMETIME", input.Version, gameSettingComponent);
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
                                            at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_DEFSETTINGS_GAME_MOUSEVISIBLE", input.Version, gameSettingComponent);
                                            if (at != null)
                                            {
                                                game.MouseVisible = bool.Parse(at.Value);
                                            }
                                            break;
                                        case TargetPlatform.WindowsPhone:
                                            at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_DEFSETTINGS_GAME_ORIENTATION", input.Version, gameSettingComponent);
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
                                            at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_DEFSETTINGS_GAME_WINDOWRESIZEABLE", input.Version, gameSettingComponent);
                                            if (at != null)
                                            {
                                                game.WindowResize = bool.Parse(at.Value);
                                                if (game.WindowResize)
                                                {
                                                    context.Logger.LogWarning(null, null, Messages.Game_WindowResizeable);
                                                }
                                            }
                                            at = ContentTagManager.GetXMLAtt("GAME_SETTINGS_DEFSETTINGS_GAME_TITLE", input.Version, gameSettingComponent);
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

        private static void LoadSpriteTypes(string spriteSheet, ContentProcessorContext context)
        {
            SpriteSheetProcessor.SpriteSheetTypes = new Dictionary<int, SpriteSheetProcessor.SpriteType>();
            SpriteSheetProcessor.Process(new SGDEImport().Import(spriteSheet, null), context, false);
        }

        #endregion

        #region Writing

        public static bool Write(double version, XmlNode parent, ProcessedContent content)
        {
            SGDeContent.DataTypes.Game game = content as SGDeContent.DataTypes.Game;
            if (game == null)
            {
                return false;
            }
            //Write out the game resources
            XmlElement element1 = parent.OwnerDocument.CreateElement(ContentTagManager.GetTagValue("IMPORT_GAME_ELEMENT", version));
            parent.AppendChild(element1);

            //TODO

            //Write out the game settings
            element1 = parent.OwnerDocument.CreateElement(ContentTagManager.GetTagValue("GAME_SETTINGS", version));
            parent.AppendChild(element1);

            //TODO

            return true;
        }

        #endregion
    }
}

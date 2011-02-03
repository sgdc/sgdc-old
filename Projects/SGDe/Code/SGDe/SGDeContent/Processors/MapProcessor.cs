using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using SGDeContent.DataTypes;
using System.IO;
using Microsoft.Xna.Framework;

namespace SGDeContent.Processors
{
    public class MapProcessor
    {
        /// <summary>
        /// This represents the currently processing entity ID. This is used when processing Entitys for the Map.
        /// </summary>
        public static int CurrentEntityID = -1;
        /// <summary>
        /// This is the currently processing map.
        /// </summary>
        public static Map map = null;

        public static Map Process(Content input, ContentProcessorContext context)
        {
            return Process(input.document.DocumentElement, input.Version, context);
        }

        public static Map Process(XmlElement input, double version, ContentProcessorContext context)
        {
            Map map = new Map();
            MapProcessor.map = map;

            #region Resource Processing

            XmlElement element = ContentTagManager.GetXMLElem("MAP_RESOURCE", version, input);

            //Process the resources
            if (element == null)
            {
                throw new InvalidContentException(Messages.Map_MissingResourceElement);
            }
            foreach (XmlElement resourceComponent in element)
            {
                if (ContentTagManager.TagMatches("MAP_RESOURCE_ENTITIES", resourceComponent.Name, version))
                {
                    #region Entities

                    map.DevID(ContentTagManager.GetXMLAtt("GENERAL_DEVELOPER_ID", version, resourceComponent), map.Entities);
                    foreach (XmlElement entity in resourceComponent)
                    {
                        int id = int.Parse(ContentTagManager.GetXMLAtt("GENERAL_ID", version, entity).Value);
                        if (map.EntityID.Contains(id))
                        {
                            throw new InvalidContentException(string.Format(Messages.Map_EntityIDExists, id));
                        }
                        map.EntityID.Add(id);
                        string entityRef = SGDEProcessor.GetInnerText(entity.ChildNodes[0]);
                        if (string.IsNullOrWhiteSpace(entityRef))
                        {
                            //Actual entity
                            map.Entities.Add(EntityProcessor.Process((XmlElement)entity.ChildNodes[0], version, context));
                        }
                        else
                        {
                            //Entity reference
                            if (!entityRef.EndsWith(".sgde", StringComparison.OrdinalIgnoreCase))
                            {
                                throw new InvalidContentException(Messages.Map_EntityRefNotSGDE);
                            }
                            map.Entities.Add(Utils.CompileExternal<Entity>(entityRef, context));
                        }
                        map.DevID(ContentTagManager.GetXMLAtt("GENERAL_DEVELOPER_ID", version, entity), map.Entities[map.Entities.Count - 1]);
                    }

                    #endregion
                }
            }

            #endregion

            #region Map Processing

            element = ContentTagManager.GetXMLElem("IMPORT_MAP_ELEMENT", version, input);

            if (element == null)
            {
                throw new InvalidContentException(Messages.Map_MissingMapElement);
            }
            foreach (XmlElement mapComponent in element)
            {
                if (ContentTagManager.TagMatches("MAP_MAP_LAYOUT", mapComponent.Name, version))
                {
                    #region Layout

                    foreach (XmlElement layoutComponent in mapComponent)
                    {
                        object[] entityPos; //Used to uniquely identify the layout component
                        if (ContentTagManager.TagMatches("MAP_MAP_LAYOUT_POSITION", layoutComponent.Name, version))
                        {
                            #region EntityLayout

                            entityPos = new object[2];
                            entityPos[0] = CurrentEntityID = int.Parse(ContentTagManager.GetXMLAtt("GENERAL_ID", version, layoutComponent).Value);
                            map.DevID(ContentTagManager.GetXMLAtt("GENERAL_DEVELOPER_ID", version, layoutComponent), entityPos);
                            foreach (XmlElement entityComponent in layoutComponent)
                            {
                                if (ContentTagManager.TagMatches("MAP_MAP_LAYOUT_POSITION_LAYOUT", entityComponent.Name, version))
                                {
                                    #region Layout

                                    string innerText = SGDEProcessor.GetInnerText(entityComponent);
                                    if (ContentTagManager.TagMatches("MAP_MAP_LAYOUT_POSITION_LAYOUT_PREDEFINED", innerText, version))
                                    {
                                        entityPos[1] = null; //All layout information is defined by Entity element itself
                                    }
                                    else if (ContentTagManager.TagMatches("MAP_MAP_LAYOUT_POSITION_LAYOUT_DEFINED", innerText, version))
                                    {
                                        entityPos[1] = EntityProcessor.Process((XmlElement)entityComponent, version, context);
                                    }

                                    #endregion
                                }
                            }
                            map.MapComponents.Add(entityPos);

                            #endregion
                        }
                        else if (ContentTagManager.TagMatches("MAP_MAP_LAYOUT_OPERATION", layoutComponent.Name, version))
                        {
                            entityPos = new object[1];
                            entityPos[0] = CodeProcessor.Process(layoutComponent, version, context);
                            map.MapComponents.Add(entityPos);
                        }
                    }

                    #endregion
                }
                else if (ContentTagManager.TagMatches("MAP_MAP_PHYSICS", mapComponent.Name, version))
                {
                    #region Physics

                    XmlAttribute at = ContentTagManager.GetXMLAtt("MAP_MAP_PHYSICS_ENABLED", version, mapComponent);
                    if (at != null)
                    {
                        if (!bool.Parse(at.Value))
                        {
                            map.Physics = false;
                            //Physics are not enabled so don't use/process it.
                            continue;
                        }
                    }
                    foreach (XmlElement physicsComponent in mapComponent)
                    {
                        if (ContentTagManager.TagMatches("MAP_MAP_PHYSICS_PHARAOH", physicsComponent.Name, version))
                        {
                            #region Physics Pharaoh

                            foreach (XmlElement pharaohComponent in physicsComponent)
                            {
                                if (ContentTagManager.TagMatches("MAP_MAP_PHYSICS_PHARAOH_CELL", pharaohComponent.Name, version))
                                {
                                    #region Cell

                                    XmlNode val = null;
                                    at = ContentTagManager.GetXMLAtt("GENERAL_WIDTH", version, pharaohComponent);
                                    if (at != null)
                                    {
                                        val = at;
                                    }
                                    else if (pharaohComponent.ChildNodes.Count > 0)
                                    {
                                        for (int i = 0; i < pharaohComponent.ChildNodes.Count; i++)
                                        {
                                            if (ContentTagManager.TagMatches("GENERAL_WIDTH", ContentTagManager.GetXMLAtt("GENERAL_ID", version, pharaohComponent.ChildNodes[i]).Value, version))
                                            {
                                                val = pharaohComponent.ChildNodes[i];
                                            }
                                        }
                                    }
                                    if (val != null)
                                    {
                                        map.Physics_CellSize_Width = CodeProcessor.Process(val, version, context);
                                    }
                                    val = null;
                                    at = ContentTagManager.GetXMLAtt("GENERAL_HEIGHT", version, pharaohComponent);
                                    if (at != null)
                                    {
                                        val = at;
                                    }
                                    else if (pharaohComponent.ChildNodes.Count > 0)
                                    {
                                        for (int i = 0; i < pharaohComponent.ChildNodes.Count; i++)
                                        {
                                            if (ContentTagManager.TagMatches("GENERAL_HEIGHT", ContentTagManager.GetXMLAtt("GENERAL_ID", version, pharaohComponent.ChildNodes[i]).Value, version))
                                            {
                                                val = pharaohComponent.ChildNodes[i];
                                            }
                                        }
                                    }
                                    if (val != null)
                                    {
                                        map.Physics_CellSize_Height = CodeProcessor.Process(val, version, context);
                                    }

                                    #endregion
                                }
                                else if (ContentTagManager.TagMatches("MAP_MAP_PHYSICS_PHARAOH_WORLD", pharaohComponent.Name, version))
                                {
                                    #region World

                                    XmlNode val = null;
                                    at = ContentTagManager.GetXMLAtt("GENERAL_WIDTH", version, pharaohComponent);
                                    if (at != null)
                                    {
                                        val = at;
                                    }
                                    else if (pharaohComponent.ChildNodes.Count > 0)
                                    {
                                        for (int i = 0; i < pharaohComponent.ChildNodes.Count; i++)
                                        {
                                            if (ContentTagManager.TagMatches("GENERAL_WIDTH", ContentTagManager.GetXMLAtt("GENERAL_ID", version, pharaohComponent.ChildNodes[i]).Value, version))
                                            {
                                                val = pharaohComponent.ChildNodes[i];
                                            }
                                        }
                                    }
                                    if (val != null)
                                    {
                                        map.Physics_World_Width = CodeProcessor.Process(val, version, context);
                                    }
                                    val = null;
                                    at = ContentTagManager.GetXMLAtt("GENERAL_HEIGHT", version, pharaohComponent);
                                    if (at != null)
                                    {
                                        val = at;
                                    }
                                    else if (pharaohComponent.ChildNodes.Count > 0)
                                    {
                                        for (int i = 0; i < pharaohComponent.ChildNodes.Count; i++)
                                        {
                                            if (ContentTagManager.TagMatches("GENERAL_HEIGHT", ContentTagManager.GetXMLAtt("GENERAL_ID", version, pharaohComponent.ChildNodes[i]).Value, version))
                                            {
                                                val = pharaohComponent.ChildNodes[i];
                                            }
                                        }
                                    }
                                    if (val != null)
                                    {
                                        map.Physics_World_Height = CodeProcessor.Process(val, version, context);
                                    }

                                    #endregion
                                }
                                else if (ContentTagManager.TagMatches("MAP_MAP_PHYSICS_PHARAOH_GRAVITY", pharaohComponent.Name, version))
                                {
                                    #region Gravity

                                    Vector2 grav = new Vector2();
                                    at = ContentTagManager.GetXMLAtt("GENERAL_X", version, pharaohComponent);
                                    if (at != null)
                                    {
                                        grav.X = float.Parse(at.Value);
                                    }
                                    at = ContentTagManager.GetXMLAtt("GENERAL_Y", version, pharaohComponent);
                                    if (at != null)
                                    {
                                        grav.Y = float.Parse(at.Value);
                                    }
                                    if (grav.X != 0 || grav.Y != 0)
                                    {
                                        map.Physics_Gravity = grav;
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

            #endregion

            //Reset the process values
            CurrentEntityID = -1;
            MapProcessor.map = null;

            map.Validate(context);
            return map;
        }
    }
}

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
            return Process(input.document.DocumentElement, context);
        }

        public static Map Process(XmlElement input, ContentProcessorContext context)
        {
            Map map = new Map();
            MapProcessor.map = map;

            #region Resource Processing

            XmlElement element = input["Resources"];

            //Process the resources
            if (element == null)
            {
                throw new InvalidContentException(Messages.Map_MissingResourceElement);
            }
            foreach (XmlElement resourceComponent in element)
            {
                if (resourceComponent.Name.Equals("Entities"))
                {
                    #region Entities

                    map.DevID(resourceComponent.Attributes["DID"], map.Entities);
                    foreach (XmlElement entity in resourceComponent)
                    {
                        int id = int.Parse(entity.Attributes["ID"].Value);
                        if (map.EntityID.Contains(id))
                        {
                            throw new InvalidContentException(string.Format(Messages.Map_EntityIDExists, id));
                        }
                        map.EntityID.Add(id);
                        string entityRef = SGDEProcessor.GetInnerText(entity.ChildNodes[0]);
                        if (string.IsNullOrWhiteSpace(entityRef))
                        {
                            //Actual entity
                            map.Entities.Add(EntityProcessor.Process((XmlElement)entity.ChildNodes[0], context));
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
                        map.DevID(entity.Attributes["DID"], map.Entities[map.Entities.Count - 1]);
                    }

                    #endregion
                }
            }

            #endregion

            #region Map Processing

            element = input["Map"];

            if (element == null)
            {
                throw new InvalidContentException(Messages.Map_MissingMapElement);
            }
            foreach (XmlElement mapComponent in element)
            {
                if (mapComponent.Name.Equals("Layout"))
                {
                    #region Layout

                    foreach (XmlElement layoutComponent in mapComponent)
                    {
                        object[] entityPos; //Used to uniquely identify the layout component
                        if (layoutComponent.Name.Equals("EntityPos"))
                        {
                            #region EntityPos

                            entityPos = new object[2];
                            entityPos[0] = CurrentEntityID = int.Parse(layoutComponent.Attributes["ID"].Value);
                            map.DevID(layoutComponent.Attributes["DID"], entityPos);
                            foreach (XmlElement entityComponent in layoutComponent)
                            {
                                if (entityComponent.Name.Equals("Layout"))
                                {
                                    #region Layout

                                    string innerText = SGDEProcessor.GetInnerText(entityComponent);
                                    if (innerText.Equals("EntityDefined"))
                                    {
                                        entityPos[1] = null; //All layout information is defined by Entity element itself
                                    }
                                    else if (innerText.Equals("Defined"))
                                    {
                                        entityPos[1] = EntityProcessor.Process((XmlElement)entityComponent, context);
                                    }

                                    #endregion
                                }
                            }
                            map.MapComponents.Add(entityPos);

                            #endregion
                        }
                        else if (layoutComponent.Name.Equals("Operation"))
                        {
                            entityPos = new object[1];
                            entityPos[0] = CodeProcessor.Process(layoutComponent, context);
                            map.MapComponents.Add(entityPos);
                        }
                    }

                    #endregion
                }
                else if (mapComponent.Name.Equals("Physics"))
                {
                    #region Physics

                    XmlAttribute at = mapComponent.Attributes["Enabled"];
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
                        if (physicsComponent.Name.Equals("Pharaoh"))
                        {
                            #region Physics Pharaoh

                            foreach (XmlElement pharaohComponent in physicsComponent)
                            {
                                if (pharaohComponent.Name.Equals("Cell"))
                                {
                                    #region Cell

                                    XmlNode val = null;
                                    at = pharaohComponent.Attributes["Width"];
                                    if (at != null)
                                    {
                                        val = at;
                                    }
                                    else if (pharaohComponent.ChildNodes.Count > 0)
                                    {
                                        for (int i = 0; i < pharaohComponent.ChildNodes.Count; i++)
                                        {
                                            if (pharaohComponent.ChildNodes[i].Attributes["ID"].Value.Equals("Width"))
                                            {
                                                val = pharaohComponent.ChildNodes[i];
                                            }
                                        }
                                    }
                                    if (val != null)
                                    {
                                        map.Physics_CellSize_Width = CodeProcessor.Process(val, context);
                                    }
                                    val = null;
                                    at = pharaohComponent.Attributes["Height"];
                                    if (at != null)
                                    {
                                        val = at;
                                    }
                                    else if (pharaohComponent.ChildNodes.Count > 0)
                                    {
                                        for (int i = 0; i < pharaohComponent.ChildNodes.Count; i++)
                                        {
                                            if (pharaohComponent.ChildNodes[i].Attributes["ID"].Value.Equals("Height"))
                                            {
                                                val = pharaohComponent.ChildNodes[i];
                                            }
                                        }
                                    }
                                    if (val != null)
                                    {
                                        map.Physics_CellSize_Height = CodeProcessor.Process(val, context);
                                    }

                                    #endregion
                                }
                                else if (pharaohComponent.Name.Equals("World"))
                                {
                                    #region World

                                    XmlNode val = null;
                                    at = pharaohComponent.Attributes["Width"];
                                    if (at != null)
                                    {
                                        val = at;
                                    }
                                    else if (pharaohComponent.ChildNodes.Count > 0)
                                    {
                                        for (int i = 0; i < pharaohComponent.ChildNodes.Count; i++)
                                        {
                                            if (pharaohComponent.ChildNodes[i].Attributes["ID"].Value.Equals("Width"))
                                            {
                                                val = pharaohComponent.ChildNodes[i];
                                            }
                                        }
                                    }
                                    if (val != null)
                                    {
                                        map.Physics_World_Width = CodeProcessor.Process(val, context);
                                    }
                                    val = null;
                                    at = pharaohComponent.Attributes["Height"];
                                    if (at != null)
                                    {
                                        val = at;
                                    }
                                    else if (pharaohComponent.ChildNodes.Count > 0)
                                    {
                                        for (int i = 0; i < pharaohComponent.ChildNodes.Count; i++)
                                        {
                                            if (pharaohComponent.ChildNodes[i].Attributes["ID"].Value.Equals("Height"))
                                            {
                                                val = pharaohComponent.ChildNodes[i];
                                            }
                                        }
                                    }
                                    if (val != null)
                                    {
                                        map.Physics_World_Height = CodeProcessor.Process(val, context);
                                    }

                                    #endregion
                                }
                                else if (pharaohComponent.Name.Equals("Gravity"))
                                {
                                    #region Gravity

                                    Vector2 grav = new Vector2();
                                    at = pharaohComponent.Attributes["X"];
                                    if (at != null)
                                    {
                                        grav.X = float.Parse(at.Value);
                                    }
                                    at = pharaohComponent.Attributes["Y"];
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

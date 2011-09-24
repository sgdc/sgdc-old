using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE.Graphics;
using SGDE.Content.DataTypes;
using SGDE.Input;

namespace SGDE.Content.Readers
{
    /// <summary>
    /// Read and process a MapContent class
    /// </summary>
    internal class MapReader : ContentTypeReader<MapContent>
    {
        /// <summary>
        /// Read a MapContent.
        /// </summary>
        protected override MapContent Read(ContentReader input, MapContent existingInstance)
        {
            MapContent content = new MapContent();
            //Read physics information
            if (content.enPhysics = input.ReadBoolean())
            {
                SGDE.Content.Code.Code code = input.ReadRawObject<SGDE.Content.Code.Code>();
                float value = (float)code.Evaluate();
                code = input.ReadRawObject<SGDE.Content.Code.Code>();
                content.physicsCellSize = new Vector2(value, (float)code.Evaluate());
                code = input.ReadRawObject<SGDE.Content.Code.Code>();
                value = (float)code.Evaluate();
                code = input.ReadRawObject<SGDE.Content.Code.Code>();
                content.physicsWorldSize = new Vector2(value, (float)code.Evaluate());
                content.physicsGravity = input.ReadVector2();

                SGDE.Physics.PhysicsPharaoh pharaoh = SGDE.Physics.PhysicsPharaoh.GetInstance();
                pharaoh.Initialize(content.physicsWorldSize, content.physicsCellSize);
                pharaoh.SetGravity(content.physicsGravity);
            }
            //Read resources
            int count = input.ReadInt32();
            content.builders = new List<EntityBuilder>(count);
            ContentUtil.DeveloperID(ref content.developerTypes, input, content.builders);
            ContentUtil.LoadingBuilders = true;
            for (int i = 0; i < count; i++)
            {
                Entity en;
                ContentUtil.PrepTempDID();
                if (input.ReadBoolean())
                {
                    en = input.ReadRawObject<Entity>();
                }
                else
                {
                    en = input.ReadExternalReference<Entity>();
                }
                //en.SetID((uint)i + 1);
                ContentUtil.FinishTempDID(ref content.developerTypes);
                Dictionary<string, object> physics = ContentUtil.temp.ContainsKey("EntityPhysics") ? (Dictionary<string, object>)ContentUtil.temp["EntityPhysics"] : null;
                if (physics != null)
                {
                    ContentUtil.temp.Remove("EntityPhysics"); //Keep it clean
                }
                EntityBuilder builder = new EntityBuilder(en, physics);
                ContentUtil.DeveloperID(ref content.developerTypes, input, builder);
                content.builders.Add(builder);
            }
            ContentUtil.LoadingBuilders = false;
            //Read map information
            count = input.ReadInt32();
            content.dEntities = new List<Entity>(count);
            content.uEntities = new List<Entity>(count);
            List<Entity> entities = new List<Entity>(count);
            InputManager iman = Game.CurrentGame.imanager;
            for (int i = 0; i < count; i++)
            {
                switch (input.ReadInt32())
                {
                    case 0:
                        RunOperation(ref content, input.ReadRawObject<SGDE.Content.Code.Code>());
                        break;
                    case 1:
                        ContentUtil.PrepTempDID();
                        int builderIndex = input.ReadInt32();
                        ContentUtil.LoadingBuilders = true;
                        Entity ent = input.ReadObject<Entity>();
                        Dictionary<string, object> physics = ContentUtil.temp.ContainsKey("EntityPhysics") ? (Dictionary<string, object>)ContentUtil.temp["EntityPhysics"] : null;
                        if (physics != null)
                        {
                            ContentUtil.temp.Remove("EntityPhysics"); //Keep it clean
                        }
                        ContentUtil.LoadingBuilders = false;
                        Entity resultingEntity = content.builders[builderIndex].Create(ent, physics);
                        if(resultingEntity is InputHandler)
                        {
                            iman.AddNewHandler((InputHandler)resultingEntity);
                        }
                        entities.Add(resultingEntity);
                        ContentUtil.FinishTempDID(ref content.developerTypes);
                        break;
                }
                ContentUtil.DeveloperID(ref content.developerTypes, input, entities[entities.Count - 1]);
            }
            //Read map sorting order
            List<int> sort = input.ReadObject<List<int>>();
            EventHandler<EventArgs> eventHandler = new EventHandler<EventArgs>(content.UpdateOrderChanged);
            for (int i = 0; i < count; i++)
            {
                Entity en = entities[sort[i]];
                en.UpdateOrderChanged += eventHandler;
                content.uEntities.Add(en);
            }
            sort = input.ReadObject<List<int>>();
            eventHandler = new EventHandler<EventArgs>(content.DrawOrderChanged);
            for (int i = 0; i < count; i++)
            {
                Entity en = entities[sort[i]];
                en.SpriteImage.DrawOrderChanged += eventHandler;
                content.dEntities.Add(en);
            }
            //Now do any extra sorting that didn't occur otherwise
            content.Sort();
            return content;
        }

        private void RunOperation(ref MapContent content, SGDE.Content.Code.Code code)
        {
            //TODO: Don't forget "SGDE.Physics.PhysicsPharaoh.loadingBuilders = false;"
            //TODO: Don't forget to add entity to InputManager if it uses InputHandler
        }
    }

    /// <summary>
    /// Read and process a MapSettings class
    /// </summary>
    internal class MapSettingsReader : ContentTypeReader<MapSettings>
    {
        /// <summary>
        /// Read a MapSettings.
        /// </summary>
        protected override MapSettings Read(ContentReader input, MapSettings existingInstance)
        {
            MapSettings set = new MapSettings();
            set.CameraPosition = input.ReadObject<Vector2?>();
            set.CameraRotation = input.ReadObject<float?>();
            set.CameraScale = input.ReadObject<Vector2?>();
            set.CameraBounds = input.ReadObject<Vector4?>();
            set.OrderSeperation = input.ReadObject<float?>();
            set.CentralOrder = input.ReadObject<int?>();
            return set;
        }
    }
}

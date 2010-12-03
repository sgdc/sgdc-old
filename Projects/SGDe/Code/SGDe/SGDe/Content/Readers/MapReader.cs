using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE.Graphics;
using SGDE.Content.DataTypes;

namespace SGDE.Content.Readers
{
    /// <summary>
    /// Read and process a MapContent class
    /// </summary>
    public class MapReader : ContentTypeReader<MapContent>
    {
        /// <summary>
        /// Read a MapContent.
        /// </summary>
        protected override MapContent Read(ContentReader input, MapContent existingInstance)
        {
            MapContent content = new MapContent();
            //Read physics information
            if (input.ReadBoolean())
            {
                SGDE.Content.Code.Code code = input.ReadRawObject<SGDE.Content.Code.Code>();
                float value = (float)code.Evaluate();
                code = input.ReadRawObject<SGDE.Content.Code.Code>();
                Vector2 cellSize = new Vector2(value, (float)code.Evaluate());
                code = input.ReadRawObject<SGDE.Content.Code.Code>();
                value = (float)code.Evaluate();
                code = input.ReadRawObject<SGDE.Content.Code.Code>();
                SGDE.Physics.PhysicsPharaoh pharaoh = SGDE.Physics.PhysicsPharaoh.GetInstance();
                pharaoh.Initialize(new Vector2(value, (float)code.Evaluate()), cellSize);
                pharaoh.SetGravity(input.ReadVector2());
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
            content.entities = new List<Entity>(count);
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
                        content.entities.Add(content.builders[builderIndex].Create(ent, physics));
                        ContentUtil.FinishTempDID(ref content.developerTypes);
                        break;
                }
                ContentUtil.DeveloperID(ref content.developerTypes, input, content.entities[content.entities.Count - 1]);
            }
            return content;
        }

        private void RunOperation(ref MapContent content, SGDE.Content.Code.Code code)
        {
            //TODO: Don't forget "SGDE.Physics.PhysicsPharaoh.loadingBuilders = false;"
        }
    }
}

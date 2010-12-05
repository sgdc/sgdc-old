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
    /// Read and process a Entity class
    /// </summary>
    public class EntityReader : ContentTypeReader<Entity>
    {
        /// <summary>
        /// Read a Entity.
        /// </summary>
        protected override Entity Read(ContentReader input, Entity existingInstance)
        {
            Entity entity;
            //Create a new Entity of the proper type
            if (input.ReadBoolean())
            {
                Type t = Type.GetType(input.ReadString());
                int acount = input.ReadInt32();
                object[] args = null;
                if (acount >= 0)
                {
                    args = new object[acount];
                    for (int i = 0; i < args.Length; i++)
                    {
                        object obj = input.ReadObject<object>();
                        if (obj is SGDE.Content.Code.Code)
                        {
                            SGDE.Content.Code.Code code = (SGDE.Content.Code.Code)obj;
                            string s = input.ReadString();
                            Type type = Type.GetType(s);
                            try
                            {
                                obj = Convert.ChangeType(code.Evaluate(), type);
                            }
                            catch
                            {
                                obj = Activator.CreateInstance(type);
                            }
                        }
                        args[i] = obj;
                    }
                }
                entity = (Entity)Activator.CreateInstance(t, args);
                entity.args = args;
            }
            else
            {
                entity = new GenericEntity();
            }
            //Read Entity values
            //-Read sprite information
            Sprite sprite = new Sprite();
            entity.SpriteImage = sprite;
            SpriteManager manager = SpriteManager.GetInstance(input.ContentManager);
            sprite.baseTexture = manager.GetTexture(input.ReadInt32());
            ContentUtil.TempDeveloperID(input, sprite);
            Color? tint = input.ReadObject<Color?>();
            if (tint.HasValue)
            {
                sprite.Tint = tint.Value;
            }
            if (input.ReadBoolean())
            {
                sprite.overrideAtt = input.ReadObject<Sprite.SpriteAttributes>();
            }
            ReadAnimation(ref sprite, input);
            if (input.ReadBoolean())
            {
                sprite.animStart = input.ReadInt32(); //Begin
                sprite.animEnd = input.ReadInt32(); //End
                //If either value is -1 then reset to default animation value
                if (sprite.animStart < 0)
                {
                    sprite.animStart = 0;
                }
                else
                {
                    sprite.frame = sprite.animStart;
                }
                if (sprite.animEnd < 0)
                {
                    sprite.animEnd = sprite.animation.frameCount;
                }
            }
            //-Read physics
            ProcessPhysics(ref entity, input);
            //Read in SceneNode information
            if (input.ReadBoolean())
            {
                input.ReadRawObject<SceneNode>(entity);
                ContentUtil.TempDeveloperID(input, (SceneNode)entity);
            }
            return entity;
        }

        #region ReadAnimation

        private static void ReadAnimation(ref Sprite sprite, ContentReader input)
        {
            bool builtInAnimation = input.ReadBoolean();
            int defAnimation = input.ReadInt32();
            List<SpriteManager.SpriteAnimation> animations = input.ReadObject<List<SpriteManager.SpriteAnimation>>();
            if (!builtInAnimation)
            {
                SpriteManager manager = SpriteManager.GetInstance(input.ContentManager);
                if (defAnimation == -1)
                {
                    defAnimation = 0;
                }
                if (animations != null)
                {
                    bool gotAn = false;
                    for (int i = 0; i < animations.Count; i++)
                    {
                        SpriteManager.SpriteAnimation animation = animations[i];
                        int value = manager.AddAnimation(animation, spriteId: (ushort)animation.ID);
                        if (!gotAn && i == defAnimation)
                        {
                            gotAn = true;
                            defAnimation = value;
                        }
                    }
                }
            }
            sprite.animation = SpriteManager.GetInstance(input.ContentManager).GetFrames(defAnimation);
            sprite.FPS = sprite.animation.DefaultFPS;
            sprite.animStart = sprite.frame = 0;
            sprite.animEnd = sprite.animation.frameCount;
        }

        #endregion

        #region ProcessPhysics

        private static void ProcessPhysics(ref Entity entity, ContentReader input)
        {
            Dictionary<string, object> physics = input.ReadObject<Dictionary<string, object>>();
            if (physics != null)
            {
                ContentUtil.temp.Add("EntityPhysics", physics);
                ProcessPhysics(ref entity, physics);
            }
        }

        internal static void ProcessPhysics(ref Entity entity, Dictionary<string, object> physics)
        {
            if (physics != null)
            {
                bool initializePostCreate = false;
                bool dontCallEnableIfNotEnabled = false;
                if (physics.ContainsKey("PostSetup"))
                {
                    initializePostCreate = (bool)physics["PostSetup"];
                }
                if (physics.ContainsKey("EnableOnEnable"))
                {
                    dontCallEnableIfNotEnabled = (bool)physics["EnableOnEnable"];
                }
                SGDE.Physics.Collision.CollisionUnit unit;
                if (!initializePostCreate)
                {
                    entity.Initialize();
                    unit = entity.GetCollisionUnit();
                }

                //Baby processes
                SGDE.Physics.PhysicsBaby baby = entity.GetPhysicsBaby();
                if (physics.ContainsKey("Static"))
                {
                    baby.SetStatic((bool)physics["Static"]);
                }
                if (physics.ContainsKey("Forces"))
                {
                    List<object> forces = (List<object>)physics["Forces"];
                    foreach (object force in forces)
                    {
                        if (force is Vector2)
                        {
                            baby.AddForce((Vector2)force);
                        }
                        else
                        {
                            if (force.GetType().IsArray)
                            {
                                object[] arr = (object[])force;
                                baby.AddForce(new Vector2((float)((SGDE.Content.Code.Code)arr[0]).Evaluate(), (float)((SGDE.Content.Code.Code)arr[1]).Evaluate()));
                            }
                            else
                            {
                                throw new NotImplementedException("Single Code Execution is not implemented until Code supports it");
                            }
                        }
                    }
                }
                if (physics.ContainsKey("Velocity"))
                {
                    object[] velocityValue = (object[])physics["Velocity"];
                    switch (velocityValue.Length)
                    {
                        case 1:
                            baby.SetVelocity((Vector2)velocityValue[0]);
                            break;
                        case 2:
                            SGDE.Content.Code.Code code = (SGDE.Content.Code.Code)velocityValue[0];
                            float val = (float)code.Evaluate();
                            code = (SGDE.Content.Code.Code)velocityValue[1];
                            baby.SetVelocity(new Vector2(val, (float)code.Evaluate()));
                            break;
                    }
                }

                //EnablePhysics
                bool enable = false, collision = false;
                if (physics.ContainsKey("Enabled"))
                {
                    enable = (bool)physics["Enabled"];
                }
                if (physics.ContainsKey("Collision"))
                {
                    collision = (bool)physics["Collision"];
                }
                if (!(dontCallEnableIfNotEnabled && !enable))
                {
                    entity.EnablePhysics(enable, collision);
                }

                if (initializePostCreate)
                {
                    entity.Initialize();
                    unit = entity.GetCollisionUnit();
                }

                //Unit processes
                //TODO
            }
        }

        #endregion

        private class GenericEntity : Entity { }
    }
}

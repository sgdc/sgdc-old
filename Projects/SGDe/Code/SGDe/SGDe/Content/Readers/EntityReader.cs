﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE.Graphics;
using SGDE.Content.DataTypes;
using System.Reflection;

namespace SGDE.Content.Readers
{
    /// <summary>
    /// Read and process a Entity class
    /// </summary>
    internal class EntityReader : ContentTypeReader<Entity>
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
                Type[] types = null;
                if (acount >= 0)
                {
                    List<Type> typeInfo = new List<Type>();
                    args = new object[acount];
                    for (int i = 0; i < args.Length; i++)
                    {
                        object obj = input.ReadObject<object>();
                        if (obj is SGDE.Content.Code.Code)
                        {
                            //Code might not always return the correct type, which is the reason for the specific reference
                            SGDE.Content.Code.Code code = (SGDE.Content.Code.Code)obj;
                            Type type = Type.GetType(input.ReadString());
                            typeInfo.Add(type);
                            try
                            {
#if WINDOWS
                                obj = Convert.ChangeType(code.Evaluate(), type);
#else
                                obj = Convert.ChangeType(code.Evaluate(), type, null);
#endif
                            }
                            catch
                            {
                                obj = Activator.CreateInstance(type);
                            }
                        }
                        else if (obj == null)
                        {
                            //A null object could have a specific type, save it for when trying to find a constructor
                            if (input.ReadBoolean())
                            {
                                typeInfo.Add(Type.GetType(input.ReadString()));
                            }
                            else
                            {
                                //Unused/assigned type
                                typeInfo.Add(typeof(void));
                            }
                        }
                        else
                        {
                            //Just get the normal type
                            typeInfo.Add(obj.GetType());
                        }
                        args[i] = obj;
                    }
                    types = typeInfo.ToArray();
                }
                entity = CreateEntityInstance(t, ref args, types);
                entity.args = args;
            }
            else
            {
                //No special type, make generic entity
                entity = new GenericEntity();
            }
            //Read Entity values
            //-Read sprite information
            entity.SpriteImage = input.ReadObject<Sprite>();
            ContentUtil.TempDeveloperID(input, entity.SpriteImage);
            //-Read physics
            ProcessPhysics(ref entity, input);
            //Read in SceneNode information
            entity.Enabled = input.ReadBoolean();
            entity.UpdateOrder = input.ReadInt32();
            if (input.ReadBoolean())
            {
                input.ReadRawObject<SceneNode>(entity);
                ContentUtil.TempDeveloperID(input, (SceneNode)entity);
            }
            if (entity.SpriteImage != null)
            {
                SceneNode sNode = entity.SpriteImage;
                ((SceneNode)entity).CopyTo(ref sNode);
            }
            //-Read sprite-offset
            Vector2 offset = input.ReadVector2();
            if (entity.SpriteImage != null)
            {
                entity.SpriteImage.Translate(offset);
            }
            return entity;
        }

        /// <summary>
        /// Get the current version of the Entity Reader.
        /// </summary>
        public override int TypeVersion
        {
            get
            {
                return 2;
            }
        }

        #region ProcessPhysics

        private static void ProcessPhysics(ref Entity entity, ContentReader input)
        {
            Dictionary<string, object> physics = input.ReadObject<Dictionary<string, object>>();
            if (physics != null)
            {
                ContentUtil.temp.Add("EntityPhysics", physics);
                if (entity.SpriteImage != null)
                {
                    ProcessPhysics(ref entity, physics, false);
                }
            }
        }

        internal static void ProcessPhysics(ref Entity entity, Dictionary<string, object> physics, bool modification)
        {
            if (physics != null)
            {
                bool initializePostCreate = false;
                bool dontCallEnableIfNotEnabled = false;
                if (physics.ContainsKey("PS"))
                {
                    //PostSetup
                    initializePostCreate = (bool)physics["PS"];
                }
                if (physics.ContainsKey("EE"))
                {
                    //EnableOnEnable
                    dontCallEnableIfNotEnabled = (bool)physics["EE"];
                }
                SGDE.Physics.Collision.CollisionUnit unit;
                if (!initializePostCreate)
                {
                    if (!modification)
                    {
                        entity.InSetUpCollision();
                    }
                    unit = entity.GetCollisionUnit();
                }

                //Baby processes
                SGDE.Physics.PhysicsBaby baby = entity.GetPhysicsBaby();
                if (modification)
                {
                    //Count call "entity.EnablePhysics(false, collision);" but until the remove collision unit portion is finished, it's safer to simply do this:
                    SGDE.Physics.PhysicsPharaoh.GetInstance().RemovePhysicsBaby(baby);
                }
                if (physics.ContainsKey("S"))
                {
                    //Static
                    baby.SetStatic((bool)physics["S"]);
                }
                if (physics.ContainsKey("F"))
                {
                    //Forces
                    List<object> forces = (List<object>)physics["F"];
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
                if (physics.ContainsKey("V"))
                {
                    //Velocity
                    object[] velocityValue = (object[])physics["V"];
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
                if (physics.ContainsKey("E"))
                {
                    //Enabled
                    enable = (bool)physics["E"];
                }
                if (physics.ContainsKey("C"))
                {
                    //Collision
                    collision = (bool)physics["C"];
                }
                if (!(dontCallEnableIfNotEnabled && !enable))
                {
                    entity.EnablePhysics(enable, collision);
                }

                if (initializePostCreate)
                {
                    if (!modification)
                    {
                        entity.InSetUpCollision();
                    }
                    unit = entity.GetCollisionUnit();
                }

                //Unit processes
                //TODO
            }
        }

        #endregion

        #region Complex Generate

        internal static Entity CreateEntityInstance(Type t, object[] args)
        {
            return CreateEntityInstance(t, ref args, null);
        }

        internal static Entity CreateEntityInstance(Type t, ref object[] args, Type[] types)
        {
            Entity ent = null;
            Exception e = null;
            try
            {
                ent = (Entity)Activator.CreateInstance(t, args);
            }
            catch (MissingMethodException mme)
            {
                e = mme;
            }
            catch (NullReferenceException nre)
            {
                e = nre;
            }
            if (e != null)
            {
                ConstructorInfo constructor = FindClosestMatch(t.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance), ref args, types);
                if (constructor == null)
                {
                    throw e;
                }
                ent = (Entity)constructor.Invoke(args);
            }
            return ent;
        }

        internal static ConstructorInfo FindClosestMatch(ConstructorInfo[] constructors, ref object[] args, Type[] types)
        {
            //Attribute is used as a "use default value" marker
            ConstructorInfo conInfo = null;
            if (args == null)
            {
                if (types == null)
                {
                    //Find default constructor
                    foreach (ConstructorInfo con in constructors)
                    {
                        ParameterInfo[] paramaters = con.GetParameters();
                        if (paramaters.Length == 0)
                        {
                            conInfo = con;
                            break;
                        }
                        bool good = true;
                        foreach (ParameterInfo param in paramaters)
                        {
                            if ((param.Attributes & ParameterAttributes.HasDefault) != ParameterAttributes.HasDefault)
                            {
                                good = false;
                                break;
                            }
                        }
                        if (good)
                        {
                            args = new object[paramaters.Length];
                            for (int i = 0; i < args.Length; i++)
                            {
                                args[i] = paramaters[i].DefaultValue;
                            }
                            conInfo = con;
                            break;
                        }
                    }
                }
                /* At completion, it was realized that it wouldn't be useful. Having just types might allow for the selection of a constructor but arguments need to be passed into the constructor
                else
                {
                    //Find constructor that matches types
                    //Check argument types because they will be used to find the constructor
                    bool good = true;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (types[i] == null)
                        {
                            good = false;
                            break;
                        }
                    }
                    if (good)
                    {
                        foreach (ConstructorInfo con in constructors)
                        {
                            good = true;
                            ParameterInfo[] paramaters = con.GetParameters();
                            int typeIndex = 0;
                            int aTypeIndex = 0;
                            while (typeIndex < paramaters.Length)
                            {
                                if (aTypeIndex >= types.Length)
                                {
                                    break;
                                }
                                Type paramType = paramaters[typeIndex++].ParameterType;
                                bool got = false;
                                for (int i = aTypeIndex; i < types.Length; i++)
                                {
                                    if (paramType.IsAssignableFrom(types[i]))
                                    {
                                        got = true;
                                        aTypeIndex++;
                                        break;
                                    }
                                }
                                if (!got)
                                {
                                    good = false;
                                    break;
                                }
                            }
                            if (good)
                            {
                                //Found it
                                //Generate args
                                args = new object[paramaters.Length];
                                //TODO for another time

                                //Return
                                conInfo = con;
                                break;
                            }
                        }
                    }
                }
                */
            }
            else
            {
                if (types == null)
                {
                    //Make a list of types
                    types = new Type[args.Length];
                }
                if (args.Length == types.Length)
                {
                    //Check argument types because they will be used to find the constructor
                    bool good = true;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (types[i] == null)
                        {
                            //Get the argument type
                            if (args[i] != null)
                            {
                                types[i] = args[i].GetType();
                            }
                            else
                            {
                                types[i] = typeof(void);
                            }
                        }
                        else
                        {
                            //Check the argument type, if it is a mismatch then we might have an issue
                            if (args[i] != null)
                            {
                                if (!types[i].Equals(typeof(Attribute)) && !types[i].Equals(args[i].GetType()))
                                {
                                    good = false;
                                }
                            }
                            //Don't need to check null types
                        }
                    }
                    if (good)
                    {
                        //We only want it when we know all the argument types and arguments, no mismatch
                        foreach (ConstructorInfo con in constructors)
                        {
                            good = true;
                            ParameterInfo[] paramaters = con.GetParameters();
                            if (paramaters.Length == args.Length)
                            {
                                for (int i = 0; i < args.Length; i++)
                                {
                                    Type paramType = paramaters[i].ParameterType;
                                    if (types[i].Equals(typeof(Attribute)))
                                    {
                                        if ((paramaters[i].Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault)
                                        {
                                            args[i] = paramaters[i].DefaultValue;
                                        }
                                        else
                                        {
                                            if (paramType.IsValueType)
                                            {
                                                args[i] = Activator.CreateInstance(paramType);
                                            }
                                            else
                                            {
                                                args[i] = null;
                                            }
                                        }
                                    }
                                    else if (args[i] == null)
                                    {
                                        if (paramType.IsValueType)
                                        {
                                            //Null is a no-no for a value type.
                                            good = false;
                                            break;
                                        }
                                        else if (!types[i].Equals(typeof(void)))
                                        {
                                            //If the types weren't generalted and actually have a type then check it
                                            if (!paramType.IsAssignableFrom(types[i]))
                                            {
                                                good = false;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!paramType.IsInstanceOfType(args[i]))
                                        {
                                            good = false;
                                            break;
                                        }
                                    }
                                }
                                if (good)
                                {
                                    //Found it
                                    conInfo = con;
                                    break;
                                }
                            }
                            /* Also unnecessery, if arguments are passed. It is from EntityReader or EntityLoader (which gets it's args from EntityReader). The arg count is determined at compilation time. It is true that
                             * the last elements might not be used but it would be unlikely unless someone really needs to have one, large-multi-arg-constructor-that-only-some-args-would-be-used-from, constructor.
                            else
                            {
                                //TODO
                            }
                            */
                        }
                    }
                }
            }
            return conInfo;
        }

        #endregion

        private class GenericEntity : Entity { }
    }
}

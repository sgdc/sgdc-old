using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDeContent.DataTypes;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using Microsoft.Xna.Framework;
using SGDeContent.DataTypes.Sprites;

namespace SGDeContent.Processors
{
    public class EntityProcessor
    {
        public static Entity Process(Content input, ContentProcessorContext context)
        {
            return Process(ContentTagManager.GetXMLElem("IMPORT_ENTITIY_ELEMENT", input.Version, input.document.DocumentElement), input.Version, context);
        }

        public static Entity Process(XmlElement input, double version, ContentProcessorContext context)
        {
            Entity entity = new Entity();
            XmlAttribute at = ContentTagManager.GetXMLAtt("ENTITY_NAME", version, input);
            if (at != null)
            {
                entity.Name = at.Value;
            }
            for (int e = 0; e < input.ChildNodes.Count; e++)
            {
                XmlNode xmlNode = input.ChildNodes[e];
                if (!(xmlNode is XmlElement))
                {
                    continue;
                }
                XmlElement entityComponent = xmlNode as XmlElement;
                if (ContentTagManager.TagMatches("ENTITY_NODE", entityComponent.Name, version))
                {
                    #region Node

                    Node node = new Node();
                    bool nonDefault = false;

                    at = ContentTagManager.GetXMLAtt("GENERAL_DEVELOPER_ID", version, entityComponent);
                    if (at != null)
                    {
                        nonDefault = true;
                        entity.DevID(at, node);
                    }
                    bool gotScale = false;
                    foreach (XmlElement nodeComponent in entityComponent)
                    {
                        if (ContentTagManager.TagMatches("ENTITY_NODE_VECTOR2", nodeComponent.Name, version))
                        {
                            #region Vector2

                            string type = ContentTagManager.GetXMLAtt("GENERAL_ID", version, nodeComponent).Value;
                            at = ContentTagManager.GetXMLAtt("GENERAL_X", version, nodeComponent);
                            if (at != null)
                            {
                                object val;
                                try
                                {
                                    val = float.Parse(at.Value);
                                }
                                catch
                                {
                                    val = CodeProcessor.Process(at, version, context);
                                }
                                if (ContentTagManager.TagMatches("ENTITY_NODE_VECTOR2_TRANSLATION", type, version))
                                {
                                    if (val is float)
                                    {
                                        node.Translation.X = (float)val;
                                    }
                                    else
                                    {
                                        node.tx = (SGDeContent.DataTypes.Code.Code)val;
                                    }
                                }
                                else if (ContentTagManager.TagMatches("ENTITY_NODE_VECTOR2_SCALE", type, version))
                                {
                                    if (val is float)
                                    {
                                        node.Scale.X = (float)val;
                                    }
                                    else
                                    {
                                        node.sx = (SGDeContent.DataTypes.Code.Code)val;
                                    }
                                }
                            }
                            at = ContentTagManager.GetXMLAtt("GENERAL_Y", version, nodeComponent);
                            if (at != null)
                            {
                                object val;
                                try
                                {
                                    val = float.Parse(at.Value);
                                }
                                catch
                                {
                                    val = CodeProcessor.Process(at, version, context);
                                }
                                if (ContentTagManager.TagMatches("ENTITY_NODE_VECTOR2_TRANSLATION", type, version))
                                {
                                    if (val is float)
                                    {
                                        node.Translation.Y = (float)val;
                                    }
                                    else
                                    {
                                        node.ty = (SGDeContent.DataTypes.Code.Code)val;
                                    }
                                }
                                else if (ContentTagManager.TagMatches("ENTITY_NODE_VECTOR2_SCALE", type, version))
                                {
                                    if (val is float)
                                    {
                                        node.Scale.Y = (float)val;
                                    }
                                    else
                                    {
                                        node.sy = (SGDeContent.DataTypes.Code.Code)val;
                                    }
                                }
                            }
                            if (ContentTagManager.TagMatches("ENTITY_NODE_VECTOR2_TRANSLATION", type, version))
                            {
                                if (node.Translation != Vector2.Zero || (node.tx != null || node.ty != null))
                                {
                                    nonDefault = true;
                                }
                            }
                            else if (ContentTagManager.TagMatches("ENTITY_NODE_VECTOR2_SCALE", type, version))
                            {
                                gotScale = true;
                                if (node.Scale != Vector2.One || (node.sx != null || node.sy != null))
                                {
                                    nonDefault = true;
                                }
                            }

                            #endregion
                        }
                        else if (ContentTagManager.TagMatches("ENTITY_NODE_VECTOR1", nodeComponent.Name, version))
                        {
                            #region Float

                            float val = 0;

                            at = ContentTagManager.GetXMLAtt("ENTITY_NODE_VECTOR1_VALUE", version, nodeComponent);
                            if (at != null)
                            {
                                try
                                {
                                    val = float.Parse(at.Value);
                                }
                                catch
                                {
                                    val = Convert.ToSingle(CodeProcessor.Process(at, version, context).Constant);
                                }
                            }
                            at = ContentTagManager.GetXMLAtt("GENERAL_ID", version, nodeComponent);
                            if (ContentTagManager.TagMatches("ENTITY_NODE_VECTOR1_ROTATION", at.Value, version))
                            {
                                node.Rotation = val;
                                if (val != 0)
                                {
                                    nonDefault = true;
                                }
                            }

                            #endregion
                        }
                    }

                    if (nonDefault)
                    {
                        entity.NonDefaultNode = true;
                        if (!gotScale)
                        {
                            node.Scale = Vector2.One;
                        }
                        entity.Node = node;
                    }

                    #endregion
                }
                else if (ContentTagManager.TagMatches("ENTITY_SPRITE", entityComponent.Name, version))
                {
                    #region Sprite

                    at = ContentTagManager.GetXMLAtt("ENTITY_SPRITE_ID", version, entityComponent);
                    if (at == null)
                    {
                        if (MapProcessor.CurrentEntityID >= 0 && MapProcessor.map != null)
                        {
                            //Inheret the Sprite ID
                            if (MapProcessor.map.Entities[MapProcessor.CurrentEntityID] is Entity)
                            {
                                entity.Sprite = CopySprite(((Entity)MapProcessor.map.Entities[MapProcessor.CurrentEntityID]).Sprite);
                            }
                        }
                        if (entity.Sprite == null)
                        {
                            throw new InvalidContentException(Messages.Entity_Sprite_RequiresID);
                        }
                    }
                    else
                    {
                        int id = int.Parse(at.Value);
                        if (id < 0)
                        {
                            throw new InvalidContentException(Messages.Entity_Sprite_PositiveID);
                        }
                        entity.Sprite = CreateSprite(id);
                    }

                    at = ContentTagManager.GetXMLAtt("GENERAL_DEVELOPER_ID", version, entityComponent);
                    if (at != null)
                    {
                        entity.DevID(at, entity.Sprite);
                    }
                    at = ContentTagManager.GetXMLAtt("ENTITY_SPRITE_REGION", version, entityComponent);
                    if (at != null)
                    {
                        #region Region

                        string value = at.Value;
                        if (!ContentTagManager.TagMatches("ENTITY_SPRITE_REGION_FULL", value, version, StringComparison.OrdinalIgnoreCase))
                        {
                            string[] components = value.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                            if (components.Length < 1 || components.Length > 2)
                            {
                                context.Logger.LogWarning(null, null, Messages.Entity_Sprite_InvalidRegion);
                            }
                            else
                            {
                                int[] values = new int[components.Length];
                                bool error = false;
                                try
                                {
                                    for (int i = 0; i < components.Length; i++)
                                    {
                                        values[i] = int.Parse(components[i].Trim());
                                    }
                                }
                                catch
                                {
                                    error = true;
                                    context.Logger.LogWarning(null, null, Messages.Entity_Sprite_CannotParseRegion);
                                }
                                if (!error)
                                {
                                    entity.Sprite.HasRegion = true;
                                    switch (values.Length)
                                    {
                                        case 1:
                                            entity.Sprite.RegionBegin = entity.Sprite.RegionEnd = -1;
                                            if (value.IndexOf('-') > value.IndexOf(components[0]))
                                            {
                                                //Region defines starting portion of region
                                                entity.Sprite.RegionBegin = values[0];
                                            }
                                            else
                                            {
                                                //Region defines ending portion of region
                                                entity.Sprite.RegionEnd = values[0];
                                            }
                                            break;
                                        case 2:
                                            entity.Sprite.RegionBegin = values[0];
                                            entity.Sprite.RegionEnd = values[1];
                                            break;
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    at = ContentTagManager.GetXMLAtt("ENTITY_SPRITE_COLOR", version, entityComponent);
                    if (at != null)
                    {
                        #region Color

                        try
                        {
                            /* Bug causes this not to be set properly. Try 0xFF008000 for example. Supposed to be A:255, R:0, G:128, B:0. Instead green is 255.
                            Microsoft.Xna.Framework.Graphics.PackedVector.Byte4 packedVector = new Microsoft.Xna.Framework.Graphics.PackedVector.Byte4(); //Preperation
                            packedVector.PackedValue = Convert.ToUInt32(at.Value, 16); //Keep it clean
                            entity.Tint = new Color(packedVector.ToVector4()); //Build it
                             */
                            Color col = new Color(); //Build
                            col.PackedValue = Convert.ToUInt32(at.Value, 16); //Keep it clean
                            entity.Sprite.Tint = col;
                        }
                        catch
                        {
                            context.Logger.LogWarning(null, null, Messages.Animation_InvalidColor, at.Value);
                        }

                        #endregion
                    }
                    at = ContentTagManager.GetXMLAtt("ENTITY_SPRITE_OVERRIDE", version, entityComponent);
                    if (at != null)
                    {
                        #region Override

                        entity.Sprite.OverrideAttributes = Utils.ParseEnum<SGDE.Graphics.Sprite.SpriteAttributes>(at.Value, SGDE.Graphics.Sprite.SpriteAttributes.None, context.Logger);

                        //Handle specific cases
                        if (((entity.Sprite.OverrideAttributes & SGDE.Graphics.Sprite.SpriteAttributes.RotationAbs) == SGDE.Graphics.Sprite.SpriteAttributes.RotationAbs) &&
                            ((entity.Sprite.OverrideAttributes & SGDE.Graphics.Sprite.SpriteAttributes.RotationRel) == SGDE.Graphics.Sprite.SpriteAttributes.RotationRel))
                        {
                            context.Logger.LogWarning(null, null, Messages.Entity_Sprite_RelAbsRotation);
                            entity.Sprite.OverrideAttributes &= ~SGDE.Graphics.Sprite.SpriteAttributes.RotationRel;
                        }
                        if (((entity.Sprite.OverrideAttributes & SGDE.Graphics.Sprite.SpriteAttributes.ScaleAbs) == SGDE.Graphics.Sprite.SpriteAttributes.ScaleAbs) &&
                            ((entity.Sprite.OverrideAttributes & SGDE.Graphics.Sprite.SpriteAttributes.ScaleRel) == SGDE.Graphics.Sprite.SpriteAttributes.ScaleRel))
                        {
                            context.Logger.LogWarning(null, null, Messages.Entity_Sprite_RelAbsScale);
                            entity.Sprite.OverrideAttributes &= ~SGDE.Graphics.Sprite.SpriteAttributes.ScaleRel;
                        }

                        if (entity.Sprite.OverrideAttributes != SGDE.Graphics.Sprite.SpriteAttributes.None)
                        {
                            entity.Sprite.HasOverride = true;
                        }

                        #endregion
                    }
                    int count = 0;
                    int aid = -1;
                    foreach (XmlElement spriteComponent in entityComponent)
                    {
                        #region Child components

                        if (ContentTagManager.TagMatches("SPRITESHEET_MAP_COMP_ANIMATION", spriteComponent.Name, version))
                        {
                            #region Animation

                            if (++count >= 2)
                            {
                                context.Logger.LogWarning(null, null, Messages.Entity_Sprite_TooManyAnimations);
                                continue;
                            }

                            if (entity.Sprite.Animations == null)
                            {
                                //Only one animation is supported at this time but use a list for future support.
                                entity.Sprite.Animations = new List<AnimationSet>();
                            }
                            Animation animation = AnimationProcessor.Process(spriteComponent, version, context);
                            if (entity.Sprite.BuiltInAnimation = animation.BuiltIn)
                            {
                                foreach (AnimationSet set in animation.Sets)
                                {
                                    if (set.Default)
                                    {
                                        if (aid == -1)
                                        {
                                            aid = set.Index;
                                        }
                                        else
                                        {
                                            context.Logger.LogWarning(null, null, Messages.Entity_Sprite_DefaultTooMany);
                                        }
                                    }
                                    entity.Sprite.Animations.Add(set);
                                }
                                entity.Sprite.AnimationLocal = true;
                            }
                            else
                            {
                                aid = animation.ID;
                            }

                            #endregion
                        }
                        else
                        {
                            ProcessSprite(ref entity.Sprite, spriteComponent, version);
                        }

                        #endregion
                    }
                    if (!SpriteRequirementsMet(entity.Sprite, version))
                    {
                        SpecifySpriteRequirements(entity.Sprite, version, context);
                    }
                    at = ContentTagManager.GetXMLAtt("ENTITY_SPRITE_ANIMATION_ID", version, entityComponent);
                    if (at == null)
                    {
                        if (entity.Sprite.AnimationID < 0)
                        {
                            if (MapProcessor.CurrentEntityID >= 0 && MapProcessor.map != null)
                            {
                                //Inheret the Animation ID
                                if (MapProcessor.map.Entities[MapProcessor.CurrentEntityID] is Entity)
                                {
                                    aid = ((Entity)MapProcessor.map.Entities[MapProcessor.CurrentEntityID]).Sprite.AnimationID;
                                }
                            }
                        }
                    }
                    else
                    {
                        aid = int.Parse(at.Value);
                        if (aid < 0)
                        {
                            throw new InvalidContentException(Messages.Entity_Sprite_PositiveAID);
                        }
                        aid--; //Adjust for writing
                    }
                    entity.Sprite.AnimationID = aid;
                    at = ContentTagManager.GetXMLAtt("ENTITY_SPRITE_ANIMATION_LOCALAID", version, entityComponent);
                    if (at != null)
                    {
                        entity.Sprite.AnimationLocal = bool.Parse(at.Value);
                    }

                    #endregion
                }
                else if (ContentTagManager.TagMatches("ENTITY_PHYSICS", entityComponent.Name, version))
                {
                    #region Physics

                    entity.Physics = new Dictionary<string, object>();
                    at = ContentTagManager.GetXMLAtt("ENTITY_PHYSICS_ENABLED", version, entityComponent);
                    if (at != null)
                    {
                        entity.Physics.Add("Enabled", bool.Parse(at.Value));
                    }
                    at = ContentTagManager.GetXMLAtt("ENTITY_PHYSICS_ENABLEDONENABLED", version, entityComponent);
                    if (at != null)
                    {
                        entity.Physics.Add("EnableOnEnable", bool.Parse(at.Value));
                    }
                    at = ContentTagManager.GetXMLAtt("ENTITY_PHYSICS_COLLISION", version, entityComponent);
                    if (at != null)
                    {
                        entity.Physics.Add("Collision", bool.Parse(at.Value));
                    }
                    at = ContentTagManager.GetXMLAtt("ENTITY_PHYSICS_STATIC", version, entityComponent);
                    if (at != null)
                    {
                        entity.Physics.Add("Static", bool.Parse(at.Value));
                    }
                    at = ContentTagManager.GetXMLAtt("ENTITY_PHYSICS_POSTSETUP", version, entityComponent);
                    if (at != null)
                    {
                        entity.Physics.Add("PostSetup", bool.Parse(at.Value));
                    }
                    foreach (XmlElement physicsElement in entityComponent)
                    {
                        if (ContentTagManager.TagMatches("ENTITY_PHYSICS_VELOCITY", physicsElement.Name, version))
                        {
                            #region Velocity

                            Vector2 vect = new Vector2();
                            object[] vecto = null;
                            at = ContentTagManager.GetXMLAtt("GENERAL_X", version, physicsElement);
                            if (at != null)
                            {
                                try
                                {
                                    vect.X = float.Parse(at.Value);
                                }
                                catch
                                {
                                    vecto = new object[2];
                                    vecto[0] = CodeProcessor.Process(at, version, context);
                                }
                            }
                            at = ContentTagManager.GetXMLAtt("GENERAL_Y", version, physicsElement);
                            if (at != null)
                            {
                                if (vecto != null && vecto.Length == 2)
                                {
                                    vecto[1] = CodeProcessor.Process(at, version, context);
                                }
                                else
                                {
                                    try
                                    {
                                        vect.Y = float.Parse(at.Value);
                                        vecto = new object[] { vect };
                                    }
                                    catch
                                    {
                                        if (vecto == null)
                                        {
                                            vecto = new object[2];
                                            SGDeContent.DataTypes.Code.Code code = new DataTypes.Code.Code();
                                            code.Constant = true;
                                            code.ConstantValue = vect.X;
                                            vecto[0] = code;
                                        }
                                        vecto[1] = CodeProcessor.Process(at, version, context);
                                    }
                                }
                            }
                            else if (vecto != null && vecto.Length == 2)
                            {
                                SGDeContent.DataTypes.Code.Code code = new DataTypes.Code.Code();
                                code.Constant = true;
                                code.ConstantValue = 0f;
                                vecto[1] = code;
                            }

                            switch (vecto.Length)
                            {
                                case 1:
                                    vect = (Vector2)vecto[0];
                                    if (vect.X == 0 && vect.Y == 0)
                                    {
                                        vecto = null;
                                    }
                                    break;
                                case 2:
                                    SGDeContent.DataTypes.Code.Code code = (SGDeContent.DataTypes.Code.Code)vecto[0];
                                    if (code.Constant && code.ConstantValue.Equals(0))
                                    {
                                        code = (SGDeContent.DataTypes.Code.Code)vecto[1];
                                        if (code.Constant && code.ConstantValue.Equals(0))
                                        {
                                            vecto = null;
                                        }
                                    }
                                    break;
                            }
                            if (vecto != null)
                            {
                                entity.Physics.Add("Velocity", vecto);
                            }

                            #endregion
                        }
                        else if (ContentTagManager.TagMatches("ENTITY_PHYSICS_FORCES", physicsElement.Name, version))
                        {
                            #region Forces

                            List<object> pforce = new List<object>();
                            foreach (XmlElement force in physicsElement)
                            {
                                bool add = false;
                                string innerText = SGDEProcessor.GetInnerText(force);
                                if (string.IsNullOrWhiteSpace(innerText))
                                {
                                    #region Attempt at simple processing

                                    Vector2 vect = new Vector2();
                                    at = ContentTagManager.GetXMLAtt("GENERAL_X", version, force);
                                    if (at != null)
                                    {
                                        try
                                        {
                                            vect.X = float.Parse(at.Value);
                                        }
                                        catch
                                        {
                                            add = true;
                                        }
                                    }
                                    at = ContentTagManager.GetXMLAtt("GENERAL_Y", version, force);
                                    if (at != null && !add)
                                    {
                                        try
                                        {
                                            vect.Y = float.Parse(at.Value);
                                        }
                                        catch
                                        {
                                            add = true;
                                        }
                                    }
                                    if (!add)
                                    {
                                        pforce.Add(vect);
                                    }

                                    #endregion
                                }
                                else
                                {
                                    add = true;
                                }
                                if (add)
                                {
                                    #region Code processing

                                    if (string.IsNullOrWhiteSpace(innerText))
                                    {
                                        object[] obj = new object[2];
                                        at = ContentTagManager.GetXMLAtt("GENERAL_X", version, force);
                                        if (at != null)
                                        {
                                            obj[0] = CodeProcessor.Process(at, version, context);
                                        }
                                        else
                                        {
                                            SGDeContent.DataTypes.Code.Code code = new DataTypes.Code.Code();
                                            code.Constant = true;
                                            code.ConstantValue = 0f;
                                            obj[0] = code;
                                        }
                                        at = ContentTagManager.GetXMLAtt("GENERAL_Y", version, force);
                                        if (at != null)
                                        {
                                            obj[1] = CodeProcessor.Process(at, version, context);
                                        }
                                        else
                                        {
                                            SGDeContent.DataTypes.Code.Code code = new DataTypes.Code.Code();
                                            code.Constant = true;
                                            code.ConstantValue = 0f;
                                            obj[1] = code;
                                        }
                                        pforce.Add(obj);
                                    }
                                    else
                                    {
                                        pforce.Add(CodeProcessor.Process(force, version, context));
                                    }

                                    #endregion
                                }
                            }
                            if (pforce.Count != 0)
                            {
                                entity.Physics.Add("Forces", pforce);
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
                else if (ContentTagManager.TagMatches("ENTITY_CUSTOMENTITY", entityComponent.Name, version))
                {
                    #region Custom Entity

                    Type type = Type.GetType(ContentTagManager.GetXMLAtt("ENTITY_CUSTOMENTITY_BASE", version, entityComponent).Value);
                    if (type != null)
                    {
                        Type entityType = typeof(SGDE.Entity);
                        if (!type.Equals(entityType) && !type.IsSubclassOf(entityType))
                        {
                            context.Logger.LogWarning(null, null, Messages.Entity_Custom_NotBasedOffEntity);
                            continue;
                        }
                    }
                    else
                    {
                        context.Logger.LogWarning(null, null, Messages.Entity_Custom_CantFindType);
                    }
                    entity.SpecialType = SGDEProcessor.GetInnerText(entityComponent).Trim();
                    type = Type.GetType(entity.SpecialType);
                    if (type != null)
                    {
                        entity.SpecialType = type.AssemblyQualifiedName;
                    }
                    foreach (XmlNode customEntityComponent in entityComponent)
                    {
                        if (!(customEntityComponent is XmlElement))
                        {
                            continue;
                        }

                        if (ContentTagManager.TagMatches("ENTITY_CUSTOMENTITY_CONSTRUCTOR", customEntityComponent.Name, version))
                        {
                            #region Contructor

                            //Get index values
                            List<int> indexes = new List<int>();
                            for (int i = 0; i < customEntityComponent.ChildNodes.Count; i++)
                            {
                                int index = int.Parse(ContentTagManager.GetXMLAtt("ENTITY_CUSTOMENTITY_CONSTRUCTOR_INDEX", version, customEntityComponent.ChildNodes[i]).Value);
                                if (index < 0)
                                {
                                    throw new InvalidContentException(Messages.Entity_NeedPosInt);
                                }
                                if (indexes.Contains(index))
                                {
                                    throw new InvalidContentException(string.Format(Messages.Entity_IndexNeeded, index));
                                }
                                indexes.Add(index);
                            }
                            int[] temp = indexes.ToArray();
                            Array.Sort(temp);
                            //Determine number of arguments
                            int tcount = Math.Max(temp[temp.Length - 1] + 1, customEntityComponent.ChildNodes.Count);
                            entity.Args = new List<object>(tcount);
                            entity.ArgTypes = new Queue<string>(tcount);
                            for (int i = 0; i < customEntityComponent.ChildNodes.Count; i++)
                            {
                                string typeS = ContentTagManager.GetXMLAtt("ENTITY_CUSTOMENTITY_CONSTRUCTOR_TYPE", version, customEntityComponent.ChildNodes[i]).Value;
                                type = Type.GetType(typeS);
                                string value = ContentTagManager.GetXMLAtt("ENTITY_CUSTOMENTITY_CONSTRUCTOR_VALUE", version, customEntityComponent.ChildNodes[i]).Value.Trim();
                                if (typeof(string).Equals(type))
                                {
                                    //The type is a String so simply pass the value.
                                    entity.Args.Add(value);
                                }
                                else
                                {
                                    if (type != null)
                                    {
                                        System.Reflection.MethodInfo parse = type.GetMethod("Parse", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, Type.DefaultBinder, new Type[] { typeof(string) }, null);
                                        try
                                        {
                                            entity.Args.Add(parse.Invoke(null, new object[] { value }));
                                        }
                                        catch
                                        {
                                            SGDeContent.DataTypes.Code.Code code = SGDeContent.Processors.CodeProcessor.Process(ContentTagManager.GetXMLAtt("ENTITY_CUSTOMENTITY_CONSTRUCTOR_VALUE", version, customEntityComponent.ChildNodes[i]), version, context);
                                            if (code == null)
                                            {
                                                try
                                                {
                                                    entity.Args.Add(Activator.CreateInstance(type));
                                                }
                                                catch
                                                {
                                                    entity.Args.Add(null);
                                                }
                                            }
                                            else
                                            {
                                                entity.Args.Add(code);
                                            }
                                            entity.ArgTypes.Enqueue(type.AssemblyQualifiedName);
                                        }
                                    }
                                    else
                                    {
                                        /* CodeProcessor takes care of this
                                        if (value.Equals("null", StringComparison.OrdinalIgnoreCase))
                                        {
                                            entity.Args.Add(null);
                                        }
                                         */
                                        entity.Args.Add(SGDeContent.Processors.CodeProcessor.Process(customEntityComponent.ChildNodes[i].Attributes["Value"], version, context));
                                        entity.ArgTypes.Enqueue(typeof(void).AssemblyQualifiedName);
                                    }
                                }
                            }
                            //Now go through and make sure the arguments are in order, if not then rearrange them
                            if (!temp.SequenceEqual(indexes.ToArray()) || tcount != indexes.Count)
                            {
                                #region Rearrange arguments

                                //Not in same order, rearrange
                                //-First copy the elements (in the correct order)
                                List<object> objs = new List<object>();
                                List<string> types = new List<string>();
                                List<string> ttypes = new List<string>();
                                //--Get the types in a list
                                for (int i = 0; i < entity.Args.Count; i++)
                                {
                                    object obj = entity.Args[i];
                                    if (obj == null || obj is SGDeContent.DataTypes.Code.Code)
                                    {
                                        ttypes.Add(entity.ArgTypes.Dequeue());
                                    }
                                    else
                                    {
                                        ttypes.Add(null);
                                    }
                                }
                                //--Get the elements and rearrange the types
                                for (int i = 0; i < tcount; i++)
                                {
                                    int index = indexes.IndexOf(i);
                                    if (index < 0)
                                    {
                                        //We'll get back to this
                                        continue;
                                    }
                                    object obj = entity.Args[index];
                                    objs.Add(obj);
                                    if (obj == null || obj is SGDeContent.DataTypes.Code.Code)
                                    {
                                        types.Add(ttypes[index]);
                                    }
                                }
                                if (entity.ArgTypes.Count > 0)
                                {
                                    throw new InvalidOperationException(Messages.Entity_ArgsNotEmpty);
                                }
                                entity.Args.Clear();
                                //-Now that the elements have been copied in the correct order, add missing types
                                for (int i = 0, v = 0, t = 0; i < tcount; i++)
                                {
                                    int index = indexes.IndexOf(i);
                                    if (index < 0)
                                    {
                                        entity.Args.Add(null);
                                        entity.ArgTypes.Enqueue(typeof(void).AssemblyQualifiedName);
                                    }
                                    else
                                    {
                                        object obj = objs[v++];
                                        entity.Args.Add(obj);
                                        if (obj == null || obj is SGDeContent.DataTypes.Code.Code)
                                        {
                                            entity.ArgTypes.Enqueue(types[t++]);
                                        }
                                    }
                                }

                                #endregion
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
            }
            return entity;
        }

        private static Sprite CreateSprite(int id)
        {
            switch (SpriteSheetProcessor.SpriteSheetTypes[id])
            {
                case SpriteSheetProcessor.SpriteType.Bitmap:
                    BitmapSprite bs = new BitmapSprite();
                    bs.SpriteID = id;
                    bs.Visible = true;
                    return bs;
            }
            return null;
        }

        private static Sprite CopySprite(Sprite src)
        {
            if (src is BitmapSprite)
            {
                BitmapSprite dstB = new BitmapSprite();
                dstB.SpriteID = ((BitmapSprite)src).SpriteID;
                dstB.Visible = src.Visible;
                return dstB;
            }
            return null;
        }

        private static void ProcessSprite(ref Sprite sp, XmlElement spriteComponent, double version)
        {
            //TODO: Extra components to a Sprite: If a vector based Sprite, get the scale of the sprite
        }

        private static bool SpriteRequirementsMet(Sprite sp, double version)
        {
            //TODO: If a vector based Sprite, make sure it has a scale
            return true;
        }

        private static void SpecifySpriteRequirements(Sprite sp, double version, ContentProcessorContext context)
        {
            //If vector based Sprite, print out that a scale is required and default to a scale of 1 for 1
        }
    }
}

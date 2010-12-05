using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDeContent.DataTypes;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using Microsoft.Xna.Framework;

namespace SGDeContent.Processors
{
    public class EntityProcessor
    {
        public static Entity Process(Content input, ContentProcessorContext context)
        {
            return Process(input.document.DocumentElement["Entity"], context);
        }

        public static Entity Process(XmlElement input, ContentProcessorContext context)
        {
            Entity entity = new Entity();
            XmlAttribute at = input.Attributes["Name"];
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
                if (entityComponent.Name.Equals("Node"))
                {
                    #region Node

                    Node node = new Node();
                    bool nonDefault = false;

                    at = entityComponent.Attributes["DID"];
                    if (at != null)
                    {
                        nonDefault = true;
                        entity.DevID(at, node);
                    }
                    bool gotScale = false;
                    foreach (XmlElement nodeComponent in entityComponent)
                    {
                        if (nodeComponent.Name.Equals("Vector2"))
                        {
                            #region Vector2

                            string type = nodeComponent.Attributes["ID"].Value;
                            at = nodeComponent.Attributes["X"];
                            if (at != null)
                            {
                                object val;
                                try
                                {
                                    val = float.Parse(at.Value);
                                }
                                catch
                                {
                                    val = CodeProcessor.Process(at, context);
                                }
                                switch (type)
                                {
                                    case "Translation":
                                        if (val is float)
                                        {
                                            node.Translation.X = (float)val;
                                        }
                                        else
                                        {
                                            node.tx = (SGDeContent.DataTypes.Code.Code)val;
                                        }
                                        break;
                                    case "Scale":
                                        if (val is float)
                                        {
                                            node.Scale.X = (float)val;
                                        }
                                        else
                                        {
                                            node.sx = (SGDeContent.DataTypes.Code.Code)val;
                                        }
                                        break;
                                }
                            }
                            at = nodeComponent.Attributes["Y"];
                            if (at != null)
                            {
                                object val;
                                try
                                {
                                    val = float.Parse(at.Value);
                                }
                                catch
                                {
                                    val = CodeProcessor.Process(at, context);
                                }
                                switch (type)
                                {
                                    case "Translation":
                                        if (val is float)
                                        {
                                            node.Translation.Y = (float)val;
                                        }
                                        else
                                        {
                                            node.ty = (SGDeContent.DataTypes.Code.Code)val;
                                        }
                                        break;
                                    case "Scale":
                                        if (val is float)
                                        {
                                            node.Scale.Y = (float)val;
                                        }
                                        else
                                        {
                                            node.sy = (SGDeContent.DataTypes.Code.Code)val;
                                        }
                                        break;
                                }
                            }
                            switch (type)
                            {
                                case "Translation":
                                    if (node.Translation != Vector2.Zero || (node.tx != null || node.ty != null))
                                    {
                                        nonDefault = true;
                                    }
                                    break;
                                case "Scale":
                                    gotScale = true;
                                    if (node.Scale != Vector2.One || (node.sx != null || node.sy != null))
                                    {
                                        nonDefault = true;
                                    }
                                    break;
                            }

                            #endregion
                        }
                        else if (nodeComponent.Name.Equals("Float"))
                        {
                            #region Float

                            float val = 0;

                            at = nodeComponent.Attributes["Value"];
                            if (at != null)
                            {
                                try
                                {
                                    val = float.Parse(at.Value);
                                }
                                catch
                                {
                                    val = Convert.ToSingle(CodeProcessor.Process(at, context).Constant);
                                }
                            }
                            at = nodeComponent.Attributes["ID"];
                            if (at.Value.Equals("Rotation"))
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
                else if (entityComponent.Name.Equals("Sprite"))
                {
                    #region Sprite

                    at = entityComponent.Attributes["SID"];
                    if (at == null)
                    {
                        entity.SpriteID = -1;
                        if (MapProcessor.CurrentEntityID >= 0 && MapProcessor.map != null)
                        {
                            //Inheret the Sprite ID
                            if (MapProcessor.map.Entities[MapProcessor.CurrentEntityID] is Entity)
                            {
                                entity.SpriteID = ((Entity)MapProcessor.map.Entities[MapProcessor.CurrentEntityID]).SpriteID;
                            }
                        }
                        if (entity.SpriteID == -1)
                        {
                            throw new InvalidContentException(Messages.Entity_Sprite_RequiresID);
                        }
                    }
                    else
                    {
                        entity.SpriteID = int.Parse(at.Value);
                        if (entity.SpriteID < 0)
                        {
                            throw new InvalidContentException(Messages.Entity_Sprite_PositiveID);
                        }
                    }
                    at = entityComponent.Attributes["DID"];
                    if (at != null)
                    {
                        entity.SpriteDID = new object();
                        entity.DevID(at, entity.SpriteDID);
                    }
                    at = entityComponent.Attributes["Region"];
                    if (at != null)
                    {
                        #region Region

                        string value = at.Value;
                        if (!value.Equals("Full", StringComparison.OrdinalIgnoreCase))
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
                                    entity.HasRegion = true;
                                    switch (values.Length)
                                    {
                                        case 1:
                                            entity.RegionBegin = entity.RegionEnd = -1;
                                            if (value.IndexOf('-') > value.IndexOf(components[0]))
                                            {
                                                //Region defines starting portion of region
                                                entity.RegionBegin = values[0];
                                            }
                                            else
                                            {
                                                //Region defines ending portion of region
                                                entity.RegionEnd = values[0];
                                            }
                                            break;
                                        case 2:
                                            entity.RegionBegin = values[0];
                                            entity.RegionEnd = values[1];
                                            break;
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    at = entityComponent.Attributes["Color"];
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
                            entity.Tint = col;
                        }
                        catch
                        {
                            context.Logger.LogWarning(null, null, Messages.Animation_InvalidColor, at.Value);
                        }

                        #endregion
                    }
                    at = entityComponent.Attributes["Override"];
                    if (at != null)
                    {
                        #region Override

                        string[] values = at.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        entity.OverrideAttributes = SGDE.Graphics.Sprite.SpriteAttributes.None;
                        SGDE.Graphics.Sprite.SpriteAttributes temp;
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (!Enum.TryParse<SGDE.Graphics.Sprite.SpriteAttributes>(values[i].Trim(), out temp))
                            {
                                context.Logger.LogWarning(null, null, Messages.Entity_Sprite_InvalidSpriteOverride, values[i].Trim());
                            }
                            else
                            {
                                entity.OverrideAttributes |= temp;
                            }
                        }

                        //Handle specific cases
                        if (((entity.OverrideAttributes & SGDE.Graphics.Sprite.SpriteAttributes.RotationAbs) == SGDE.Graphics.Sprite.SpriteAttributes.RotationAbs) &&
                            ((entity.OverrideAttributes & SGDE.Graphics.Sprite.SpriteAttributes.RotationRel) == SGDE.Graphics.Sprite.SpriteAttributes.RotationRel))
                        {
                            context.Logger.LogWarning(null, null, Messages.Entity_Sprite_RelAbsRotation);
                            entity.OverrideAttributes &= ~SGDE.Graphics.Sprite.SpriteAttributes.RotationRel;
                        }
                        if (((entity.OverrideAttributes & SGDE.Graphics.Sprite.SpriteAttributes.ScaleAbs) == SGDE.Graphics.Sprite.SpriteAttributes.ScaleAbs) &&
                            ((entity.OverrideAttributes & SGDE.Graphics.Sprite.SpriteAttributes.ScaleRel) == SGDE.Graphics.Sprite.SpriteAttributes.ScaleRel))
                        {
                            context.Logger.LogWarning(null, null, Messages.Entity_Sprite_RelAbsScale);
                            entity.OverrideAttributes &= ~SGDE.Graphics.Sprite.SpriteAttributes.ScaleRel;
                        }

                        if (entity.OverrideAttributes != SGDE.Graphics.Sprite.SpriteAttributes.None)
                        {
                            entity.HasOverride = true;
                        }

                        #endregion
                    }
                    int count = 0;
                    foreach (XmlElement spriteComponent in entityComponent)
                    {
                        #region Child components

                        if (count >= 1)
                        {
                            context.Logger.LogWarning(null, null, Messages.Entity_Sprite_TooManyAnimations);
                            break;
                        }
                        if (spriteComponent.Name.Equals("Animation"))
                        {
                            count++;

                            #region Animation

                            if (entity.Animations == null)
                            {
                                //Only one animation is supported at this time but use a list for future support.
                                entity.Animations = new List<AnimationSet>();
                            }
                            Animation animation = AnimationProcessor.Process(spriteComponent, context);
                            entity.DefaultAnimation = -1;
                            if (entity.BuiltInAnimation = animation.BuiltIn)
                            {
                                foreach (AnimationSet set in animation.Sets)
                                {
                                    set.SpriteID = entity.SpriteID;
                                    if (set.Default)
                                    {
                                        if (entity.DefaultAnimation == -1)
                                        {
                                            entity.DefaultAnimation = set.Index;
                                        }
                                        else
                                        {
                                            context.Logger.LogWarning(null, null, Messages.Entity_Sprite_DefaultTooMany);
                                        }
                                    }
                                    entity.Animations.Add(set);
                                }
                            }
                            else
                            {
                                entity.DefaultAnimation = animation.ID;
                            }

                            #endregion
                        }

                        #endregion
                    }

                    #endregion
                }
                else if (entityComponent.Name.Equals("Physics"))
                {
                    #region Physics

                    entity.Physics = new Dictionary<string, object>();
                    at = entityComponent.Attributes["Enabled"];
                    if (at != null)
                    {
                        entity.Physics.Add("Enabled", bool.Parse(at.Value));
                    }
                    at = entityComponent.Attributes["EnableOnEnable"];
                    if (at != null)
                    {
                        entity.Physics.Add("EnableOnEnable", bool.Parse(at.Value));
                    }
                    at = entityComponent.Attributes["Collision"];
                    if (at != null)
                    {
                        entity.Physics.Add("Collision", bool.Parse(at.Value));
                    }
                    at = entityComponent.Attributes["Static"];
                    if (at != null)
                    {
                        entity.Physics.Add("Static", bool.Parse(at.Value));
                    }
                    at = entityComponent.Attributes["PostSetup"];
                    if (at != null)
                    {
                        entity.Physics.Add("PostSetup", bool.Parse(at.Value));
                    }
                    foreach (XmlElement physicsElement in entityComponent)
                    {
                        if (physicsElement.Name.Equals("Velocity"))
                        {
                            #region Velocity

                            Vector2 vect = new Vector2();
                            object[] vecto = null;
                            at = physicsElement.Attributes["X"];
                            if (at != null)
                            {
                                try
                                {
                                    vect.X = float.Parse(at.Value);
                                }
                                catch
                                {
                                    vecto = new object[2];
                                    vecto[0] = CodeProcessor.Process(at, context);
                                }
                            }
                            at = physicsElement.Attributes["Y"];
                            if (at != null)
                            {
                                if (vecto != null && vecto.Length == 2)
                                {
                                    vecto[1] = CodeProcessor.Process(at, context);
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
                                        vecto[1] = CodeProcessor.Process(at, context);
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
                        else if (physicsElement.Name.Equals("Forces"))
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
                                    at = force.Attributes["X"];
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
                                    at = force.Attributes["Y"];
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
                                        at = force.Attributes["X"];
                                        if (at != null)
                                        {
                                            obj[0] = CodeProcessor.Process(at, context);
                                        }
                                        else
                                        {
                                            SGDeContent.DataTypes.Code.Code code = new DataTypes.Code.Code();
                                            code.Constant = true;
                                            code.ConstantValue = 0f;
                                            obj[0] = code;
                                        }
                                        at = force.Attributes["Y"];
                                        if (at != null)
                                        {
                                            obj[1] = CodeProcessor.Process(at, context);
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
                                        pforce.Add(CodeProcessor.Process(force, context));
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
                else if (entityComponent.Name.Equals("CustomEntity"))
                {
                    #region Custom Entity

                    Type type = Type.GetType(entityComponent.Attributes["Base"].Value);
                    if (!type.IsSubclassOf(typeof(SGDE.Entity)))
                    {
                        context.Logger.LogWarning(null, null, Messages.Entity_Custom_NotBasedOffEntity);
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

                        #region Contructor

                        if (customEntityComponent.Name.Equals("Contructor"))
                        {
                            entity.Args = new List<object>();
                            entity.ArgTypes = new Queue<string>();
                            for (int i = 0; i < customEntityComponent.ChildNodes.Count; i++)
                            {
                                int index = int.Parse(customEntityComponent.ChildNodes[i].Attributes["Index"].Value);
                                string typeS = customEntityComponent.ChildNodes[i].Attributes["Type"].Value;
                                type = Type.GetType(typeS);
                                string value = customEntityComponent.ChildNodes[i].Attributes["Value"].Value.Trim();
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
                                            SGDeContent.DataTypes.Code.Code code = SGDeContent.Processors.CodeProcessor.Process(customEntityComponent.ChildNodes[i].Attributes["Value"], context);
                                            if (code == null)
                                            {
                                                entity.Args.Add(Activator.CreateInstance(type));
                                            }
                                            else
                                            {
                                                entity.Args.Add(code);
                                                entity.ArgTypes.Enqueue(type.AssemblyQualifiedName);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        /*
                                        if (value.Equals("null", StringComparison.OrdinalIgnoreCase))
                                        {
                                            entity.Args.Add(null);
                                        }
                                         */
                                        entity.Args.Add(SGDeContent.Processors.CodeProcessor.Process(customEntityComponent.ChildNodes[i].Attributes["Value"], context));
                                    }
                                }
                            }
                        }

                        #endregion
                    }

                    #endregion
                }
            }
            return entity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SGDeContent.DataTypes;

namespace SGDeContent.Writers
{
    [ContentTypeWriter]
    public class EntityWriter : ContentTypeWriter<Entity>
    {
        protected override void Write(ContentWriter output, Entity value)
        {
            //Write out data type (used to create new instance)
            if (!string.IsNullOrWhiteSpace(value.SpecialType))
            {
                output.Write(true);
                output.Write(value.SpecialType);
                if (value.Args != null)
                {
                    output.Write(value.Args.Count);
                    for (int i = 0; i < value.Args.Count; i++)
                    {
                        object obj = value.Args[i];
                        output.WriteObject(obj);
                        if (obj is SGDE.Content.Code.Code)
                        {
                            output.Write(value.ArgTypes.Dequeue());
                        }
                        if (obj == null && value.ArgTypes.Count > 0)
                        {
                            if (Type.GetType(value.ArgTypes.Peek()).Equals(typeof(void)))
                            {
                                output.Write(false);
                                value.ArgTypes.Dequeue();
                            }
                            else
                            {
                                output.Write(true);
                                output.Write(value.ArgTypes.Dequeue());
                            }
                        }
                    }
                }
                else
                {
                    output.Write(-1);
                }
            }
            else
            {
                output.Write(false);
            }
            //Write out Entity values
            //-Write out sprite information
            output.Write(value.SpriteID);
            value.DeveloperIDWriter(output, value.SpriteDID); //Write it after so that a valid texture can be loaded (or exception thrown) before being added to developer ID content.
            output.WriteObject(value.Tint);
            output.Write(value.HasOverride);
            if (value.HasOverride)
            {
                output.WriteObject(value.OverrideAttributes);
            }
            output.Write(value.BuiltInAnimation);
            output.Write(value.AnimationLocal);
            output.Write(value.AnimationID + 1);
            if (value.Animations == null)
            {
                output.Write(false);
            }
            else
            {
                output.Write(true);
                output.Write(value.Animations.Count);
                foreach (AnimationSet ani in value.Animations)
                {
                    output.WriteRawObject(ani);
                }
            }
            output.Write(value.HasRegion);
            if (value.HasRegion)
            {
                output.Write(value.RegionBegin);
                output.Write(value.RegionEnd);
            }
            //-Write out physics values
            output.WriteObject(value.Physics);
            //Write standard node values
            output.Write(value.NonDefaultNode);
            if (value.NonDefaultNode)
            {
                output.WriteRawObject(value.Node);
                value.DeveloperIDWriter(output, value.Node);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.EntityReader).AssemblyQualifiedName;
        }
    }
}

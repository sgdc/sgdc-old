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
            output.WriteObject(value.Sprite);
            value.DeveloperIDWriter(output, value.Sprite); //Write it after so that a valid texture can be loaded (or exception thrown) before being added to developer ID content.
            //-Write out physics values
            output.WriteObject(value.Physics);
            //Write standard node values
            output.Write(true); //TODO: If the entity is enabled or not
            output.Write(value.NonDefaultNode);
            if (value.NonDefaultNode)
            {
                output.WriteRawObject(value.Node);
                value.DeveloperIDWriter(output, value.Node);
            }
        }

        public override int TypeVersion
        {
            get
            {
                return 1;
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.EntityReader).AssemblyQualifiedName;
        }
    }
}

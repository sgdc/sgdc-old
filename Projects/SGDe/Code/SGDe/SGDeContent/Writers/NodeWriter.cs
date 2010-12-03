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
    public class NodeWriter : ContentTypeWriter<Node>
    {
        protected override void Write(ContentWriter output, Node value)
        {
            if (value.tx == null)
            {
                output.Write(true);
                output.Write(value.Translation);
            }
            else
            {
                output.Write(false);
                output.WriteRawObject(value.tx);
                output.WriteRawObject(value.ty);
            }
            output.Write(value.Rotation);
            if (value.sx == null)
            {
                output.Write(true);
                output.Write(value.Scale);
            }
            else
            {
                output.Write(false);
                output.WriteRawObject(value.sx);
                output.WriteRawObject(value.sy);
            }
            output.Write(value.Children.Count);
            foreach (Node child in value.Children)
            {
                output.WriteSharedResource(child);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.NodeReader).AssemblyQualifiedName;
        }
    }
}

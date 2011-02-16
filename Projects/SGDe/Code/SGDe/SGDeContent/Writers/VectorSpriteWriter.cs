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
using SGDeContent.DataTypes.Code;
using SGDeContent.DataTypes.Sprites;

namespace SGDeContent.Writers
{
    [ContentTypeWriter]
    public class VectorSpriteWriter : SpriteWriter<VectorRefSprite>
    {
        public override void WriteSpecific(ContentWriter output, VectorRefSprite value)
        {
            //TODO
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.VectorSpriteReader).AssemblyQualifiedName;
        }
    }
}

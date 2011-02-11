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
    public class BitmapSpriteWriter : SpriteWriter<BitmapSprite>
    {
        public override void WriteSpecific(ContentWriter output, BitmapSprite value)
        {
            output.Write(value.SpriteID);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.BitmapSpriteReader).AssemblyQualifiedName;
        }
    }
}

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
using SGDeContent.DataTypes.Sprites.SVG;

namespace SGDeContent.Writers
{
    [ContentTypeWriter]
    public class SVGWriter : ContentTypeWriter<SVGfx>
    {
        protected override void Write(ContentWriter output, SVGfx value)
        {
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.MapReader).AssemblyQualifiedName;
        }
    }
}

﻿using System;
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

namespace SGDeContent.Writers
{
    [ContentTypeWriter]
    public class CodeWriter : ContentTypeWriter<Code>
    {
        protected override void Write(ContentWriter output, Code value)
        {
            output.Write(value.Constant);
            if (value.Constant)
            {
                output.WriteObject(value.ConstantValue);
            }
            else
            {
                //TODO
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.CodeReader).AssemblyQualifiedName;
        }
    }
}

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

namespace SGDeContent.Writers
{
    [ContentTypeWriter]
    public class AnimationWriter : ContentTypeWriter<AnimationSet>
    {
        protected override void Write(ContentWriter output, AnimationSet value)
        {
            output.Write(value.SpriteID);
            output.Write(value.FPS);
            output.Write(value.Frames.Count);
            output.Write(value.Used);
            foreach (AnimationFrame frame in value.Frames)
            {
                output.Write(frame.Used);
                if ((frame.Used & SGDE.Content.Readers.AnimationReader.EffectUsed) == SGDE.Content.Readers.AnimationReader.EffectUsed)
                {
                    output.WriteObject(frame.Effect);
                }
                if ((frame.Used & SGDE.Content.Readers.AnimationReader.ColorUsed) == SGDE.Content.Readers.AnimationReader.ColorUsed)
                {
                    output.Write(frame.Color);
                }
                if ((frame.Used & SGDE.Content.Readers.AnimationReader.RegionUsed) == SGDE.Content.Readers.AnimationReader.RegionUsed)
                {
                    output.WriteObject(frame.Region);
                }
                if ((frame.Used & SGDE.Content.Readers.AnimationReader.RotationUsed) == SGDE.Content.Readers.AnimationReader.RotationUsed)
                {
                    output.Write(frame.Rotation);
                }
                if ((frame.Used & SGDE.Content.Readers.AnimationReader.OriginUsed) == SGDE.Content.Readers.AnimationReader.OriginUsed)
                {
                    output.Write(frame.Origin);
                }
                if ((frame.Used & SGDE.Content.Readers.AnimationReader.ScaleUsed) == SGDE.Content.Readers.AnimationReader.ScaleUsed)
                {
                    output.Write(frame.Scale);
                }
            }
            value.DeveloperIDWriter(output, value);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SGDE.Content.Readers.AnimationReader).AssemblyQualifiedName;
        }
    }
}

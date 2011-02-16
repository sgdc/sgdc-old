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
    public abstract class SpriteWriter<T> : ContentTypeWriter<T> where T : Sprite
    {
        protected override void Write(ContentWriter output, T value)
        {
            output.Write(value.Visible);
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
            output.Write(value.SpriteID);
            WriteSpecific(output, value);
        }

        public abstract void WriteSpecific(ContentWriter output, T value);
    }
}

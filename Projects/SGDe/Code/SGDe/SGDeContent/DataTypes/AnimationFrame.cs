using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SGDeContent.DataTypes
{
    public class AnimationFrame : ProcessedContent
    {
        public byte Used;
        public Microsoft.Xna.Framework.Graphics.SpriteEffects Effect;
        public Color Color;
        public Rectangle Region;
        public float Rotation;
        public Vector2 Origin, Scale;

        public AnimationFrame()
        {
            Scale = new Vector2(1, 1);
        }

        public AnimationFrame(AnimationFrame original)
        {
            this.Used = original.Used;
            this.Effect = original.Effect;
            this.Color = Color.FromNonPremultiplied(original.Color.ToVector4());
            this.Region = new Rectangle(original.Region.X, original.Region.Y, original.Region.Width, original.Region.Height);
            this.Rotation = original.Rotation;
            this.Origin = original.Origin;
            this.Scale = original.Scale;
        }
    }
}

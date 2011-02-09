using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE.Graphics;
using SGDE.Content.DataTypes;

namespace SGDE.Content.Readers
{
    /// <summary>
    /// Read and process a SpriteAnimation class
    /// </summary>
    internal class AnimationReader : ContentTypeReader<SpriteManager.SpriteAnimation>
    {
        public const byte EffectUsed = 0x1;
        public const byte ColorUsed = 0x2;
        public const byte RegionUsed = 0x4;
        public const byte RotationUsed = 0x8;
        public const byte OriginUsed = 0x10;
        public const byte ScaleUsed = 0x20;

        /// <summary>
        /// Read a SpriteAnimation.
        /// </summary>
        protected override SpriteManager.SpriteAnimation Read(ContentReader input, SpriteManager.SpriteAnimation existingInstance)
        {
            SpriteManager.SpriteAnimation animation = new SpriteManager.SpriteAnimation();
            animation.IsValid = true;
            animation.DefaultFPS = input.ReadSingle();
            animation.frameCount = input.ReadInt32();
            byte used = input.ReadByte();
            if ((used & EffectUsed) == EffectUsed)
            {
                animation.effects = new SpriteEffects[animation.frameCount];
            }
            if ((used & ColorUsed) == ColorUsed)
            {
                animation.colors = new Color?[animation.frameCount];
            }
            if ((used & RegionUsed) == RegionUsed)
            {
                animation.frames = new Rectangle?[animation.frameCount];
            }
            if ((used & RotationUsed) == RotationUsed)
            {
                animation.rotation = new float?[animation.frameCount];
            }
            if ((used & OriginUsed) == OriginUsed)
            {
                animation.origin = new Vector2?[animation.frameCount];
            }
            if ((used & ScaleUsed) == ScaleUsed)
            {
                animation.scale = new Vector2?[animation.frameCount];
            }
            for (int i = 0; i < animation.frameCount; i++)
            {
                used = input.ReadByte();
                if ((used & EffectUsed) == EffectUsed)
                {
                    animation.effects[i] = input.ReadObject<SpriteEffects>();
                }
                if ((used & ColorUsed) == ColorUsed)
                {
                    animation.colors[i] = input.ReadColor();
                }
                if ((used & RegionUsed) == RegionUsed)
                {
                    animation.frames[i] = input.ReadObject<Rectangle>();
                }
                if ((used & RotationUsed) == RotationUsed)
                {
                    animation.rotation[i] = input.ReadSingle();
                }
                if ((used & OriginUsed) == OriginUsed)
                {
                    animation.origin[i] = input.ReadVector2();
                }
                if ((used & ScaleUsed) == ScaleUsed)
                {
                    animation.scale[i] = input.ReadVector2();
                }
            }
            ContentUtil.TempDeveloperID(input, animation);
            return animation;
        }
    }
}

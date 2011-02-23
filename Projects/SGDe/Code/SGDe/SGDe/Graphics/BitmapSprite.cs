using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE.Graphics
{
    /// <summary>
    /// A bitmap based Sprite.
    /// </summary>
    internal class BitmapSprite : Sprite
    {
        internal Texture2D baseTexture;

        public override void Draw(GameTime gameTime)
        {
            //Get tint values
            Color? tint = animation.Tint(frame);
            //Get rotation
            float? rotationAn = animation.Rotation(frame);
            float rotation;
            if (rotationAn.HasValue)
            {
                if (OverrideAnimation(SpriteAttributes.RotationAbs, null))
                {
                    rotation = rotationAn.Value;
                }
                else
                {
                    float trot = MathHelper.ToRadians(base.GetRotation());
                    if (OverrideAnimation(SpriteAttributes.RotationRel, null))
                    {
                        rotation = rotationAn.Value + trot;
                    }
                    else
                    {
                        rotation = trot;
                    }
                }
            }
            else
            {
                rotation = MathHelper.ToRadians(base.GetRotation());
            }
            //Get origin
            Vector2? origin = animation.Origin(frame);
            //Get scale
            Vector2? scaleAn = animation.Scale(frame);
            Vector2 scale;
            if (scaleAn.HasValue)
            {
                if (OverrideAnimation(SpriteAttributes.ScaleAbs, null))
                {
                    scale = scaleAn.Value;
                }
                else
                {
                    Vector2 tscale = base.GetScale();
                    if (OverrideAnimation(SpriteAttributes.ScaleRel, null))
                    {
                        scale = scaleAn.Value + tscale;
                    }
                    else
                    {
                        scale = tscale;
                    }
                }
            }
            else
            {
                scale = base.GetScale();
            }
            //Draw...
            Vector2 pos = GetTranslation() + (this.offsetOrigin && origin.HasValue ? -origin.Value : Vector2.Zero);
            SpriteManager.spriteBat.Draw(baseTexture,
                pos,
                animation.Frame(frame),
                tint.HasValue && OverrideAnimation(SpriteAttributes.Tint, null) ? tint.Value : this.Tint,
                rotation,
                origin.HasValue ? origin.Value : Vector2.Zero,
                scale,
                animation.Effect(frame), 0);
        }

        public override Vector2 Center
        {
            get
            {
                Vector2 center = GetTranslation();

                Rectangle? rect = animation.Frame(frame);
                float width, height;
                if (rect.HasValue)
                {
                    width = rect.Value.Width;
                    height = rect.Value.Height;
                }
                else
                {
                    width = baseTexture.Width;
                    height = baseTexture.Height;
                }
                center.X += width / 2f;
                center.Y += height / 2f;

                return center;
            }
        }

        public override int Width
        {
            get
            {
                Rectangle? rect = animation.Frame(frame);
                if (rect.HasValue)
                {
                    return rect.Value.Width;
                }
                return baseTexture.Width;
            }
        }

        public override int Height
        {
            get
            {
                Rectangle? rect = animation.Frame(frame);
                if (rect.HasValue)
                {
                    return rect.Value.Height;
                }
                return baseTexture.Height;
            }
        }

        public override T GetAsType<T>()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(Texture2D)))
            {
                return (T)((object)this.baseTexture); //Need to do a little work around to get this to compile.
            }
            return base.GetAsType<T>();
        }

        protected override void CopySpriteTo(ref Sprite sp)
        {
            ((BitmapSprite)sp).baseTexture = this.baseTexture;
        }
    }
}

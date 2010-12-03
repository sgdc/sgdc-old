using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE;

namespace SGDE.Graphics
{
    /// <summary>
    /// A drawable sprite object that can be displayed on screen.
    /// </summary>
    public class Sprite : SceneNode
    {
        //FUTURE: Effects like blur, shadows, glow

        internal Texture2D baseTexture;
        internal SpriteManager.SpriteAnimation animation;
        private float fps, curPos;
        internal SpriteAttributes overrideAtt;
        private int frame; //FUTURE: Subregion so that an animation can be loaded but a subregion of animation can be defined
        private bool backwords;

        /// <summary>
        /// Sprite attributes that can be overriden by Sprite animation.
        /// </summary>
        [Flags]
        public enum SpriteAttributes
        {
            /// <summary>
            /// This space intentionally left blank...
            /// </summary>
            None = 0x0,
            /// <summary>
            /// The Sprite's tint.
            /// </summary>
            Tint = 0x1,
            /// <summary>
            /// The Sprite's rotation is absoulte.
            /// </summary>
            RotationAbs = 0x2,
            /// <summary>
            /// The Sprite's rotation is relitive.
            /// </summary>
            RotationRel = 0x4,
            /// <summary>
            /// The Sprite's scale is absoulte.
            /// </summary>
            ScaleAbs = 0x8,
            /// <summary>
            /// The Sprite's scale is relitive.
            /// </summary>
            ScaleRel = 0x10,
            /// <summary>
            /// The Sprite's FPS
            /// </summary>
            FPS = 0x20 //FPS is initially set on load, can be set by dev using FPS attribute, but if this is used then if/when an animation change occurs the FPS gets set to the value specified in the animation
        }

        /*
        /// <summary>
        /// Get or set SpriteBatch to use when drawing. Can only be done once.
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get
            {
                return SpriteManager.spriteMan;
            }
            set
            {
                if (SpriteManager.spriteMan == null)
                {
                    SpriteManager.spriteMan = value;
                }
                else
                {
                    throw new InvalidOperationException("SpriteManager already exists");
                }
            }
        }
         */

        /// <summary>
        /// Create a new Sprite with default values.
        /// </summary>
        public Sprite()
        {
            Tint = Color.White;
            overrideAtt = SpriteAttributes.None;
            Tint = Color.White;
            //overrideAtt = (int)(SpriteAttributes.Tint | SpriteAttributes.RotationAbs | SpriteAttributes.ScaleAbs);
        }

        /// <summary>
        /// Draw this Sprite.
        /// </summary>
        /// <param name="gameTime">The GameTime since the last draw.</param>
        public void Draw(GameTime gameTime)
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
            SpriteManager.spriteBat.Draw(baseTexture,
                GetTranslation(), 
                animation.Frame(frame), 
                tint.HasValue && OverrideAnimation(SpriteAttributes.Tint, null) ? tint.Value : this.Tint, 
                rotation, 
                origin.HasValue ? origin.Value : Vector2.Zero, 
                scale, 
                animation.Effect(frame), 0);
        }

        /// <summary>
        /// Update this Sprite.
        /// </summary>
        /// <param name="gameTime">The GameTime since the last update.</param>
        public void Update(GameTime gameTime)
        {
            if (this.fps > 0)
            {
                SpriteManager.FrameAdjustment(ref this.frame, this.animation.frameCount, this.fps, ref this.curPos, gameTime, this.backwords);
            }
        }

        /// <summary>
        /// Some attributes about a Sprite can be determined by it's animation. This function allows the determination of what attributes are allowed to be set by the animation and what are user determined.
        /// </summary>
        /// <param name="component">What component of the Sprite should be get/set.</param>
        /// <param name="allowed"><code>true</code> if this compoennt can be overriden by the Sprite's animation, <code>false</code> is otherwise. Can also be set to <code>null</code> if the value should be returned.</param>
        /// <returns><code>true</code> if the the Sprite's animation can override the developer setting, <code>false</code> if otherwise.</returns>
        public bool OverrideAnimation(SpriteAttributes component, bool? allowed)
        {
            if (allowed.HasValue)
            {
                if (allowed.Value)
                {
                    overrideAtt |= component;
                }
                else
                {
                    overrideAtt &= ~component;
                }
                return allowed.Value;
            }
            else
            {
                return (overrideAtt & component) == component;
            }
        }

        /// <summary>
        /// Get the center of the Sprite based off it's current animation.
        /// </summary>
        public Vector2 Center
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

        /// <summary>
        /// Get the width of the Sprite based off it's current animation.
        /// </summary>
        public int Width
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

        /// <summary>
        /// Get the height of the Sprite based off it's current animation.
        /// </summary>
        public int Height
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

        /// <summary>
        /// Get or set the tint to apply to this Sprite. This can be overriden, if allowed, by the Sprite animation.
        /// </summary>
        public Color Tint { get; set; }

        /// <summary>
        /// Get or set the current animation Frame
        /// </summary>
        public int Frame
        {
            get
            {
                return this.frame;
            }
            set
            {
                if (value < 0 || value >= animation.frameCount)
                {
                    return;
                }
                this.frame = value;
            }
        }

        /// <summary>
        /// Get or set the current animation FPS.
        /// </summary>
        public float FPS
        {
            get
            {
                if (this.backwords)
                {
                    return -this.fps;
                }
                return this.fps;
            }
            set
            {
                this.backwords = value < 0;
                this.fps = value;
            }
        }

        /* For later implementation
        public void LoadAnimation(string name)
        {
            //TODO
        }
         */

        /// <summary>
        /// Get this node element as the specified type.
        /// </summary>
        /// <typeparam name="T">Type of object to get.</typeparam>
        /// <returns>This node as the specified type or the default value of that type.</returns>
        public override T GetAsType<T>()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(Texture2D)))
            {
                return (T)((object)this.baseTexture); //Need to do a little work around to get this to compile.
            }
            return base.GetAsType<T>();
        }
    }
}
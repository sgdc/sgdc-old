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
    public abstract class Sprite : SceneNode
    {
        internal SpriteManager.SpriteAnimation animation;
        internal SpriteAttributes OverrideAttributes { get { return this.overrideAtt; } set { this.overrideAtt = value; } }
        internal int frame, animStart, animEnd;
        internal bool offsetOrigin;
        internal int id;

        /// <summary>
        /// The current SpriteAttributes that can override the Sprite's default attributes.
        /// </summary>
        protected SpriteAttributes overrideAtt;

        private bool backwords;
        private float fps, curPos;

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

        /// <summary>
        /// Create a new Sprite with default values.
        /// </summary>
        protected Sprite()
        {
            Tint = Color.White;
            overrideAtt = SpriteAttributes.None;
        }

        /// <summary>
        /// Draw this Sprite.
        /// </summary>
        /// <param name="gameTime">The GameTime since the last draw.</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Update this Sprite.
        /// </summary>
        /// <param name="gameTime">The GameTime since the last update.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (this.fps > 0)
            {
                SpriteManager.FrameAdjustment(ref this.frame, this.animStart, this.animEnd, this.fps, ref this.curPos, gameTime, this.backwords);
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
        public abstract Vector2 Center { get; }

        /// <summary>
        /// Get the width of the Sprite based off it's current animation.
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// Get the height of the Sprite based off it's current animation.
        /// </summary>
        public abstract int Height { get; }

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

        /// <summary>
        /// Get if this Sprite is potentially visible on screen or not.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                //TODO: Look at Draw and pull out the information so that it can simply be extracted and tested.
                return true;
            }
        }

        /// <summary>
        /// If this Sprite is visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Set the animation for the Sprite. <b>IMPORTANT:</b> If the animation is successfully set then the FPS, current frame of animation, and animation region are reset to the default values of the animation.
        /// </summary>
        /// <param name="id">The ID of the animation. This is the ID used locally by the Sprite.</param>
        /// <returns><code>true</code> if the animation was set, <code>false</code> if otherwise.</returns>
        public bool SetAnimation(int id)
        {
            return SetAnimation(id, false);
        }

        /// <summary>
        /// Set the animation for the Sprite. <b>IMPORTANT:</b> If the animation is successfully set then the FPS, current frame of animation, and animation region are reset to the default values of the animation.
        /// </summary>
        /// <param name="id">The ID of the animation. This is the ID used locally by the Sprite, if the ID could identify global animations then the ID can identify a global animation ID.</param>
        /// <param name="checkGlobal">If global animation should be checked if Sprite specific animation is not found.</param>
        /// <returns><code>true</code> if the animation was set, <code>false</code> if otherwise.</returns>
        public bool SetAnimation(int id, bool checkGlobal)
        {
            return SetAnimation(id, checkGlobal, false);
        }

        /// <summary>
        /// Set the animation for the Sprite. <b>IMPORTANT:</b> If the animation is successfully set then the FPS, current frame of animation, and animation region are reset to the default values of the animation.
        /// </summary>
        /// <param name="id">The ID of the animation. This is the ID used locally by the Sprite, if the ID could identify global animations then the ID can identify a global animation ID.</param>
        /// <param name="checkGlobal">If global animation should be checked if Sprite specific animation is not found.</param>
        /// <param name="global">If global animation should be checked before Sprite specific animation. If this is <code>true</code> than <paramref name="checkGlobal"/> is ignored.</param>
        /// <returns><code>true</code> if the animation was set, <code>false</code> if otherwise.</returns>
        public bool SetAnimation(int id, bool checkGlobal, bool global)
        {
            SpriteManager man = SpriteManager.GetInstance();
            SpriteManager.SpriteAnimation ani;
            if (global)
            {
                ani = man.GetFrames(-1, id);
                if (!ani.IsValid)
                {
                    ani = man.GetFrames(this.id, id);
                }
            }
            else
            {
                ani = man.GetFrames(this.id, id);
                if (!ani.IsValid && checkGlobal)
                {
                    ani = man.GetFrames(-1, id);
                }
            }
            SetAnimation(ani);
            return ani.IsValid;
        }

        /// <summary>
        /// Set the animation for the Sprite. <b>IMPORTANT:</b> If the animation is successfully set then the FPS, current frame of animation, and animation region are reset to the default values of the animation.
        /// </summary>
        /// <param name="gameElement">The Asset ID of the animation.</param>
        /// <returns><code>true</code> if the animation was set, <code>false</code> if otherwise.</returns>
        public bool SetAnimation(string gameElement)
        {
            SpriteManager.SpriteAnimation ani = SpriteManager.GetInstance().GetFrames(gameElement);
            SetAnimation(ani);
            return ani.IsValid;
        }

        private void SetAnimation(SpriteManager.SpriteAnimation ani)
        {
            if (ani.IsValid)
            {
                this.animation = ani;
                this.FPS = ani.DefaultFPS;
                this.curPos = this.animStart = this.frame = 0;
                this.animEnd = ani.frameCount;
            }
        }

        internal void CopySpriteToIn(ref Sprite sp)
        {
            sp.Visible = this.Visible;
            sp.animation = this.animation;
            sp.overrideAtt = this.overrideAtt;
            sp.frame = this.frame;
            sp.animStart = this.animStart;
            sp.animEnd = this.animEnd;
            sp.FPS = this.FPS;
            sp.Tint = this.Tint;
            this.CopySpriteTo(ref sp);
        }

        /// <summary>
        /// Copy this Sprite to another Sprite.
        /// </summary>
        /// <param name="sp">The Sprite to copy to.</param>
        protected abstract void CopySpriteTo(ref Sprite sp);
    }
}
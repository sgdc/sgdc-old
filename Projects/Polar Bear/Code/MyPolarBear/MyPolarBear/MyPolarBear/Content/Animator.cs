using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyPolarBear.Content
{
    public class Animator
    {
        public Dictionary<string, Animation> Animations;
        public String CurrentAnimation;

        private Animation mCurrAnimation;
        private Animation mAni;

        public Animator()
        {
            Animations = new Dictionary<string, Animation>();
            CurrentAnimation = null;
            mCurrAnimation = null;
        }

        /*
         * bRestart : if true and animation is already playing, restart animation
         */
        public void PlayAnimation(String animation, bool bRestart)
        {
            if (!Animations.ContainsKey(animation) || (!bRestart && animation.Equals(CurrentAnimation)))
            {
                return;
            }

            mAni = Animations[animation];

            if (mCurrAnimation == null || mAni.Priority > mCurrAnimation.Priority
                || (mCurrAnimation.Priority % 10 == 0 && mCurrAnimation.Priority == mAni.Priority))
            {
                mCurrAnimation = mAni;
                CurrentAnimation = animation;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale, Color tint, float rot, 
            Vector2 orig, float layer)
        {
            if (mCurrAnimation == null)
            {
                return;
            }

            mCurrAnimation.Draw(spriteBatch, position, scale, tint, rot, orig, layer);
        }
    }
}

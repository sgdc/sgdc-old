using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyPolarBear.Content
{
    public class Animation
    {
        public Texture2D SpriteSheet;
        public int SpritesPerSheet;
        public int AnimationFrame;
        public int FrameDelay;
        public bool bIsFinished;
        public int Priority;
        public bool bShouldLoop;
        public SpriteEffects SpriteEff;

        private Rectangle mSourceRect;
        private int mMaxFrame;

        /* 
         * priority : higher priorities can inturrupt lower priority animations
         * % 10 priorities (0, 10, 20, 30, etc) can also be inturrupted by priorites of equal value
         */
        public Animation(Texture2D spriteSheet, int numSprites, int frameDelay, int priority, bool bLoop, SpriteEffects eff)
        {
            SpriteSheet = spriteSheet;
            SpritesPerSheet = numSprites;
            FrameDelay = frameDelay;
            AnimationFrame = 0;
            mMaxFrame = numSprites * frameDelay;
            bIsFinished = false;
            Priority = priority;
            bShouldLoop = bLoop;
            SpriteEff = eff;

            mSourceRect = new Rectangle(0, 0, SpriteSheet.Width, SpriteSheet.Height / numSprites);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale, Color tint, float rot,
            Vector2 orig, float layer)
        {
            mSourceRect.Y = mSourceRect.Height * (AnimationFrame / FrameDelay);

            spriteBatch.Draw(SpriteSheet, position, mSourceRect, tint, rot, orig, scale, SpriteEff, layer);

            if (!bIsFinished || bShouldLoop)
            {
                AnimationFrame++;

                if (AnimationFrame % (mMaxFrame) == 0)
                {
                    AnimationFrame = 0;
                    bIsFinished = true;
                }

                if (AnimationFrame == 1)
                {
                    bIsFinished = false;
                }
            }
        }

        //public Rectangle GetSourceRect()
        //{
        //    mSourceRect.Y = mSourceRect.Height * (AnimationFrame / FrameDelay);

        //    if (!bIsFinished || bShouldLoop)
        //    {
        //        AnimationFrame++;

        //        if (AnimationFrame % (mMaxFrame) == 0)
        //        {
        //            AnimationFrame = 0;
        //            bIsFinished = true;
        //        }

        //        if (AnimationFrame == 1)
        //        {
        //            bIsFinished = false;
        //        }
        //    }

        //    return mSourceRect;
        //}
    }
}

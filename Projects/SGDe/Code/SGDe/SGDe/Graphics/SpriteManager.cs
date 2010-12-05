using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SGDE.Graphics
{
    internal class SpriteManager
    {
        internal static SpriteBatch spriteBat;
        private static SpriteManager spManager;

        public static SpriteManager GetInstance()
        {
            return GetInstance(null);
        }

        public static SpriteManager GetInstance(ContentManager manager)
        {
            if (spManager == null)
            {
                spManager = manager.Load<SpriteManager>("SpriteSheet");
            }
            return spManager;
        }

        private List<Texture2D> textures;
        private List<SpriteAnimation[]> animations;

        public Texture2D GetTexture(int index)
        {
            return textures[index];
        }

        public int AddTexture(Texture2D texture, int verifyId)
        {
            if (texture == null)
            {
                return -1;
            }
            if (textures == null)
            {
                textures = new List<Texture2D>();
            }
            if (textures.Count != verifyId)
            {
                throw new ArgumentException(Messages.SpriteManager_SpriteIDMismatch);
            }
            textures.Add(texture);
            return verifyId;
        }

        public int AddAnimation(SpriteAnimation animation, ushort spriteId = 0, int verifyId = 0)
        {
            if (!animation.IsValid)
            {
                return -1;
            }
            if (animations == null)
            {
                animations = new List<SpriteAnimation[]>();
            }
            //Prep for the animation
            ushort animationId = 0;
            bool verify = false;
            if (spriteId == 0)
            {
                spriteId = (ushort)((verifyId >> 16) & 0xFFFFU);
                animationId = (ushort)(verifyId & 0xFFFFU);
                verify = true;
            }
            else
            {
                spriteId--; //1 based index to zero based index
            }
            //Process the animation
            SpriteAnimation[] anim;
            if (animations.Count > spriteId)
            {
                SpriteAnimation[] sp = animations[spriteId];
                anim = new SpriteAnimation[sp.Length + 1];
                Array.Copy(sp, anim, sp.Length);
            }
            else
            {
                anim = new SpriteAnimation[1];
                if (animations.Count != spriteId)
                {
                    throw new ArgumentException(Messages.SpriteManager_AnimationSetIDMismatch);
                }
                animations.Add(anim);
            }
            //Add the animation
            if (verify)
            {
                if (anim.Length - 1 != animationId)
                {
                    throw new ArgumentException(Messages.SpriteManager_AnimationIDMismatch);
                }
                anim[animationId] = animation;
            }
            else
            {
                anim[animationId = (ushort)(anim.Length - 1)] = animation;
                verifyId = (spriteId << 16) | animationId;
            }
            return verifyId;
        }

        public bool HasTexture(int index)
        {
            return index >= 0 && index < textures.Count;
        }

        public SpriteAnimation GetFrames(int id)
        {
            ushort spriteId = (ushort)((id >> 16) & 0xFFFFU);
            ushort animationId = (ushort)(id & 0xFFFFU);
            if (animations == null)
            {
                return new SpriteAnimation();
            }
            if (animations.Count <= spriteId)
            {
                return new SpriteAnimation();
            }
            if (animations[spriteId].Length <= animationId)
            {
                return new SpriteAnimation();
            }
            return animations[spriteId][animationId];
        }

        public static void FrameAdjustment(ref int curFrame, int originFrame, int maxFrame, float fps, ref float current, GameTime gameTime, bool backwords)
        {
            float frame = 1000f / fps;
            float totalFrame = (gameTime.ElapsedGameTime.Milliseconds + current) / frame;
            int count = (int)(totalFrame / 1);
            current = (totalFrame % 1) * frame;
            if (backwords)
            {
                curFrame -= count;
                if (curFrame < 0)
                {
                    if (curFrame < -maxFrame)
                    {
                        curFrame %= maxFrame; //This will do a normal mod operation, if the abs value is less or equal to maxFrame then the value will be positive and the simple addition operation will not work correctly
                    }
                    curFrame += maxFrame;
                }
            }
            else
            {
                curFrame += count;
                curFrame %= maxFrame; //This makes sure the frame never exceeds the maximum.
            }
        }

        public struct SpriteAnimation
        {
            public bool IsValid { get; set; }

            public int frameCount;
            public SpriteEffects[] effects;
            public Color?[] colors;
            public Rectangle?[] frames;
            public float?[] rotation;
            public Vector2?[] origin;
            public Vector2?[] scale;
            public float DefaultFPS { get; set; }
            public int ID;

            public SpriteEffects Effect(int frame)
            {
                return GetValue(frame, frameCount, effects);
            }

            public Color? Tint(int frame)
            {
                return GetValue(frame, frameCount, colors);
            }

            public Rectangle? Frame(int frame)
            {
                return GetValue(frame, frameCount, frames);
            }

            public float? Rotation(int frame)
            {
                return GetValue(frame, frameCount, rotation);
            }

            public Vector2? Origin(int frame)
            {
                return GetValue(frame, frameCount, origin);
            }

            public Vector2? Scale(int frame)
            {
                return GetValue(frame, frameCount, scale);
            }

            private static T GetValue<T>(int frame, int max, T[] values)
            {
                if (values == null)
                {
                    return default(T);
                }
                if (frame > max || frame < 0)
                {
                    return default(T);
                }
                return values[frame];
            }
        }
    }
}

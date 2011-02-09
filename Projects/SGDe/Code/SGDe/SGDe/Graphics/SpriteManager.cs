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
        internal static SpriteManager spManager;

        public static SpriteManager GetInstance()
        {
            if (spManager == null)
            {
                throw new NullReferenceException();
            }
            return spManager;
        }

        private List<Texture2D> textures;
        private List<SpriteAnimation> animations;

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

        public int AddAnimation(SpriteAnimation animation)
        {
            if (!animation.IsValid)
            {
                return -1;
            }
            if (animations == null)
            {
                animations = new List<SpriteAnimation>();
            }
            animations.Add(animation);
            return animations.Count;
        }

        public bool HasTexture(int index)
        {
            return index >= 0 && index < textures.Count;
        }

        public SpriteAnimation GetFrames(int id)
        {
            if (id <= 0 || animations == null)
            {
                return new SpriteAnimation();
            }
            id--;
            if (animations.Count < id)
            {
                return new SpriteAnimation();
            }
            return animations[id];
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

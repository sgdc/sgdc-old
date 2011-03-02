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
        #region Static

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

        #endregion

        #region Textures

        private List<object> textures;

        public int AddTexture(object texture, int verifyId)
        {
            if (texture == null)
            {
                return -1;
            }
            if (textures == null)
            {
                textures = new List<object>();
            }
            if (textures.Count != verifyId)
            {
                throw new ArgumentException(Messages.SpriteManager_SpriteIDMismatch);
            }
            textures.Add(texture);
            return verifyId;
        }

        //Bitmap texture
        public Texture2D GetBitmapTexture(int index)
        {
            return textures[index] as Texture2D;
        }

        //SVG texture
        public SGDE.Graphics.SVG.SVG GetVectorTexture(int index)
        {
            return textures[index] as SGDE.Graphics.SVG.SVG;
        }

        #endregion

        #region Animations

        private List<SpriteAnimation> animations;
        private Dictionary<int, int> animationMapping;
        private Dictionary<string, int> dAnimMapping;

        public int AddAnimation(SpriteAnimation animation, int sid, string name)
        {
            if (!animation.IsValid)
            {
                return -1;
            }
            if (animations == null)
            {
                animations = new List<SpriteAnimation>();
                animationMapping = new Dictionary<int, int>();
                dAnimMapping = new Dictionary<string, int>();
            }
            animations.Add(animation);
            if (sid >= 0)
            {
                if (!animationMapping.ContainsKey(sid))
                {
                    animationMapping.Add(sid, animations.Count);
                }
            }
            if (!string.IsNullOrEmpty(name))
            {
                dAnimMapping.Add(name, animations.Count);
            }
            return animations.Count;
        }

        public SpriteAnimation GetFrames(int id)
        {
            return GetFrames(-1, id);
        }

        public SpriteAnimation GetFrames(int sid, int id)
        {
            if (id <= 0 || animations == null)
            {
                return new SpriteAnimation();
            }
            id--;
            if (sid >= 0)
            {
                if (animationMapping.ContainsKey(sid))
                {
                    id += animationMapping[sid];
                }
                else
                {
                    return new SpriteAnimation();
                }
            }
            if (animations.Count < id)
            {
                return new SpriteAnimation();
            }
            return animations[id];
        }

        public SpriteAnimation GetFrames(string name)
        {
            int id = 0;
            if (dAnimMapping.ContainsKey(name))
            {
                id = dAnimMapping[name];
            }
            return GetFrames(id);
        }

        public static void FrameAdjustment(ref int curFrame, int originFrame, int maxFrame, float fps, ref float current, GameTime gameTime, bool backwords)
        {
            float frame = 1000f / fps;
            float totalFrame = (gameTime.ElapsedGameTime.Milliseconds + current) / frame;
            int count = (int)totalFrame;
            current = (totalFrame % 1) * frame;
            if (count > 0)
            {
                if (backwords)
                {
                    curFrame -= count;
                    if (curFrame < originFrame)
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
                    if (curFrame < originFrame)
                    {
                        curFrame = originFrame;
                    }
                }
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

        #endregion
    }
}

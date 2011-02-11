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
    /// Read and process a Sprite class
    /// </summary>
    internal abstract class SpriteReader<T> : ContentTypeReader<T> where T : Sprite
    {
        /// <summary>
        /// Read a Sprite.
        /// </summary>
        protected override T Read(ContentReader input, T existingInstance)
        {
            T sprite = CreateInstance();
            sprite.Visible = input.ReadBoolean();
            Color? tint = input.ReadObject<Color?>();
            if (tint.HasValue)
            {
                sprite.Tint = tint.Value;
            }
            if (input.ReadBoolean())
            {
                sprite.OverrideAttributes = input.ReadObject<Sprite.SpriteAttributes>();
            }
            ReadAnimation(ref sprite, input);
            if (input.ReadBoolean())
            {
                sprite.animStart = input.ReadInt32(); //Begin
                sprite.animEnd = input.ReadInt32(); //End
                //If either value is -1 then reset to default animation value
                if (sprite.animStart < 0)
                {
                    sprite.animStart = 0;
                }
                else
                {
                    sprite.frame = sprite.animStart;
                }
                if (sprite.animEnd < 0)
                {
                    sprite.animEnd = sprite.animation.frameCount;
                }
            }
            ReadSpecific(input, sprite);
            return sprite;
        }

        public abstract T CreateInstance();

        public abstract void ReadSpecific(ContentReader input, T instance);

        #region ReadAnimation

        private static void ReadAnimation(ref T sprite, ContentReader input)
        {
            bool builtInAnimation = input.ReadBoolean();
            bool localAnimation = input.ReadBoolean();
            int animationID = input.ReadInt32();
            List<SpriteManager.SpriteAnimation> animations = null;
            if (input.ReadBoolean())
            {
                int count = input.ReadInt32();
                animations = new List<SpriteManager.SpriteAnimation>(count);
                for (int i = 0; i < count; i++)
                {
                    animations.Add(input.ReadRawObject<SpriteManager.SpriteAnimation>());
                }
            }
            SpriteManager manager = SpriteManager.GetInstance();
            if (builtInAnimation)
            {
                if (animations != null)
                {
                    bool gotAn = false;
                    for (int i = 1; i <= animations.Count; i++)
                    {
                        SpriteManager.SpriteAnimation animation = animations[i - 1];
                        int value = manager.AddAnimation(animation);
                        if (localAnimation && !gotAn && i == animationID)
                        {
                            gotAn = true;
                            animationID = value;
                        }
                    }
                }
            }
            sprite.animation = manager.GetFrames(animationID);
            sprite.FPS = sprite.animation.DefaultFPS;
            sprite.animStart = sprite.frame = 0;
            sprite.animEnd = sprite.animation.frameCount;
        }

        #endregion
    }
}

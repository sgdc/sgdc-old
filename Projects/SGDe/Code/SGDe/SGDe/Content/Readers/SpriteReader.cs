﻿using System;
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
            sprite.DrawOrder = input.ReadInt32();
            Color? tint = input.ReadObject<Color?>();
            if (tint.HasValue)
            {
                sprite.Tint = tint.Value;
            }
            if (input.ReadBoolean())
            {
                sprite.OverrideAttributes = input.ReadObject<Sprite.SpriteAttributes>();
            }
            sprite.offsetOrigin = input.ReadBoolean();
            sprite.id = input.ReadInt32();
            ReadAnimation(ref sprite, sprite.id, input);
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
            HandleSpecific(input, sprite.id, sprite);
            return sprite;
        }

        public abstract T CreateInstance();

        public abstract void HandleSpecific(ContentReader input, int id, T instance);

        #region ReadAnimation

        private static void ReadAnimation(ref T sprite, int sid, ContentReader input)
        {
            bool builtInAnimation = input.ReadBoolean();
            bool localAnimation = input.ReadBoolean();
            int animationID = input.ReadInt32();
            List<SpriteManager.SpriteAnimation> animations = null;
            List<string> names = null;
            if (input.ReadBoolean())
            {
                int count = input.ReadInt32();
                animations = new List<SpriteManager.SpriteAnimation>(count);
                names = new List<string>(count);
                for (int i = 0; i < count; i++)
                {
                    SpriteManager.SpriteAnimation ani = input.ReadRawObject<SpriteManager.SpriteAnimation>();
                    animations.Add(ani);
                    names.Add(ContentUtil.ExtractDeveloperID(ani));
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
                        int value = manager.AddAnimation(animation, sid, names[i - 1]);
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

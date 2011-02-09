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
    /// Read and process a SpriteManager class
    /// </summary>
    internal class SpriteManagerReader : ContentTypeReader<SpriteManager>
    {
        /// <summary>
        /// Read a SpriteManager.
        /// </summary>
        protected override SpriteManager Read(ContentReader input, SpriteManager existingInstance)
        {
            SpriteManager manager = new SpriteManager();
            List<Texture> textures = input.ReadObject<List<Texture>>();
            int id = 0;
            foreach (Texture2D tex in textures)
            {
                id = manager.AddTexture(tex, id) + 1;
            }
            int count = input.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int lc = input.ReadInt32();
                if (lc < 0)
                {
                    continue;
                }
                for (int k = 0; k < lc; k++)
                {
                    manager.AddAnimation(input.ReadObject<SpriteManager.SpriteAnimation>());
                }
            }
            return manager;
        }
    }
}

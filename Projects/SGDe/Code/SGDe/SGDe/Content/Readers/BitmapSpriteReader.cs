using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using SGDE.Graphics;

namespace SGDE.Content.Readers
{
    /// <summary>
    /// Read and process a BitmapSprite class
    /// </summary>
    internal class BitmapSpriteReader : SpriteReader<BitmapSprite>
    {
        public override BitmapSprite CreateInstance()
        {
            return new BitmapSprite();
        }

        public override void HandleSpecific(ContentReader input, int id, BitmapSprite instance)
        {
            instance.baseTexture = SpriteManager.GetInstance().GetBitmapTexture(id);
        }
    }
}

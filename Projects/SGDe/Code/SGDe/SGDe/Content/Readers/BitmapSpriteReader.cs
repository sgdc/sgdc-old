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

        public override void ReadSpecific(ContentReader input, BitmapSprite instance)
        {
            instance.baseTexture = SpriteManager.GetInstance().GetBitmapTexture(input.ReadInt32());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE.Graphics
{
    /// <summary>
    /// A vector based Sprite.
    /// </summary>
    internal class VectorSprite : Sprite
    {
        internal SGDE.Graphics.SVG.SVG svg;

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GraphicsDevice gd = SpriteManager.spriteBat.GraphicsDevice;
            //TODO
        }

        public override Microsoft.Xna.Framework.Vector2 Center
        {
            get { throw new NotImplementedException(); }
        }

        public override int Width
        {
            get { throw new NotImplementedException(); }
        }

        public override int Height
        {
            get { throw new NotImplementedException(); }
        }

        protected override void CopySpriteTo(ref Sprite sp)
        {
            throw new NotImplementedException();
        }
    }
}

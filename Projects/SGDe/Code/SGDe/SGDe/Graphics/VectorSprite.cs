using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Graphics
{
    /// <summary>
    /// A vector based Sprite.
    /// </summary>
    internal class VectorSprite : Sprite
    {
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
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

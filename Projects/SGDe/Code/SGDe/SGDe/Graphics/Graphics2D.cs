using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE.Graphics
{
    /// <summary>
    /// 2D graphics processor.
    /// </summary>
    public sealed class Graphics2D : SpriteBatch
    {
        private Game game;

        internal Graphics2D(Game game)
            : base(game.GraphicsDevice)
        {
            this.game = game;
        }
    }
}

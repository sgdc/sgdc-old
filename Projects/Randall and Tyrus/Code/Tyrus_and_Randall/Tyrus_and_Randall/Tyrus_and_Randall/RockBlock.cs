using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SGDE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SGDE.Physics.Collision;

namespace Tyrus_and_Randall
{
    class RockBlock : Entity
    {
        private static bool down;

        public RockBlock()
            : this(0.0f, 0.0f)
        { }

        public RockBlock(Vector2 pos)
            : this(pos.X, pos.Y)
        { }

        public RockBlock(float x, float y) :base(x,y)
        {
			id = 9;
            down = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (down && this.GetTranslation().X == 4416)
                this.SetTranslation(new Vector2(4304f, 416f));
        }

        public static void Fall()
        {
            down = true;
        }
    }
}

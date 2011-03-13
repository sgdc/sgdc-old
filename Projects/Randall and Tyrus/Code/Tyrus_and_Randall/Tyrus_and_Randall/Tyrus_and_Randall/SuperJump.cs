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
    class SuperJump : Powerup
    {
        public SuperJump()
            : this(0.0f, 0.0f)
        { }

        public SuperJump(Vector2 pos)
            : this(pos.X, pos.Y)
        { }

        public SuperJump(float x, float y)
            : base(x, y)
        {
            id = 8;
        }

        public override void Activate(Player e)
        {
            e.jumpMultiplier = 1.3f;
        }
    }
}

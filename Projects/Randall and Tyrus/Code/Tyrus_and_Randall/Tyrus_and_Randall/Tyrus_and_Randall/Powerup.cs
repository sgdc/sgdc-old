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
    abstract class Powerup : Entity
    {
        public Powerup()
            : this(0.0f, 0.0f)
        { }

        public Powerup(Vector2 pos)
            : this(pos.X, pos.Y)
        { }

        public Powerup(float x, float y) :base(x,y)
        {
			id = 8;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.GetCollisionUnit().IsSolid()) this.GetCollisionUnit().SetSolid(false);
        }

        public abstract void Activate(Player e);
    }
}
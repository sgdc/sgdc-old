using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SGDE;
using Microsoft.Xna.Framework;
using SGDE.Physics.Collision;

namespace PolarBear
{
    class Enemy : Entity
    {
        public override void Update(GameTime gameTime)
        {

            if (mCollisionUnit.HasCollisions())
            {
                this.SpriteImage.Tint = Color.Red;
            }
            else
            {
                this.SpriteImage.Tint = Color.White;
            }

            base.Update(gameTime);
        }

    }
}

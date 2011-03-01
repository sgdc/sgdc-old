using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Tyrus_and_Randall
{
    class Ground : Entity
    {
        public Ground()
            : this(0, 0)
        {
        }

        public Ground(float x = 0, float y = 0)
            : base(x, y)
        {
            this.mPhysBaby.SetStatic(true);

            id = 2;
        }

        public Ground(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        protected override void SetUpCollision()
        {
            SetCollisionUnit(new SGDE.Physics.Collision.CollisionUnit(this, image.GetTranslation(), image.GetTranslation() + new Vector2(image.Width, image.Height), SGDE.Physics.Collision.CollisionUnit.CollisionType.COLLISION_BOX, null, false));
            this.mCollisionUnit.SetSolid(true);
            //this.mPhysBaby.AddCollisionUnit(this.mCollisionUnit);
            //base.SetUpCollision();
        }
    }
}

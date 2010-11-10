using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SGDE;
using SGDE.Physics.Collision;

namespace TestDemo
{
    public class BounceBall : Entity
    {
        private List<CollisionUnit> mPrevCheckedUnits;

        public BounceBall()
            : this(0, 0)
        {
        }

        public BounceBall(int x, int y)
            : base(x, y)
        {
            mPrevCheckedUnits = new List<CollisionUnit>();
        }

        public override void Update(GameTime gameTime)
        {
            CollisionUnit unit = GetCollisionUnit();

            foreach (CollisionUnit other in mPrevCheckedUnits)
            {
                other.GetOwner().SetColor(Color.White);
            }

            mPrevCheckedUnits = new List<CollisionUnit>(GetCollisionUnit().GetCheckedUnits());

            foreach (CollisionUnit other in mPrevCheckedUnits)
            {
                other.GetOwner().SetColor(Color.Green);
            }

            if (GetCollisionUnit().HasCollisions())
            {
                //Velocity(Velocity().X * -1, Velocity().Y * -1);

                foreach (CollisionUnit other in GetCollisionUnit().GetCollisions())
                {
                    other.GetOwner().SetColor(Color.Pink);
                }
            }
        }
    }
}

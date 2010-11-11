using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SGDE.Physics.Collision;

namespace SGDE.Physics
{
    public class PhysicsBaby
    {
        private bool bStatic;
        private Vector2 mVelocity;
        private Entity mOwner;
        private List<CollisionUnit> mCollisionUnits;

        public PhysicsBaby(Entity owner)
        {
            bStatic = false;
            mVelocity = new Vector2(0.0f, 0.0f);

            mOwner = owner;
            mCollisionUnits = new List<CollisionUnit>();
        }

        public void SetStatic(bool beStatic)
        {
            bStatic = beStatic;
        }

        public bool IsStatic()
        {
            return bStatic;
        }

        public void SetVelocity(Vector2 velocity)
        {
            mVelocity = velocity;
        }

        public void AddVelocity(Vector2 velocity)
        {
            mVelocity += velocity;
        }

        public Vector2 GetVelocity()
        {
            return mVelocity;
        }

        public void AddCollisionUnit(CollisionUnit unit)
        {
            mCollisionUnits.Add(unit);
        }

        public void Update(GameTime gameTime)
        {
            foreach (CollisionUnit unit in mCollisionUnits)
            {
                if (unit.HasCollisions())
                {
                    foreach (CollisionUnit other in unit.GetCollisions())
                    {
                        AddBounce(unit, other);
                    }
                }
            }

            mOwner.Translate((int)mVelocity.X, (int)mVelocity.Y);
        }

        // Add bounce to unit
        private void AddBounce(CollisionUnit unit, CollisionUnit other)
        {
            Vector2 unitCircleCenter;
            Vector2 otherCircleCenter;
            int unitRadius;
            int otherRadius;
            Vector2 oldVelocity;
            double alpha;

            if (unit.GetCollisionType() == CollisionUnit.CIRCLE_COLLISION && other.GetCollisionType() == CollisionUnit.CIRCLE_COLLISION)
            {
                unitCircleCenter = unit.GetCircleCenter();
                unitRadius = unit.GetCircleRadius();

                otherCircleCenter = other.GetCircleCenter();
                otherRadius = other.GetCircleRadius();

                oldVelocity = mVelocity;

                alpha = Math.Atan((otherCircleCenter.X - unitCircleCenter.X) / (unitCircleCenter.Y - otherCircleCenter.Y));

                mVelocity.X = (float)(oldVelocity.X * Math.Cos(2 * alpha) + oldVelocity.Y * Math.Sin(2 * alpha));
                mVelocity.Y = (float)(oldVelocity.X * Math.Sin(2 * alpha) - oldVelocity.Y * Math.Cos(2 * alpha));

                mOwner.Translate((float)0.04 * (unitCircleCenter.X - otherCircleCenter.X), (float)0.04 * (unitCircleCenter.Y - otherCircleCenter.Y));
            }
        }
    }
}

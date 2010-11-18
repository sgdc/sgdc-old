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
            //foreach (CollisionUnit unit in mCollisionUnits)
            //{
            //    if (unit.HasCollisions())
            //    {
            //        foreach (CollisionUnit other in unit.GetCollisions())
            //        {
            //            AddBounce2(unit, other);
            //        }
            //    }
            //}

            mOwner.Translate((int)mVelocity.X, (int)mVelocity.Y);
        }

        // Add bounce to unit
        //private void AddBounce(CollisionUnit unit, CollisionUnit other)
        //{
        //    Vector2 unitCircleCenter;
        //    Vector2 otherCircleCenter;
        //    int unitRadius;
        //    int otherRadius;
        //    Vector2 oldVelocity;
        //    double alpha;

        //    if (unit.GetCollisionType() == CollisionUnit.CIRCLE_COLLISION && other.GetCollisionType() == CollisionUnit.CIRCLE_COLLISION)
        //    {
        //        unitCircleCenter = unit.GetCircleCenter();
        //        unitRadius = unit.GetCircleRadius();

        //        otherCircleCenter = other.GetCircleCenter();
        //        otherRadius = other.GetCircleRadius();

        //        oldVelocity = mVelocity;

        //        alpha = Math.Atan((otherCircleCenter.X - unitCircleCenter.X) / (unitCircleCenter.Y - otherCircleCenter.Y));

        //        mVelocity.X = (float)(oldVelocity.X * Math.Cos(2 * alpha) + oldVelocity.Y * Math.Sin(2 * alpha));
        //        mVelocity.Y = (float)(oldVelocity.X * Math.Sin(2 * alpha) - oldVelocity.Y * Math.Cos(2 * alpha));

        //        // some fudge
        //        mOwner.Translate((float)0.04 * (unitCircleCenter.X - otherCircleCenter.X), (float)0.04 * (unitCircleCenter.Y - otherCircleCenter.Y));
        //    }
        //}

        // Add bounce to unit - no trig, takes masses and velocities into account
        public void AddBounce2(CollisionUnit unit, CollisionUnit other)
        {
            Vector2 unitCircleCenter;
            Vector2 otherCircleCenter;
            int unitRadius;
            int otherRadius;
            Vector2 oldVelocity;
            //double alpha;

            Vector2 norm;
            Vector2 unitNorm;
            Vector2 unitTan = new Vector2();
            Vector2 oldVelocityOther = new Vector2(0, 0);
            float velocityNorm;
            float velocityTan;
            float velocityNormOther;
            float velocityTanOther;
            float newVelocityScalar;
            float newVelocityScalarOther;
            Vector2 newVelocityNorm;
            Vector2 newVelocityNormOther;
            Vector2 newVelocityTan;
            Vector2 newVelocityTanOther;
            Vector2 newVelocity;
            Vector2 newVelocityOther;
            float mass = 1;
            float massOther = 10000;
            Vector2 velocityDiff;

            if (unit.GetCollisionType() == CollisionUnit.COLLISION_CIRCLE && other.GetCollisionType() == CollisionUnit.COLLISION_CIRCLE)
            {
                unitCircleCenter = unit.GetCircleCenter();
                unitRadius = unit.GetCircleRadius();

                otherCircleCenter = other.GetCircleCenter();
                otherRadius = other.GetCircleRadius();

                oldVelocity = mVelocity;

                ////////////////

                norm = otherCircleCenter - unitCircleCenter;
                unitNorm = norm / ((float)Math.Sqrt(norm.X * norm.X + norm.Y * norm.Y));
                unitTan.X = unitNorm.Y * -1;
                unitTan.Y = unitNorm.X;

                velocityNorm = Vector2.Dot(unitNorm, oldVelocity);
                velocityTan = Vector2.Dot(unitTan, oldVelocity);
                velocityNormOther = Vector2.Dot(unitNorm, oldVelocityOther);
                velocityTanOther = Vector2.Dot(unitTan, oldVelocityOther);

                newVelocityScalar = (velocityNorm * (mass - massOther) + 2 * massOther * velocityNormOther) / (mass + massOther);
                newVelocityScalarOther = (velocityNormOther * (massOther - mass) + 2 * mass * velocityNorm) / (mass + massOther);

                newVelocityNorm = newVelocityScalar * unitNorm;
                newVelocityNormOther = newVelocityScalarOther * unitNorm;

                newVelocityTan = velocityTan * unitTan;
                newVelocityTanOther = velocityTanOther * unitTan;

                newVelocity = newVelocityNorm + newVelocityTan;
                newVelocityOther = newVelocityNormOther + newVelocityTanOther;

                velocityDiff = newVelocity - mVelocity;
                mVelocity = newVelocity;

                // some fudge
                if (velocityDiff.X > 1 || velocityDiff.X < -1 || velocityDiff.Y > 1 || velocityDiff.Y < -1)
                {
                    mOwner.Translate((float)0.04 * (unitCircleCenter.X - otherCircleCenter.X), (float)0.04 * (unitCircleCenter.Y - otherCircleCenter.Y));
                }
            }
            else if (other.GetCollisionType() == CollisionUnit.COLLISION_BOX)
            {
                mVelocity *= -1;
            }
            else
            {
                mVelocity *= -1;
            }
        }
    }
}

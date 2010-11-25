using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE.Physics
{
   public class PhysicsBaby
   {
      private bool bStatic;
      private Vector2 mVelocity;
      private Vector2 mForces;
      private Entity mOwner;
      private List<SGDE.Physics.Collision.CollisionUnit> mCollisionUnits;

      public PhysicsBaby(Entity owner)
      {
         bStatic = false;
         mVelocity = new Vector2(0.0f, 0.0f);
         mForces = new Vector2(0.0f, 0.0f);

         mOwner = owner;
         mCollisionUnits = new List<SGDE.Physics.Collision.CollisionUnit>();
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

      public void SetForces(Vector2 forces)
      {
         mForces = forces;
      }

      public void AddForce(Vector2 force)
      {
         mForces += force;
      }

      public Vector2 GetForces()
      {
         return mForces;
      }

      public void AddCollisionUnit(SGDE.Physics.Collision.CollisionUnit unit)
      {
         mCollisionUnits.Add(unit);
      }

      public void Update(GameTime gameTime)
      {
         mOwner.Translate(mVelocity);

         mVelocity += mForces * (float)gameTime.ElapsedGameTime.TotalSeconds;
      }

      public void AddBounce(SGDE.Physics.Collision.CollisionUnit unit, SGDE.Physics.Collision.CollisionUnit other)
      {
         Vector2 unitCircleCenter;
         Vector2 otherCircleCenter;
         int unitRadius;
         int otherRadius;
         Vector2 oldVelocity;

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

         if (unit.GetCollisionType() == SGDE.Physics.Collision.CollisionUnit.COLLISION_CIRCLE && other.GetCollisionType() == SGDE.Physics.Collision.CollisionUnit.COLLISION_CIRCLE)
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

            newVelocity *= 0.99f;

            velocityDiff = newVelocity - mVelocity;
            mVelocity = newVelocity;

            // some fudge
            if (velocityDiff.X > 1 || velocityDiff.X < -1 || velocityDiff.Y > 1 || velocityDiff.Y < -1)
            {
               mOwner.Translate(new Vector2((float)0.04 * (unitCircleCenter.X - otherCircleCenter.X), (float)0.04 * (unitCircleCenter.Y - otherCircleCenter.Y)));
            }
         }
         else
         {
            newVelocity = mVelocity * -0.8f;
            velocityDiff = newVelocity - mVelocity;
            mVelocity = newVelocity;
            unitCircleCenter = unit.GetCircleCenter();
            otherCircleCenter = unit.GetCollisionPoint(other);

            if (velocityDiff.X > 1 || velocityDiff.X < -1 || velocityDiff.Y > 1 || velocityDiff.Y < -1)
            {
               mOwner.Translate(new Vector2((float)0.04 * (unitCircleCenter.X - otherCircleCenter.X), (float)0.04 * (unitCircleCenter.Y - otherCircleCenter.Y)));
            }
         }
      }
   }
}

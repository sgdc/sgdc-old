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
    /// <summary>
    /// General physical properties.
    /// </summary>
    public class PhysicsBaby
    {
        private bool bStatic;
        private Vector2 mVelocity;
        private Vector2 mForces;
        internal Entity mOwner;
        private List<CollisionUnit> mCollisionUnits;

        /// <summary>
        /// Instantiate a new PhysicsBaby for a specific Entity.
        /// </summary>
        /// <param name="owner">The Entity that "owns" this PhysicsBaby.</param>
        public PhysicsBaby(Entity owner)
        {
            bStatic = false;
            mVelocity = new Vector2(0.0f, 0.0f);
            mForces = new Vector2(0.0f, 0.0f);

            mOwner = owner;
            mCollisionUnits = new List<CollisionUnit>();
        }

        /// <summary>
        /// Set if the PhysicsBaby is static (doesn't move).
        /// </summary>
        /// <param name="beStatic"><code>true</code> if the PhysicsBaby is static, <code>false</code> if otherwise.</param>
        public void SetStatic(bool beStatic)
        {
            bStatic = beStatic;
        }

        /// <summary>
        /// Get if the PhysicsBaby is static (doesn't move).
        /// </summary>
        /// <returns><code>true</code> if the PhysicsBaby is static, <code>false</code> if otherwise.</returns>
        public bool IsStatic()
        {
            return bStatic;
        }

        /// <summary>
        /// Set the total velocity.
        /// </summary>
        /// <param name="velocity">The velocity for the PhysicsBaby.</param>
        public void SetVelocity(Vector2 velocity)
        {
            mVelocity = velocity;
        }

        /// <summary>
        /// Add velocity to the PhysicsBaby's velocity.
        /// </summary>
        /// <param name="velocity">The delta velocity to add to the current velocity.</param>
        public void AddVelocity(Vector2 velocity)
        {
            mVelocity += velocity;
        }

        /// <summary>
        /// Get the current total velocity.
        /// </summary>
        /// <returns>The current total velocity.</returns>
        public Vector2 GetVelocity()
        {
            return mVelocity;
        }

        /// <summary>
        /// Set the total, additional, forces.
        /// </summary>
        /// <param name="forces">Total additional forces.</param>
        public void SetForces(Vector2 forces)
        {
            mForces = forces;
        }

        /// <summary>
        /// Add a force.
        /// </summary>
        /// <param name="force">The delta force to add.</param>
        public void AddForce(Vector2 force)
        {
            mForces += force;
        }

        /// <summary>
        /// Get the total, additional, forces.
        /// </summary>
        /// <returns>Total additional forces.</returns>
        public Vector2 GetForces()
        {
            return mForces;
        }

        /// <summary>
        /// Add a CollisionUnit to associate with this PhysicsBaby.
        /// </summary>
        /// <param name="unit">The CollisionUnit to add.</param>
        public void AddCollisionUnit(CollisionUnit unit)
        {
            mCollisionUnits.Add(unit);
        }

        /// <summary>
        /// Update the position and velocity of the PhysicsBaby.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public void Update(GameTime gameTime)
        {
            mOwner.Translate(mVelocity.X, mVelocity.Y);

            mVelocity += mForces * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Add a bounce between two CollisionUnits based on the velocity.
        /// </summary>
        /// <param name="unit">The main collision unit involved in the "bounce".</param>
        /// <param name="other">The secondary collision unit involved in the "bounce".</param>
        public void AddBounce(CollisionUnit unit, CollisionUnit other)
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

            if (unit.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionUnit.CollisionType.COLLISION_CIRCLE)
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
                //if (velocityDiff.X > 1 || velocityDiff.X < -1 || velocityDiff.Y > 1 || velocityDiff.Y < -1)
                //{
                    mOwner.Translate(0.04f * (unitCircleCenter.X - otherCircleCenter.X), 0.04f * (unitCircleCenter.Y - otherCircleCenter.Y));
                //}
            }
            else
            {
                newVelocity = mVelocity * -0.8f;
                velocityDiff = newVelocity - mVelocity;
                mVelocity = newVelocity;
                unitCircleCenter = unit.GetCircleCenter();
                otherCircleCenter = unit.GetCollisionPoint(other);

                //if (velocityDiff.X > 1 || velocityDiff.X < -1 || velocityDiff.Y > 1 || velocityDiff.Y < -1)
                //{
                    mOwner.Translate(0.04f * (unitCircleCenter.X - otherCircleCenter.X), 0.04f * (unitCircleCenter.Y - otherCircleCenter.Y));
                //}
            }
        }
    }
}

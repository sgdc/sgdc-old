using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SGDE.Physics.Collision
{
    /// <summary>
    /// Defines the collision properties and methods to check collisions for an Entity.
    /// </summary>
    public class CollisionUnit : SceneNode
    {
        /// <summary>
        /// Types of collisions that can occur.
        /// </summary>
        public enum CollisionType : byte
        {
            /// <summary>
            /// No collision
            /// </summary>
            COLLISION_NONE,
            /// <summary>
            /// Circle based collision
            /// </summary>
            COLLISION_CIRCLE,
            /// <summary>
            /// Line based collision
            /// </summary>
            COLLISION_LINE,
            /// <summary>
            /// Box based collision
            /// </summary>
            COLLISION_BOX
        };

        private Texture2D mCollisionMask;
        private CollisionType mCollisionType;
        private Entity mOwner;
        private bool bPixelCollision;
        private bool bNeedsUpdate;
        private bool bHasCollisions;
        private Vector2 mCircleCenter;
        private int mCircleRadius;
        private Vector2 mPoint1;                    // line or box (upper left) collision
        private Vector2 mPoint2;                    // line or box (lower right) collision
        private List<CollisionUnit> mCollisions;
        private List<CollisionUnit> mCheckedUnits;  // for testing
        private CollisionUnit mLastCheckedBy;
        private int mCheckTimeStamp;                // logical time when last checked by another unit
        private bool bCollisionsChanged;
        private int mCollisionTimeStamp;            // when last reported change in collisions
        private int mFullCheckTimeStamp;            // when last checked all surrounding units for collisions
        private bool mSolid;

        /// <summary>
        /// Instantiate a new CollisionUnit. This constructor is for circle based collisions.
        /// </summary>
        /// <param name="owner">The "owner" Entity that this CollisionUnit will be for.</param>
        /// <param name="center">The location of the circle center.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="collisionMask">Collision mask for the object.</param>
        /// <param name="bUsePixelCollision">Is pixel-perfect collisions should be used.</param>
        public CollisionUnit(Entity owner, Vector2 center, int radius, Texture2D collisionMask, bool bUsePixelCollision)
        {
            InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
            mCircleCenter = center;
            mCircleRadius = radius;
            mCollisionType = CollisionType.COLLISION_CIRCLE;
        }

        /// <summary>
        /// Instantiate a new CollisionUnit. This constructor is for line and box based collisions.
        /// </summary>
        /// <param name="owner">The "owner" Entity that this COllisionUnit will be for.</param>
        /// <param name="point1">A point on the line or a corner of the box. For line, this is the start. For box, this is the upper-left corner.</param>
        /// <param name="point2">A point on the line or a corner of the box. For line, this is the end. For box, this is the lower-right corner.</param>
        /// <param name="collisionType">
        /// The collision type to use. The only accepted value is <see cref="CollisionType.COLLISION_LINE"/>, which makes this a line based collision unit. All other values default to box based collision.
        /// </param>
        /// <param name="collisionMask">Collision mask for the object.</param>
        /// <param name="bUsePixelCollision">Is pixel-perfect collisions should be used.</param>
        public CollisionUnit(Entity owner, Vector2 point1, Vector2 point2, CollisionType collisionType, Texture2D collisionMask, bool bUsePixelCollision)
        {
            InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
            mPoint1 = point1;
            mPoint2 = point2;
            if (collisionType == CollisionType.COLLISION_LINE)
            {
                mCollisionType = CollisionType.COLLISION_LINE;
            }
            else
            {
                mCollisionType = CollisionType.COLLISION_BOX;
            }
        }

        /// <summary>
        /// Instantiate a new CollisionUnit. This constructor is for circle based collisions.
        /// </summary>
        /// <param name="owner">The "owner" Entity that this CollisionUnit will be for.</param>
        /// <param name="location">The top-left corner of the <paramref name="collisionMask"/>.</param>
        /// <param name="collisionMask">Collision mask for the object.</param>
        /// <param name="bUsePixelCollision">Is pixel-perfect collisions should be used.</param>
        public CollisionUnit(Entity owner, Vector2 location, Texture2D collisionMask, bool bUsePixelCollision)
        {
            InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
            CalculateCircle(location);
            mCollisionType = CollisionType.COLLISION_CIRCLE;
        }

        private void InitializeCommonStuff(Entity owner, Texture2D collisionMask, bool bUsePixelCollision)
        {
            mOwner = owner;
            mCollisionMask = collisionMask;
            bPixelCollision = bUsePixelCollision;
            bNeedsUpdate = true;
            mCollisions = new List<CollisionUnit>();
            mCheckedUnits = new List<CollisionUnit>();
            mLastCheckedBy = null;
            mCheckTimeStamp = 0;
            bCollisionsChanged = false;
            mCollisionTimeStamp = 0;
            mFullCheckTimeStamp = 0;
            mSolid = true;
        }

        private void CalculateCircle(Vector2 location)
        {
            if (mCollisionMask != null)
            {
                mCircleCenter.X = location.X + mCollisionMask.Width / 2;
                mCircleCenter.Y = location.Y + mCollisionMask.Height / 2;

                mCircleRadius = (int)Math.Max(mCollisionMask.Height, mCollisionMask.Width) / 2;
            }
        }

        /// <summary>
        /// Get is pixel-perfect collisions are enabled.
        /// </summary>
        /// <returns><code>true</code> if they are enabled, <code>false</code> if otherwise.</returns>
        public bool UsesPixelCollision()
        {
            return bPixelCollision;
        }

        /// <summary>
        /// For circle based collisions, get the circle center.
        /// </summary>
        /// <returns>The circle center of the CollisionUnit.</returns>
        public Vector2 GetCircleCenter()
        {
            return mCircleCenter;
        }

        /// <summary>
        /// For circle based collisions, get the circle radius.
        /// </summary>
        /// <returns>The circle radius of the CollisionUnit.</returns>
        public int GetCircleRadius()
        {
            return mCircleRadius;
        }

        /// <summary>
        /// For line based collisions, get the line start.
        /// </summary>
        /// <returns>The start of the line.</returns>
        public Vector2 GetLineStart()
        {
            return mPoint1;
        }

        /// <summary>
        /// For line based collisions, get the line end.
        /// </summary>
        /// <returns>The end of the line.</returns>
        public Vector2 GetLineEnd()
        {
            return mPoint2;
        }

        /// <summary>
        /// For box based collisions, get the upper-left corner of the box. For circle based collisions, get the upper-left bound of the circle.
        /// </summary>
        /// <returns>The upper-left corner or bound for the collision unit.</returns>
        public Vector2 GetUpperLeft()
        {
            if (mCollisionType == CollisionType.COLLISION_CIRCLE)
            {
                return new Vector2(mCircleCenter.X - mCircleRadius, mCircleCenter.Y - mCircleRadius);
            }
            else
            {
                return mPoint1;
            }
        }

        /// <summary>
        /// For box based collisions, get the lower-right corner of the box. For circle based collisions, get the lower-right bound of the circle.
        /// </summary>
        /// <returns>The lower-right corner or bound for the collision unit.</returns>
        public Vector2 GetLowerRight()
        {
            if (mCollisionType == CollisionType.COLLISION_CIRCLE)
            {
                return new Vector2(mCircleCenter.X + mCircleRadius, mCircleCenter.Y + mCircleRadius);
            }
            else
            {
                return mPoint2;
            }
        }

        /// <summary>
        /// For box based collisions, get the width of the box. For circle based collisions, get the circle diameter.
        /// </summary>
        /// <returns>The width or radius of the collision unit.</returns>
        public int GetWidth()
        {
            if (mCollisionType == CollisionType.COLLISION_CIRCLE)
            {
                return mCircleRadius * 2;
            }
            else if (mCollisionType == CollisionType.COLLISION_BOX)
            {
                return (int)(mPoint2.X - mPoint1.X);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// For box based collisions, get the height of the box. For circle based collisions, get the circle diameter.
        /// </summary>
        /// <returns>The height or radius of the collision unit.</returns>
        public int GetHeight()
        {
            if (mCollisionType == CollisionType.COLLISION_CIRCLE)
            {
                return mCircleRadius * 2;
            }
            else if (mCollisionType == CollisionType.COLLISION_BOX)
            {
                return (int)(mPoint2.Y - mPoint1.Y);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Get the CollisionUnit type.
        /// </summary>
        /// <returns>The type of collision unit.</returns>
        public CollisionType GetCollisionType()
        {
            return mCollisionType;
        }

        /// <summary>
        /// Get the "owner" Entity of this CollisionUnit.
        /// </summary>
        /// <returns>The CollisionUnit owner.</returns>
        public Entity GetOwner()
        {
            return mOwner;
        }

        /// <summary>
        /// Get the last collision unit that was checked with this collision unit.
        /// </summary>
        /// <returns>The last checked collision unit.</returns>
        public CollisionUnit GetLastCheckedBy()
        {
            return mLastCheckedBy;
        }

        /// <summary>
        /// Set the last collision unit to be checked with this collision unit.
        /// </summary>
        /// <param name="unit">The last checked collision unit.</param>
        public void SetLastCheckedBy(CollisionUnit unit)
        {
            mLastCheckedBy = unit;
        }

        /// <summary>
        /// Get the last time that the collision unit was checked.
        /// </summary>
        /// <returns>The last time the collision unit was checked.</returns>
        public int GetCheckTimeStamp()
        {
            return mCheckTimeStamp;
        }

        /// <summary>
        /// Set the last time the collision unit was checked.
        /// </summary>
        /// <param name="timeStamp">The last time the collision unit was checked.</param>
        public void SetCheckTimeStamp(int timeStamp)
        {
            mCheckTimeStamp = timeStamp;
        }

        /// <summary>
        /// Get the last time the collision unit was updated.
        /// </summary>
        /// <returns>The last time the collision unit was updated.</returns>
        public int GetFullCheckTimeStamp()
        {
            return mFullCheckTimeStamp;
        }

        /// <summary>
        /// Set the last time the collision unit was updated.
        /// </summary>
        /// <param name="timeStamp">The last time the collision unit was updated.</param>
        public void SetFullCheckTimeStamp(int timeStamp)
        {
            mFullCheckTimeStamp = timeStamp;
        }

        /// <summary>
        /// Set if the collision unit is solid.
        /// </summary>
        /// <param name="blockOthers"><code>true</code> if the collision unit is solid, <code>false</code> if otherwise.</param>
        public void SetSolid(bool blockOthers)
        {
            mSolid = blockOthers;
        }

        /// <summary>
        /// Get if the collision unit is solid.
        /// </summary>
        /// <returns><code>true</code> if the collision unit is solid, <code>false</code> if otherwise.</returns>
        public bool IsSolid()
        {
            return mSolid;
        }

        /// <summary>
        /// Get the last CollisionUnits that this collision unit was checked with.
        /// </summary>
        /// <returns>Last checked collision units.</returns>
        public List<CollisionUnit> GetCheckedUnits()
        {
            return mCheckedUnits;
        }

        /// <summary>
        /// Clear any collisions, whether they occured or not.
        /// </summary>
        public void ClearCollisions()
        {
            mCollisions.Clear();

            bHasCollisions = false;
        }

        /// <summary>
        /// Clear the list of checked units.
        /// </summary>
        public void ClearCheckedUnits()
        {
            mCheckedUnits.Clear();
        }

        /// <summary>
        /// Determine if this CollisionUnit collides with another.
        /// </summary>
        /// <param name="other">The CollisionUnit to check for collision.</param>
        /// <returns><code>true</code> if this collision unit collides with the <paramref name="other"/> CollisionUnit, <code>false</code> if otherwise.</returns>
        public bool CollidesWith(CollisionUnit other)
        {
            Vector2 otherCircleCenter;
            int otherCircleRadius;
            float dX;
            float dY;
            float dist;
            Vector2 otherUpperLeft;
            Vector2 otherLowerRight;

            mCheckedUnits.Add(other);

            if (mCollisionType == CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionType.COLLISION_CIRCLE)
            {
                otherCircleCenter = other.GetCircleCenter();
                otherCircleRadius = other.GetCircleRadius();

                dX = mCircleCenter.X - otherCircleCenter.X;
                dY = mCircleCenter.Y - otherCircleCenter.Y;
                dist = (dX * dX) + (dY * dY);

                // (radius + radius)^2 instead of more expensive squareRoot(dist)
                return (dist <= (mCircleRadius + otherCircleRadius) * (mCircleRadius + otherCircleRadius));
            }
            else if (mCollisionType == CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionType.COLLISION_BOX)
            {
                return CircleBoxCollision(mCircleCenter, mCircleRadius, other.GetUpperLeft(), other.GetLowerRight());
            }
            else if (mCollisionType == CollisionType.COLLISION_BOX && other.GetCollisionType() == CollisionType.COLLISION_CIRCLE)
            {
                return CircleBoxCollision(other.GetCircleCenter(), other.GetCircleRadius(), mPoint1, mPoint2);
            }
            else if (mCollisionType == CollisionType.COLLISION_BOX && other.GetCollisionType() == CollisionType.COLLISION_BOX)
            {
                otherUpperLeft = other.GetUpperLeft();
                otherLowerRight = other.GetLowerRight();

                return (mPoint1.X <= otherLowerRight.X && mPoint1.Y <= otherLowerRight.Y && otherUpperLeft.X <= mPoint2.X && otherUpperLeft.Y <= mPoint2.Y);
            }

            // TODO: other collision types - line, pixel

            return false;
        }

        /// <summary>
        /// Update the collision status between this and the <paramref name="other"/> CollisionUnit.
        /// </summary>
        /// <param name="other">The CollisionUnit to update.</param>
        public void UpdateCollisionsWith(CollisionUnit other)
        {
            if (CollidesWith(other))
            {
                AddCollision(other);
                other.AddCollision(this);
            }
            else
            {
                RemoveCollision(other);
                other.RemoveCollision(this);
            }
        }

        private bool CircleBoxCollision(Vector2 circleCenter, int circleRadius, Vector2 boxUpperLeft, Vector2 boxLowerRight)
        {
            float dX;
            float dY;
            float dist;

            if (circleCenter.X > boxLowerRight.X)
            {
                if (circleCenter.Y > boxLowerRight.Y)
                {
                    dX = circleCenter.X - boxLowerRight.X;
                    dY = circleCenter.Y - boxLowerRight.Y;
                    dist = (dX * dX) + (dY * dY);

                    return dist <= circleRadius * circleRadius;
                }
                else if (circleCenter.Y < boxUpperLeft.Y)
                {
                    dX = circleCenter.X - boxLowerRight.X;
                    dY = circleCenter.Y - boxUpperLeft.Y;
                    dist = (dX * dX) + (dY * dY);

                    return dist <= circleRadius * circleRadius;
                }
                else
                {
                    return circleCenter.X <= boxLowerRight.X + circleRadius;
                }
            }
            else if (circleCenter.X < boxUpperLeft.X)
            {
                if (circleCenter.Y > boxLowerRight.Y)
                {
                    dX = circleCenter.X - boxUpperLeft.X;
                    dY = circleCenter.Y - boxLowerRight.Y;
                    dist = (dX * dX) + (dY * dY);

                    return dist <= circleRadius * circleRadius;
                }
                else if (circleCenter.Y < boxUpperLeft.Y)
                {
                    dX = circleCenter.X - boxUpperLeft.X;
                    dY = circleCenter.Y - boxUpperLeft.Y;
                    dist = (dX * dX) + (dY * dY);

                    return dist <= circleRadius * circleRadius;
                }
                else
                {
                    return circleCenter.X >= boxUpperLeft.X - circleRadius;
                }
            }
            else if (circleCenter.Y > boxLowerRight.Y)
            {
                return circleCenter.Y <= boxLowerRight.Y + circleRadius;
            }
            else if (circleCenter.Y < boxUpperLeft.Y)
            {
                return circleCenter.Y >= boxUpperLeft.Y - circleRadius;
            }
            else
            {
                // in box
                return true;
            }
        }

        /// <summary>
        /// Get the point of collision with the <paramref name="other"/> CollisionUnit. This does not do any real collision checks so the returned value might not be the actual collision point.
        /// </summary>
        /// <param name="other">The CollisionUnit to get the collision point from.</param>
        /// <returns>A Vector2 representing the collision point.</returns>
        public Vector2 GetCollisionPoint(CollisionUnit other)
        {
            Vector2 otherCircleCenter;
            int otherCircleRadius;
            Vector2 boxUpperLeft;
            Vector2 boxLowerRight;
            float dX;
            float dY;
            float ratio;
            Vector2 collisionPoint = new Vector2(-1, -1);

            if (mCollisionType == CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionType.COLLISION_CIRCLE)
            {
                otherCircleCenter = other.GetCircleCenter();
                otherCircleRadius = other.GetCircleRadius();

                dX = otherCircleCenter.X - mCircleCenter.X;
                dY = otherCircleCenter.Y - mCircleCenter.Y;

                ratio = ((float)mCircleRadius) / (mCircleRadius + otherCircleRadius);

                dX *= ratio;
                dY *= ratio;

                collisionPoint = new Vector2(mCircleCenter.X + dX, mCircleCenter.Y + dY);
            }
            else if ((mCollisionType == CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionType.COLLISION_BOX)
                    || (mCollisionType == CollisionType.COLLISION_BOX && other.GetCollisionType() == CollisionType.COLLISION_CIRCLE))
            {
                if (mCollisionType == CollisionType.COLLISION_CIRCLE)
                {
                    otherCircleCenter = mCircleCenter;
                    otherCircleRadius = mCircleRadius;
                    boxUpperLeft = other.GetUpperLeft();
                    boxLowerRight = other.GetLowerRight();
                }
                else
                {
                    otherCircleCenter = other.GetCircleCenter();
                    otherCircleRadius = other.GetCircleRadius();
                    boxUpperLeft = mPoint1;
                    boxLowerRight = mPoint2;
                }

                if (otherCircleCenter.X > boxLowerRight.X)
                {
                    if (otherCircleCenter.Y > boxLowerRight.Y)
                    {
                        collisionPoint = boxLowerRight;
                    }
                    else if (otherCircleCenter.Y < boxUpperLeft.Y)
                    {
                        collisionPoint = new Vector2(boxLowerRight.X, boxUpperLeft.Y);
                    }
                    else
                    {
                        collisionPoint = new Vector2(otherCircleCenter.X - otherCircleRadius, otherCircleCenter.Y);
                    }
                }
                else if (otherCircleCenter.X < boxUpperLeft.X)
                {
                    if (otherCircleCenter.Y > boxLowerRight.Y)
                    {
                        collisionPoint = new Vector2(boxUpperLeft.X, boxLowerRight.Y);
                    }
                    else if (otherCircleCenter.Y < boxUpperLeft.Y)
                    {
                        collisionPoint = boxUpperLeft;
                    }
                    else
                    {
                        collisionPoint = new Vector2(otherCircleCenter.X + otherCircleRadius, otherCircleCenter.Y);
                    }
                }
                else if (otherCircleCenter.Y > boxLowerRight.Y)
                {
                    collisionPoint = new Vector2(otherCircleCenter.X, otherCircleCenter.Y - otherCircleRadius);
                }
                else if (otherCircleCenter.Y < boxUpperLeft.Y)
                {
                    collisionPoint = new Vector2(otherCircleCenter.X, otherCircleCenter.Y + otherCircleRadius);
                }
                else
                {
                    // inside
                    collisionPoint = otherCircleCenter;
                }
            }

            return collisionPoint;
        }

        /// <summary>
        /// Add a collision to with another unit.
        /// </summary>
        /// <param name="other">The CollisionUnit that was collided with.</param>
        public void AddCollision(CollisionUnit other)
        {
            if (!mCollisions.Contains(other))
            {
                mCollisions.Add(other);

                bCollisionsChanged = true;
                
                if (!bHasCollisions)
                {
                    bHasCollisions = true;
                }
            }
        }

        /// <summary>
        /// Remove a collision with another unit.
        /// </summary>
        /// <param name="other">The CollisionUnit to remove the collision from.</param>
        public void RemoveCollision(CollisionUnit other)
        {
            if (mCollisions.Remove(other))
            {
                bCollisionsChanged = true;
            }

            bHasCollisions = mCollisions.Count > 0;
        }

        /// <summary>
        /// Translate the CollisionUnit a specified amount.
        /// </summary>
        /// <param name="translation">The delta translation to move the unit.</param>
        public override void Translate(Vector2 translation)
        {
            base.Translate(translation);
            CollisionChief chief = CollisionChief.GetInstance();
            chief.TranslateCollisionUnit(this, translation);

            if (mCollisionType == CollisionType.COLLISION_CIRCLE)
            {
                mCircleCenter += translation;
            }
            else if (mCollisionType == CollisionType.COLLISION_LINE || mCollisionType == CollisionType.COLLISION_BOX)
            {
                mPoint1 += translation;
                mPoint2 += translation;
            }

            if (!bNeedsUpdate)
            {
                bNeedsUpdate = true;
                chief.UpdateUnit(this);
            }
        }

        /// <summary>
        /// Rotate the CollisionUnit a specified amount.
        /// </summary>
        /// <param name="rotation">The delta rotation of the CollisionUnit, in radians.</param>
        public override void Rotate(float rotation)
        {
            base.Rotate(rotation);
            CollisionChief chief = CollisionChief.GetInstance();
            chief.RotateCollisionUnit(this, rotation);

            if (mCollisionType == CollisionType.COLLISION_CIRCLE || mCollisionType == CollisionType.COLLISION_NONE)
            {
                return;
            }
            else if (mCollisionType == CollisionType.COLLISION_BOX)
            {
                // TODO
            }
            else if (mCollisionType == CollisionType.COLLISION_LINE)
            {
                // TODO
            }

            if (!bNeedsUpdate)
            {
                bNeedsUpdate = true;
                chief.UpdateUnit(this);
            }
        }

        /// <summary>
        /// Scale the CollisionUnit a specified amount.
        /// </summary>
        /// <param name="scale">The delta scale of the CollisionUnit.</param>
        public override void Scale(Vector2 scale)
        {
            base.Scale(scale);
            CollisionChief chief = CollisionChief.GetInstance();

            // no inside out scaling
            if (scale.X < 0)
            {
                scale.X *= -1;
            }
            if (scale.Y < 0)
            {
                scale.Y *= -1;
            }

            chief.ScaleCollisionUnit(this, scale);

            if (mCollisionType == CollisionType.COLLISION_NONE)
            {
                return;
            }
            else if (mCollisionType == CollisionType.COLLISION_CIRCLE)
            {
                // no ovals
                mCircleRadius = (int)(mCircleRadius * ((scale.X + scale.Y) / 2));
            }
            else if (mCollisionType == CollisionType.COLLISION_BOX)
            {
                int oldWidth = GetWidth();
                int oldHeight = GetHeight();

                int xChange = (int)(((oldWidth * scale.X) - oldWidth) / 2);
                int yChange = (int)(((oldHeight * scale.Y) - oldHeight) / 2);

                mPoint1.X -= xChange;
                mPoint1.Y -= yChange;

                mPoint2.X += xChange;
                mPoint2.Y += yChange;
            }
            else if (mCollisionType == CollisionType.COLLISION_LINE)
            {
                // TODO
            }

            if (!bNeedsUpdate)
            {
                bNeedsUpdate = true;
                chief.UpdateUnit(this);
            }
        }

        /// <summary>
        /// Set if the CollisionUnit needs to be updated.
        /// </summary>
        /// <param name="needsUpdate"><code>true</code> if the unit needs to be updated, <code>false</code> if otherwise.</param>
        public void NeedsCollisionUpdate(bool needsUpdate)
        {
            bNeedsUpdate = needsUpdate;
        }

        /// <summary>
        /// Get if the CollisionUnit needs to be updated.
        /// </summary>
        /// <returns><code>true</code> if the unit needs to be updated, <code>false</code> if otherwise.</returns>
        public bool NeedsCollisionUpdate()
        {
            return bNeedsUpdate;
        }

        /// <summary>
        /// Get if collisions changed since the last time checked.
        /// </summary>
        /// <param name="timeStamp">The current timestamp.</param>
        /// <returns><code>true</code> if the collisions changed since the last time checked, <code>false</code> if otherwise.</returns>
        public bool CollisionsChanged(int timeStamp)
        {
            if (bCollisionsChanged && mCollisionTimeStamp < timeStamp)
            {
                mCollisionTimeStamp = timeStamp;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set if collisions changed on this unit.
        /// </summary>
        /// <param name="bChanged"><code>true</code> if the collisions changed, <code>false</code> if otherwise.</param>
        public void CollisionsChanged(bool bChanged)
        {
            bCollisionsChanged = bChanged;
        }

        /// <summary>
        /// Get if the CollisionUnit has collisions.
        /// </summary>
        /// <returns><code>true</code> if the unit has collisions, <code>false</code> if otherwise.</returns>
        public bool HasCollisions()
        {
            //return mCollisions.Count > 0;
            return bHasCollisions;
        }

        /// <summary>
        /// Get a list of all collisions.
        /// </summary>
        /// <returns>The list of collisions with this unit.</returns>
        public List<CollisionUnit> GetCollisions()
        {
            return mCollisions;
        }
    }
}

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SGDE.Physics.Collision
{
<<<<<<< .mine
    public class CollisionUnit : SceneNode
    {
        public enum CollisionType : byte
        {
            COLLISION_NONE,
            COLLISION_CIRCLE,
            COLLISION_LINE,
            COLLISION_BOX
        };
=======
   public class CollisionUnit : SceneNode
   {
      public enum CollisionType : byte
      {
         COLLISION_NONE,
         COLLISION_CIRCLE,
         COLLISION_LINE,
         COLLISION_BOX
      };
>>>>>>> .r58

<<<<<<< .mine
        private Texture2D mCollisionMask;
        private CollisionType mCollisionType;
        private Entity mOwner;
        private bool bPixelCollision;
        private bool bNeedsUpdate;
        private bool bHasCollisions;
        private Vector2 mCircleCenter;
        private int mCircleRadius;
        private Vector2 mPoint1;                 // line or box (upper left) collision
        private Vector2 mPoint2;                   // line or box (lower right) collision
        private List<CollisionUnit> mCollisions;
        private List<CollisionUnit> mCheckedUnits;  // for testing
        private CollisionUnit mLastCheckedBy;
        private int mCheckTimeStamp;                // logical time when last checked by another unit
        private bool bCollisionsChanged;
        private int mCollisionTimeStamp;            // when last reported change in collisions
        private int mFullCheckTimeStamp;            // when last checked all surrounding units for collisions
        private bool mSolid;
=======
      private Texture2D mCollisionMask;
      private CollisionType mCollisionType;
      private Entity mOwner;
      private bool bPixelCollision;
      private bool bNeedsUpdate;
      private bool bHasCollisions;
      private Vector2 mCircleCenter;
      private int mCircleRadius;
      private Vector2 mPoint1;             // line or box (upper left) collision
      private Vector2 mPoint2;               // line or box (lower right) collision
      private List<CollisionUnit> mCollisions;
      private List<CollisionUnit> mCheckedUnits;  // for testing
      private CollisionUnit mLastCheckedBy;
      private int mCheckTimeStamp;            // logical time when last checked by another unit
      private bool bCollisionsChanged;
      private int mCollisionTimeStamp;         // when last reported change in collisions
      private int mFullCheckTimeStamp;         // when last checked all surrounding units for collisions
      private bool mSolid;
>>>>>>> .r58

<<<<<<< .mine
        public CollisionUnit(Entity owner, Vector2 center, int radius, Texture2D collisionMask, bool bUsePixelCollision)
        {
            InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
            mCircleCenter = center;
            mCircleRadius = radius;
            mCollisionType = CollisionType.COLLISION_CIRCLE;
        }
=======
      public CollisionUnit(Entity owner, Vector2 center, int radius, Texture2D collisionMask, bool bUsePixelCollision)
      {
         InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
         mCircleCenter = center;
         mCircleRadius = radius;
         mCollisionType = CollisionType.COLLISION_CIRCLE;
      }
>>>>>>> .r58

<<<<<<< .mine
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
=======
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
>>>>>>> .r58

<<<<<<< .mine
        // location: top left of collisionMask image
        public CollisionUnit(Entity owner, Vector2 location, Texture2D collisionMask, bool bUsePixelCollision)
        {
            InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
            CalculateCircle(location);
            mCollisionType = CollisionType.COLLISION_CIRCLE;
        }
=======
      // location: top left of collisionMask image
      public CollisionUnit(Entity owner, Vector2 location, Texture2D collisionMask, bool bUsePixelCollision)
      {
         InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
         CalculateCircle(location);
         mCollisionType = CollisionType.COLLISION_CIRCLE;
      }
>>>>>>> .r58

      //public CollisionUnit(Entity owner, Vector2 upperLeft, Vector2 lowerRight, Texture2D collisionMask, bool bUsePixelCollision)
      //{
      //   InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
      //   mPoint1 = upperLeft;
      //   mPoint2 = lowerRight;
      //   mCollisionType = BOX_COLLISION;
      //}

<<<<<<< .mine
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
=======
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
>>>>>>> .r58

<<<<<<< .mine
=======
      private void CalculateCircle(Vector2 location)
      {
         if (mCollisionMask != null)
         {
            mCircleCenter.X = location.X + mCollisionMask.Width / 2;
            mCircleCenter.Y = location.Y + mCollisionMask.Height / 2;

>>>>>>> .r58
            mCircleRadius = (int)Math.Max(mCollisionMask.Height, mCollisionMask.Width) / 2;
         }
      }

      public bool UsesPixelCollision()
      {
         return bPixelCollision;
      }

      public Vector2 GetCircleCenter()
      {
         return mCircleCenter;
      }

      public int GetCircleRadius()
      {
         return mCircleRadius;
      }

      public Vector2 GetLineStart()
      {
         return mPoint1;
      }

      public Vector2 GetLineEnd()
      {
         return mPoint2;
      }

      // for box or circle
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

      // for box or circle
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

<<<<<<< .mine
        // for box or circle
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
=======
      public CollisionType GetCollisionType()
      {
         return mCollisionType;
      }
>>>>>>> .r58

<<<<<<< .mine
        // for box or circle
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
=======
      public Entity GetOwner()
      {
         return mOwner;
      }
>>>>>>> .r58

<<<<<<< .mine
        public CollisionType GetCollisionType()
        {
            return mCollisionType;
        }
=======
      public CollisionUnit GetLastCheckedBy()
      {
         return mLastCheckedBy;
      }
>>>>>>> .r58

      public void SetLastCheckedBy(CollisionUnit unit)
      {
         mLastCheckedBy = unit;
      }

      public int GetCheckTimeStamp()
      {
         return mCheckTimeStamp;
      }

      public void SetCheckTimeStamp(int timeStamp)
      {
         mCheckTimeStamp = timeStamp;
      }

      public int GetFullCheckTimeStamp()
      {
         return mFullCheckTimeStamp;
      }

      public void SetFullCheckTimeStamp(int timeStamp)
      {
         mFullCheckTimeStamp = timeStamp;
      }

      public void SetSolid(bool blockOthers)
      {
         mSolid = blockOthers;
      }

      public bool IsSolid()
      {
         return mSolid;
      }

<<<<<<< .mine
        public void SetSolid(bool blockOthers)
        {
            mSolid = blockOthers;
        }
=======
      public List<CollisionUnit> GetCheckedUnits()
      {
         return mCheckedUnits;
      }
>>>>>>> .r58

<<<<<<< .mine
        public bool IsSolid()
        {
            return mSolid;
        }
=======
      public void ClearCollisions()
      {
         mCollisions.Clear();
>>>>>>> .r58

         bHasCollisions = false;
      }

      public void ClearCheckedUnits()
      {
         mCheckedUnits.Clear();
      }

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

<<<<<<< .mine
            if (mCollisionType == CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionType.COLLISION_CIRCLE)
            {
                otherCircleCenter = other.GetCircleCenter();
                otherCircleRadius = other.GetCircleRadius();
=======
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
>>>>>>> .r58

            return (mPoint1.X <= otherLowerRight.X && mPoint1.Y <= otherLowerRight.Y && otherUpperLeft.X <= mPoint2.X && otherUpperLeft.Y <= mPoint2.Y);
         }

<<<<<<< .mine
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
=======
         // TODO: other collision types - line, pixel
>>>>>>> .r58

         return false;
      }

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

<<<<<<< .mine
            if (mCollisionType == CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionType.COLLISION_CIRCLE)
            {
                otherCircleCenter = other.GetCircleCenter();
                otherCircleRadius = other.GetCircleRadius();
=======
         if (mCollisionType == CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionType.COLLISION_CIRCLE)
         {
            otherCircleCenter = other.GetCircleCenter();
            otherCircleRadius = other.GetCircleRadius();
>>>>>>> .r58

            dX = otherCircleCenter.X - mCircleCenter.X;
            dY = otherCircleCenter.Y - mCircleCenter.Y;

            ratio = ((float)mCircleRadius) / (mCircleRadius + otherCircleRadius);

            dX *= ratio;
            dY *= ratio;

<<<<<<< .mine
                collisionPoint = new Vector2(mCircleCenter.X + dX, mCircleCenter.Y + dY);
            }
            else if ((mCollisionType == CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionType.COLLISION_BOX)
                    || (mCollisionType == CollisionType.COLLISION_BOX && other.GetCollisionType() == CollisionType.COLLISION_CIRCLE))
=======
            collisionPoint = new Vector2(mCircleCenter.X + dX, mCircleCenter.Y + dY);
         }
         else if ((mCollisionType == CollisionType.COLLISION_CIRCLE && other.GetCollisionType() == CollisionType.COLLISION_BOX)
               || (mCollisionType == CollisionType.COLLISION_BOX && other.GetCollisionType() == CollisionType.COLLISION_CIRCLE))
         {
            if (mCollisionType == CollisionType.COLLISION_CIRCLE)
>>>>>>> .r58
            {
<<<<<<< .mine
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
=======
               otherCircleCenter = mCircleCenter;
               otherCircleRadius = mCircleRadius;
               boxUpperLeft = other.GetUpperLeft();
               boxLowerRight = other.GetLowerRight();
>>>>>>> .r58
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
<<<<<<< .mine

            bHasCollisions = mCollisions.Count > 0;
        }

        public override void Translate(float x, float y)
        {
            base.Translate(x, y);
            CollisionChief chief = CollisionChief.GetInstance();
            chief.TranslateCollisionUnit(this, x, y);
=======
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
>>>>>>> .r58
<<<<<<< .mine

            if (mCollisionType == CollisionType.COLLISION_CIRCLE)
=======
            else if (otherCircleCenter.Y > boxLowerRight.Y)
>>>>>>> .r58
            {
               collisionPoint = new Vector2(otherCircleCenter.X, otherCircleCenter.Y - otherCircleRadius);
            }
<<<<<<< .mine
            else if (mCollisionType == CollisionType.COLLISION_LINE || mCollisionType == CollisionType.COLLISION_BOX)
=======
            else if (otherCircleCenter.Y < boxUpperLeft.Y)
>>>>>>> .r58
            {
               collisionPoint = new Vector2(otherCircleCenter.X, otherCircleCenter.Y + otherCircleRadius);
            }
            else
            {
<<<<<<< .mine
                bNeedsUpdate = true;
                chief.UpdateUnit(this);
=======
               // inside
               collisionPoint = otherCircleCenter;
>>>>>>> .r58
            }
         }

<<<<<<< .mine
        public override void Rotate(float rotation)
        {
            base.Rotate(rotation);
            CollisionChief chief = CollisionChief.GetInstance();
            chief.RotateCollisionUnit(this, rotation);
=======
         return collisionPoint;
      }
>>>>>>> .r58

      public void AddCollision(CollisionUnit other)
      {
         if (!mCollisions.Contains(other))
         {
            mCollisions.Add(other);

            bCollisionsChanged = true;
            
            if (!bHasCollisions)
            {
<<<<<<< .mine
                bNeedsUpdate = true;
                chief.UpdateUnit(this);
=======
               bHasCollisions = true;
>>>>>>> .r58
            }
         }
      }

<<<<<<< .mine
        public override void Scale(Vector2 scale)
        {
            base.Scale(scale);
            CollisionChief chief = CollisionChief.GetInstance();
            chief.ScaleCollisionUnit(this, scale);
=======
      public void RemoveCollision(CollisionUnit other)
      {
         if (mCollisions.Remove(other))
         {
            bCollisionsChanged = true;
         }
>>>>>>> .r58

         bHasCollisions = mCollisions.Count > 0;
      }

<<<<<<< .mine
            if (!bNeedsUpdate)
            {
                bNeedsUpdate = true;
                chief.UpdateUnit(this);
            }
        }
=======
      public override void Translate(Vector2 trans)
      {
         base.Translate(trans);
         CollisionChief.GetInstance( ).TranslateCollisionUnit(this, trans.X, trans.Y);
>>>>>>> .r58

         if (mCollisionType == CollisionType.COLLISION_CIRCLE)
         {
            mCircleCenter += trans;
         }
         else if (mCollisionType == CollisionType.COLLISION_LINE || mCollisionType == CollisionType.COLLISION_BOX)
         {
            mPoint1 += trans;
            mPoint2 += trans;
         }

         if (!bNeedsUpdate)
         {
            bNeedsUpdate = true;
            CollisionChief.GetInstance().UpdateUnit(this);
         }
      }

      public override void Rotate(ushort rotation)
      {
         base.Rotate(rotation);
         CollisionChief.GetInstance().RotateCollisionUnit(this, rotation);

         // TODO

         if (!bNeedsUpdate)
         {
            bNeedsUpdate = true;
            CollisionChief.GetInstance().UpdateUnit(this);
         }
      }

      public override void Scale(Vector2 scale)
      {
         base.Scale(scale);
         CollisionChief.GetInstance().ScaleCollisionUnit(this, scale);

         // TODO

         if (!bNeedsUpdate)
         {
            bNeedsUpdate = true;
            CollisionChief.GetInstance().UpdateUnit(this);
         }
      }

      public void NeedsCollisionUpdate(bool needsUpdate)
      {
         bNeedsUpdate = needsUpdate;
      }

      public bool NeedsCollisionUpdate()
      {
         return bNeedsUpdate;
      }

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

      public void CollisionsChanged(bool bChanged)
      {
         bCollisionsChanged = bChanged;
      }

      public bool HasCollisions()
      {
         //return mCollisions.Count > 0;
         return bHasCollisions;
      }

      public List<CollisionUnit> GetCollisions()
      {
         return mCollisions;
      }
   }
}

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SGDE.Physics.Collision
{
   public class CollisionUnit : SceneNode
   {
      public const int COLLISION_NONE = 0;
      public const int COLLISION_CIRCLE = 1;
      public const int COLLISION_LINE = 2;
      public const int COLLISION_BOX = 3;

      private CollisionChief mCollisionChief;
      private Texture2D mCollisionMask;
      private int mCollisionType;
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
      private bool mBlockOthers;

      public CollisionUnit(Entity owner, Vector2 center, int radius, Texture2D collisionMask, bool bUsePixelCollision)
      {
         InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
         mCircleCenter = center;
         mCircleRadius = radius;
         mCollisionType = COLLISION_CIRCLE;
      }

      public CollisionUnit(Entity owner, Vector2 point1, Vector2 point2, int collisionType, Texture2D collisionMask, bool bUsePixelCollision)
      {
         InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
         mPoint1 = point1;
         mPoint2 = point2;
         if (collisionType == COLLISION_LINE)
         {
            mCollisionType = COLLISION_LINE;
         }
         else
         {
            mCollisionType = COLLISION_BOX;
         }
      }

      // location: top left of collisionMask image
      public CollisionUnit(Entity owner, Vector2 location, Texture2D collisionMask, bool bUsePixelCollision)
      {
         InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
         CalculateCircle(location);
         mCollisionType = COLLISION_CIRCLE;
      }

      //public CollisionUnit(Entity owner, Vector2 upperLeft, Vector2 lowerRight, Texture2D collisionMask, bool bUsePixelCollision)
      //{
      //   InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
      //   mPoint1 = upperLeft;
      //   mPoint2 = lowerRight;
      //   mCollisionType = BOX_COLLISION;
      //}

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
         mBlockOthers = true;
      }

      public void SetCollisionChief(CollisionChief chief)
      {
         mCollisionChief = chief;
         mCollisionChief.UpdateUnit(this);
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
         if (mCollisionType == COLLISION_CIRCLE)
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
         if (mCollisionType == COLLISION_CIRCLE)
         {
            return new Vector2(mCircleCenter.X + mCircleRadius, mCircleCenter.Y + mCircleRadius);
         }
         else
         {
            return mPoint2;
         }
      }

      public int GetCollisionType()
      {
         return mCollisionType;
      }

      public Entity GetOwner()
      {
         return mOwner;
      }

      public CollisionUnit GetLastCheckedBy()
      {
         return mLastCheckedBy;
      }

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

      public void BlockOthers(bool blockOthers)
      {
         mBlockOthers = blockOthers;
      }

      public bool BlockOthers()
      {
         return mBlockOthers;
      }

      public List<CollisionUnit> GetCheckedUnits()
      {
         return mCheckedUnits;
      }

      public void ClearCollisions()
      {
         mCollisions.Clear();

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

         if (mCollisionType == COLLISION_CIRCLE && other.GetCollisionType() == COLLISION_CIRCLE)
         {
            otherCircleCenter = other.GetCircleCenter();
            otherCircleRadius = other.GetCircleRadius();

            dX = mCircleCenter.X - otherCircleCenter.X;
            dY = mCircleCenter.Y - otherCircleCenter.Y;
            dist = (dX * dX) + (dY * dY);

            // (radius + radius)^2 instead of more expensive squareRoot(dist)
            return (dist <= (mCircleRadius + otherCircleRadius) * (mCircleRadius + otherCircleRadius));
         }
         else if (mCollisionType == COLLISION_CIRCLE && other.GetCollisionType() == COLLISION_BOX)
         {
            return CircleBoxCollision(mCircleCenter, mCircleRadius, other.GetUpperLeft(), other.GetLowerRight());
         }
         else if (mCollisionType == COLLISION_BOX && other.GetCollisionType() == COLLISION_CIRCLE)
         {
            return CircleBoxCollision(other.GetCircleCenter(), other.GetCircleRadius(), mPoint1, mPoint2);
         }
         else if (mCollisionType == COLLISION_BOX && other.GetCollisionType() == COLLISION_BOX)
         {
            otherUpperLeft = other.GetUpperLeft();
            otherLowerRight = other.GetLowerRight();

            return (mPoint1.X <= otherLowerRight.X && mPoint1.Y <= otherLowerRight.Y && otherUpperLeft.X <= mPoint2.X && otherUpperLeft.Y <= mPoint2.Y);
         }

         // TODO: other collision types - line, pixel

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

         if (mCollisionType == COLLISION_CIRCLE && other.GetCollisionType() == COLLISION_CIRCLE)
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
         else if ((mCollisionType == COLLISION_CIRCLE && other.GetCollisionType() == COLLISION_BOX)
               || (mCollisionType == COLLISION_BOX && other.GetCollisionType() == COLLISION_CIRCLE))
         {
            if (mCollisionType == COLLISION_CIRCLE)
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

      public void RemoveCollision(CollisionUnit other)
      {
         if (mCollisions.Remove(other))
         {
            bCollisionsChanged = true;
         }

         bHasCollisions = mCollisions.Count > 0;
      }

      public override void Translate(Vector2 trans)
      {
         base.Translate(trans);
         if (mCollisionChief != null)
         {
            mCollisionChief.TranslateCollisionUnit(this, trans.X, trans.Y);
         }

         if (mCollisionType == COLLISION_CIRCLE)
         {
            mCircleCenter += trans;
         }
         else if (mCollisionType == COLLISION_LINE || mCollisionType == COLLISION_BOX)
         {
            mPoint1 += trans;
            mPoint2 += trans;
         }

         if (!bNeedsUpdate)
         {
            bNeedsUpdate = true;
            if (mCollisionChief != null)
            {
               mCollisionChief.UpdateUnit(this);
            }
         }
      }

      public override void Rotate(ushort rotation)
      {
         base.Rotate(rotation);
         if (mCollisionChief != null)
         {
            mCollisionChief.RotateCollisionUnit(this, rotation);
         }

         // TODO

         if (!bNeedsUpdate)
         {
            bNeedsUpdate = true;
            if (mCollisionChief != null)
            {
               mCollisionChief.UpdateUnit(this);
            }
         }
      }

      public override void Scale(Vector2 scale)
      {
         base.Scale(scale);
         if (mCollisionChief != null)
         {
            mCollisionChief.ScaleCollisionUnit(this, scale);
         }

         // TODO

         if (!bNeedsUpdate)
         {
            bNeedsUpdate = true;
            if (mCollisionChief != null)
            {
               mCollisionChief.UpdateUnit(this);
            }
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

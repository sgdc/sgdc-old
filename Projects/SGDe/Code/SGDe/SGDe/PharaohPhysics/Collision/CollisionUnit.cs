using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SGDE
{
    public class CollisionUnit : SceneNode
    {
        public const int CIRCLE_COLLISION = 0;
        public const int LINE_COLLISION = 1;
        private CollisionChief mCollisionChief;
        private Texture2D mCollisionMask;
        private int mCollisionType;
        private Entity mOwner;
        private bool bPixelCollision;
        private bool bNeedsUpdate;
        private bool bHasCollisions;
        private Vector2 mCircleCenter;
        private int mCircleRadius;
        private Vector2 mLineStart;
        private Vector2 mLineEnd;
        private List<CollisionUnit> mCollisions;
        private List<CollisionUnit> mCheckedUnits;  // for testing
        private CollisionUnit mLastCheckedBy;
        private int mCheckTimeStamp;

        public CollisionUnit(Entity owner, Vector2 center, int radius, Texture2D collisionMask, bool bUsePixelCollision)
        {
            InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
            mCircleCenter = center;
            mCircleRadius = radius;
            mCollisionType = CIRCLE_COLLISION;
        }

        public CollisionUnit(Entity owner, Vector2 lineStart, Vector2 lineEnd, Texture2D collisionMask, bool bUsePixelCollision)
        {
            InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
            mLineStart = lineStart;
            mLineEnd = lineEnd;
            mCollisionType = LINE_COLLISION;
        }

        // location: top left of collisionMask image
        public CollisionUnit(Entity owner, Vector2 location, Texture2D collisionMask, bool bUsePixelCollision)
        {
            InitializeCommonStuff(owner, collisionMask, bUsePixelCollision);
            CalculateCircle(location);
            mCollisionType = CIRCLE_COLLISION;
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
            return mLineStart;
        }

        public Vector2 GetLineEnd()
        {
            return mLineEnd;
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

            mCheckedUnits.Add(other);

            if (mCollisionType == CIRCLE_COLLISION && other.GetCollisionType() == CIRCLE_COLLISION)
            {
                otherCircleCenter = other.GetCircleCenter();
                otherCircleRadius = other.GetCircleRadius();

                dX = mCircleCenter.X - otherCircleCenter.X;
                dY = mCircleCenter.Y - otherCircleCenter.Y;
                dist = (dX * dX) + (dY * dY);

                // (radius + radius)^2 instead of more expensive squareRoot(dist)
                return (dist <= (mCircleRadius + otherCircleRadius) * (mCircleRadius + otherCircleRadius));
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
        }

        public void AddCollision(CollisionUnit other)
        {
            if (!mCollisions.Contains(other))
            {
                mCollisions.Add(other);

                if (!bHasCollisions)
                {
                    bHasCollisions = true;
                }
            }
        }

        public void RemoveCollision(CollisionUnit other)
        {
            mCollisions.Remove(other);

            bHasCollisions = mCollisions.Count > 0;
        }

        public override void Translate(float x, float y)
        {
            base.Translate(x, y);
            if (mCollisionChief != null)
            {
                mCollisionChief.TranslateCollisionUnit(this, x, y);
            }

            if (mCollisionType == CIRCLE_COLLISION)
            {
                mCircleCenter.X += x;
                mCircleCenter.Y += y;
            }
            else if (mCollisionType == LINE_COLLISION)
            {
                mLineStart.X += x;
                mLineStart.Y += y;
                mLineEnd.X += x;
                mLineEnd.Y += y;
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

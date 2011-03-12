using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SGDE.Physics.Collision;

namespace Tyrus_and_Randall
{
    class Enemy : Entity
    {
        private Boolean onGround;
        private enum EnemyDirection { Right, Left, StandingRight, StandingLeft };
        private EnemyDirection dir;

        public Enemy()
            : this(0, 0)
        {
        }

        public Enemy(float x = 0, float y = 0)
            : base(x, y)
        {
            onGround = false;
            id = 4;
            dir = EnemyDirection.StandingRight;
        }

        public Enemy(Vector2 position)
            : this(position.X, position.Y)
        {
        }

        /*protected override void SetUpCollision()
        {
            SetCollisionUnit(new CollisionUnit(this, image.GetTranslation(), image.GetTranslation() + new Vector2(image.Width, image.Height), CollisionUnit.CollisionType.COLLISION_BOX, null, false));
            this.mCollisionUnit.SetSolid(true);
        }*/

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.GetVelocity().X > 0)
            {
                if(dir != EnemyDirection.Right)
                {
                    dir = EnemyDirection.Right;
                    this.SpriteImage.SetAnimation("EnemyWalkRight");
                }
            }
            else if (this.GetVelocity().X < 0)
            {
                if (dir != EnemyDirection.Left)
                {
                    dir = EnemyDirection.Left;
                    this.SpriteImage.SetAnimation("EnemyWalkLeft");
                }
            }
            else
            {
                if (dir == EnemyDirection.Right)
                    dir = EnemyDirection.StandingRight;
                else if (dir == EnemyDirection.Left)
                    dir = EnemyDirection.StandingLeft;
            }
        }

        public override void CollisionChange()
        {
            onGround = false;
            if (!mPhysBaby.IsStatic())
            {
                foreach (CollisionUnit other in GetCollisionUnit().GetCollisions())
                {
                    if (other.IsSolid() && ((Entity)other.GetParent()).GetID() != 1 && ((Entity)other.GetParent()).GetID() != 3 && ((Entity)(other.GetParent())).GetID() != 4 && ((Entity)(other.GetParent())).GetID() != 8)
                    {
                        Vector2 intersect = GetIntersectionRectangle(this.GetCollisionUnit(), other);

                        if (Math.Abs(intersect.X) > Math.Abs(intersect.Y) && !intersect.Equals(Vector2.Zero))
                        {
                            this.SetVelocity(this.GetVelocity().X, 0.0f);
                            if (intersect.Y <= 0)
                            {
                                this.SetTranslation(new Vector2(this.GetTranslation().X, this.GetTranslation().Y + intersect.Y - 0.01f));
                                onGround = true;
                            }
                            else
                            {
                                this.SetTranslation(new Vector2(this.GetTranslation().X, this.GetTranslation().Y + intersect.Y + 0.01f));
                            }
                        }
                        else if (!intersect.Equals(Vector2.Zero))
                        {
                            if (((Entity)other.GetParent()).GetID() != 1)
                            {
                                this.SetVelocity(-this.GetVelocity().X, this.GetVelocity().Y);
                            }
                            if (intersect.X <= 0)
                            {
                                this.SetTranslation(new Vector2(this.GetTranslation().X + intersect.X - 0.01f, this.GetTranslation().Y));
                            }
                            else
                            {
                                this.SetTranslation(new Vector2(this.GetTranslation().X + intersect.X + 0.01f, this.GetTranslation().Y));
                            }
                        }
                    }
                }
            }
        }

        protected static Vector2 GetIntersectionRectangle(CollisionUnit A, CollisionUnit B)
        {
            // Calculate half sizes.
            float halfWidthA = (A.GetLowerRight().X - A.GetUpperLeft().X) / 2.0f;
            float halfHeightA = (A.GetLowerRight().Y - A.GetUpperLeft().Y) / 2.0f;
            float halfWidthB = (B.GetLowerRight().X - B.GetUpperLeft().X) / 2.0f;
            float halfHeightB = (B.GetLowerRight().Y - B.GetUpperLeft().Y) / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(A.GetUpperLeft().X + halfWidthA, A.GetUpperLeft().Y + halfHeightA);
            Vector2 centerB = new Vector2(B.GetUpperLeft().X + halfWidthB, B.GetUpperLeft().Y + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }
    }
}
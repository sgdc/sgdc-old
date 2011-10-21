using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyPolarBear.GameObjects;

namespace MyPolarBear.AI
{
    class FollowPlayerAI
    {
        private Entity mFollower;
        private Entity mLeader;

        public FollowPlayerAI(Entity follower, Entity leader)
        {
            mFollower = follower;
            mLeader = leader;
        }

        public bool DoAI(GameTime gameTime)
        {
            //beFollowing(gameTime);

            //return true;

            if (mLeader == null)
            {
                foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
                {
                    if (ent is PolarBear)
                    {
                        mLeader = (PolarBear)ent;
                        break;
                    }
                }
            }

            if (mLeader == null || !((PolarBear)mLeader).IsAlive)
            {
                return false;
            }

            beBoidLike();

            return true;
        }

        //private void beFollowing(GameTime gameTime)
        //{
        //    if (mLeader == null)
        //    {
        //        foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
        //        {
        //            if (ent is PolarBear)
        //            {
        //                mLeader = (PolarBear)ent;
        //                break;
        //            }
        //        }
        //    }

        //    if (mLeader == null || !((PolarBear)mLeader).IsAlive)
        //    {
        //        return false;
        //    }

        //    beBoidLike();
        //}

        private void beBoidLike()
        {
            Vector2 velocityFollow = new Vector2(0, 0);
            Vector2 velocityMoveWith = new Vector2(0, 0);
            Vector2 velocityAvoid = new Vector2(0, 0);
            int followerCount = 0;
            float dist = 0;

            if (mLeader != null)
            {
                velocityFollow = mLeader.Position - mFollower.Position;
                dist = Math.Abs(velocityFollow.Length());

                if (dist > 200)
                {
                    velocityFollow.Normalize();
                    velocityFollow *= 200;
                }
                else if (dist <= 60)
                {
                    velocityFollow = Vector2.Zero;
                }
            }

            foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
            {
                //if (ent is Enemy && ent != mFollower && ((Enemy)ent).CurrentState == State.Following)
                if (ent != mFollower && ent != mLeader)
                {
                    dist = Math.Abs((mFollower.Position - ent.Position).Length());

                    if (dist < 30)
                    {
                        //velocityMoveWith += ((Enemy)ent).Velocity;
                        velocityMoveWith += ent.Velocity;
                        followerCount++;
                    }

                    if (dist < 10)
                    {
                        //velocityAvoid += mFollower.Position - ((Enemy)ent).Position;
                        velocityAvoid += mFollower.Position - ent.Position;
                    }
                }
            }

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {
                if (mFollower.CollisionBox.Intersects(element.CollisionRect) && !(element.Type.Equals("Ice")
                    || element.Type.Equals("SoftGround") || element.Type.Equals("BabyPlant")))
                {
                    velocityAvoid += mFollower.Position - element.Position;
                }
            }

            if (followerCount > 1)
            {
                velocityMoveWith = velocityMoveWith / followerCount;
            }

            dist = Math.Abs((mFollower.Position - mLeader.Position).Length());
            if (mLeader != null && dist < 30)
            {
                velocityAvoid += mFollower.Position - mLeader.Position;
            }

            mFollower.Velocity = mFollower.Velocity * 0.2f + velocityFollow * 0.02f + velocityMoveWith * 0.0f + velocityAvoid * 0.2f;
            //Velocity.Normalize();
            //Velocity *= 1.0f;
        }
    }
}

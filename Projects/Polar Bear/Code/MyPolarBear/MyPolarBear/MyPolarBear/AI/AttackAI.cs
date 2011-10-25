using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyPolarBear.Interfaces;
using MyPolarBear.Content;

namespace MyPolarBear.AI
{
    class AttackAI : AIComponent
    {
        private String mTargetType;
        private ITargetable mTarget;
        private Entity mAttacker;

        public AttackAI(Entity attacker, String targetType)
            : base()
        {
            mAttacker = attacker;
            mTargetType = targetType;

            mTarget = null;
        }

        public override void LoadContent()
        {
            mTexture = ContentManager.GetTexture("AttackCommand");
        }

        public override void DoAI(GameTime gameTime)
        {
            CurrentState = State.Good;

            if (mTarget == null)
            {
                getTarget();
            }

            if (mTarget == null)
            {
                CurrentState = State.Problem;
            }
            else
            {
                attackTarget();
            }
        }

        private void getTarget()
        {
            int currDistance = 0;
            int bestDistance = 1000000;

            // find closest target
            foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
            {
                if (ent.GetTargetType().Equals(mTargetType) && ent != mAttacker)
                {
                    currDistance = (int)(Math.Abs(ent.Position.X - mAttacker.Position.X) + Math.Abs(ent.Position.Y - mAttacker.Position.Y));
                    if (currDistance < bestDistance)
                    {
                        bestDistance = currDistance;
                        mTarget = ent;
                    }
                }
            }

            // if still haven't found
            if (mTarget == null)
            {
                currDistance = 0;
                bestDistance = 1000000;

                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.GetTargetType().Equals(mTargetType))
                    {
                        currDistance = (int)(Math.Abs(ele.Position.X - mAttacker.Position.X) + Math.Abs(ele.Position.Y - mAttacker.Position.Y));
                        if (currDistance < bestDistance)
                        {
                            bestDistance = currDistance;
                            mTarget = ele;
                        }
                    }
                }
            }
        }

        private void attackTarget()
        {
            Rectangle biggerRect = new Rectangle(mAttacker.CollisionBox.X - mAttacker.CollisionBox.Width / 2, 
                mAttacker.CollisionBox.Y - mAttacker.CollisionBox.Height / 2, mAttacker.CollisionBox.Width * 2, mAttacker.CollisionBox.Height * 2);

            //if (mAttacker.CollisionBox.Intersects(mTarget.GetCollisionRect()))
            if (biggerRect.Intersects(mTarget.GetCollisionRect()))
            {
                // attack
                if (mTarget is IDamageable)
                {
                    ((IDamageable)mTarget).TakeDamage(1, "fire", mAttacker);
                }

                CurrentState = State.Done;
                mTarget = null;
            }
            else
            {
                Vector2 dir = mTarget.GetPosition() - mAttacker.Position;
                dir.Normalize();
                mAttacker.Velocity = dir * 3.0f;
            }
        }
    }
}

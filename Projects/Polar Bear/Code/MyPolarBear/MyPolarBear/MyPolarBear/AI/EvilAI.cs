using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyPolarBear.GameObjects;
using MyPolarBear.Content;
using MyPolarBear.GameScreens;
using MyPolarBear.Pathfinding;
using MyPolarBear.Audio;

namespace MyPolarBear.AI
{
    class EvilAI
    {
        private Entity mEnt;
        private Entity mTargetBear;
        //private List<Vector2> mPath;
        //private int pathPos;
        private int attackCounter;
        private int attackTimer;

        public EvilAI(Entity ent)
        {
            mEnt = ent;
            mTargetBear = null;
        }

        public bool DoAI(GameTime gameTime)
        {
            if (mTargetBear == null)
            {
                foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
                {
                    if (ent is PolarBear)
                    {
                        mTargetBear = (PolarBear)ent;
                        break;
                    }
                }
                //mPath = null;
                //pathPos = 0;
            }

            // polar bear not found for some reason
            if (mTargetBear == null)
            {
                //dealWithCollisions();
                return true;
            }

            // try straight line attack
            Vector2 direction = mTargetBear.Position - mEnt.Position;
            if (direction.Length() <= 500)
            {
                direction.Normalize();
                mEnt.Velocity = direction * 3.0f;
            }

            Rectangle moveRect = new Rectangle(mEnt.CollisionBox.X + (int)mEnt.Velocity.X, mEnt.CollisionBox.Y + (int)mEnt.Velocity.Y,
                mEnt.CollisionBox.Width, mEnt.CollisionBox.Height);

            foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
            {
                if (moveRect.Intersects(ele.CollisionRect) && !(ele.Type.Equals("Ice") || ele.Type.Equals("SoftGround")))
                {
                    attackLevelElement(ele, gameTime);
                }
            }

            if (moveRect.Intersects(mTargetBear.CollisionBox))
            {
                if (mTargetBear is PolarBear && ((PolarBear)mTargetBear).IsAlive)
                {
                    //PolarBear.CurHitPoints -= 1;
                    PolarBear.TakeDamage(1, mEnt);
                    //SoundManager.PlaySound("Ow");
                    mEnt.Velocity = Vector2.Zero;
                }
            }

            foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
            {
                if (moveRect.Intersects(ent.CollisionBox) && ent is Enemy && ((Enemy)ent).CurrentState != Enemy.State.Evil
                    && ((Enemy)ent).CurrentState != Enemy.State.Afraid)
                {
                    ((Enemy)ent).CurrentState = Enemy.State.Evil;
                    //SoundManager.PlaySound("Roar");
                }
            }

            if (attackCounter == 0)
            {
                //dealWithCollisions();
            }
            else
            {
                if ((mEnt.Position.X > GameScreens.GameScreen.WORLDWIDTH / 2 && mEnt.Velocity.X > 0) || (mEnt.Position.X < -GameScreens.GameScreen.WORLDWIDTH / 2 && mEnt.Velocity.X < 0))
                {
                    mEnt.Velocity = new Vector2(mEnt.Velocity.X * -1, mEnt.Velocity.Y);
                }

                if ((mEnt.Position.Y > GameScreens.GameScreen.WORLDHEIGHT / 2 && mEnt.Velocity.Y > 0) || (mEnt.Position.Y < -GameScreens.GameScreen.WORLDHEIGHT / 2 && mEnt.Velocity.Y < 0))
                {
                    mEnt.Velocity = new Vector2(mEnt.Velocity.X, mEnt.Velocity.Y * -1);
                }
            }

            return true;
        }

        private void attackLevelElement(LevelElement ele, GameTime gameTime)
        {
            attackTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (attackTimer > 500)
            {
                attackCounter++;
            }

            if (attackCounter >= 3)
            {
                if (ele.Type.Equals("Tree") || ele.Type.Equals("Tree2"))
                {
                    ele.Type = "Stump";
                    ele.Tex = ContentManager.GetTexture("Stump");
                    GameScreen.CurWorldHealth--;
                    SoundManager.PlaySound("Thump");
                }
                else if (ele.Type.Equals("Stump") || ele.Type.Equals("BabyPlant"))
                {
                    if (ele.Type.Equals("BabyPlant"))
                    {
                        GameScreen.CurWorldHealth--;
                    }
                    ele.Type = "SoftGround";
                    ele.Tex = ContentManager.GetTexture("SoftGround");
                    AGrid.GetInstance().addResource(ele);
                    SoundManager.PlaySound("Thump");
                }

                attackCounter = 0;
                attackTimer = 0;
            }
        }
    }
}

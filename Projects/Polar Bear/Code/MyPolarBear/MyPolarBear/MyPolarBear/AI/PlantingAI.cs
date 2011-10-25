using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyPolarBear.Pathfinding;
using MyPolarBear.GameScreens;
using MyPolarBear.Content;
using MyPolarBear.Audio;

namespace MyPolarBear.AI
{
    class PlantingAI : AIComponent
    {
        private Entity mPlanter;
        private List<Vector2> path;
        private bool bHasSeed;
        private bool bHasPath;
        private int pathPos;

        public PlantingAI(Entity planter)
            :base()
        {
            mPlanter = planter;
        }

        public override void DoAI(GameTime gameTime)
        {
            CurrentState = State.Good;

            if (bHasSeed)
            {
                Rectangle moveRect = new Rectangle(mPlanter.CollisionBox.X + (int)mPlanter.Velocity.X, mPlanter.CollisionBox.Y + (int)mPlanter.Velocity.Y, mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Height);

                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.Type.Equals("SoftGround") && (moveRect.Intersects(ele.CollisionRect)
                        || mPlanter.CollisionBox.Intersects(ele.CollisionRect)))
                    {
                        ele.Type = "BabyPlant";
                        ele.Tex = ContentManager.GetTexture("BabyPlant");
                        AGrid.GetInstance().addResource(ele);
                        GameScreen.CurWorldHealth++;
                        SoundManager.PlaySound("PlantSeed");
                        bHasSeed = false;
                        bHasPath = false;
                        path = null;
                        mPlanter.Velocity *= -1;

                        //return true;
                        return;
                    }
                }
            }
            else
            {
                Rectangle moveRect = new Rectangle(mPlanter.CollisionBox.X + (int)mPlanter.Velocity.X, mPlanter.CollisionBox.Y + (int)mPlanter.Velocity.Y, mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Height);

                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.Type.Equals("Bush") && (moveRect.Intersects(ele.CollisionRect)
                        || mPlanter.CollisionBox.Intersects(ele.CollisionRect)))
                    {
                        bHasSeed = true;
                        bHasPath = false;
                        path = null;
                        mPlanter.Velocity *= -1;
                        SoundManager.PlaySound("PickSeed");
                        //return true;
                        return;
                    }
                }
            }


            if (!bHasSeed && !bHasPath)
            {
                path = AGrid.GetInstance().getPath(mPlanter.Position, ANode.SEED_SOURCE);
                if (path == null)
                {
                    bHasSeed = false;
                    bHasPath = false;
                    pathPos = 0;
                    //CurrentState = State.Aimless;
                    //return false;
                    CurrentState = State.Problem;
                    return;
                }
                bHasPath = true;
                pathPos = 0;
            }
            else if (!bHasPath)
            {
                int currDistance = 0;
                int bestDistance = 1000000;
                LevelElement targetSoftGround = null;

                // find closest soft ground spot
                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.Type.Equals("SoftGround"))
                    {
                        currDistance = (int)(Math.Abs(ele.Position.X - mPlanter.Position.X) + Math.Abs(ele.Position.Y - mPlanter.Position.Y));
                        if (currDistance < bestDistance)
                        {
                            bestDistance = currDistance;
                            targetSoftGround = ele;
                        }
                    }
                }

                //path = AGrid.GetInstance().getPath(Position, ANode.PLANT_AREA);
                if (targetSoftGround != null)
                {
                    path = AGrid.GetInstance().getPath(mPlanter.Position, targetSoftGround.Position);

                    if (path == null)
                    {
                        //bHasSeed = false;
                        bHasPath = false;
                        pathPos = 0;
                        //CurrentState = State.Aimless;
                        //return false;
                        CurrentState = State.Problem;
                        return;
                    }

                    bHasPath = true;
                    pathPos = 0;
                }
            }

            if (bHasPath && path != null && path.Count > 0)
            {
                if ((int)mPlanter.Position.X < (int)path[pathPos].X + 20 && (int)mPlanter.Position.X > (int)path[pathPos].X - 20
                        && (int)mPlanter.Position.Y < (int)path[pathPos].Y + 20 && (int)mPlanter.Position.Y > (int)path[pathPos].Y - 20)
                {
                    pathPos++;
                }

                if (path.Count > pathPos)
                {
                    Vector2 dir = path[pathPos] - mPlanter.Position;
                    dir.Normalize();
                    mPlanter.Velocity = dir * 4;
                }
                else
                {
                    path = null;
                    pathPos = 0;
                    bHasPath = false;

                    Rectangle biggerRect = new Rectangle(mPlanter.CollisionBox.X - mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Y - mPlanter.CollisionBox.Height, mPlanter.CollisionBox.Width * 3, mPlanter.CollisionBox.Height * 3);

                    if (bHasSeed)
                    {
                        //Rectangle biggerRect = new Rectangle(CollisionBox.X - CollisionBox.Width, CollisionBox.Y - CollisionBox.Height, CollisionBox.Width * 2, CollisionBox.Height * 2);

                        foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                        {
                            if (ele.Type.Equals("SoftGround") && (ele.CollisionRect.Intersects(mPlanter.CollisionBox)
                                || biggerRect.Intersects(ele.CollisionRect)))
                            {
                                ele.Type = "BabyPlant";
                                ele.Tex = ContentManager.GetTexture("BabyPlant");
                                AGrid.GetInstance().addResource(ele);
                                //AGrid.GetInstance().mGrid[(int)((ele.Position.X + 2048) / 50), (int)((ele.Position.Y + 2048) / 50)].Type = ANode.NOT_SPECIAL;
                                GameScreen.CurWorldHealth++;
                                bHasSeed = false;
                                SoundManager.PlaySound("PlantSeed");
                                break;
                            }
                        }

                        if (bHasSeed)
                        {
                            bHasPath = false;
                            path = null;
                            //CurrentState = State.Aimless;

                            //////////////
                            //return false;
                            CurrentState = State.Problem;
                            return;
                        }
                    }
                    else
                    {
                        foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                        {
                            if (ele.Type.Equals("Bush") && (ele.CollisionRect.Intersects(mPlanter.CollisionBox)
                                || biggerRect.Intersects(ele.CollisionRect)))
                            {
                                bHasSeed = true;
                                SoundManager.PlaySound("PickSeed");
                                break;
                            }
                        }

                        if (!bHasSeed)
                        {
                            bHasPath = false;
                            path = null;
                            //CurrentState = State.Aimless;

                            ////////////////
                            //return false;
                            CurrentState = State.Problem;
                            return;
                        }
                    }
                }
            }
            else
            {
                bHasPath = false;
            }

            if ((mPlanter.Position.X > GameScreens.GameScreen.WORLDWIDTH / 2 && mPlanter.Velocity.X > 0) || (mPlanter.Position.X < -GameScreens.GameScreen.WORLDWIDTH / 2 && mPlanter.Velocity.X < 0))
            {
                mPlanter.Velocity = new Vector2(mPlanter.Velocity.X * -1, mPlanter.Velocity.Y);
            }

            if ((mPlanter.Position.Y > GameScreens.GameScreen.WORLDHEIGHT / 2 && mPlanter.Velocity.Y > 0) || (mPlanter.Position.Y < -GameScreens.GameScreen.WORLDHEIGHT / 2 && mPlanter.Velocity.Y < 0))
            {
                mPlanter.Velocity = new Vector2(mPlanter.Velocity.X, mPlanter.Velocity.Y * -1);
            }

            // collision stuff
            if (bHasSeed)
            {
                Rectangle moveRect = new Rectangle(mPlanter.CollisionBox.X + (int)mPlanter.Velocity.X, mPlanter.CollisionBox.Y + (int)mPlanter.Velocity.Y, mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Height);

                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.Type.Equals("SoftGround") && (moveRect.Intersects(ele.CollisionRect)
                        || mPlanter.CollisionBox.Intersects(ele.CollisionRect)))
                    {
                        ele.Type = "BabyPlant";
                        ele.Tex = ContentManager.GetTexture("BabyPlant");
                        GameScreen.CurWorldHealth++;
                        AGrid.GetInstance().addResource(ele);
                        SoundManager.PlaySound("PlantSeed");
                        bHasSeed = false;
                        bHasPath = false;
                        path = null;
                        //Velocity *= -1;
                        mPlanter.Velocity = ((mPlanter.Position - ele.Position) / 20);

                        //return true;
                        return;
                    }
                }
            }
            else
            {
                Rectangle moveRect = new Rectangle(mPlanter.CollisionBox.X + (int)mPlanter.Velocity.X, mPlanter.CollisionBox.Y + (int)mPlanter.Velocity.Y, mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Height);

                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.Type.Equals("Bush") && (moveRect.Intersects(ele.CollisionRect)
                        || mPlanter.CollisionBox.Intersects(ele.CollisionRect)))
                    {
                        bHasSeed = true;
                        bHasPath = false;
                        path = null;
                        SoundManager.PlaySound("PlantSeed");
                        //Velocity *= -1;
                        mPlanter.Velocity = ((mPlanter.Position - ele.Position) / 20);

                        //return true;
                        return;
                    }
                }
            }

            Rectangle moverRect = new Rectangle(mPlanter.CollisionBox.X + (int)mPlanter.Velocity.X, mPlanter.CollisionBox.Y + (int)mPlanter.Velocity.Y, mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Height);
            foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
            {
                if (moverRect.Intersects(ele.CollisionRect) && !(ele.Type.Equals("Ice")
                    || ele.Type.Equals("SoftGround") || ele.Type.Equals("BabyPlant")))
                {
                    bHasPath = false;
                    path = null;
                    //Velocity *= -1;
                    mPlanter.Velocity = ((mPlanter.Position - ele.Position) / 20);
                    //CurrentState = State.Aimless;

                    //return false;
                    CurrentState = State.Problem;
                    return;
                }
            }

            //return true;
        }
    }
}

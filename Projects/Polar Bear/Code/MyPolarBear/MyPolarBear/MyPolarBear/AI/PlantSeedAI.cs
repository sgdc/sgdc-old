using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPolarBear.GameObjects;
using Microsoft.Xna.Framework;
using MyPolarBear.Content;
using MyPolarBear.Pathfinding;
using MyPolarBear.GameScreens;
using MyPolarBear.Audio;

namespace MyPolarBear.AI
{
    class PlantSeedAI : AIComponent
    {
        private Entity mPlanter;
        private SeedPouch mSeedPouch;
        private List<Vector2> path;
        private bool bHasPath;
        private int pathPos;

        public PlantSeedAI(Entity planter, SeedPouch pouch)
            : base()
        {
            mPlanter = planter;
            mSeedPouch = pouch;

            path = null;
            bHasPath = false;
            pathPos = 0;
        }

        public override void LoadContent()
        {
            mTexture = ContentManager.GetTexture("PlantCommand");
        }

        public override void DoAI(GameTime gameTime)
        {
            CurrentState = State.Good;

            plantSeed();

            if (CurrentState != State.Good)
            {
                return;
            }

            if (!bHasPath)
            {
                findSoftGround();
            }

            if (bHasPath && path != null && path.Count > 0)
            {
                goToSoftGround();
            }
            else
            {
                bHasPath = false;
                //findSoftGround();
            }
        }

        private void plantSeed()
        {
            //Rectangle moveRect = new Rectangle(mPlanter.CollisionBox.X + (int)mPlanter.Velocity.X, mPlanter.CollisionBox.Y + (int)mPlanter.Velocity.Y, mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Height);
            Rectangle biggerRect = new Rectangle(mPlanter.CollisionBox.X - mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Y - mPlanter.CollisionBox.Height, mPlanter.CollisionBox.Width * 3, mPlanter.CollisionBox.Height * 3);

            foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
            {
                if (ele.Type.Equals("SoftGround") && (biggerRect.Intersects(ele.CollisionRect)
                    || mPlanter.CollisionBox.Intersects(ele.CollisionRect)))
                {
                    if (mSeedPouch.RemoveSeeds(1) >= 1)
                    {
                        ele.Type = "BabyPlant";
                        ele.Tex = ContentManager.GetTexture("BabyPlant");
                        AGrid.GetInstance().addResource(ele);
                        GameScreen.CurWorldHealth++;
                        SoundManager.PlaySound("PlantSeed");
                        //bHasSeed = false;
                        //if (mSeedPouch.RemoveSeeds(1) >= 1)
                        //{
                        CurrentState = State.Done;
                    }
                    else
                    {
                        CurrentState = State.Problem;
                        //CurrentState = State.Done;
                    }
                    bHasPath = false;
                    path = null;
                    mPlanter.Velocity *= -1;

                    return;
                }
            }
        }

        private void findSoftGround()
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

            //path = AGrid.GetInstance().getPath(mPlanter.Position, ANode.PLANT_AREA);
            if (targetSoftGround != null)
            {
                path = AGrid.GetInstance().getPath(mPlanter.Position, targetSoftGround.Position);

                if (path == null)
                {
                    //bHasSeed = false;
                    bHasPath = false;
                    pathPos = 0;
                    CurrentState = State.Problem;
                }
                else
                {
                    bHasPath = true;
                    pathPos = 0;
                }
            }
        }

        private void goToSoftGround()
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
                CurrentState = State.Problem;
            }
        }
    }
}

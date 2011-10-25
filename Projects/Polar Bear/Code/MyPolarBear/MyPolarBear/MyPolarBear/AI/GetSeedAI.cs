using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyPolarBear.Audio;
using MyPolarBear.Pathfinding;
using MyPolarBear.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using MyPolarBear.Content;

namespace MyPolarBear.AI
{
    class GetSeedAI : AIComponent
    {
        private Entity mPlanter;
        private SeedPouch mSeedPouch;
        private List<Vector2> path;
        private bool bHasSeed;
        private bool bHasPath;
        private int pathPos;
        //private Texture2D mTexture;

        public GetSeedAI(Entity planter, SeedPouch pouch)
            : base()
        {
            mPlanter = planter;
            mSeedPouch = pouch;
            bHasSeed = false;
            bHasPath = false;
            pathPos = 0;

            //LoadContent();
        }

        public override void LoadContent()
        {
            mTexture = ContentManager.GetTexture("SeedCommand");
        }

        public override void ResetAI()
        {
            base.ResetAI();

            bHasPath = false;
            bHasSeed = false;
            pathPos = 0;
        }

        public override void DoAI(GameTime gameTime)
        {
            CurrentState = State.Good;
            bHasSeed = false;

            pickSeed();

            if (bHasSeed)
            {
                CurrentState = State.Done;
                return;
            }

            if (!bHasPath)
            {
                findBush();
            }

            if (bHasPath && path != null && path.Count > 0)
            {
                goToBush();
            }
            else
            {
                bHasPath = false;
                //findBush();
            }
        }

        private void pickSeed()
        {
            //Rectangle moveRect = new Rectangle(mPlanter.CollisionBox.X + (int)mPlanter.Velocity.X, mPlanter.CollisionBox.Y + (int)mPlanter.Velocity.Y, mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Height);
            Rectangle biggerRect = new Rectangle(mPlanter.CollisionBox.X - mPlanter.CollisionBox.Width, mPlanter.CollisionBox.Y - mPlanter.CollisionBox.Height, mPlanter.CollisionBox.Width * 3, mPlanter.CollisionBox.Height * 3);

            foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
            {
                if (ele.Type.Equals("Bush") && (biggerRect.Intersects(ele.CollisionRect)
                    || mPlanter.CollisionBox.Intersects(ele.CollisionRect)))
                {
                    if (mSeedPouch.AddSeeds(1) >= 1)
                    {
                        bHasSeed = true;
                        SoundManager.PlaySound("PickSeed");
                    }
                    else
                    {
                        CurrentState = State.Problem;
                    }

                    //mSeedPouch.AddSeeds(1);
                    //if (mSeedPouch.AddSeeds(1) < 1)
                    //{
                    //    CurrentState = State.Problem;
                    //}

                    bHasPath = false;
                    path = null;
                    mPlanter.Velocity *= -1;
                    //SoundManager.PlaySound("PickSeed");
                    return;
                }
            }
        }

        private void findBush()
        {
            path = AGrid.GetInstance().getPath(mPlanter.Position, ANode.SEED_SOURCE);
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

        private void goToBush()
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

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear.Content;
using MyPolarBear.Pathfinding;
using MyPolarBear.GameScreens;

namespace MyPolarBear.GameObjects
{
    class Enemy : Entity
    {
        public enum State
        {
            Aimless,
            Afraid,
            Following,
            Planting,
            Evil
        }

        public State CurrentState;

        public bool IsAlive;
        Random random = new Random();

        private PolarBear followBear;
        //public bool bFollowBear;
        private Vector2 mScale;

        public bool bHasSeenSeedGather;
        public bool bHasSeenPlanting;
        public double retryTimer;
        public int stuckCounter;
        public int stuckTimer;

        private List<Vector2> path;
        //private List<Vector2> softGroundPath;
        private bool bHasSeed;
        private bool bHasPath;
        private int pathPos;

        public Enemy(Vector2 position)
            : base(position)
        {            
            IsAlive = true;            
            followBear = null;
            //bFollowBear = false;
            Scale = 1;
            mScale = new Vector2(Scale, Scale);
            CurrentState = State.Aimless;
        }

        public override void LoadContent()
        {
            Animation ani = new Animation(ContentManager.GetTexture("BrownBearWalkRight"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkRight", ani);

            ani = new Animation(ContentManager.GetTexture("BrownBearWalkRight"), 4, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("walkLeft", ani);

            ani = new Animation(ContentManager.GetTexture("BrownBearWalkFront"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkFront", ani);

            ani = new Animation(ContentManager.GetTexture("BrownBearWalkBack"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkBack", ani);

            base.LoadContent();

            if (CollisionBox.Width == 0 || CollisionBox.Height == 0)
            {
                CollisionBox = new Rectangle(CollisionBox.X, CollisionBox.Y, 40, 40);
            }
        }

        public override void Update(GameTime gameTime)
        {
            switch (CurrentState)
            {
                case State.Aimless:
                    beAimless(gameTime);
                    break;
                case State.Afraid:
                    beAfraid(gameTime);
                    break;
                case State.Following:
                    beFollowing(gameTime);
                    break;
                case State.Planting:
                    bePlanting(gameTime);
                    break;
                case State.Evil:
                    beEvil(gameTime);
                    break;
                default:
                    beAimless(gameTime);
                    break;
            }

            if (Velocity.X > 0 && Velocity.X > Velocity.Y && Velocity.X > Velocity.Y * -1)
            {
                mAnimator.PlayAnimation("walkRight", false);
            }
            else if (Velocity.X < 0 && Velocity.X * -1 > Velocity.Y && Velocity.X * -1 > Velocity.Y * -1)
            {
                mAnimator.PlayAnimation("walkLeft", false);
            }
            else if (Velocity.Y > 0)
            {
                mAnimator.PlayAnimation("walkFront", false);
            }
            else
            {
                mAnimator.PlayAnimation("walkBack", false);
            }

            //dealWithCollisions();

            Position += Velocity;

            base.Update(gameTime);
        }

        private void beAimless(GameTime gameTime)
        {
            if (bHasSeenPlanting)
            {
                retryTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (retryTimer / 1000 > 5)
                {
                    CurrentState = State.Planting;
                    retryTimer = 0;
                    return;
                }
            }

            stuckTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (stuckTimer / 1000 > 5)
            {
                if (stuckCounter > 100)
                {
                    Position = new Vector2(350, 350);
                }

                stuckCounter = 0;
                stuckTimer = 0;
            }


            if ((Position.X > GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X > 0) || (Position.X < -GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X < 0))
            {
                Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            }

            if ((Position.Y > GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y > 0) || (Position.Y < -GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y < 0))
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
            }

            // collide with level elements
            Rectangle travelRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {
                if (travelRect.Intersects(element.CollisionRect) && !(element.Type.Equals("Ice")
                    || element.Type.Equals("SoftGround") || element.Type.Equals("BabyPlant")))
                {
                    stuckCounter++;
                    Velocity = ((Position - element.Position) / 20);
                }
            }
        }

        private void beAfraid(GameTime gameTime)
        {

        }

        // follow player around and learn from his behavior
        private void beFollowing(GameTime gameTime)
        {
            if (followBear == null)
            {
                foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
                {
                    if (ent is PolarBear)
                    {
                        followBear = (PolarBear)ent;
                        break;
                    }
                }
            }

            if (followBear != null)
            {
                Vector2 direction = followBear.Position - Position;
                if (Math.Abs(direction.Length()) > 30)
                {
                    direction.Normalize();
                    Velocity = direction * 3.0f;
                }
                else
                {
                    Velocity = Vector2.Zero;
                }
            }

            dealWithCollisions();
        }

        private void bePlanting(GameTime gameTime)
        {
            if (bHasSeed)
            {
                Rectangle moveRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.Type.Equals("SoftGround") && (moveRect.Intersects(ele.CollisionRect)
                        || CollisionBox.Intersects(ele.CollisionRect)))
                    {
                        ele.Type = "BabyPlant";
                        ele.Tex = ContentManager.GetTexture("BabyPlant");
                        GameScreen.CurWorldHealth++;

                        bHasSeed = false;
                        bHasPath = false;
                        path = null;
                        Velocity *= -1;

                        return;
                    }
                }
            }
            else
            {
                Rectangle moveRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.Type.Equals("Bush") && (moveRect.Intersects(ele.CollisionRect)
                        || CollisionBox.Intersects(ele.CollisionRect)))
                    {
                        bHasSeed = true;
                        bHasPath = false;
                        path = null;
                        Velocity *= -1;

                        return;
                    }
                }
            }


            if (!bHasSeed && !bHasPath)
            {
                path = AGrid.GetInstance().getPath(Position, ANode.SEED_SOURCE);
                if (path == null)
                {
                    bHasSeed = false;
                    bHasPath = false;
                    pathPos = 0;
                    CurrentState = State.Aimless;
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
                        currDistance = (int)(Math.Abs(ele.Position.X - Position.X) + Math.Abs(ele.Position.Y - Position.Y));
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
                    path = AGrid.GetInstance().getPath(Position, targetSoftGround.Position);

                    if (path == null)
                    {
                        //bHasSeed = false;
                        bHasPath = false;
                        pathPos = 0;
                        CurrentState = State.Aimless;
                        return;
                    }

                    bHasPath = true;
                    pathPos = 0;
                }
            }

            if (bHasPath && path != null && path.Count > 0)
            {
                if ((int)Position.X < (int)path[pathPos].X + 20 && (int)Position.X > (int)path[pathPos].X - 20
                        && (int)Position.Y < (int)path[pathPos].Y + 20 && (int)Position.Y > (int)path[pathPos].Y - 20)
                {
                    pathPos++;
                }

                if (path.Count > pathPos)
                {
                    Vector2 dir = path[pathPos] - Position;
                    dir.Normalize();
                    Velocity = dir * 4;
                }
                else
                {
                    path = null;
                    pathPos = 0;
                    bHasPath = false;

                    Rectangle biggerRect = new Rectangle(CollisionBox.X - CollisionBox.Width, CollisionBox.Y - CollisionBox.Height, CollisionBox.Width * 3, CollisionBox.Height * 3);

                    if (bHasSeed)
                    {
                        //Rectangle biggerRect = new Rectangle(CollisionBox.X - CollisionBox.Width, CollisionBox.Y - CollisionBox.Height, CollisionBox.Width * 2, CollisionBox.Height * 2);

                        foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                        {
                            if (ele.Type.Equals("SoftGround") && (ele.CollisionRect.Intersects(CollisionBox)
                                || biggerRect.Intersects(ele.CollisionRect)))
                            {
                                ele.Type = "BabyPlant";
                                ele.Tex = ContentManager.GetTexture("BabyPlant");
                                //AGrid.GetInstance().mGrid[(int)((ele.Position.X + 2048) / 50), (int)((ele.Position.Y + 2048) / 50)].Type = ANode.NOT_SPECIAL;
                                GameScreen.CurWorldHealth++;
                                bHasSeed = false;
                                break;
                            }
                        }

                        if (bHasSeed)
                        {
                            bHasPath = false;
                            path = null;
                            CurrentState = State.Aimless;
                        }
                    }
                    else
                    {
                        foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                        {
                            if (ele.Type.Equals("Bush") && (ele.CollisionRect.Intersects(CollisionBox)
                                || biggerRect.Intersects(ele.CollisionRect)))
                            {
                                bHasSeed = true;
                                break;
                            }
                        }

                        if (!bHasSeed)
                        {
                            bHasPath = false;
                            path = null;
                            CurrentState = State.Aimless;
                        }
                    }
                }

                //foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                //{
                //    if (!ele.Type.Equals("Bush") && !ele.Type.Equals("SoftGround") && CollisionBox.Intersects(ele.CollisionRect))
                //    {
                //        //int adjustmentX = ele.CollisionRect.Width - (CollisionBox.Center.X - ele.CollisionRect.Center.X);
                //        //int adjustmentY = ele.CollisionRect.Height - (CollisionBox.Center.Y - ele.CollisionRect.Center.Y);

                //        //Position += new Vector2(adjustmentX, adjustmentY);
                //        //Position += new Vector2(10, 10);
                //    }
                //}
            }
            else
            {
                bHasPath = false;
                //dealWithCollisions();
            }

            if ((Position.X > GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X > 0) || (Position.X < -GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X < 0))
            {
                Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            }

            if ((Position.Y > GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y > 0) || (Position.Y < -GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y < 0))
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
            }

            // collision stuff
            if (bHasSeed)
            {
                Rectangle moveRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.Type.Equals("SoftGround") && (moveRect.Intersects(ele.CollisionRect)
                        || CollisionBox.Intersects(ele.CollisionRect)))
                    {
                        ele.Type = "Tree";
                        ele.Tex = ContentManager.GetTexture("Tree");
                        GameScreen.CurWorldHealth++;

                        bHasSeed = false;
                        bHasPath = false;
                        path = null;
                        //Velocity *= -1;
                        Velocity = ((Position - ele.Position) / 20);

                        return;
                    }
                }
            }
            else
            {
                Rectangle moveRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

                foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
                {
                    if (ele.Type.Equals("Bush") && (moveRect.Intersects(ele.CollisionRect)
                        || CollisionBox.Intersects(ele.CollisionRect)))
                    {
                        bHasSeed = true;
                        bHasPath = false;
                        path = null;
                        //Velocity *= -1;
                        Velocity = ((Position - ele.Position) / 20);

                        return;
                    }
                }
            }

            Rectangle moverRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);
            foreach (LevelElement ele in UpdateKeeper.getInstance().getLevelElements())
            {
                if (moverRect.Intersects(ele.CollisionRect) && !(ele.Type.Equals("Ice")
                    || ele.Type.Equals("SoftGround") || ele.Type.Equals("BabyPlant")))
                {
                    bHasPath = false;
                    path = null;
                    //Velocity *= -1;
                    Velocity = ((Position - ele.Position) / 20);
                    CurrentState = State.Aimless;

                    return;
                }
            }
        }

        private void beEvil(GameTime gameTime)
        {

        }

        private void dealWithCollisions()
        {
            if ((Position.X > GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X > 0) || (Position.X < -GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X < 0))
            {
                Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            }

            if ((Position.Y > GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y > 0) || (Position.Y < -GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y < 0))
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
            }

            // collide with level elements
            Rectangle travelRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {
                if (travelRect.Intersects(element.CollisionRect) && !(element.Type.Equals("Ice")
                    || element.Type.Equals("SoftGround") || element.Type.Equals("BabyPlant")))
                {
                    Velocity *= -1;
                }
            }
        }

        //private void considerLevelElements()
        //{
        //    // collide with level elements
        //    Rectangle travelRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

        //    foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
        //    {
        //        if (travelRect.Intersects(element.CollisionRect))
        //        {
        //            //Velocity = Vector2.Zero;
        //            Velocity *= -1;

        //            //if (element.Type.Equals("Bush"))
        //            //{
        //            //    NumSeeds++;
        //            //}

        //        }
        //    }
        //}

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Texture, Position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            //int rectWidth = Texture.Width;
            //int rectHeight = Texture.Height / 4;
            //Rectangle sourceRect = new Rectangle(0, rectHeight * (aniFrame / 8), rectWidth, rectHeight);

            //rectWidth *= 2;
            //rectHeight *= 2;
            //Rectangle destRect = new Rectangle((int)Position.X - rectWidth / 2, (int)Position.Y - rectHeight / 2, rectWidth, rectHeight);
            //spriteBatch.Draw(Texture, destRect, sourceRect, Color.White);

            //aniFrame++;
            //if (aniFrame % 32 == 0)
            //{
            //    aniFrame = 0;
            //}

            mScale.X = Scale;
            mScale.Y = Scale;

            //spriteBatch.Draw(ContentManager.GetTexture("HardRock"), CollisionBox, Color.White);
            mAnimator.Draw(spriteBatch, Position, mScale, Color.White, Rotation, Origin, 0);
        }
    }
}

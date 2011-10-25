using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear.Content;
using MyPolarBear.Pathfinding;
using MyPolarBear.GameScreens;
using MyPolarBear.Audio;
using MyPolarBear.AI;

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
            DoingCommands,
            Evil
        }

        private State mCurrentState;
        public State CurrentState
        {
            get
            {
                return mCurrentState;
            }
            set
            {
                changeState(value);
            }
        }

        public bool IsAlive;
        Random random = new Random();

        private PolarBear followBear;
        private Vector2 mScale;

        //public bool bHasSeenSeedGather;
        //public bool bHasSeenPlanting;
        public double retryTimer;
        public int stuckCounter;
        public int stuckTimer;

        private FollowPlayerAI mFollowPlayerAI;
        private AfraidAI mAfraidAI;
        private PlantingAI mPlantingAI;
        private EvilAI mEvilAI;

        private GetSeedAI mGetSeedAI;
        private PlantSeedAI mPlantSeedAI;
        private bool bCurrGetSeed;

        private List<AIComponent> mCommands;
        private int mNumCommands;
        private int mNextCommandIndex;
        private AIComponent mCurrCommand;

        public SeedPouch Pouch;
        private bool bListening;

        public Enemy(Vector2 position)
            : base(position)
        {            
            IsAlive = true;            
            followBear = null;
            Scale = 1;
            mScale = new Vector2(Scale, Scale);
            CurrentState = State.Aimless;

            mFollowPlayerAI = new FollowPlayerAI(this, followBear);
            mAfraidAI = new AfraidAI(this);
            mPlantingAI = new PlantingAI(this);
            mEvilAI = new EvilAI(this);

            SeedPouch pouch = new SeedPouch(1);
            mGetSeedAI = new GetSeedAI(this, pouch);
            mPlantSeedAI = new PlantSeedAI(this, pouch);
            bCurrGetSeed = true;

            mCommands = new List<AIComponent>();
            mNumCommands = 0;
            mNextCommandIndex = 0;
            mCurrCommand = null;

            AddCommand(mGetSeedAI);
            AddCommand(mPlantSeedAI);

            Pouch = new SeedPouch(2);
            bListening = false;
        }

        public override void LoadContent()
        {
            // good bears
            Animation ani = new Animation(ContentManager.GetTexture("BrownBearWalkRight"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkRight", ani);

            ani = new Animation(ContentManager.GetTexture("BrownBearWalkRight"), 4, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("walkLeft", ani);

            ani = new Animation(ContentManager.GetTexture("BrownBearWalkFront"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkFront", ani);

            ani = new Animation(ContentManager.GetTexture("BrownBearWalkBack"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkBack", ani);

            // evil bears
            ani = new Animation(ContentManager.GetTexture("WoodBearWalkRight"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("evilwalkRight", ani);

            ani = new Animation(ContentManager.GetTexture("WoodBearWalkRight"), 5, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("evilwalkLeft", ani);

            ani = new Animation(ContentManager.GetTexture("WoodBearWalkFront"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("evilwalkFront", ani);

            ani = new Animation(ContentManager.GetTexture("WoodBearWalkBack"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("evilwalkBack", ani);

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
                    //doCommands(gameTime);
                    break;
                case State.Evil:
                    beEvil(gameTime);
                    break;
                case State.DoingCommands:
                    doCommands(gameTime);
                    break;
                default:
                    beAimless(gameTime);
                    break;
            }

            PlayAppropriateAnimation();

            Position += Velocity;

            base.Update(gameTime);
        }

        public void PlayAppropriateAnimation()
        {
            if (Velocity.X > 0 && Velocity.X > Velocity.Y && Velocity.X > Velocity.Y * -1)
            {
                if (CurrentState == State.Evil)
                {
                    mAnimator.PlayAnimation("evilwalkRight", false);
                }
                else
                {
                    mAnimator.PlayAnimation("walkRight", false);
                }
            }
            else if (Velocity.X < 0 && Velocity.X * -1 > Velocity.Y && Velocity.X * -1 > Velocity.Y * -1)
            {
                if (CurrentState == State.Evil)
                {
                    mAnimator.PlayAnimation("evilwalkLeft", false);
                }
                else
                {
                    mAnimator.PlayAnimation("walkLeft", false);
                }
            }
            else if (Velocity.Y > 0)
            {
                if (CurrentState == State.Evil)
                {
                    mAnimator.PlayAnimation("evilwalkFront", false);
                }
                else
                {
                    mAnimator.PlayAnimation("walkFront", false);
                }
            }
            else
            {
                if (CurrentState == State.Evil)
                {
                    mAnimator.PlayAnimation("evilwalkBack", false);
                }
                else
                {
                    mAnimator.PlayAnimation("walkBack", false);
                }
            }
        }

        public void changeState(State newState)
        {
            if (mCurrentState == newState)
            {
                return;
            }

            bListening = false;
            mCurrentState = newState;

            switch (newState)
            {
                case State.Aimless:
                    stuckCounter = 0;
                    break;
                case State.Afraid:
                    SoundManager.PlaySound("OnFire");
                    break;
                case State.Following:
                    ClearCommands();
                    break;
                case State.Planting:
                    // nothing
                    break;
                case State.Evil:
                    SoundManager.PlaySound("Roar");
                    ClearCommands();
                    break;
                case State.DoingCommands:
                    // nothing
                    break;
                default:
                    // nothing
                    break;
            }
        }

        private void checkStuck(GameTime gameTime)
        {
            stuckTimer += gameTime.ElapsedGameTime.Milliseconds;

            int stuckThresholdTime = 1000;
            int stuckThresholdHits = 20;
            if (CurrentState == State.Aimless)
            {
                stuckThresholdTime *= 3;
                stuckThresholdHits *= 3;
            }

            if (stuckTimer > stuckThresholdTime)
            {
                if (stuckCounter > stuckThresholdHits)
                {
                    if (CurrentState == State.Aimless)
                    {
                        Position = new Vector2(350, 350);
                    }
                    else
                    {
                        changeState(State.Aimless);
                    }
                }

                stuckCounter = 0;
                stuckTimer = 0;
            }
        }

        private void beAimless(GameTime gameTime)
        {
            //if (bHasSeenPlanting)
            if (mNumCommands > 0 && !bListening)
            {
                retryTimer += gameTime.ElapsedGameTime.Milliseconds;                
                if (retryTimer / 1000 > 5)
                {
                    //CurrentState = State.Planting;
                    changeState(State.DoingCommands);
                    retryTimer = 0;
                    return;
                }                
            }

            //stuckTimer += gameTime.ElapsedGameTime.Milliseconds;
            //if (stuckTimer > 5000)
            //{
            //    if (stuckCounter > 100)
            //    {
            //        Position = new Vector2(350, 350);
            //    }

            //    stuckCounter = 0;
            //    stuckTimer = 0;
            //}
            checkStuck(gameTime);

            dealWithCollisions();
        }

        private void beAfraid(GameTime gameTime)
        {
            //bool stillAfraid = mAfraidAI.DoAI(gameTime);
            mAfraidAI.DoAI(gameTime);

            //if (!stillAfraid)
            if (mAfraidAI.CurrentState != AIComponent.State.Good)
            {
                changeState(State.Aimless);
            }

            dealWithCollisions();
        }

        // follow player around and learn from his behavior
        private void beFollowing(GameTime gameTime)
        {
            //bool stillFollowing = mFollowPlayerAI.DoAI(gameTime);
            mFollowPlayerAI.DoAI(gameTime);
            //if (!stillFollowing)
            if (mFollowPlayerAI.CurrentState != AIComponent.State.Good)
            {
                changeState(State.Aimless);
            }
            //mFollowPlayerAI.DoAI(gameTime);
        }

        private void bePlanting(GameTime gameTime)
        {
            //mPlantingAI.DoAI(gameTime);

            //if (mPlantingAI.CurrentState != AIComponent.State.Good)
            //{
            //    changeState(State.Aimless);
            //}

            if (bCurrGetSeed)
            {
                mGetSeedAI.DoAI(gameTime);

                if (mGetSeedAI.CurrentState == AIComponent.State.Done)
                {
                    bCurrGetSeed = false;
                }
                else if (mGetSeedAI.CurrentState == AIComponent.State.Problem)
                {
                    changeState(State.Aimless);
                }
            }
            else
            {
                mPlantSeedAI.DoAI(gameTime);

                if (mPlantSeedAI.CurrentState == AIComponent.State.Done)
                {
                    bCurrGetSeed = true;
                }
                else if (mPlantSeedAI.CurrentState == AIComponent.State.Problem)
                {
                    changeState(State.Aimless);
                }
            }

            dealWithCollisions();
        }

        private void beEvil(GameTime gameTime)
        {
            //bool stillEvil = mEvilAI.DoAI(gameTime);
            mEvilAI.DoAI(gameTime);

            //if (!stillEvil)
            if (mEvilAI.CurrentState != AIComponent.State.Good)
            {
                changeState(State.Aimless);
            }

            dealWithCollisions();
        }

        private void dealWithCollisions()
        {
            bool wasCollision = false;

            if ((Position.X > GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X > 0) || (Position.X < -GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X < 0))
            {
                Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
                wasCollision = true;
            }

            if ((Position.Y > GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y > 0) || (Position.Y < -GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y < 0))
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
                wasCollision = true;
            }

            // collide with level elements
            Rectangle travelRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

            foreach (Entity entity in UpdateKeeper.getInstance().getEntities())
            {
                if (!(entity is Enemy) && entity != this && entity != followBear && travelRect.Intersects(entity.CollisionBox))
                {
                    Velocity = ((Position - entity.Position) * 0.05f);
                    wasCollision = true;
                }
            }

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {
                if (travelRect.Intersects(element.CollisionRect) && !(element.Type.Equals("Ice")
                    || element.Type.Equals("SoftGround") || element.Type.Equals("BabyPlant")))
                {
                    //Velocity *= -1;
                    stuckCounter++;
                    Velocity = ((Position - element.Position) * 0.05f);
                    wasCollision = true;
                }
            }

            if (wasCollision && CurrentState == State.Planting)// || CurrentState == State.DoingCommands)
            {
                changeState(State.Aimless);
            }
        }

        //public void AddCommand(String command)
        public void AddCommand(AIComponent command)
        {
            if (bListening)
            {
                mCommands.Add(command);
                mNumCommands++;
            }
        }

        public void ListenForCommands()
        {
            bListening = true;
            ClearCommands();
        }

        public void StartCommands()
        {
            bListening = false;
            changeState(State.DoingCommands);
        }

        public void ClearCommands()
        {
            mCommands.Clear();
            mNumCommands = 0;
            mNextCommandIndex = 0;
            mCurrCommand = null;
        }

        private void doCommands(GameTime gameTime)
        {
            if (mNumCommands <= 0)
            {
                changeState(State.Aimless);
                return;
            }

            if (mCurrCommand == null || mCurrCommand.CurrentState == AIComponent.State.Done)
            {
                if (mNextCommandIndex >= mNumCommands)
                {
                    mNextCommandIndex = 0;
                }
                mCurrCommand = mCommands[mNextCommandIndex];
                mNextCommandIndex++;
            }

            mCurrCommand.DoAI(gameTime);

            if (mCurrCommand.CurrentState == AIComponent.State.Problem)
            {
                changeState(State.Aimless);
            }

            checkStuck(gameTime);
            dealWithCollisions();
        }

        public override String GetTargetType()
        {
            if (CurrentState == State.Evil)
            {
                return "Enemy";
            }
            else
            {
                return "Bear";
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mScale.X = Scale;
            mScale.Y = Scale;

            mAnimator.Draw(spriteBatch, Position, mScale, Color.White, Rotation, Origin, 0);

            if (CurrentState == State.Afraid)
            {
                spriteBatch.Draw(ContentManager.GetTexture("FireAttack"), Position, Color.White);
            }

            if (mNumCommands >= 1 && bListening)
            {
                //mCommands[0].Draw(spriteBatch, Position);
                drawCommands(spriteBatch);
            }

            if (!bListening && mCurrCommand != null)
            {
                mCurrCommand.Draw(spriteBatch, new Vector2(Position.X, Position.Y - 20));
            }
        }

        private void drawCommands(SpriteBatch spriteBatch)
        {
            Vector2 drawPos = Position;
            int commandWidth = 20;
            int commandHeight = 20;
            drawPos.X -= commandWidth * (mNumCommands * 0.5f);
            //drawPos.Y -= commandHeight * (mNumCommands * 0.5f);
            drawPos.Y -= commandHeight;

            for (int i = 0; i < mNumCommands; i++)
            {
                mCommands[i].Draw(spriteBatch, drawPos);
                drawPos.X += commandWidth;
            }
        }
    }
}

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

        public bool bHasSeenSeedGather;
        public bool bHasSeenPlanting;
        public double retryTimer;
        public int stuckCounter;
        public int stuckTimer;

        private FollowPlayerAI mFollowPlayerAI;
        private AfraidAI mAfraidAI;
        private PlantingAI mPlantingAI;
        private EvilAI mEvilAI;

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

            //dealWithCollisions();

            Position += Velocity;

            base.Update(gameTime);
        }

        public void changeState(State newState)
        {
            if (mCurrentState == newState)
            {
                return;
            }

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
                    // nothing
                    break;
                case State.Planting:
                    // nothing
                    break;
                case State.Evil:
                    SoundManager.PlaySound("Roar");
                    break;
                default:
                    // nothing
                    break;
            }
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
            if (stuckTimer > 5000)
            {
                if (stuckCounter > 100)
                {
                    Position = new Vector2(350, 350);
                }

                stuckCounter = 0;
                stuckTimer = 0;
            }

            dealWithCollisions();
        }

        private void beAfraid(GameTime gameTime)
        {
            bool stillAfraid = mAfraidAI.DoAI(gameTime);

            if (!stillAfraid)
            {
                changeState(State.Aimless);
            }

            dealWithCollisions();
        }

        // follow player around and learn from his behavior
        private void beFollowing(GameTime gameTime)
        {
            bool stillFollowing = mFollowPlayerAI.DoAI(gameTime);
            if (!stillFollowing)
            {
                changeState(State.Aimless);
            }
            //mFollowPlayerAI.DoAI(gameTime);
        }

        private void bePlanting(GameTime gameTime)
        {
            bool stillPlanting = mPlantingAI.DoAI(gameTime);
            if (!stillPlanting)
            {
                changeState(State.Aimless);
            }
        }

        private void beEvil(GameTime gameTime)
        {
            bool stillEvil = mEvilAI.DoAI(gameTime);

            if (!stillEvil)
            {
                changeState(State.Aimless);
            }

            dealWithCollisions();
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

            foreach (Entity entity in UpdateKeeper.getInstance().getEntities())
            {
                if (entity != this && entity != followBear && travelRect.Intersects(entity.CollisionBox))
                {
                    Velocity = ((Position - entity.Position) * 0.05f);
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
                }
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
        }
    }
}

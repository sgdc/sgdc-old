using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear.Content;
using MyPolarBear.Audio;
using MyPolarBear.AI;

namespace MyPolarBear.GameObjects
{
    class Animal : Entity
    {
        //private PolarBear followBear;
        public Random random = new Random();                
        private Vector2 mScale;
        private FollowPlayerAI mFollowPlayerAI;

        public enum Types
        {
            Tiger, 
            Lion,
            Panther
        }

        public enum States
        {
            Following,
            Paired,
            Idle
        }

        public enum Genders
        {
            Male,
            Female
        }

        public Types Type;
        public States State;
        public Genders Gender;

        public Animal(Vector2 position, Types type, Genders gender)
            : base(position)
        {                        
            //followBear = null;            
            Type = type;
            Gender = gender;
            State = States.Idle;
            Scale = 1;
            mScale = new Vector2(Scale, Scale);

            mFollowPlayerAI = new FollowPlayerAI(this, null);
        }


        public override void LoadContent()
        {
            //Add animal textures or spritesheets
            Animation ani = new Animation(ContentManager.GetTexture("TigerIdle"), 2, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("TigerIdle", ani);
            ani = new Animation(ContentManager.GetTexture("TigerWalkRight"), 2, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("TigerWalkRight", ani);
            ani = new Animation(ContentManager.GetTexture("TigerWalkRight"), 2, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("TigerWalkLeft", ani);
            ani = new Animation(ContentManager.GetTexture("TigerWalkBack"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("TigerWalkBack", ani);
            ani = new Animation(ContentManager.GetTexture("TigerWalkFront"), 2, 16, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("TigerWalkFront", ani);

            ani = new Animation(ContentManager.GetTexture("LionIdle"), 2, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("LionIdle", ani);
            ani = new Animation(ContentManager.GetTexture("LionWalkRight"), 2, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("LionWalkRight", ani);
            ani = new Animation(ContentManager.GetTexture("LionWalkRight"), 2, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("LionWalkLeft", ani);
            ani = new Animation(ContentManager.GetTexture("LionWalkBack"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("LionWalkBack", ani);
            ani = new Animation(ContentManager.GetTexture("LionWalkFront"), 2, 16, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("LionWalkFront", ani);

            ani = new Animation(ContentManager.GetTexture("PantherIdle"), 2, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("PantherIdle", ani);
            ani = new Animation(ContentManager.GetTexture("PantherWalkRight"), 2, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("PantherWalkRight", ani);
            ani = new Animation(ContentManager.GetTexture("PantherWalkRight"), 2, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("PantherWalkLeft", ani);
            ani = new Animation(ContentManager.GetTexture("PantherWalkBack"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("PantherWalkBack", ani);
            ani = new Animation(ContentManager.GetTexture("PantherWalkFront"), 2, 16, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("PantherWalkFront", ani);

            switch (Type)
            {
                case Types.Tiger: mAnimator.PlayAnimation("TigerIdle", false);
                    break;
                case Types.Lion: mAnimator.PlayAnimation("LionIdle", false);
                    break;
                case Types.Panther: mAnimator.PlayAnimation("PantherIdle", false);
                    break;
            }
            
            CollisionBox = new Rectangle(CollisionBox.X, CollisionBox.Y, 25, 25);

            base.LoadContent();
        }

        private void SelectAnimation()
        {
            if (Velocity.X > 0 && Velocity.X > Velocity.Y && Velocity.X > Velocity.Y * -1)
            {
                switch (Type)
                {
                    case Types.Tiger: mAnimator.PlayAnimation("TigerWalkRight", false);
                        break;
                    case Types.Lion: mAnimator.PlayAnimation("LionWalkRight", false);
                        break;
                    case Types.Panther: mAnimator.PlayAnimation("PantherWalkRight", false);
                        break;
                }
            }
            else if (Velocity.X < 0 && Velocity.X * -1 > Velocity.Y && Velocity.X * -1 > Velocity.Y * -1)
            {
                switch (Type)
                {
                    case Types.Tiger: mAnimator.PlayAnimation("TigerWalkLeft", false);
                        break;
                    case Types.Lion: mAnimator.PlayAnimation("LionWalkLeft", false);
                        break;
                    case Types.Panther: mAnimator.PlayAnimation("PantherWalkLeft", false);
                        break;
                }
            }
            else if (Velocity.Y > 0)
            {
                switch (Type)
                {
                    case Types.Tiger: mAnimator.PlayAnimation("TigerWalkFront", false);
                        break;
                    case Types.Lion: mAnimator.PlayAnimation("LionWalkFront", false);
                        break;
                    case Types.Panther: mAnimator.PlayAnimation("PantherWalkFront", false);
                        break;
                }
            }
            else if (Velocity.Y < 0)
            {
                switch (Type)
                {
                    case Types.Tiger: mAnimator.PlayAnimation("TigerWalkBack", false);
                        break;
                    case Types.Lion: mAnimator.PlayAnimation("LionWalkBack", false);
                        break;
                    case Types.Panther: mAnimator.PlayAnimation("PantherWalkBack", false);
                        break;
                }
            }
            else
            {
                switch (Type)
                {
                    case Types.Tiger: mAnimator.PlayAnimation("TigerIdle", false);
                        break;
                    case Types.Lion: mAnimator.PlayAnimation("LionIdle", false);
                        break;
                    case Types.Panther: mAnimator.PlayAnimation("PantherIdle", false);
                        break;
                }
            }
        }

        public override String GetTargetType()
        {
            return "Animal";
        }

        public override void Update(GameTime gameTime)
        {
            //if (followBear == null)
            //{
            //    foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
            //    {
            //        if (ent is PolarBear)
            //        {
            //            followBear = (PolarBear)ent;
            //            break;
            //        }
            //    }
            //}

            //if (State != States.Paired)
            //{
            //    if (followBear != null && State == States.Following)
            //    {
            //        Vector2 direction = followBear.Position - Position;
            //        if (Math.Abs(direction.Length()) > 40)
            //        {
            //            direction.Normalize();
            //            Velocity = direction * 3.0f;
            //        }
            //        else
            //            Velocity = Vector2.Zero;
            //    }
            //}
            if (State == States.Following)
            {
                mFollowPlayerAI.DoAI(gameTime);

                //if (!mFollowPlayerAI.DoAI(gameTime))
                if (mFollowPlayerAI.CurrentState != AIComponent.State.Good)
                {
                    State = States.Idle;
                    Velocity = Vector2.Zero;
                }
            }
            else if (State == States.Paired)
            {
                Velocity = Vector2.Zero;
                SelectAnimation();
                return;
            }

            SelectAnimation();

            foreach (Entity entity in UpdateKeeper.getInstance().getEntities())
            {
                if (entity is Projectile)
                {
                    if (entity.CollisionBox.Intersects(CollisionBox))
                    {
                        if (PolarBear.power == PolarBear.Power.Normal && State == States.Idle)
                        {
                            State = States.Following;
                            ((Projectile)entity).IsAlive = false;
                            SoundManager.PlaySound("Growl");
                        }
                    }
                }
                
                if (entity is Animal && State == States.Following)
                {
                    if ( ((Animal)entity).Type == Type && ((Animal)entity).Gender != Gender)
                    {
                        if (entity.CollisionBox.Intersects(CollisionBox))
                        {
                            State = States.Paired;
                            SoundManager.PlaySound("Growl");
                            GameScreens.GameScreen.NumAnimals++;
                        }
                    }
                }
            }

            //if (!followBear.IsAlive)
            //{
            //    State = States.Idle;
            //    Velocity = Vector2.Zero;
            //}


            Rectangle travelRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {
                if (travelRect.Intersects(element.CollisionRect))
                {
                    if (!(element.Type.Equals("BabyPlant") || element.Type.Equals("SoftGround") || element.Type.Equals("Ice")))
                        Velocity *= -1;
                }
            }           

            Position += Velocity;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            mScale.X = Scale;
            mScale.Y = Scale;

            mAnimator.Draw(spriteBatch, Position, mScale, Color.White, Rotation, Origin, 0);
        }
    }
}

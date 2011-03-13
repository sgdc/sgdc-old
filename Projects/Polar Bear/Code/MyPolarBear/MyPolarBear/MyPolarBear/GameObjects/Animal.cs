using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear.Content;

namespace MyPolarBear.GameObjects
{
    class Animal : Entity
    {
        private PolarBear followBear;
        
        private Vector2 mScale;              

        public enum Species
        {
            Tiger, 
            Lion
        }

        public enum CurrentState
        {
            Following,
            Paired,
            Idle
        }

        public Species Type;
        public CurrentState State;

        public Animal(Vector2 position, Species type)
            : base(position)
        {                        
            followBear = null;            
            Type = type;
            State = CurrentState.Idle;
            Scale = 1;
            mScale = new Vector2(Scale, Scale);            
        }


        public override void LoadContent()
        {
            //Add animal textures or spritesheets
            Animation ani = new Animation(ContentManager.GetTexture("TigerIdle"), 2, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("TigerIdle", ani);
            
            ani = new Animation(ContentManager.GetTexture("LionIdle"), 2, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("LionIdle", ani);

            switch (Type)
            {
                case Species.Tiger: mAnimator.PlayAnimation("TigerIdle", false);
                    break;
                case Species.Lion: mAnimator.PlayAnimation("LionIdle", false);
                    break;
            }
            
            CollisionBox = new Rectangle(CollisionBox.X, CollisionBox.Y, 25, 25);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
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

            if (State != CurrentState.Paired)
            {
                if (followBear != null && State == CurrentState.Following)
                {
                    Vector2 dist = followBear.Position - Position;
                    if (Math.Abs(dist.Length()) > 15)
                    {
                        dist.Normalize();
                        Velocity = dist * 2.0f;
                    }
                    else
                        Velocity = Vector2.Zero;
                }
            }
            else
            {
                Velocity = Vector2.Zero;
            }

            foreach (Entity entity in UpdateKeeper.getInstance().getEntities())
            {
                if (entity is Projectile)
                {
                    if (entity.CollisionBox.Intersects(CollisionBox))
                    {
                        if (PolarBear.power == PolarBear.Power.Normal && State == CurrentState.Idle)
                            State = CurrentState.Following;
                    }
                }
                
                if (entity is Animal && ((Animal)entity).Type == Type)
                {
                    if (entity.CollisionBox.Intersects(CollisionBox))
                    {
                        //State = CurrentState.Paired;
                    }
                }
            }


            Rectangle travelRect = new Rectangle(CollisionBox.X + (int)Velocity.X, CollisionBox.Y + (int)Velocity.Y, CollisionBox.Width, CollisionBox.Height);

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {
                if (travelRect.Intersects(element.CollisionRect))
                {
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

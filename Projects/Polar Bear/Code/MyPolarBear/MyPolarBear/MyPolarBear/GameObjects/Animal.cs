using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear.Content;

namespace MyPolarBear.GameObjects
{
    class Animal : Entity
    {
        public bool IsAlive;
        Random random = new Random();

        private PolarBear followBear;
        public bool bFollowBear;
        private Vector2 mScale;

        public Animal(Vector2 position)
            : base(position)
        {            
            IsAlive = true;            
            followBear = null;
            bFollowBear = false;
            Scale = 1;
            mScale = new Vector2(Scale, Scale);            
        }


        public override void LoadContent()
        {
            //Add animal textures or spritesheets

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

            if (followBear != null && bFollowBear)
            {                
                Vector2 dist = followBear.Position - Position;
                if (Math.Abs(dist.X) > 10 || Math.Abs(dist.Y) > 10)
                {
                    dist.Normalize();
                    Velocity = dist * 2.0f;
                }
                else
                {
                    Velocity = Vector2.Zero;
                }                
            }

            foreach (Entity entity in UpdateKeeper.getInstance().getEntities())
            {
                if (entity is Projectile)
                {
                    if (entity.CollisionBox.Intersects(CollisionBox))
                    {
                        if (PolarBear.power == PolarBear.Power.Normal)
                            bFollowBear = true;
                    }
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

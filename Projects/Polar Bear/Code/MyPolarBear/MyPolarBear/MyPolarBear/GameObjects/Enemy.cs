using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear.Content;

namespace MyPolarBear.GameObjects
{
    class Enemy : Entity
    {
        public int aniFrame;
        public bool moveRight;
        public bool moveDown;
        public bool alive;
        Random random = new Random();
        public Vector2 followVelocity;
        public Vector2 swarmPos;

        //private Animator mAnimator;
        private PolarBear followBear;
        public bool bFollowBear;
        private Vector2 mScale;

        public Enemy(Vector2 position)
            : base(position)
        {
            moveRight = true;
            alive = true;
            moveDown = true;
            followVelocity = new Vector2(0, 0);
            swarmPos = new Vector2(0, 0);
            followBear = null;
            bFollowBear = false;
            Scale = 2;
            mScale = new Vector2(Scale, Scale);
            //mAnimator = new Animator();
        }

        public override void LoadContent()
        {
            //Texture = ContentManager.GetTexture("FireWalkRight");

            Animation ani = new Animation(ContentManager.GetTexture("FireWalkingRight"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkRight", ani);

            ani = new Animation(ContentManager.GetTexture("FireWalkingRight"), 4, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("walkLeft", ani);

            ani = new Animation(ContentManager.GetTexture("FireWalkingFront"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkFront", ani);

            ani = new Animation(ContentManager.GetTexture("FireWalkingBack"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkBack", ani);

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
                //followVelocity = (followBear.Position - Position) * 10;
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

                //Velocity = new Vector2(5, 0);
            }

            if ((Position.X > GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X > 0) || (Position.X < -GameScreens.GameScreen.WORLDWIDTH / 2 && Velocity.X < 0))
            {
                //Velocity = new Vector2(0, Velocity.Y);
                Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            }

            if ((Position.Y > GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y > 0) || (Position.Y < -GameScreens.GameScreen.WORLDHEIGHT / 2 && Velocity.Y < 0))
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
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


            Position += Velocity;

            base.Update(gameTime);
        }

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

            mAnimator.Draw(spriteBatch, Position, mScale, Color.White, Rotation, Origin, 0);
        }
    }
}

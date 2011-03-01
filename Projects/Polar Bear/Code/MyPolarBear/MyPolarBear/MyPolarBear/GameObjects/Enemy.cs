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

        private PolarBear followBear;
        public bool bFollowBear;

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
        }

        public override void LoadContent()
        {
            Texture = ContentManager.GetTexture("FireWalkRight");

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
                if (dist.X > 10 || dist.Y > 10)
                {
                    dist.Normalize();
                    Velocity = dist * 2.0f;
                }
                //Velocity = new Vector2(5, 0);
            }
            //else
            //{
                if ((Position.X > GameScreens.GameScreen.WORLDWIDTH - 400 && Velocity.X > 0) || (Position.X < 400 && Velocity.X < 0))
                {
                    //Velocity = new Vector2(0, Velocity.Y);
                    Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
                }

                if ((Position.Y > GameScreens.GameScreen.WORLDHEIGHT - 400 && Velocity.Y > 0) || (Position.Y < 400 && Velocity.Y < 0))
                {
                    Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
                }
            //}

            Position += Velocity;
            //if (alive)
            //{
            //    swarmPos += followVelocity;
            //}

            //if (Position.X > swarmPos.X + 500 && moveRight)
            //{
            //    moveRight = false;
            //    Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            //}

            //if (Position.X < swarmPos.X && !moveRight)
            //{
            //    moveRight = true;
            //    Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            //}

            //if (Position.Y > swarmPos.Y + 500 && moveDown)
            //{
            //    moveDown = false;
            //    Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
            //}

            //if (Position.Y < swarmPos.Y && !moveDown)
            //{
            //    moveDown = true;
            //    Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
            //}

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Texture, Position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            int rectWidth = Texture.Width;
            int rectHeight = Texture.Height / 4;
            Rectangle sourceRect = new Rectangle(0, rectHeight * (aniFrame / 8), rectWidth, rectHeight);

            rectWidth *= 2;
            rectHeight *= 2;
            Rectangle destRect = new Rectangle((int)Position.X - rectWidth / 2, (int)Position.Y - rectHeight / 2, rectWidth, rectHeight);
            spriteBatch.Draw(Texture, destRect, sourceRect, Color.White);

            aniFrame++;
            if (aniFrame % 32 == 0)
            {
                aniFrame = 0;
            }
        }
    }
}

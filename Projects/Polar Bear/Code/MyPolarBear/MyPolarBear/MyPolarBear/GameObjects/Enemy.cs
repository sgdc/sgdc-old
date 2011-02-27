using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public Enemy(Vector2 position)
            : base(position)
        {
            moveRight = true;
            alive = true;
            moveDown = true;
            followVelocity = new Vector2(0, 0);
            swarmPos = new Vector2(0, 0);
        }

        public override void Update(GameTime gameTime)
        {
            //if (moveRight)
            //{
            //    Position = new Vector2(Position.X + random.Next(0, 5), Position.Y);
            //}
            //else
            //{
            //    Position = new Vector2(Position.X - 10, Position.Y);
            //}

            Position += Velocity;
            if (alive)
            {
                swarmPos += followVelocity;
            }

            if (Position.X > swarmPos.X + 500 && moveRight)
            {
                moveRight = false;
                Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            }

            if (Position.X < swarmPos.X && !moveRight)
            {
                moveRight = true;
                Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            }

            if (Position.Y > swarmPos.Y + 500 && moveDown)
            {
                moveDown = false;
                Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
            }

            if (Position.Y < swarmPos.Y && !moveDown)
            {
                moveDown = true;
                Velocity = new Vector2(Velocity.X, Velocity.Y * -1);
            }

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

            //if (isFiring)
            //{
            aniFrame++;
            if (aniFrame % 32 == 0)
            {
                //isFiring = false;
                aniFrame = 0;
            }
            //}
        }
    }
}

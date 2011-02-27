using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear;

namespace MyPolarBear.GameObjects
{
    class PolarBear : Entity
    {        
        public enum Power
        {
            Normal,
            Ice,
            Fire,
            Grass
        }

        public Power power = Power.Normal;
        public int aniFrame;
        public bool isFiring;


        public PolarBear(Vector2 position)
            : base(position)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void SwitchPowers()
        {
            switch (power)
            {
                case Power.Normal:
                    power = Power.Ice;
                    Texture = Game1.GetTextureAt(1);
                    break;
                case Power.Ice:
                    power = Power.Fire;
                    Texture = Game1.GetTextureAt(2);
                    break;
                case Power.Fire:
                    power = Power.Grass;
                    Texture = Game1.GetTextureAt(3);
                    break;
                case Power.Grass:
                    power = Power.Normal;
                    Texture = Game1.GetTextureAt(0);
                    break;
            }
        }

        public Projectile ShootProjectile(Vector2 direction)
        {
            return new Projectile(Position, 10.0f, direction, power);
        }

        public void FireIce()
        {
            aniFrame = 0;
            isFiring = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Texture, Position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            int rectWidth = Texture.Width;
            int rectHeight = Texture.Height / 7;
            Rectangle sourceRect = new Rectangle(0, rectHeight * (aniFrame / 8), rectWidth, rectHeight);

            rectWidth *= 2;
            rectHeight *= 2;
            Rectangle destRect = new Rectangle((int)Position.X - rectWidth / 2, (int)Position.Y - rectHeight / 2, rectWidth, rectHeight);
            spriteBatch.Draw(Texture, destRect, sourceRect, Color.White);

            if (isFiring)
            {
                aniFrame++;
                if (aniFrame % 56 == 0)
                {
                    isFiring = false;
                    aniFrame = 0;
                }
            }
        }
    }
}

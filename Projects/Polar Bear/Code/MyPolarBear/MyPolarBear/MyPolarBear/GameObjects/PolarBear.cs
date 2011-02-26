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
    }
}

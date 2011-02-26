using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyPolarBear.GameObjects
{
    class Projectile : Entity
    {
        private PolarBear.Power _type;
        public PolarBear.Power Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private bool _isAlive;
        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }

        public Projectile(Vector2 position, float speed, Vector2 direction, PolarBear.Power type) 
            : base(position)
        {
            Position = position;
            Velocity = new Vector2(speed);
            Direction = Vector2.Normalize(direction);            
            Type = type;
            IsAlive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                switch (Type)
                {
                    case PolarBear.Power.Normal:
                        Texture = Game1.GetTextureAt(4);
                        break;
                    case PolarBear.Power.Ice:
                        Texture = Game1.GetTextureAt(5);
                        break;
                    case PolarBear.Power.Fire:
                        Texture = Game1.GetTextureAt(6);
                        break;
                    case PolarBear.Power.Grass:
                        Texture = Game1.GetTextureAt(7);
                        break;
                } 
                     
                Position += Direction * Velocity;

                base.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}

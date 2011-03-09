using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear.Content;

namespace MyPolarBear.GameObjects
{
    class Boss : Entity
    {

        public int Health = 100;
        public bool IsAlive = true;

        public Boss(Vector2 position)
            : base(position)
        {

        }

        public override void LoadContent()
        {
            Texture = ContentManager.GetTexture("ForestBoss");
            Scale = 2;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                foreach (Entity entity in UpdateKeeper.getInstance().getEntities())
                {
                    if (entity is Projectile)
                    {
                        if (CollisionBox.Intersects(entity.CollisionBox))
                        {
                            if (PolarBear.power == PolarBear.Power.Fire)
                            {
                                Scale -= 0.01f;
                                Health -= 1;                                
                            }
                            else if (PolarBear.power == PolarBear.Power.Normal)
                            {
                                if (PolarBear.CurHitPoints < PolarBear.MaxHitPoints)
                                    PolarBear.CurHitPoints += 1;
                                
                            }
                            else
                                Scale += 0.01f;
                        }
                    }
                }

                if (Health == 0)
                    IsAlive = false;

                Health = (int)MathHelper.Clamp((float)Health, 0.0f, 100.0f);
            }
            
            base.Update(gameTime);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
                base.Draw(spriteBatch);
        }

        public void ChaseEntity(Entity entity)
        {
            if (IsAlive)
            {
                Vector2 direction = entity.Position - Position;
                direction.Normalize();
                Position += direction * 2;
            }
        }

        public void HitEntity(Entity entity)
        {
            if (IsAlive)
            {
                if (CollisionBox.Intersects(entity.CollisionBox))
                {
                    PolarBear.CurHealth -= 10;
                    PolarBear.CurHitPoints -= 1;
                }
            }
        }







    }
}

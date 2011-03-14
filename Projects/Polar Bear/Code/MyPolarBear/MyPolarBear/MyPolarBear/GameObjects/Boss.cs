using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear.Content;
using MyPolarBear.GameScreens;
using MyPolarBear.Pathfinding;
using MyPolarBear.Audio;

namespace MyPolarBear.GameObjects
{
    class Boss : Entity
    {

        public static int Health = 100;
        public bool IsAlive = true;
        private int onFireTimer = 0;
        private Random random = new Random();

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
            if (!IsAlive)
            {
                UpdateKeeper.getInstance().removeEntity(this);
                DrawKeeper.getInstance().removeEntity(this);
                return;
            }

            onFireTimer += gameTime.ElapsedGameTime.Milliseconds;

            foreach (Entity entity in UpdateKeeper.getInstance().getEntities())
            {             
                if (entity is Projectile)
                {
                    if (CollisionBox.Intersects(entity.CollisionBox))
                    {
                        if (PolarBear.power == PolarBear.Power.Fire)
                        {
                            SoundManager.PlaySound("OnFire");                            
                            if (onFireTimer >= 2000)
                                onFireTimer = 0;
                            Health -= 1;                                
                        }
                        else if (PolarBear.power == PolarBear.Power.Normal)
                        {
                            if (PolarBear.CurHitPoints < PolarBear.MaxHitPoints)
                                PolarBear.CurHitPoints += 1;

                        }
                    }
                }
                else if (entity is Enemy && CollisionBox.Intersects(entity.CollisionBox))
                {
                    if (((Enemy)entity).CurrentState != Enemy.State.Evil)
                    {
                        ((Enemy)entity).CurrentState = Enemy.State.Evil;
                        SoundManager.PlaySound("Roar");
                    }
                }

                if (Health == 0)
                {
                    IsAlive = false;
                    SoundManager.PlaySound("BossDying");
                }

                Health = (int)MathHelper.Clamp((float)Health, 0.0f, 100.0f);                
            }

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {
                if (CollisionBox.Intersects(element.CollisionRect))
                {
                    if (element.Type.Equals("Tree2") || element.Type.Equals("Tree"))
                    {
                        element.Type = "Stump";
                        element.Tex = ContentManager.GetTexture("Stump");
                        GameScreen.CurWorldHealth--;
                        SoundManager.PlaySound("Thump");
                    }
                    else if (element.Type.Equals("BabyPlant"))
                    {
                        element.Type = "SoftGround";
                        element.Tex = ContentManager.GetTexture("SoftGround");
                        AGrid.GetInstance().addResource(element);
                        SoundManager.PlaySound("Thump");
                    }
                    else if (!(element.Type.Equals("Grass") || element.Type.Equals("GrassBig") || element.Type.Equals("Water")
                        || element.Type.Equals("Water2") || element.Type.Equals("Stump") || element.Type.Equals("SoftGround")
                        || element.Type.Equals("Bush") || element.Type.Equals("Ice")))
                    {
                        UpdateKeeper.getInstance().removeLevelElement(element);
                        DrawKeeper.getInstance().removeLevelElement(element);
                        SoundManager.PlaySound("Thump");
                    }
                }
            }
            
            base.Update(gameTime);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                spriteBatch.DrawString(ContentManager.GetFont("Calibri"), "HP: " + Boss.Health.ToString(), new Vector2(Position.X - 100.0f, Position.Y - 125.0f), Color.Yellow);                
                base.Draw(spriteBatch);
                if (onFireTimer < 2000)
                {
                    spriteBatch.Draw(ContentManager.GetTexture("Fire"), new Vector2(Position.X + random.Next(-200, 150), Position.Y + random.Next(-75, 75)), Color.White);
                    spriteBatch.Draw(ContentManager.GetTexture("Fire"), new Vector2(Position.X + random.Next(-200, 150), Position.Y + random.Next(-75, 75)), Color.White);
                }
            }
        }

        public void ChaseEntity(Entity entity)
        {
            if (IsAlive)
            {
                Vector2 direction = entity.Position - Position;
                direction.Normalize();
                Position += direction * 2;

                if (CollisionBox.Intersects(entity.CollisionBox))
                {                    
                    PolarBear.CurHitPoints -= 1;
                    entity.Position += direction * 5;
                    SoundManager.PlaySound("Ow");
                }
            }
        }
    }
}

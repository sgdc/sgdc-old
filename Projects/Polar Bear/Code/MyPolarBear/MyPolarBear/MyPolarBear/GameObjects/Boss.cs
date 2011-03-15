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

        public static int Health;
        public bool IsAlive;
        private int onFireTimer;
        private Random random = new Random();
        public Vector2 mScale;

        public Boss(Vector2 position)
            : base(position)
        {
            Health = 100;
            IsAlive = true;
            onFireTimer = 0;
            Scale = 2;
            mScale = new Vector2(Scale, Scale);
        }

        public override void LoadContent()
        {
            Texture = ContentManager.GetTexture("ForestBossIdle");

            Animation ani = new Animation(ContentManager.GetTexture("ForestBossAttack"), 2, 24, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("ForestBossAttack", ani); 
            ani = new Animation(ContentManager.GetTexture("ForestBossIdle"), 1, 24, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("ForestBossIdle", ani);

            mAnimator.PlayAnimation("ForestBossIdle", false);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive)
            {
                //UpdateKeeper.getInstance().removeEntity(this);
                //DrawKeeper.getInstance().removeEntity(this);
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
                else if (entity is PolarBear)
                {
                    Vector2 distance = Position - entity.Position;
                    if (Math.Abs(distance.Length()) < ScreenManager.SCREENWIDTH / 2 && ((PolarBear)entity).IsAlive)
                    {
                        Velocity = new Vector2(2, 2);
                        mAnimator.PlayAnimation("ForestBossAttack", false);
                        ChaseEntity(entity);
                    }
                    else
                    {
                        Velocity = Vector2.Zero;
                        mAnimator.PlayAnimation("ForestBossIdle", false);
                    }
                }                              
            }

            if (Health == 0)
            {
                IsAlive = false;
                SoundManager.PlaySound("BossDying");
            }

            Health = (int)MathHelper.Clamp((float)Health, 0.0f, 100.0f);  

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
                if (Health == 0)
                {
                    if (element.Type.Equals("Sand"))
                    {
                        element.Type = "Water";
                        element.Tex = ContentManager.GetTexture("Water");
                    }
                    if (element.Type.Equals("Blocks"))
                    {
                        UpdateKeeper.getInstance().removeLevelElement(element);
                        DrawKeeper.getInstance().removeLevelElement(element);
                    }
                }
            }
            
            base.Update(gameTime);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                spriteBatch.DrawString(ContentManager.GetFont("Calibri"), "HP: " + Boss.Health.ToString(), new Vector2(Position.X - 50.0f, Position.Y - 175.0f), Color.Red);                
                //base.Draw(spriteBatch);
                mScale.X = Scale;
                mScale.Y = Scale;
                
                mAnimator.Draw(spriteBatch, Position, mScale, Color.White, Rotation, Origin, 0);

                if (onFireTimer < 2000)
                {
                    spriteBatch.Draw(ContentManager.GetTexture("FireAttack"), new Vector2(Position.X + random.Next(-200, 150), Position.Y + random.Next(-75, 75)), Color.White);
                    spriteBatch.Draw(ContentManager.GetTexture("FireAttack"), new Vector2(Position.X + random.Next(-200, 150), Position.Y + random.Next(-75, 75)), Color.White);
                }
            }
        }

        private void ChaseEntity(Entity entity)
        {
            if (IsAlive)
            {                
                Vector2 direction = entity.Position - Position;
                direction.Normalize();
                Position += direction * Velocity;                
               
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

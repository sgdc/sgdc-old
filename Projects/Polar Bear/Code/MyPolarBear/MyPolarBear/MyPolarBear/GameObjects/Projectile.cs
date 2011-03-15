using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.GameScreens;
using MyPolarBear.Input;
using MyPolarBear.Content;
using MyPolarBear.Pathfinding;
using MyPolarBear.Audio;

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

        public override void LoadContent()
        {           
            switch (Type)
            {
                case PolarBear.Power.Normal:
                    Texture = ContentManager.GetTexture("Heart");
                    Scale = 0.1f;
                    break;
                case PolarBear.Power.Ice:
                    Texture = ContentManager.GetTexture("IceAttack");
                    Scale = 1.0f;
                    break;
                case PolarBear.Power.Fire:
                    Texture = ContentManager.GetTexture("FireAttack");
                    Scale = 1.0f;
                    break;
                case PolarBear.Power.Lighting:
                    //Texture = ContentManager.GetTexture("LightningPowerHeart");
                    break;
            }            

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

            Position += Direction * Velocity;

            //if (InputManager.Mouse.IsButtonReleased(MouseComponent.MouseButton.Right))
              //  Position = InputManager.Mouse.GetCurrentMousePosition() + ScreenManager.camera.TopLeft;

            foreach (Entity entity in UpdateKeeper.getInstance().getEntities())
            {               
                if (CollisionBox.Intersects(entity.CollisionBox))
                {
                    hitEntity(entity);
                }
            }

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {                
                if (CollisionBox.Intersects(element.CollisionRect))
                {                    

                    if (!(element.Type.Equals("Grass") || element.Type.Equals("GrassBig") || element.Type.Equals("Ice")
                        || element.Type.Equals("SoftGround")))
                    {
                        if (Type == PolarBear.Power.Fire && (element.Type.Equals("Stump") || element.Type.Equals("BabyPlant")))
                        {
                            //UpdateKeeper.getInstance().removeLevelElement(element);
                            //DrawKeeper.getInstance().removeLevelElement(element);
                            if (element.Type.Equals("BabyPlant"))
                            {
                                GameScreen.CurWorldHealth--;
                            }
                            element.Type = "SoftGround";
                            element.Tex = ContentManager.GetTexture("SoftGround");
                            AGrid.GetInstance().addResource(element);
                        }
                        if (element.Type.Equals("Water") && Type == PolarBear.Power.Ice)
                        {
                            element.Type = "Ice";
                            element.Tex = ContentManager.GetTexture("Ice");
                        }
                        SoundManager.PlaySound("Thump");
                        IsAlive = false;
                    }
                }
            }

            if (Position.X > GameScreen.WORLDWIDTH || Position.X < -GameScreen.WORLDWIDTH)
                IsAlive = false;
            if (Position.Y > GameScreen.WORLDHEIGHT || Position.Y < -GameScreen.WORLDHEIGHT)
                IsAlive = false;

            base.Update(gameTime);
        }

        private void hitEntity(Entity entity)
        {
            if (entity is Enemy)
            {
                if (Type == PolarBear.Power.Normal && ((Enemy)entity).CurrentState != Enemy.State.Following
                    && ((Enemy)entity).CurrentState != Enemy.State.Evil && ((Enemy)entity).CurrentState != Enemy.State.Afraid)
                {
                    ((Enemy)entity).CurrentState = Enemy.State.Following;
                    SoundManager.PlaySound("Yay");
                    IsAlive = false;
                }
                else if (Type == PolarBear.Power.Fire && ((Enemy)entity).CurrentState == Enemy.State.Evil)// && (((Enemy)entity).CurrentState == Enemy.State.Evil)
                {
                    ((Enemy)entity).CurrentState = Enemy.State.Afraid;
                    IsAlive = false;
                }
            }
            else if (entity is Boss)
            {                     
                IsAlive = false;
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

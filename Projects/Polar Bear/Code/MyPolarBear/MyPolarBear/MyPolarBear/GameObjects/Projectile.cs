using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.GameScreens;
using MyPolarBear.Input;
using MyPolarBear.Content;

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
                    break;
                case PolarBear.Power.Ice:
                    Texture = ContentManager.GetTexture("IcePowerHeart");
                    break;
                case PolarBear.Power.Fire:
                    Texture = ContentManager.GetTexture("FirePowerHeart");
                    break;
                case PolarBear.Power.Lighting:
                    Texture = ContentManager.GetTexture("LightningPowerHeart");
                    break;
            }

            Scale = 0.1f;

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

            if (InputManager.Mouse.IsButtonReleased(MouseComponent.MouseButton.Right))
                Position = InputManager.Mouse.GetCurrentMousePosition() + ScreenManager.camera.TopLeft;

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
                    IsAlive = false;
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
                if (PolarBear.power == PolarBear.Power.Normal)
                    ((Enemy)entity).bFollowBear = true;
                IsAlive = false;
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

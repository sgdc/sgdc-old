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

        public bool attached;

        public Projectile(Vector2 position, float speed, Vector2 direction, PolarBear.Power type) 
            : base(position)
        {
            Position = position;
            Velocity = new Vector2(speed);
            Direction = Vector2.Normalize(direction);            
            Type = type;
            IsAlive = true;
            attached = false;
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

            Scale = 0.25f;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.GamePad.IsButtonPressed(Buttons.B) || !IsAlive)
            {
                UpdateKeeper.getInstance().removeEntity(this);
                DrawKeeper.getInstance().removeEntity(this);
                return;
            }

            //if (IsAlive)
            //{      
            //    Position += Direction * Velocity;

            //    base.Update(gameTime);
            //}

            Position += Direction * Velocity;

            if (InputManager.Mouse.IsButtonReleased(MouseComponent.MouseButton.Right))
                Position = InputManager.Mouse.GetCurrentMousePosition() + ScreenManager.camera.TopLeft;

            foreach (Entity eni in UpdateKeeper.getInstance().getEntities())
            {
                //if (eni is Enemy && CollisionBox.Intersects(eni.CollisionBox))
                //{
                //    if (!attached)
                //    {
                //        //Position = eni.Position;
                //        //((Enemy)eni).alive = false;
                //        //attached = true;
                //        hitEntity(eni);
                //    }

                //}                
                if (CollisionBox.Intersects(eni.CollisionBox))
                {
                    hitEntity(eni);
                }
            }

            base.Update(gameTime);
        }

        private void hitEntity(Entity eni)
        {
            if (eni is Enemy)
            {
                ((Enemy)eni).bFollowBear = true;
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

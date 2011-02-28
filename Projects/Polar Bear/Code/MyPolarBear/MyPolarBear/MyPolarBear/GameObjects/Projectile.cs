using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyPolarBear.GameScreens;
using Microsoft.Xna.Framework.Input;
using MyPolarBear.Input;

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
                    Texture = Game1.textures["Images/Heart"];
                    break;
                case PolarBear.Power.Ice:
                    Texture = Game1.textures["Images/IcyHeart"];
                    break;
                case PolarBear.Power.Fire:
                    Texture = Game1.textures["Images/FieryHeart"];
                    break;
                case PolarBear.Power.Grass:
                    Texture = Game1.textures["Images/GrassyHeart"];
                    break;
            }

            Scale = 0.25f;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.gamepad.IsButtonPressed(Buttons.B) || !IsAlive)
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

            if (ScreenManager.mouse.IsButtonReleased(MouseComponent.MouseButton.Right))
                Position = ScreenManager.mouse.GetCurrentMousePosition() + ScreenManager.camera.TopLeft;

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

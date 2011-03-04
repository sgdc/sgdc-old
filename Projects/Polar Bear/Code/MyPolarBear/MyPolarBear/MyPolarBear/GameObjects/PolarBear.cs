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
    class PolarBear : Entity
    {        
        public enum Power
        {
            Normal,
            Ice,
            Fire,
            Grass
        }

        public Power power;
        public int aniFrame;
        public bool isFiring;

        private int timeProjectileFired;
        //private Animator mAnimator;
        private Vector2 mScale;


        public PolarBear(Vector2 position)
            : base(position)
        {
            power = Power.Ice;
            //mAnimator = new Animator();
            Scale = 2;
            mScale = new Vector2(Scale, Scale);
        }

        public override void LoadContent()
        {
            //Texture = ContentManager.GetTexture("IceWalkingRight");

            Animation ani = new Animation(ContentManager.GetTexture("IceWalkingRight"), 5, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkRight", ani);

            ani = new Animation(ContentManager.GetTexture("IceWalkingRight"), 5, 8, 0, true, SpriteEffects.FlipHorizontally);
            mAnimator.Animations.Add("walkLeft", ani);

            ani = new Animation(ContentManager.GetTexture("IceWalkingFront"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkFront", ani);

            ani = new Animation(ContentManager.GetTexture("IceWalkingBack"), 4, 8, 0, true, SpriteEffects.None);
            mAnimator.Animations.Add("walkBack", ani);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Keyboard.IsKeyPressed(Keys.Left))
            {
                Translate(new Vector2(-5.0f, 0.0f));
                mAnimator.PlayAnimation("walkLeft", false);
            }
            if (InputManager.Keyboard.IsKeyPressed(Keys.Right))
            {
                Translate(new Vector2(5.0f, 0.0f));
                mAnimator.PlayAnimation("walkRight", false);
            }
            if (InputManager.Keyboard.IsKeyPressed(Keys.Up))
            {
                Translate(new Vector2(0.0f, -5.0f));
                mAnimator.PlayAnimation("walkBack", false);
            }
            if (InputManager.Keyboard.IsKeyPressed(Keys.Down))
            {
                Translate(new Vector2(0.0f, 5.0f));
                mAnimator.PlayAnimation("walkFront", false);
            }
            if (InputManager.Keyboard.IsKeyReleased(Keys.Space))
                SwitchPowers();

            if (InputManager.GamePad.GetThumbStickState(GamePadComponent.Thumbstick.Left) != Vector2.Zero)
                Translate(InputManager.GamePad.GetThumbStickState(GamePadComponent.Thumbstick.Left) * 5);

            if (InputManager.GamePad.IsButtonReleased(Buttons.RightShoulder))
                SwitchPowers();

            if (InputManager.Mouse.IsButtonReleased(MouseComponent.MouseButton.Left))
            {
                Projectile projectile = ShootProjectile(InputManager.Mouse.GetCurrentMousePosition() - ScreenManager.camera.ScreenCenter);

                //projectile.LoadContent(Game1.GetTextureAt(4), 0.25f);
                projectile.LoadContent();
                projectile.IsAlive = true;
                UpdateKeeper.getInstance().addEntity(projectile);
                DrawKeeper.getInstance().addEntity(projectile);
            }

            if (InputManager.GamePad.GetThumbStickState(GamePadComponent.Thumbstick.Right).Length() >= .5)
            {
                Projectile projectile = ShootProjectile(InputManager.GamePad.GetThumbStickState(GamePadComponent.Thumbstick.Right));
                if (gameTime.TotalGameTime.TotalMilliseconds - timeProjectileFired >= 500)
                {
                    //projectile.LoadContent(Game1.GetTextureAt(4), 0.25f);
                    projectile.LoadContent();
                    UpdateKeeper.getInstance().addEntity(projectile);
                    DrawKeeper.getInstance().addEntity(projectile);
                    projectile.IsAlive = true;
                    timeProjectileFired = (int)gameTime.TotalGameTime.TotalMilliseconds;
                    InputManager.GamePad.StartVibration(1.0f, 0.0f);
                }
            }

            if (InputManager.Keyboard.IsKeyPressed(Keys.R))
            {
                FireIce();
            }

            base.Update(gameTime);
        }

        public void SwitchPowers()
        {
            switch (power)
            {
                case Power.Normal: power = Power.Ice;
                    break;
                case Power.Ice: power = Power.Fire;
                    break;
                case Power.Fire: power = Power.Grass;
                    break;
                case Power.Grass: power = Power.Normal;
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
            //int rectWidth = Texture.Width;
            //int rectHeight = Texture.Height / 5;
            //Rectangle sourceRect = new Rectangle(0, rectHeight * (aniFrame / 8), rectWidth, rectHeight);

            //rectWidth *= 2;
            //rectHeight *= 2;
            //Rectangle destRect = new Rectangle((int)Position.X - rectWidth / 2, (int)Position.Y - rectHeight / 2, rectWidth, rectHeight);
            //spriteBatch.Draw(Texture, destRect, sourceRect, Color.White);

            ////if (isFiring)
            ////{
            //    aniFrame++;
            //    if (aniFrame % 40 == 0)
            //    {
            //        isFiring = false;
            //        aniFrame = 0;
            //    }
            ////}
            mScale.X = Scale;
            mScale.Y = Scale;

            mAnimator.Draw(spriteBatch, Position, mScale, Color.White, Rotation, Origin, 0);
        }
    }
}

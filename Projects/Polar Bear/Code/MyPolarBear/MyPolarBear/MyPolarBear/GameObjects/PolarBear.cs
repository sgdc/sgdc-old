using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyPolarBear;
using MyPolarBear.GameScreens;
using Microsoft.Xna.Framework.Input;
using MyPolarBear.Input;

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
        public int aniFrame;
        public bool isFiring;

        private int timeProjectileFired;


        public PolarBear(Vector2 position)
            : base(position)
        {
            
        }

        public override void LoadContent()
        {
            Texture = Game1.textures["SpriteSheets/Arctic/walkingRight"];

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Left))
                Translate(new Vector2(-5.0f, 0.0f));
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Right))
                Translate(new Vector2(5.0f, 0.0f));
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Up))
                Translate(new Vector2(0.0f, -5.0f));
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Down))
                Translate(new Vector2(0.0f, 5.0f));
            if (ScreenManager.keyboard.IsKeyReleased(Keys.Space))
                SwitchPowers();

            if (ScreenManager.gamepad.GetThumbStickState(GamePadComponent.Thumbstick.Left) != Vector2.Zero)
                Translate(ScreenManager.gamepad.GetThumbStickState(GamePadComponent.Thumbstick.Left) * 5);

            if (ScreenManager.gamepad.IsButtonReleased(Buttons.RightShoulder))
                SwitchPowers();

            if (ScreenManager.mouse.IsButtonReleased(MouseComponent.MouseButton.Left))
            {
                Projectile projectile = ShootProjectile(ScreenManager.mouse.GetCurrentMousePosition() - ScreenManager.camera.ScreenCenter);

                //projectile.LoadContent(Game1.GetTextureAt(4), 0.25f);
                projectile.LoadContent();
                projectile.IsAlive = true;
                UpdateKeeper.getInstance().addEntity(projectile);
                DrawKeeper.getInstance().addEntity(projectile);
            }

            if (ScreenManager.gamepad.GetThumbStickState(GamePadComponent.Thumbstick.Right).Length() >= .5)
            {
                Projectile projectile = ShootProjectile(ScreenManager.gamepad.GetThumbStickState(GamePadComponent.Thumbstick.Right));
                if (gameTime.TotalGameTime.TotalMilliseconds - timeProjectileFired >= 500)
                {
                    //projectile.LoadContent(Game1.GetTextureAt(4), 0.25f);
                    projectile.LoadContent();
                    UpdateKeeper.getInstance().addEntity(projectile);
                    DrawKeeper.getInstance().addEntity(projectile);
                    projectile.IsAlive = true;
                    timeProjectileFired = (int)gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            if (ScreenManager.keyboard.IsKeyPressed(Keys.R))
            {
                FireIce();
            }

            base.Update(gameTime);
        }

        public void SwitchPowers()
        {
            switch (power)
            {
                case Power.Normal:
                    power = Power.Ice;
                    //Texture = Game1.GetTextureAt(1);
                    break;
                case Power.Ice:
                    power = Power.Fire;
                    //Texture = Game1.GetTextureAt(2);
                    break;
                case Power.Fire:
                    power = Power.Grass;
                    //Texture = Game1.GetTextureAt(3);
                    break;
                case Power.Grass:
                    power = Power.Normal;
                    //Texture = Game1.GetTextureAt(0);
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
            int rectWidth = Texture.Width;
            int rectHeight = Texture.Height / 5;
            Rectangle sourceRect = new Rectangle(0, rectHeight * (aniFrame / 8), rectWidth, rectHeight);

            rectWidth *= 2;
            rectHeight *= 2;
            Rectangle destRect = new Rectangle((int)Position.X - rectWidth / 2, (int)Position.Y - rectHeight / 2, rectWidth, rectHeight);
            spriteBatch.Draw(Texture, destRect, sourceRect, Color.White);

            //if (isFiring)
            //{
                aniFrame++;
                if (aniFrame % 40 == 0)
                {
                    isFiring = false;
                    aniFrame = 0;
                }
            //}
        }
    }
}

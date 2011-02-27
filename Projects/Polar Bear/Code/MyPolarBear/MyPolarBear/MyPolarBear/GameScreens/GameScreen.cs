using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MyPolarBear.Input;
using MyPolarBear.GameObjects;
using MyPolarBear.GameScreens;

namespace MyPolarBear.GameScreens
{
    class GameScreen : Screen
    {
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        
        PolarBear polarBear;

        const int maxEnemies = 50;
        Enemy[] enemies;

        Entity reticule;
        Random random = new Random();


        List<Projectile> projectiles = new List<Projectile>();
        List<Projectile> deadProjectiles = new List<Projectile>();

        int timeProjectileFired;

        public GameScreen(ScreenType screenType) : base(screenType)
        {
            
        }             

        public void LoadContent()
        {           
            //audioEngine = new AudioEngine(@"Content\Sound\GameSounds.xgs");
            //waveBank = new WaveBank(audioEngine, @"Content\Sound\Wave Bank.xwb");
            //soundBank = new SoundBank(audioEngine, @"Content\Sound\Sound Bank.xsb");
            //soundBank.PlayCue("Music");
            
            polarBear = new PolarBear(new Vector2(400, 400));
            polarBear.LoadContent(Game1.GetTextureAt(0), 1.0f);
            reticule = new Entity(Vector2.Zero);
            reticule.LoadContent(Game1.GetTextureAt(9), 0.5f);

            enemies = new Enemy[maxEnemies];

            for (int i = 0; i < maxEnemies; i++)
            {
                enemies[i] = new Enemy(new Vector2(MathHelper.Lerp(0.0f, 800.0f, (float)random.NextDouble()), MathHelper.Lerp(0.0f, 800.0f, (float)random.NextDouble())));
                enemies[i].Velocity = new Vector2(random.Next(1, 20), random.Next(1, 20));
                enemies[i].LoadContent(Game1.GetTextureAt(10), 1.0f);
            }
            
            ScreenManager.camera.FocusEntity = polarBear;
        }


        public void Update(GameTime gameTime)
        {
            if (ScreenManager.gamepad.IsButtonReleased(Buttons.Start))           
                ScreenManager.screenType = ScreenType.PauseScreen;                
            if (ScreenManager.gamepad.IsButtonReleased(Buttons.Back))
                ScreenManager.isExiting = true;

            if (ScreenManager.keyboard.IsKeyPressed(Keys.Left))
                polarBear.Translate(new Vector2(-5.0f, 0.0f));
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Right))
                polarBear.Translate(new Vector2(5.0f, 0.0f));
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Up))
                polarBear.Translate(new Vector2(0.0f, -5.0f));
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Down))
                polarBear.Translate(new Vector2(0.0f, 5.0f));
            if (ScreenManager.keyboard.IsKeyReleased(Keys.Space))
                polarBear.SwitchPowers();

            if (ScreenManager.mouse.IsButtonReleased(MouseComponent.MouseButton.Left))
            {
                Projectile projectile = polarBear.ShootProjectile(ScreenManager.mouse.GetCurrentMousePosition() - ScreenManager.camera.ScreenCenter);
                
                projectile.LoadContent(Game1.GetTextureAt(4), 0.25f);
                projectiles.Add(projectile);
                projectile.IsAlive = true;
            }

            if (ScreenManager.gamepad.GetThumbStickState(GamePadComponent.Thumbstick.Left) != Vector2.Zero)
                polarBear.Translate(ScreenManager.gamepad.GetThumbStickState(GamePadComponent.Thumbstick.Left) * 5);

            if (ScreenManager.gamepad.GetThumbStickState(GamePadComponent.Thumbstick.Right).Length() >= .5)
            {
                Projectile projectile = polarBear.ShootProjectile(ScreenManager.gamepad.GetThumbStickState(GamePadComponent.Thumbstick.Right));
                if (gameTime.TotalGameTime.TotalMilliseconds - timeProjectileFired >= 500)
                {
                    projectile.LoadContent(Game1.GetTextureAt(4), 0.25f);
                    projectiles.Add(projectile);
                    projectile.IsAlive = true;
                    timeProjectileFired = (int)gameTime.TotalGameTime.TotalMilliseconds;
                }                                 
            }

            if (ScreenManager.gamepad.IsButtonReleased(Buttons.RightShoulder))
                polarBear.SwitchPowers();

            if (ScreenManager.gamepad.GetTriggerState(GamePadComponent.Trigger.Left) != 0)
                ScreenManager.camera.Zoom(-0.01f);
            if (ScreenManager.gamepad.GetTriggerState(GamePadComponent.Trigger.Right) != 0)
                ScreenManager.camera.Zoom(0.01f);

            if (ScreenManager.keyboard.IsKeyPressed(Keys.A))
                ScreenManager.camera.Zoom(-0.01f);
            if (ScreenManager.keyboard.IsKeyPressed(Keys.S))
                ScreenManager.camera.Zoom(0.01f);

            if (ScreenManager.keyboard.IsKeyPressed(Keys.R))
            {
                polarBear.FireIce();
            }

            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime);
                if (ScreenManager.gamepad.IsButtonPressed(Buttons.B))
                    deadProjectiles.Add(projectile);
                

                foreach (Enemy eni in enemies)
                {
                    if (projectile.CollisionBox.Intersects(eni.CollisionBox))
                    {
                        //projectile.IsAlive = false;
                        //foreach (Projectile proj in projectiles)
                        //{
                        //    proj.Position = eni.Position;
                        //}
                        if (!projectile.attached)
                        {
                            projectile.Position = eni.Position;
                            eni.alive = false;
                            projectile.attached = true;
                        }

                    }
                }

                if (ScreenManager.mouse.IsButtonReleased(MouseComponent.MouseButton.Right))
                    projectile.Position = ScreenManager.mouse.GetCurrentMousePosition() + ScreenManager.camera.TopLeft;
            }

            reticule.Position = ScreenManager.mouse.GetCurrentMousePosition() + ScreenManager.camera.TopLeft; ;

            foreach (Projectile deadprojectile in deadProjectiles)
            {
                deadprojectile.IsAlive = false;
            }

            deadProjectiles.Clear();

            foreach (Enemy eni in enemies)
            {
                eni.Update(gameTime);
                eni.followVelocity = (polarBear.Position - eni.Position) * 10;
            }

            polarBear.Update(gameTime);    
                       
        }       

        public override void DrawGame(SpriteBatch spriteBatch)
        {          
            //spriteBatch.Draw(Game1.GetTextureAt(8), Vector2.Zero, Color.White);
            spriteBatch.Draw(Game1.GetTextureAt(8), Vector2.Zero, null, Color.White, 0.0f, new Vector2(Game1.GetTextureAt(8).Width / 2, Game1.GetTextureAt(8).Height / 2), 5.0f, SpriteEffects.None, 0.0f);

            polarBear.Draw(spriteBatch);

            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }

            foreach (Enemy eni in enemies)
            {
                eni.Draw(spriteBatch);
            }

            reticule.Draw(spriteBatch);

            base.DrawGame(spriteBatch);
        }        
    }
}


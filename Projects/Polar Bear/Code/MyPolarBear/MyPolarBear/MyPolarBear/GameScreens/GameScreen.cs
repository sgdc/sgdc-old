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
            
            polarBear = new PolarBear(new Vector2(0, 0));
            polarBear.LoadContent(Game1.GetTextureAt(0), 1.0f);
            
            ScreenManager.camera.FocusEntity = polarBear;
        }


        public void Update(GameTime gameTime)
        {
            if (ScreenManager.gamepad.IsButtonReleased(Buttons.Start))           
                ScreenManager.screenType = ScreenType.PauseScreen;                
            if (ScreenManager.gamepad.IsButtonReleased(Buttons.Back))
                ScreenManager.isExiting = true;

            if (ScreenManager.keyboard.IsKeyPressed(Keys.Left))
                ScreenManager.camera.Translate(new Vector2(-5.0f, 0.0f));
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Right))
                ScreenManager.camera.Translate(new Vector2(5.0f, 0.0f));
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Up))
                ScreenManager.camera.Translate(new Vector2(0.0f, -5.0f));
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Down))
                ScreenManager.camera.Translate(new Vector2(0.0f, 5.0f));
            if (ScreenManager.keyboard.IsKeyReleased(Keys.Space))
                polarBear.SwitchPowers();

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

            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime);
                if (ScreenManager.gamepad.IsButtonPressed(Buttons.B))
                    deadProjectiles.Add(projectile);
            }

            foreach (Projectile deadprojectile in deadProjectiles)
            {
                deadprojectile.IsAlive = false;
            }

            deadProjectiles.Clear();

            polarBear.Update(gameTime);    
                       
        }       

        public override void DrawGame(SpriteBatch spriteBatch)
        {          
            spriteBatch.Draw(Game1.GetTextureAt(8), Vector2.Zero, Color.White);

            polarBear.Draw(spriteBatch);

            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }

            base.DrawGame(spriteBatch);
        }        
    }
}


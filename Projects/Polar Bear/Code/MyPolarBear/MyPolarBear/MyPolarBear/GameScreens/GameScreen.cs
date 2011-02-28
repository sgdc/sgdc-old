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
        public const int WORLDWIDTH = 4096;
        public const int WORLDHEIGHT = 2600;
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        
        PolarBear polarBear;

        const int maxEnemies = 50;
        private int lovedEnemies = 0;

        Entity reticule;
        Random random = new Random();

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
            //polarBear.LoadContent(Game1.GetTextureAt(0), 1.0f);
            polarBear.LoadContent();
            UpdateKeeper.getInstance().addEntity(polarBear);
            DrawKeeper.getInstance().addEntity(polarBear);

            reticule = new Entity(Vector2.Zero);
            //reticule.LoadContent(Game1.GetTextureAt(9), 0.5f);
            reticule.LoadContent(Game1.textures["Images/Reticule"], 0.5f);

            Enemy ene;

            for (int i = 0; i < maxEnemies; i++)
            {
                ene = new Enemy(new Vector2(MathHelper.Lerp(0.0f, 800.0f, (float)random.NextDouble()), MathHelper.Lerp(0.0f, 800.0f, (float)random.NextDouble())));
                ene.Velocity = new Vector2(random.Next(1, 20), random.Next(1, 20));
                ene.LoadContent();
                UpdateKeeper.getInstance().addEntity(ene);
                DrawKeeper.getInstance().addEntity(ene);
            }
            
            ScreenManager.camera.FocusEntity = polarBear;
        }


        public void Update(GameTime gameTime)
        {
            if (ScreenManager.gamepad.IsButtonReleased(Buttons.Start))           
                ScreenManager.screenType = ScreenType.PauseScreen;                
            if (ScreenManager.gamepad.IsButtonReleased(Buttons.Back))
                ScreenManager.isExiting = true;

            if (ScreenManager.keyboard.IsKeyPressed(Keys.P))
                ScreenManager.screenType = ScreenType.PauseScreen;
            if (ScreenManager.keyboard.IsKeyPressed(Keys.Escape))
                ScreenManager.isExiting = true;

            if (ScreenManager.gamepad.GetTriggerState(GamePadComponent.Trigger.Left) != 0)
                ScreenManager.camera.Zoom(-0.01f);
            if (ScreenManager.gamepad.GetTriggerState(GamePadComponent.Trigger.Right) != 0)
                ScreenManager.camera.Zoom(0.01f);

            if (ScreenManager.keyboard.IsKeyPressed(Keys.A))
                ScreenManager.camera.Zoom(-0.01f);
            if (ScreenManager.keyboard.IsKeyPressed(Keys.S))
                ScreenManager.camera.Zoom(0.01f);

            reticule.Position = ScreenManager.mouse.GetCurrentMousePosition() + ScreenManager.camera.TopLeft;

            lovedEnemies = 0;
            foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
            {
                if (ent is Enemy)
                {
                    if (((Enemy)ent).bFollowBear)
                    {
                        lovedEnemies++;
                    }
                }
            }

            UpdateKeeper.getInstance().updateAll(gameTime);
        }       

        public override void DrawGame(SpriteBatch spriteBatch)
        {          
            //spriteBatch.Draw(Game1.GetTextureAt(8), Vector2.Zero, Color.White);
            //spriteBatch.Draw(Game1.GetTextureAt(8), Vector2.Zero, null, Color.White, 0.0f, new Vector2(Game1.GetTextureAt(8).Width / 2, Game1.GetTextureAt(8).Height / 2), 5.0f, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(Game1.textures["Images/WorldMap"], Vector2.Zero, Color.White);
            //spriteBatch.Draw(Game1.textures["Images/BasicTerrain"], Vector2.Zero, Color.White);
            spriteBatch.Draw(Game1.textures["Images/BasicTerrain"], new Rectangle(0, 0, WORLDWIDTH, WORLDHEIGHT), Color.White);

            reticule.Draw(spriteBatch);

            DrawKeeper.getInstance().drawAll(spriteBatch);

            spriteBatch.DrawString(Game1.gameFont, lovedEnemies.ToString() + "/" + maxEnemies.ToString(),
                                   ScreenManager.camera.TopLeft, Color.Yellow);

            base.DrawGame(spriteBatch);
        }        
    }
}


using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.Input;
using MyPolarBear.Content;
using MyPolarBear.GameObjects;
using MyPolarBear.GameScreens;
using System.IO;

namespace MyPolarBear.GameScreens
{
    class GameScreen : Screen
    {
        public const int WORLDWIDTH = 4096;
        public const int WORLDHEIGHT = 4096;
        
        //AudioEngine audioEngine;
        //WaveBank waveBank;
        //SoundBank soundBank;
        
        PolarBear polarBear;

        Boss forestBoss;

        

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

            forestBoss = new Boss(new Vector2(0, -1800));
            forestBoss.LoadContent();
            polarBear.LoadContent();
            UpdateKeeper.getInstance().addEntity(polarBear);
            DrawKeeper.getInstance().addEntity(polarBear);
            UpdateKeeper.getInstance().addEntity(forestBoss);
            DrawKeeper.getInstance().addEntity(forestBoss);

            reticule = new Entity(Vector2.Zero);
            //reticule.LoadContent(Game1.GetTextureAt(9), 0.5f);
            reticule.LoadContent(ContentManager.GetTexture("Reticule"), 0.5f);
            DrawKeeper.getInstance().addEntity(reticule);

            Enemy ene;            

            for (int i = 0; i < maxEnemies; i++)
            {
                ene = new Enemy(new Vector2(MathHelper.Lerp(-WORLDWIDTH / 2, WORLDWIDTH / 2, (float)random.NextDouble()), MathHelper.Lerp(-WORLDHEIGHT, WORLDHEIGHT, (float)random.NextDouble())));
                ene.Velocity = new Vector2(random.Next(1, 10), random.Next(1, 10));
                ene.LoadContent();
                UpdateKeeper.getInstance().addEntity(ene);
                DrawKeeper.getInstance().addEntity(ene);
            }
            
            ScreenManager.camera.FocusEntity = polarBear;

            LoadLevel("levelforest");
        }


        public void Update(GameTime gameTime)
        {
            if (InputManager.GamePad.IsButtonReleased(Buttons.Start))           
                ScreenManager.screenType = ScreenType.PauseScreen;                
            if (InputManager.GamePad.IsButtonReleased(Buttons.Back))
                ScreenManager.isExiting = true;

            if (InputManager.Keyboard.IsKeyPressed(Keys.P))
                ScreenManager.screenType = ScreenType.PauseScreen;
            if (InputManager.Keyboard.IsKeyPressed(Keys.Escape))
                ScreenManager.isExiting = true;

            if (InputManager.GamePad.GetTriggerState(GamePadComponent.Trigger.Left) != 0)
                ScreenManager.camera.Zoom(-0.01f);
            if (InputManager.GamePad.GetTriggerState(GamePadComponent.Trigger.Right) != 0)
                ScreenManager.camera.Zoom(0.01f);
                       
            if (Math.Abs(forestBoss.Position.Y - polarBear.Position.Y) < 500)
            {
                forestBoss.ChaseEntity(polarBear);                
            }

            reticule.Position = InputManager.Mouse.GetCurrentMousePosition() + ScreenManager.camera.TopLeft;

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
            spriteBatch.Draw(ContentManager.GetTexture("Background"), new Rectangle(-WORLDWIDTH / 2, -WORLDHEIGHT / 2, WORLDWIDTH / 2, WORLDHEIGHT / 2), Color.White);
            spriteBatch.Draw(ContentManager.GetTexture("Background"), new Rectangle(0, -WORLDHEIGHT / 2, WORLDWIDTH / 2, WORLDHEIGHT / 2), Color.White);
            spriteBatch.Draw(ContentManager.GetTexture("Background"), new Rectangle(0, 0, WORLDWIDTH / 2, WORLDHEIGHT / 2), Color.White);
            spriteBatch.Draw(ContentManager.GetTexture("Background"), new Rectangle(-WORLDWIDTH / 2, 0, WORLDWIDTH / 2, WORLDHEIGHT / 2), Color.White);                    

            DrawKeeper.getInstance().drawAll(spriteBatch);            

            base.DrawGame(spriteBatch);
        }        
    }
}


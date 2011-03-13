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
using MyPolarBear.Pathfinding;

namespace MyPolarBear.GameScreens
{
    class GameScreen : Screen
    {
        public const int WORLDWIDTH = 4096;
        public const int WORLDHEIGHT = 4096;

        public static int MaxWorldHealth;
        public static int CurWorldHealth;
        
        //AudioEngine audioEngine;
        //WaveBank waveBank;
        //SoundBank soundBank;
        
        PolarBear polarBear;

        Boss forestBoss;

        

        const int maxEnemies = 25;
        private int lovedEnemies = 0;
        
        Random random = new Random();        

        public GameScreen(ScreenType screenType) : base(screenType)
        {
            MaxWorldHealth = 100;
            CurWorldHealth = 0;
        }             

        public void LoadContent()
        {           
            //audioEngine = new AudioEngine(@"Content\Sound\GameSounds.xgs");
            //waveBank = new WaveBank(audioEngine, @"Content\Sound\Wave Bank.xwb");
            //soundBank = new SoundBank(audioEngine, @"Content\Sound\Sound Bank.xsb");
            //soundBank.PlayCue("Music");

            LoadLevel("levelforest");

            UpdateKeeper.getInstance().updateAll(new GameTime());
            AGrid.GetInstance().setLevel(UpdateKeeper.getInstance().getLevelElements());
            
            polarBear = new PolarBear(new Vector2(-1950, 1800));
            ScreenManager.camera.FocusEntity = polarBear;
            forestBoss = new Boss(new Vector2(0, -1500));
            forestBoss.LoadContent();
            polarBear.LoadContent();
            UpdateKeeper.getInstance().addEntity(polarBear);
            DrawKeeper.getInstance().addEntity(polarBear);
            UpdateKeeper.getInstance().addEntity(forestBoss);
            DrawKeeper.getInstance().addEntity(forestBoss);

            Enemy ene;            

            for (int i = 0; i < maxEnemies; i++)
            {
                //ene = new Enemy(new Vector2(MathHelper.Lerp(-WORLDWIDTH / 2, WORLDWIDTH / 2, (float)random.NextDouble()), MathHelper.Lerp(-WORLDHEIGHT, WORLDHEIGHT, (float)random.NextDouble())));
                ene = new Enemy(new Vector2(random.Next(350, 400), random.Next(350, 400)));
                ene.Velocity = new Vector2(random.Next(1, 10), random.Next(1, 10));
                ene.LoadContent();
                UpdateKeeper.getInstance().addEntity(ene);
                DrawKeeper.getInstance().addEntity(ene);
            }

            Animal tiger = new Animal(new Vector2(-1750, 1300), Animal.Species.Tiger);
            tiger.LoadContent();
            UpdateKeeper.getInstance().addEntity(tiger);
            DrawKeeper.getInstance().addEntity(tiger);

            tiger = new Animal(new Vector2(1700, 300), Animal.Species.Tiger);
            tiger.LoadContent();
            UpdateKeeper.getInstance().addEntity(tiger);
            DrawKeeper.getInstance().addEntity(tiger);

            Animal lion = new Animal(new Vector2(1900, 1900), Animal.Species.Lion);
            lion.LoadContent();
            UpdateKeeper.getInstance().addEntity(lion);
            DrawKeeper.getInstance().addEntity(lion);

            lion = new Animal(new Vector2(-1300, -1650), Animal.Species.Lion);
            lion.LoadContent();
            UpdateKeeper.getInstance().addEntity(lion);
            DrawKeeper.getInstance().addEntity(lion);

            Animal panther = new Animal(new Vector2(-550, 1350), Animal.Species.Panther);
            panther.LoadContent();
            UpdateKeeper.getInstance().addEntity(panther);
            DrawKeeper.getInstance().addEntity(panther);

            panther = new Animal(new Vector2(1900, -1900), Animal.Species.Panther);
            panther.LoadContent();
            UpdateKeeper.getInstance().addEntity(panther);
            DrawKeeper.getInstance().addEntity(panther);

            //LoadLevel("levelforest");

            //UpdateKeeper.getInstance().updateAll(new GameTime());
            //AGrid.GetInstance().setLevel(UpdateKeeper.getInstance().getLevelElements());
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
                forestBoss.ChaseEntity(polarBear);                
                      

            lovedEnemies = 0;
            foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
            {
                if (ent is Enemy)
                {
                    if (((Enemy)ent).CurrentState == Enemy.State.Following)
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


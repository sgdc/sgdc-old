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
using MyPolarBear.Audio;

namespace MyPolarBear.GameScreens
{
    class GameScreen : Screen
    {
        public const int WORLDWIDTH = 4096;
        public const int WORLDHEIGHT = 4096;

        public static int MaxWorldHealth = 100;
        public static int CurWorldHealth = 0;

        public static int MaxAnimals = 6;
        public static int NumAnimals = 0;
        
        PolarBear polarBear;
        Boss forestBoss;
       
        const int maxEnemies = 25;
        public static int NumEnemies = 0;
        const int bossMinions = 10;
        
        static Random random = new Random();

        static bool isGameWon = false;

        public GameScreen(ScreenType screenType) : base(screenType)
        {
        }             

        public void LoadContent()
        {                       
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
                ene = new Enemy(new Vector2(random.Next(350, 400), random.Next(350, 400)));
                ene.Velocity = new Vector2(random.Next(1, 10), random.Next(1, 10));
                ene.CurrentState = Enemy.State.Evil;
                ene.LoadContent();
                UpdateKeeper.getInstance().addEntity(ene);
                DrawKeeper.getInstance().addEntity(ene);
            }

            for (int i = 0; i < bossMinions; i++)
            {
                ene = new Enemy(new Vector2(random.Next(-200, 200), random.Next(-1500, -1000)));
                ene.Velocity = new Vector2(0, 0);
                ene.CurrentState = Enemy.State.Evil;
                ene.LoadContent();
                UpdateKeeper.getInstance().addEntity(ene);
                DrawKeeper.getInstance().addEntity(ene);
            }

            Animal tiger = new Animal(new Vector2(-1750, 1300), Animal.Types.Tiger, Animal.Genders.Male);
            tiger.LoadContent();
            UpdateKeeper.getInstance().addEntity(tiger);
            DrawKeeper.getInstance().addEntity(tiger);

            tiger = new Animal(new Vector2(1700, 300), Animal.Types.Tiger, Animal.Genders.Female);
            tiger.LoadContent();
            UpdateKeeper.getInstance().addEntity(tiger);
            DrawKeeper.getInstance().addEntity(tiger);

            Animal lion = new Animal(new Vector2(1900, 1900), Animal.Types.Lion, Animal.Genders.Male);
            lion.LoadContent();
            UpdateKeeper.getInstance().addEntity(lion);
            DrawKeeper.getInstance().addEntity(lion);

            lion = new Animal(new Vector2(-1300, -1650), Animal.Types.Lion, Animal.Genders.Female);
            lion.LoadContent();
            UpdateKeeper.getInstance().addEntity(lion);
            DrawKeeper.getInstance().addEntity(lion);

            Animal panther = new Animal(new Vector2(-550, 1350), Animal.Types.Panther, Animal.Genders.Male);
            panther.LoadContent();
            UpdateKeeper.getInstance().addEntity(panther);
            DrawKeeper.getInstance().addEntity(panther);

            panther = new Animal(new Vector2(1900, -1900), Animal.Types.Panther, Animal.Genders.Female);
            panther.LoadContent();
            UpdateKeeper.getInstance().addEntity(panther);
            DrawKeeper.getInstance().addEntity(panther);

            SoundManager.PlaySound("Techno");
        }

        public static void Reset()
        {           

            foreach (LevelElement element in UpdateKeeper.getInstance().getLevelElements())
            {
                UpdateKeeper.getInstance().removeLevelElement(element);
                DrawKeeper.getInstance().removeLevelElement(element);
            }

            LoadLevel("levelforest");

            foreach (Entity entity in UpdateKeeper.getInstance().getEntities())
            {
                if (entity is PolarBear)
                {
                    entity.Position = new Vector2(-1950, 1800);
                    PolarBear.CurHitPoints = 5;
                    PolarBear.NumSeeds = 0;
                    PolarBear.power = PolarBear.Power.Normal;
                    ((PolarBear)entity).IsAlive = true;
                }

                if (entity is Boss)
                {
                    entity.Position = new Vector2(0, -1500);
                    Boss.Health = 100;
                    ((Boss)entity).IsAlive = true;
                }

                if (entity is Animal)
                {
                    Animal.Types type = ((Animal)entity).Type;
                    Animal.Genders gender = ((Animal)entity).Gender;
                    switch (type)
                    {
                        case (Animal.Types.Tiger):
                            if (gender == Animal.Genders.Male)
                                entity.Position = new Vector2(-1750, 1300);
                            else
                                entity.Position = new Vector2(1700, 300);
                            break;
                        case (Animal.Types.Lion):
                            if (gender == Animal.Genders.Male)
                                entity.Position = new Vector2(1900, 1900);
                            else
                                entity.Position = new Vector2(-1300, -1650);
                            break;  
                        case (Animal.Types.Panther):
                            if (gender == Animal.Genders.Male)
                                entity.Position = new Vector2(-550, 1350);
                            else
                                entity.Position = new Vector2(1900, -1900);
                            break;
                    }
                    ((Animal)entity).State = Animal.States.Idle;
                }

                if (entity is Enemy)
                {
                    entity.Position = new Vector2(random.Next(350, 400), random.Next(350, 400));
                    entity.Velocity = new Vector2(random.Next(1, 10), random.Next(1, 10));
                    ((Enemy)entity).CurrentState = Enemy.State.Evil;
                    ((Enemy)entity).IsAlive = true;
                }
            }
            CurWorldHealth = 0;
            NumAnimals = 0;
            isGameWon = false;
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

            NumEnemies = 0;
            foreach (Entity ent in UpdateKeeper.getInstance().getEntities())
            {
                if (ent is Enemy)
                {
                    if (((Enemy)ent).CurrentState == Enemy.State.Evil)
                    {
                        NumEnemies++;
                    }
                }
            }               

            if (!polarBear.IsAlive && (InputManager.GamePad.IsButtonReleased(Buttons.A) || InputManager.Keyboard.IsKeyReleased(Keys.Enter)))
            {
                polarBear.IsAlive = true;
                PolarBear.CurHitPoints = PolarBear.MaxHitPoints;
                PolarBear.NumSeeds = 0;
                polarBear.Position = new Vector2(-1950, 1800);
                SoundManager.PlaySound("HereWeGo");
            }

            if (CurWorldHealth == MaxWorldHealth && NumEnemies == 0 && NumAnimals == MaxAnimals && !isGameWon)
            {
                isGameWon = true;
                SoundManager.PlaySound("Victory");
            }

            if (!isGameWon)
                UpdateKeeper.getInstance().updateAll(gameTime);
        }       

        public override void DrawGame(SpriteBatch spriteBatch)
        {          
            spriteBatch.Draw(ContentManager.GetTexture("Background"), new Rectangle(-WORLDWIDTH / 2, -WORLDHEIGHT / 2, WORLDWIDTH / 2, WORLDHEIGHT / 2), Color.White);
            spriteBatch.Draw(ContentManager.GetTexture("Background"), new Rectangle(0, -WORLDHEIGHT / 2, WORLDWIDTH / 2, WORLDHEIGHT / 2), Color.White);
            spriteBatch.Draw(ContentManager.GetTexture("Background"), new Rectangle(0, 0, WORLDWIDTH / 2, WORLDHEIGHT / 2), Color.White);
            spriteBatch.Draw(ContentManager.GetTexture("Background"), new Rectangle(-WORLDWIDTH / 2, 0, WORLDWIDTH / 2, WORLDHEIGHT / 2), Color.White);                                            
            
            DrawKeeper.getInstance().drawAll(spriteBatch);

            if (isGameWon)
                spriteBatch.DrawString(ContentManager.GetFont("Calibri"), "CONGRATULATIONS! YOU WON!", new Vector2(polarBear.Position.X - 250, polarBear.Position.Y), Color.Black);

            base.DrawGame(spriteBatch);
        }        
    }
}


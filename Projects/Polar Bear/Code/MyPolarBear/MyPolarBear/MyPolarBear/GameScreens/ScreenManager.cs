using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.Input;

namespace MyPolarBear.GameScreens
{
    public enum ScreenType
    {
        TitleScreen,
        PauseScreen,
        GameScreen
    }

    class ScreenManager : DrawableGameComponent
    {        
        public static ScreenType screenType;

        public static Camera camera;

        public static bool isExiting = false;
        public static bool isPaused = false;

        TitleScreen titleScreen;
        PauseScreen pauseScreen;
        GameScreen gameScreen;
        
        SpriteBatch spriteBatch;

        public ScreenManager(Game game) : base(game)
        {         
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);            
            camera = new Camera(Game.GraphicsDevice, true);

            titleScreen = new TitleScreen(ScreenType.TitleScreen);
            pauseScreen = new PauseScreen(ScreenType.PauseScreen);
            gameScreen = new GameScreen(ScreenType.GameScreen);
            gameScreen.LoadContent();                       
        }

        public override void Update(GameTime gameTime)
        {            
            InputManager.GamePad.Update();
            InputManager.Keyboard.Update();
            InputManager.Mouse.Update();

            camera.Update();

            switch (screenType)
            {
                case ScreenType.TitleScreen: titleScreen.UpdateEntries();
                    break;
                case ScreenType.PauseScreen: pauseScreen.UpdateEntries();
                    break;
                case ScreenType.GameScreen: gameScreen.Update(gameTime);
                    break;
            }

            if (isExiting)
            {
                this.Game.Exit();
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.TransformMatrix);

            switch (screenType)
            {
                case ScreenType.TitleScreen: titleScreen.DrawEntries(spriteBatch);
                    break;
                case ScreenType.PauseScreen: pauseScreen.DrawEntries(spriteBatch);
                    break;
                case ScreenType.GameScreen: gameScreen.DrawGame(spriteBatch);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}



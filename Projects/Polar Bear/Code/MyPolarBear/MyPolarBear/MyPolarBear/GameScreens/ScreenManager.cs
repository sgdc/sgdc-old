using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.Input;
using MyPolarBear.Audio;

namespace MyPolarBear.GameScreens
{
    public enum ScreenType
    {
        TitleScreen,
        PauseScreen,
        GameScreen,
        HeadsUpDisplay
    }  

    class ScreenManager : DrawableGameComponent
    {        
        public static ScreenType screenType;

        public static Camera camera;

        public static int SCREENWIDTH = 1280;
        public static int SCREENHEIGHT = 720;

        public static bool isExiting = false;
        public static bool isPaused = false;

        private TitleScreen titleScreen;
        private PauseScreen pauseScreen;
        private GameScreen gameScreen;
        private HUDScreen HUDScreen;
        
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
            HUDScreen = new HUDScreen(ScreenType.HeadsUpDisplay);            
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
                case ScreenType.PauseScreen: pauseScreen.UpdateEntries(); SoundManager.PauseAllMusic();
                    break;
                case ScreenType.GameScreen: gameScreen.Update(gameTime); SoundManager.PlayMusic("Techno");
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
            #region Draw Game Worlds
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, camera.TransformMatrix);
            switch (screenType)
            {
                case ScreenType.GameScreen:
                    gameScreen.DrawGame(spriteBatch);                    
                    break;
            }
            spriteBatch.End();
            #endregion

            #region Draw Menus 
            spriteBatch.Begin();
            switch (screenType)
            {
                case ScreenType.TitleScreen: titleScreen.DrawEntries(spriteBatch);
                    break;
                case ScreenType.PauseScreen: pauseScreen.DrawEntries(spriteBatch);
                    break;
                case ScreenType.GameScreen:  HUDScreen.DrawDisplay(spriteBatch);
                    break;
            }
            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}



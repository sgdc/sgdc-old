using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public static KeyboardComponent keyboard;
        public static GamePadComponent gamepad;
        public static MouseComponent mouse;
        public static SpriteFont spriteFont;
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
            spriteFont = Game.Content.Load<SpriteFont>("Fonts/Calibri");
            camera = new Camera(Game.GraphicsDevice, true);

            titleScreen = new TitleScreen(ScreenType.TitleScreen);
            pauseScreen = new PauseScreen(ScreenType.PauseScreen);
            gameScreen = new GameScreen(ScreenType.GameScreen);
            gameScreen.LoadContent();
            keyboard = new KeyboardComponent();
            gamepad = new GamePadComponent(PlayerIndex.One);
            mouse = new MouseComponent();
            
        }

        public override void Update(GameTime gameTime)
        {
            //TODO:    Update screens and keyboard
            keyboard.Update();
            gamepad.Update();
            mouse.Update();
            camera.Update();

            if (screenType == ScreenType.TitleScreen)
            {
                titleScreen.UpdateEntries();
            }
            else if (screenType == ScreenType.PauseScreen)
            {
                pauseScreen.UpdateEntries();
            }
            else if (screenType == ScreenType.GameScreen)
            {
                gameScreen.Update(gameTime);
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
            if (screenType == ScreenType.TitleScreen)
            {
                titleScreen.DrawEntries(spriteBatch);
            }
            else if (screenType == ScreenType.PauseScreen)
            {
                pauseScreen.DrawEntries(spriteBatch);
            }
            else if (screenType == ScreenType.GameScreen)
            {
                gameScreen.DrawGame(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}



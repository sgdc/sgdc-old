using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.Input;

namespace MyPolarBear.GameScreens
{
    class PauseScreen : Screen
    {
        public PauseScreen(ScreenType screenType) : base(screenType)
        {
            AddEntry(new Text("PAUSE MENU", new Vector2(ScreenManager.SCREENWIDTH / 2 - 110.0f, 100.0f)));
            AddEntry(new Text("Return to game", new Vector2(100, 100.0f)));
            AddEntry(new Text("Return to title screen", new Vector2(100, 0.0f)));
            AddEntry(new Text("Quit game", new Vector2(100, 0.0f)));
            FormatEntries();
        }

        public override void SelectEntry()
        {
            if (InputManager.GamePad.IsButtonReleased(Buttons.A) || InputManager.Keyboard.IsKeyReleased(Keys.Enter))
            {
                switch (Selection)
                {
                    case 1: ScreenManager.screenType = ScreenType.GameScreen; break;
                    case 2: ScreenManager.screenType = ScreenType.TitleScreen; GameScreen.Reset(); break;  
                    case 3: ScreenManager.isExiting = true; break;
                }
            }

            base.SelectEntry();
        } 
    }
}

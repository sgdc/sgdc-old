using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.Input;
using MyPolarBear.Audio;

namespace MyPolarBear.GameScreens
{
    class TitleScreen : Screen
    {
        public TitleScreen(ScreenType screenType) : base(screenType)
        {
            AddEntry(new Text("URSO'S ADVENTURES", new Vector2(ScreenManager.SCREENWIDTH / 2 - 160.0f, 100.0f)));            
            AddEntry(new Text("Play game", new Vector2(100.0f, 100.0f)));
            AddEntry(new Text("Quit game", new Vector2(100.0f, 0.0f)));
            FormatEntries();
        }

        public override void SelectEntry()
        {
            if (InputManager.GamePad.IsButtonReleased(Buttons.A) || InputManager.Keyboard.IsKeyReleased(Keys.Enter))
            {
                switch (Selection)
                {
                    case 1: ScreenManager.screenType = ScreenType.GameScreen; break;                        
                    case 2: ScreenManager.isExiting = true; break;
                }
            }

            base.SelectEntry();
        }        
    }
}

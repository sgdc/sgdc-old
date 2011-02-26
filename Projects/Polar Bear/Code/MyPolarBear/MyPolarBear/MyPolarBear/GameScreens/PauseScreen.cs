using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyPolarBear;

namespace MyPolarBear.GameScreens
{
    class PauseScreen : Screen
    {
        public PauseScreen(ScreenType screenType) : base(screenType)
        {
            AddEntry(new Text("PAUSE MENU", new Vector2(-100.0f, -150.0f)));
            AddEntry(new Text("Return to game", new Vector2(-300, 0.0f)));
            AddEntry(new Text("Return to title screen", new Vector2(-300, 0.0f)));
            AddEntry(new Text("Quit game", new Vector2(-300, 0.0f)));
            FormatEntries();
        }

        public override void SelectEntry()
        {
            if (ScreenManager.gamepad.IsButtonReleased(Buttons.A) || ScreenManager.keyboard.IsKeyReleased(Keys.Enter))
            {
                switch (Selection)
                {
                    case 1: ScreenManager.screenType = ScreenType.GameScreen; break;
                    case 2: ScreenManager.screenType = ScreenType.TitleScreen; break;  
                    case 3: ScreenManager.isExiting = true; break;
                }
            }

            base.SelectEntry();
        } 
    }
}

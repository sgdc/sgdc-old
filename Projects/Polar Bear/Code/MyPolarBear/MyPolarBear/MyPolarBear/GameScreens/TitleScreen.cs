using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.Input;

namespace MyPolarBear.GameScreens
{
    class TitleScreen : Screen
    {
        public TitleScreen(ScreenType screenType) : base(screenType)
        {
            AddEntry(new Text("POLAR BEAR", new Vector2(-100.0f, -150.0f)));            
            AddEntry(new Text("Play game", new Vector2(-300.0f, 0.0f)));
            AddEntry(new Text("Quit game", new Vector2(-300.0f, 0.0f)));
            FormatEntries();
            #region Help Items
            /*helpEntries = new List<Text>();
            helpEntries.Add(new Text("HELP", new Vector2(350, 80)));
            helpEntries.Add(new Text("Left Arrow - Move Left", new Vector2(30, 5.0f)));
            helpEntries.Add(new Text("Right Arrow - Move Right", new Vector2(30, 5.0f)));
            helpEntries.Add(new Text("Left Thumbstick - Move Paddle", new Vector2(30, 5.0f)));
            helpEntries.Add(new Text("Space Bar or B Button - Reset Ball", new Vector2(30, 5.0f)));
            helpEntries.Add(new Text("Escape or Back Button - Quit game", new Vector2(30, 5.0f)));
            helpEntries.Add(new Text("< Go back to the Main Menu", new Vector2(30, 5.0f)));*/
            #endregion
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

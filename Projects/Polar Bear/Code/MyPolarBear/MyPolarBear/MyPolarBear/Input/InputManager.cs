using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MyPolarBear.Input
{
    class InputManager
    {
        public static GamePadComponent GamePad;
        public static KeyboardComponent Keyboard;
        public static MouseComponent Mouse;

        public InputManager()
        {
            GamePad = new GamePadComponent(PlayerIndex.One);
            Keyboard = new KeyboardComponent();
            Mouse = new MouseComponent();
        }
    }
}

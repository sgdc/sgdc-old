using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MyPolarBear.Input
{
    class KeyboardComponent
    {
        private KeyboardState _currentState;
        public KeyboardState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        private KeyboardState _pastState;
        public KeyboardState PastState
        {
            get { return _pastState; }
            set { _pastState = value; }
        }

        public void Update()
        {
            PastState = CurrentState;
            CurrentState = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return CurrentState.IsKeyDown(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return (CurrentState.IsKeyDown(key) && PastState.IsKeyUp(key));
        }

        public Keys[] GetPressedKeys()
        {
            return CurrentState.GetPressedKeys();
        }      
    }
}

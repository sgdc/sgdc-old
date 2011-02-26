using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MyPolarBear.Input
{
    class GamePadComponent
    {
        private GamePadState _currentState;
        public GamePadState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        private GamePadState _pastState;
        public GamePadState PastState
        {
            get { return _pastState; }
            set { _pastState = value; }
        }

        private PlayerIndex _playerIndex;
        public PlayerIndex PlayerIndex
        {
            get { return _playerIndex; }
            set { _playerIndex = value; }
        }

        public enum Thumbstick
        {
            Left,
            Right
        }

        public enum Trigger
        {
            Left,
            Right
        }

        public enum Button
        {
            A,
            B,
            Back,
            BigButton,
            DPadDown,
            DPadLeft,
            DPadRight,
            DPadUp,
            LeftShoulder,
            LeftStick,
            LeftThumbstickDown,
            LeftThumbstickLeft,
            LeftThumbstickRight,
            LeftThumbstickUp,
            RightShoulder,
            RightStick,
            RightThumbstickDown,
            RightThumbstickLeft,
            RightThumbstickRight,
            RightThumbstickUp,
            Start,
            X,
            Y
        }

        public GamePadComponent(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }

        public void Update()
        {
            PastState = CurrentState;
            CurrentState = GamePad.GetState(PlayerIndex);
        }

        public bool IsButtonPressed(Buttons button)
        {
            return CurrentState.IsButtonDown(button);
        }

        public bool IsButtonReleased(Buttons button)
        {
            return (CurrentState.IsButtonDown(button) && PastState.IsButtonUp(button));
        }

        public Vector2 GetThumbStickState(Thumbstick thumbStick)
        {
            switch (thumbStick)
            {
                case Thumbstick.Left:
                    return CurrentState.ThumbSticks.Left * new Vector2(1, -1);
                case Thumbstick.Right:
                    return CurrentState.ThumbSticks.Right * new Vector2(1, -1);
            }
            return Vector2.Zero;
        }

        public float GetTriggerState(Trigger trigger)
        {
            switch (trigger)
            {
                case Trigger.Left:
                    return CurrentState.Triggers.Left;
                case Trigger.Right:
                    return CurrentState.Triggers.Right;
            }
            return 0;
        }
    }
}

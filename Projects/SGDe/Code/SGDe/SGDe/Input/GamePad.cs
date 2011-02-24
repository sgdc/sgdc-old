using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SGDE.Input
{
    /// <summary>
    /// The possible input components of a GamePad.
    /// </summary>
    public enum GamePadComponent
    {
        /// <summary>
        /// The A button.
        /// </summary>
        A,
        /// <summary>
        /// The B button.
        /// </summary>
        B,
        /// <summary>
        /// The X button.
        /// </summary>
        X,
        /// <summary>
        /// The Y button.
        /// </summary>
        Y,
        /// <summary>
        /// The Start button.
        /// </summary>
        Start,
        /// <summary>
        /// The Back button.
        /// </summary>
        Back,
        /// <summary>
        /// The BigButton button.
        /// </summary>
        BigButton,
        /// <summary>
        /// D-Pad, Up
        /// </summary>
        DPadUp,
        /// <summary>
        /// D-Pad, Right
        /// </summary>
        DPadRight,
        /// <summary>
        /// D-Pad, Down
        /// </summary>
        DPadDown,
        /// <summary>
        /// D-Pad, Left
        /// </summary>
        DPadLeft,
        /// <summary>
        /// The Left Shoulder (bumper) button.
        /// </summary>
        LeftShoulder,
        /// <summary>
        /// The Right Shoulder (bumper) button.
        /// </summary>
        RightShoulder,
        /// <summary>
        /// The Left Stick.
        /// </summary>
        LeftStick,
        /// <summary>
        /// The Right Stick.
        /// </summary>
        RightStick,
        /// <summary>
        /// The Left Trigger.
        /// </summary>
        LeftTrigger,
        /// <summary>
        /// The Left Trigger.
        /// </summary>
        RightTrigger
    }

    /// <summary>
    /// A gamepad input device.
    /// </summary>
    public sealed class GamePad : InputComponent
    {
        private InputManager manager;
        internal PlayerIndex index;

        internal GamePad(InputManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Always returns GamePad.
        /// </summary>
        public InputType Type
        {
            get
            {
                return InputType.GamePad;
            }
        }

        /// <summary>
        /// Get what PlayerIndex this GamePad represents.
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get
            {
                return this.index;
            }
        }

        /// <summary>
        /// Get if the GamePad is connected.
        /// </summary>
        public bool IsConnected
        {
            get
            {
#if !WINDOWS_PHONE
                return this.manager.c_pad_states[(int)index].IsConnected;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Get or set the GamePad's dead zone. This affects only the GamePad at the specified <see cref="PlayerIndex"/>. If set, the dead zone will be updated on the next update.
        /// </summary>
        public GamePadDeadZone DeadZone
        {
#if !WINDOWS_PHONE
            get
            {
                return this.manager.pad_zone[(int)index];
            }
            set
            {
                this.manager.pad_zone[(int)index] = value;
            }
#else
            get
            {
                return GamePadDeadZone.IndependentAxes;
            }
            set
            {
            }
#endif
        }

        /// <summary>
        /// Get what kind of GamePad is in use.
        /// </summary>
        public GamePadType GamePadType
        {
            get
            {
                return Microsoft.Xna.Framework.Input.GamePad.GetCapabilities(index).GamePadType;
            }
        }

        /// <summary>
        /// Get the current GamePad input state.
        /// </summary>
        /// <returns>The current, native, input state of the GamePad.</returns>
        public GamePadState GetCurrentState()
        {
#if !WINDOWS_PHONE
            return this.manager.c_pad_states[(int)index];
#else
            return default(GamePadState);
#endif
        }

        /// <summary>
        /// Get the past GamePad input state.
        /// </summary>
        /// <returns>The past, native, input state of the GamePad.</returns>
        public GamePadState GetPastState()
        {
#if !WINDOWS_PHONE
            return this.manager.o_pad_states[(int)index];
#else
            return default(GamePadState);
#endif
        }

        /// <summary>
        /// Set the game pad's vibration.
        /// </summary>
        /// <param name="motor">The speed of the motor, between 0.0 and 1.0.</param>
        /// <param name="leftMotor"><code>true</code> if the vibration should be set on the left, low-frequency, motor. <code>false</code> if the vibration should be set on the right, high-frequency, motor.</param>
        /// <returns><code>true</code> if the vibration was set, <code>false</code> if otherwise.</returns>
        public bool SetVibration(float motor, bool leftMotor)
        {
            float left = leftMotor ? motor : 0;
            float right = leftMotor ? 0 : motor;
            return SetVibration(left, right);
        }

        /// <summary>
        /// Set the game pad's vibration.
        /// </summary>
        /// <param name="leftMotor">The speed of the left motor, between 0.0 and 1.0. This motor is a low-frequency motor.</param>
        /// <param name="rightMotor">The speed of the right motor, between 0.0 and 1.0. This motor is a high-frequency motor.</param>
        /// <returns><code>true</code> if the vibration was set, <code>false</code> if otherwise.</returns>
        public bool SetVibration(float leftMotor, float rightMotor)
        {
            return Microsoft.Xna.Framework.Input.GamePad.SetVibration(index, leftMotor, rightMotor);
        }

        /// <summary>
        /// Determine if the specified button is pressed.
        /// </summary>
        /// <param name="com">The button to check if pressed. Supports A, B, X, Y, Back, Start, BigButton, LeftShoulder, LeftStick, RightShoulder, RightStick, and the D-Pad.</param>
        /// <returns><code>true</code> if the button was pressed, <code>false</code> if otherwise.</returns>
        public bool IsButtonPressed(GamePadComponent com)
        {
#if !WINDOWS_PHONE
            return this.manager.c_pad_states[(int)index].IsButtonDown(Component2Button(com));
#else
            return false;
#endif
        }

        /// <summary>
        /// Determine if the specified button is clicked. This means that the button is down right now but wasn't always pressed down.
        /// </summary>
        /// <param name="com">The button to check if pressed. Supports A, B, X, Y, Back, Start, BigButton, LeftShoulder, LeftStick, RightShoulder, RightStick, and the D-Pad.</param>
        /// <returns><code>true</code> if the button was pressed, <code>false</code> if otherwise.</returns>
        public bool IsButtonClicked(GamePadComponent com)
        {
            Buttons but = Component2Button(com);
#if !WINDOWS_PHONE
            return this.manager.c_pad_states[(int)index].IsButtonDown(but) && this.manager.o_pad_states[(int)index].IsButtonUp(but);
#else
            return false;
#endif
        }

        private Buttons Component2Button(GamePadComponent com)
        {
            Buttons but = 0;
            //Redo so multiple GamePadComponents can be checked at the same time.
            switch (com)
            {
                case GamePadComponent.A:
                    but = Buttons.A;
                    break;
                case GamePadComponent.B:
                    but = Buttons.B;
                    break;
                case GamePadComponent.X:
                    but = Buttons.X;
                    break;
                case GamePadComponent.Y:
                    but = Buttons.Y;
                    break;
                case GamePadComponent.Back:
                    but = Buttons.Back;
                    break;
                case GamePadComponent.Start:
                    but = Buttons.Start;
                    break;
                case GamePadComponent.BigButton:
                    but = Buttons.BigButton;
                    break;
                case GamePadComponent.LeftShoulder:
                    but = Buttons.LeftShoulder;
                    break;
                case GamePadComponent.LeftStick:
                    but = Buttons.LeftStick;
                    break;
                case GamePadComponent.RightShoulder:
                    but = Buttons.RightShoulder;
                    break;
                case GamePadComponent.RightStick:
                    but = Buttons.RightStick;
                    break;
                case GamePadComponent.DPadUp:
                    but = Buttons.DPadUp;
                    break;
                case GamePadComponent.DPadRight:
                    but = Buttons.DPadRight;
                    break;
                case GamePadComponent.DPadDown:
                    but = Buttons.DPadDown;
                    break;
                case GamePadComponent.DPadLeft:
                    but = Buttons.DPadLeft;
                    break;
            }
            return but;
        }

        /// <summary>
        /// Get the position of the thumbstick control as a 2D vector.
        /// </summary>
        /// <param name="com">Which thumbstick to get, only support LeftStick, and RightStick.</param>
        /// <returns>A 2D vector defining the thumbstick position.</returns>
        public Vector2 GetThumbstickPosition(GamePadComponent com)
        {
            GamePadThumbSticks stick = GetThumbStick((int)index, true);
            switch (com)
            {
                case GamePadComponent.LeftStick:
                    return stick.Left;
                case GamePadComponent.RightStick:
                    return stick.Right;
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Get the past position of the thumbstick control as a 2D vector.
        /// </summary>
        /// <param name="com">Which thumbstick to get, only support LeftStick, and RightStick.</param>
        /// <returns>A 2D vector defining the thumbstick position from the previous state.</returns>
        public Vector2 GetPastThumbstickPosition(GamePadComponent com)
        {
            GamePadThumbSticks stick = GetThumbStick((int)index, false);
            switch (com)
            {
                case GamePadComponent.LeftStick:
                    return stick.Left;
                case GamePadComponent.RightStick:
                    return stick.Right;
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Get the difference of the thumbstick between the current and previous state.
        /// </summary>
        /// <param name="com">Which thumbstick to get, only support LeftStick, and RightStick.</param>
        /// <returns>The difference between the current and past state as a 2D vector.</returns>
        public Vector2 GetThumbstickPositionDifference(GamePadComponent com)
        {
            GamePadThumbSticks cstick = GetThumbStick((int)index, true);
            GamePadThumbSticks ostick = GetThumbStick((int)index, true);
            switch (com)
            {
                case GamePadComponent.LeftStick:
                    return cstick.Left - ostick.Left;
                case GamePadComponent.RightStick:
                    return cstick.Right - ostick.Right;
            }
            return Vector2.Zero;
        }

        private GamePadThumbSticks GetThumbStick(int index, bool cur)
        {
#if !WINDOWS_PHONE
            if (cur)
            {
                return this.manager.c_pad_states[index].ThumbSticks;
            }
            else
            {
                return this.manager.o_pad_states[index].ThumbSticks;
            }
#else
            return default(GamePadThumbSticks);
#endif
        }

        /// <summary>
        /// Get the trigger position.
        /// </summary>
        /// <param name="com">Which trigger to get, only support LeftTrigger, and RightTrigger.</param>
        /// <returns>A scalar value from 0 to 1 defining the trigger position where 0 is a completely unpressed trigger.</returns>
        public float GetTrigger(GamePadComponent com)
        {
            GamePadTriggers trigger = GetTriggers((int)index, true);
            switch (com)
            {
                case GamePadComponent.LeftTrigger:
                    return trigger.Left;
                case GamePadComponent.RightTrigger:
                    return trigger.Right;
            }
            return 0;
        }

        /// <summary>
        /// Get the past trigger position.
        /// </summary>
        /// <param name="com">Which trigger to get, only support LeftTrigger, and RightTrigger.</param>
        /// <returns>A scalar value from 0 to 1 defining the trigger position of the past state where 0 is a completely unpressed trigger.</returns>
        public float GetPastTrigger(GamePadComponent com)
        {
            GamePadTriggers trigger = GetTriggers((int)index, false);
            switch (com)
            {
                case GamePadComponent.LeftTrigger:
                    return trigger.Left;
                case GamePadComponent.RightTrigger:
                    return trigger.Right;
            }
            return 0;
        }

        /// <summary>
        /// Get the difference of the trigger between the current and previous state.
        /// </summary>
        /// <param name="com">Which trigger to get, only support LeftTrigger, and RightTrigger.</param>
        /// <returns>The difference between the current and past state as a scalar value between 0 and 1.</returns>
        public float GetTriggerDifference(GamePadComponent com)
        {
            GamePadTriggers ctrigger = GetTriggers((int)index, true);
            GamePadTriggers otrigger = GetTriggers((int)index, false);
            switch (com)
            {
                case GamePadComponent.LeftStick:
                    return ctrigger.Left - otrigger.Left;
                case GamePadComponent.RightStick:
                    return ctrigger.Right - otrigger.Right;
            }
            return 0;
        }

        private GamePadTriggers GetTriggers(int index, bool cur)
        {
#if !WINDOWS_PHONE
            if (cur)
            {
                return this.manager.c_pad_states[index].Triggers;
            }
            else
            {
                return this.manager.o_pad_states[index].Triggers;
            }
#else
            return default(GamePadTriggers);
#endif
        }

        /// <summary>
        /// Get what the GamePad is capable of.
        /// </summary>
        /// <returns>The current GamePad's capabilities.</returns>
        public GamePadCapabilities GetGamePadCapability()
        {
            return Microsoft.Xna.Framework.Input.GamePad.GetCapabilities(this.index);
        }
    }
}

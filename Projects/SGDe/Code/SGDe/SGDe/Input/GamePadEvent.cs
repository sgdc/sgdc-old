using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SGDE.SceneManagement;
using SGDE.Content;

namespace SGDE
{
    public class GamePadEvent
    {
        public enum ThumbSticks { Left, Right };
        public enum Triggers { Left, Right };

        private Buttons _button;
        private ThumbSticks _thumbstick;
        private Triggers _trigger;

        public delegate void ButtonEvent(bool state);
        private event ButtonEvent _buttonEvent;

        public delegate void ThumbStickEvent(Vector2 amount);
        private event ThumbStickEvent _thumbStickEvent;

        public delegate void TriggerEvent(float amount);
        private event TriggerEvent _triggerEvent;

        /// <summary>
        /// Constructor for an Xbox 360 Controller Button Event
        /// </summary>
        /// <param name="button">A, B, X, Y, LeftShoulder, RightShoulder, LeftStick, RightStick, Back, Start, 
        ///                     DPadLeft, DPadRight, DPadUp, DPadDown</param>
        /// <param name="buttonEvent">Event for a button with a callback function</param>
        public GamePadEvent(Buttons button, ButtonEvent buttonEvent)
        {
            _button = button;
            _buttonEvent += new ButtonEvent(buttonEvent);
        }
        /// <summary>
        /// Constructor for an Xbox 360 Controller Thumbstick Event
        /// </summary>
        /// <param name="thumbstick">Left, Right</param>
        /// <param name="thumbStickEvent">Event for a thumbstick with a callback function</param>
        public GamePadEvent(ThumbSticks thumbstick, ThumbStickEvent thumbStickEvent)
        {
            _thumbstick = thumbstick;
            _thumbStickEvent += new ThumbStickEvent(thumbStickEvent);
        }
        /// <summary>
        /// Constructor for an Xbox 360 Controller Trigger Event 
        /// </summary>
        /// <param name="trigger">Left, Right</param>
        /// <param name="triggerEvent">Event for a trigger with a callback function</param>
        public GamePadEvent(Triggers trigger, TriggerEvent triggerEvent)
        {
            _trigger = trigger;
            _triggerEvent += new TriggerEvent(triggerEvent);
        }

        public void HandleEvents(GamePadState gamePadState)
        {
            if (gamePadState.IsButtonDown(_button))
            {
                if (_buttonEvent != null)
                {
                    //a button is down
                    _buttonEvent(true);
                }
            }
            if (gamePadState.IsButtonUp(_button))
            {
                if (_buttonEvent != null)
                {
                    //a button is Up
                    _buttonEvent(false);
                }
            }
            if (_thumbstick == ThumbSticks.Left)
            {
                if (_thumbStickEvent != null)
                {
                    //left thumbstick was moved
                    _thumbStickEvent(gamePadState.ThumbSticks.Left);
                }
            }
            if (_thumbstick == ThumbSticks.Right)
            {
                if (_thumbStickEvent != null)
                {
                    //right thumbstick was moved
                    _thumbStickEvent(gamePadState.ThumbSticks.Right);
                }
            }
            if (_trigger == Triggers.Left)
            {
                if (_triggerEvent != null)
                {
                    //left trigger was pressed
                    _triggerEvent(gamePadState.Triggers.Left);
                }
            }
            if (_trigger == Triggers.Right)
            {
                if (_triggerEvent != null)
                {
                    //right trigger was pressed
                    _triggerEvent(gamePadState.Triggers.Right);
                }
            }
        }
    }
}
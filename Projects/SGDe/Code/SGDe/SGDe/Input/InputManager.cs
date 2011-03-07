using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace SGDE.Input
{
    #region InputManager

    /// <summary>
    /// Handles all input.
    /// </summary>
    public class InputManager
    {
        private InputType handles;

        private List<InputHandler> handlers;
        private List<InputConverter> converters;
        private List<InputConversionHandlerInternal> conversionHandlers;
        internal List<InputComponent> components;

        //Keyboard
        internal KeyboardState c_key_state, o_key_state;
        private int keyIndex, keyConvertIndex;

        //Touchscreen
        internal TouchCollection c_touch_state, o_touch_state;
        private int touchIndex, touchConvertIndex;

        //Gamepad controller
        internal GamePadState[] c_pad_states, o_pad_states;
        internal GamePadDeadZone[] pad_zone;
        private bool[] pad_loaded;
        private int padConvertIndex;
        internal int padIndex;

        //Mouse
        internal MouseState c_mouse_state, o_mouse_state;
        private int mouseIndex, mouseConvertIndex;

#if WINDOWS
        //TODO Kinect ;)
#endif

        internal InputManager()
        {
        }

        private bool Ignore(InputHandler handle)
        {
            //We only want to ignore them if they are the only thing it supports. Support for multiple devices is fine.
#if !WINDOWS
            //TODO Kinect ;)
#endif
#if XBOX
            if (handle.Handles == InputType.Mouse)
            {
                return true;
            }
#endif
#if WINDOWS_PHONE
            if (handle.Handles == InputType.GamePad)
            {
                return true;
            }
#else
            if (handle.Handles == InputType.Touchscreen)
            {
                return true;
            }
#endif
            return false;
        }

        /// <summary>
        /// Add a new input handler.
        /// </summary>
        /// <param name="handler">The input handler to add.</param>
        public bool AddNewHandler(InputHandler handler)
        {
            if (handler != null)
            {
                if (Ignore(handler))
                {
                    //Don't add unneccessery handlers
                    return false;
                }
                if (handlers == null)
                {
                    handlers = new List<InputHandler>();
                }
                if (!handlers.Contains(handler))
                {
                    handlers.Add(handler);
                    if ((handler.Handles & this.handles) != handler.Handles)
                    {
                        if (components == null)
                        {
                            components = new List<InputComponent>(4);
                        }

                        //Add new handlers
#if WINDOWS_PHONE
                        if (((handler.Handles & InputType.Touchscreen) == InputType.Touchscreen) && ((this.handles & InputType.Touchscreen) != InputType.Touchscreen))
                        {
                            AddTouchSupport();
                        }
#else
                        if (((handler.Handles & InputType.GamePad) == InputType.GamePad) && ((this.handles & InputType.GamePad) != InputType.GamePad))
                        {
                            AddGamePadSupport();
                        }
#endif
#if !XBOX
                        if (((handler.Handles & InputType.Mouse) == InputType.Mouse) && ((this.handles & InputType.Mouse) != InputType.Mouse))
                        {
                            AddMouseSupport();
                        }
#endif
#if WINDOWS
                        //TODO Kinect ;)
#endif
                        if (((handler.Handles & InputType.Keyboard) == InputType.Keyboard) && ((this.handles & InputType.Keyboard) != InputType.Keyboard))
                        {
                            AddKeyboardSupport();
                        }
                    }
#if !WINDOWS_PHONE
                    if ((handler.Handles & InputType.GamePad) == InputType.GamePad)
                    {
                        if (handler.IndexSpecific)
                        {
                            InitializeGamePad(handler.Index);
                        }
                        else if (!pad_loaded[0])
                        {
                            InitializeGamePad(PlayerIndex.One);
                        }
                    }
#endif
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove a input handler.
        /// </summary>
        /// <param name="handler">The input handler to remove.</param>
        /// <returns><code>true</code> if the handler was removed, <code>false</code> if otherwise.</returns>
        public bool RemoveHandler(InputHandler handler)
        {
            if (handler != null)
            {
                if (handlers != null)
                {
                    if (handlers.Contains(handler))
                    {
                        return handlers.Remove(handler);
                    }
                }
            }
            return false;
        }

        private void AddTouchSupport()
        {
            this.handles |= InputType.Touchscreen;
            touchIndex = components.Count;
            components.Add(new Touchscreen(this));
            o_touch_state = c_touch_state = TouchPanel.GetState();
        }

        private void AddGamePadSupport()
        {
            this.handles |= InputType.GamePad;
            padIndex = components.Count;
            components.Add(new GamePad(this));
            c_pad_states = new GamePadState[(int)PlayerIndex.Four + 1];
            o_pad_states = new GamePadState[c_pad_states.Length];
            pad_loaded = new bool[c_pad_states.Length];
            pad_zone = new GamePadDeadZone[c_pad_states.Length];
        }

        private void InitializeGamePad(PlayerIndex index)
        {
            int i = (int)index;
            if (!pad_loaded[i])
            {
                pad_zone[i] = GamePadDeadZone.IndependentAxes;
                c_pad_states[i] = o_pad_states[i] = Microsoft.Xna.Framework.Input.GamePad.GetState(index);
                pad_loaded[i] = true;
            }
        }

        private void AddMouseSupport()
        {
            this.handles |= InputType.Mouse;
            mouseIndex = components.Count;
            components.Add(new Mouse(this));
            o_mouse_state = c_mouse_state = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        private void AddKeyboardSupport()
        {
            this.handles |= InputType.Keyboard;
            keyIndex = components.Count;
            components.Add(new Keyboard(this));
            o_key_state = c_key_state = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        }

        /// <summary>
        /// Add a new input converter.
        /// </summary>
        /// <param name="converter">The input converter to add.</param>
        /// <returns><code>true</code> if the converter was added, <code>false</code> if otherwise.</returns>
        public bool AddNewConverter(InputConverter converter)
        {
            if (converter != null)
            {
                //Check to make sure it's valid
                if ((converter.ConvertFrom.SeperatedFlags<InputType>().Length > 1) || (converter.ConvertTo.SeperatedFlags<InputType>().Length > 1))
                {
                    return false;
                }
                if (converter.ConvertTo == converter.ConvertFrom)
                {
                    return false;
                }
                //Add converter
                if (converters == null)
                {
                    converters = new List<InputConverter>();
                    conversionHandlers = new List<InputConversionHandlerInternal>();
                }
                if (!converters.Contains(converter))
                {
                    converters.Add(converter);

                    //Handle the convert to's
                    switch (converter.ConvertTo)
                    {
                        case InputType.Touchscreen:
                            int count = (touchConvertIndex & 0x7FFFFFF8) >> 3;
                            if (count == 0 && touchConvertIndex == 0)
                            {
                                touchConvertIndex = conversionHandlers.Count;
                                conversionHandlers.Add(new TouchConversionHandler(this));
                                if ((this.handles & InputType.Touchscreen) != InputType.Touchscreen)
                                {
                                    AddTouchSupport();
                                }
                            }
                            touchConvertIndex = (touchConvertIndex & 0x7) | ((count + 1) << 3);
                            break;
                        case InputType.GamePad:
                            count = (padConvertIndex & 0x7FFFFFF8) >> 3;
                            if (count == 0 && padConvertIndex == 0)
                            {
                                padConvertIndex = conversionHandlers.Count;
                                conversionHandlers.Add(new GamePadConversionHandler(this));
                                if ((this.handles & InputType.GamePad) != InputType.GamePad)
                                {
                                    AddGamePadSupport();
                                    InitializeGamePad(PlayerIndex.One);
                                    InitializeGamePad(PlayerIndex.Two);
                                    InitializeGamePad(PlayerIndex.Three);
                                    InitializeGamePad(PlayerIndex.Four);
                                }
                            }
                            padConvertIndex = (padConvertIndex & 0x7) | ((count + 1) << 3);
                            break;
                        case InputType.Mouse:
                            count = (mouseConvertIndex & 0x7FFFFFF8) >> 3;
                            if (count == 0 && mouseConvertIndex == 0)
                            {
                                mouseConvertIndex = conversionHandlers.Count;
                                conversionHandlers.Add(new MouseConversionHandler(this));
                                if ((this.handles & InputType.Mouse) != InputType.Mouse)
                                {
                                    AddMouseSupport();
                                }
                            }
                            mouseConvertIndex = (mouseConvertIndex & 0x7) | ((count + 1) << 3);
                            break;
                        case InputType.Keyboard:
                            count = (keyConvertIndex & 0x7FFFFFF8) >> 3;
                            if (count == 0 && keyConvertIndex == 0)
                            {
                                keyConvertIndex = conversionHandlers.Count;
                                conversionHandlers.Add(new KeyConversionHandler(this));
                                if ((this.handles & InputType.Keyboard) != InputType.Keyboard)
                                {
                                    AddKeyboardSupport();
                                }
                            }
                            keyConvertIndex = (keyConvertIndex & 0x7) | ((count + 1) << 3);
                            break;
                        default:
                            throw new ArgumentException(string.Format(Messages.InputManager_UnkConvertTo, converter.ConvertTo));
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove a input converter.
        /// </summary>
        /// <param name="converter">The input converter to remove.</param>
        /// <returns><code>true</code> if the converter was removed, <code>false</code> if otherwise.</returns>
        public bool RemoveConverter(InputConverter converter)
        {
            if (converter != null)
            {
                if (converters != null)
                {
                    if (converters.Contains(converter))
                    {
                        switch (converter.ConvertTo)
                        {
                            case InputType.Touchscreen:
                                int count = (touchConvertIndex & 0x7FFFFFF8) >> 3;
                                if (count > 0)
                                {
                                    touchConvertIndex = (touchConvertIndex & 0x7) | ((count - 1) << 3);
                                }
                                break;
                            case InputType.GamePad:
                                count = (padConvertIndex & 0x7FFFFFF8) >> 3;
                                if (count > 0)
                                {
                                    padConvertIndex = (padConvertIndex & 0x7) | ((count - 1) << 3);
                                }
                                break;
                            case InputType.Mouse:
                                count = (mouseConvertIndex & 0x7FFFFFF8) >> 3;
                                if (count > 0)
                                {
                                    mouseConvertIndex = (mouseConvertIndex & 0x7) | ((count - 1) << 3);
                                }
                                break;
                            case InputType.Keyboard:
                                count = (keyConvertIndex & 0x7FFFFFF8) >> 3;
                                if (count > 0)
                                {
                                    keyConvertIndex = (keyConvertIndex & 0x7) | ((count - 1) << 3);
                                }
                                break;
                            default:
                                throw new ArgumentException(string.Format(Messages.InputManager_UnkConvertTo, converter.ConvertTo));
                        }
                        return converters.Remove(converter);
                    }
                }
            }
            return false;
        }

        internal void Update(Game game, GameTime gameTime)
        {
            if (this.handles != 0)
            {
                //Handle input
                if ((this.handles & InputType.Touchscreen) == InputType.Touchscreen)
                {
                    c_touch_state = TouchPanel.GetState();

                    InputConversion(InputType.Touchscreen);

                    foreach (InputHandler handler in this.handlers)
                    {
                        if (handler.Enabled)
                        {
                            if ((handler.Handles & InputType.Touchscreen) == InputType.Touchscreen)
                            {
                                handler.HandleInput(game, this.components[this.touchIndex]);
                            }
                        }
                    }

                    o_touch_state = c_touch_state;
                }

                if ((this.handles & InputType.GamePad) == InputType.GamePad)
                {
                    HandleGameInput(PlayerIndex.One, game);
                    HandleGameInput(PlayerIndex.Two, game);
                    HandleGameInput(PlayerIndex.Three, game);
                    HandleGameInput(PlayerIndex.Four, game);
                }

                if ((this.handles & InputType.Mouse) == InputType.Mouse)
                {
                    c_mouse_state = Microsoft.Xna.Framework.Input.Mouse.GetState();

                    InputConversion(InputType.Mouse);

                    foreach (InputHandler handler in this.handlers)
                    {
                        if (handler.Enabled)
                        {
                            if ((handler.Handles & InputType.Mouse) == InputType.Mouse)
                            {
                                handler.HandleInput(game, this.components[this.mouseIndex]);
                            }
                        }
                    }

                    o_mouse_state = c_mouse_state;
                }

#if WINDOWS
                //TODO Kinect ;)
#endif

                if ((this.handles & InputType.Keyboard) == InputType.Keyboard)
                {
                    c_key_state = Microsoft.Xna.Framework.Input.Keyboard.GetState();

                    InputConversion(InputType.Keyboard);

                    foreach (InputHandler handler in this.handlers)
                    {
                        if (handler.Enabled)
                        {
                            if ((handler.Handles & InputType.Keyboard) == InputType.Keyboard)
                            {
                                handler.HandleInput(game, this.components[this.keyIndex]);
                            }
                        }
                    }

                    o_key_state = c_key_state;
                }
            }
        }

        private void HandleGameInput(PlayerIndex index, Game game)
        {
            int ind = (int)index;
            if (pad_loaded[ind])
            {
                bool indexOne = index == PlayerIndex.One;
                c_pad_states[ind] = Microsoft.Xna.Framework.Input.GamePad.GetState(index, pad_zone[ind]);

                ((GamePad)this.components[this.padIndex]).index = index;

                InputConversion(InputType.GamePad);

                foreach (InputHandler handler in this.handlers)
                {
                    if (handler.Enabled)
                    {
                        if ((handler.Handles & InputType.GamePad) == InputType.GamePad)
                        {
                            if (handler.IndexSpecific)
                            {
                                if (handler.Index == index)
                                {
                                    handler.HandleInput(game, this.components[this.padIndex]);
                                }
                            }
                            else if (indexOne)
                            {
                                handler.HandleInput(game, this.components[this.padIndex]);
                            }
                        }
                    }
                }

                o_pad_states[ind] = c_pad_states[ind];
            }
        }

        private void InputConversion(InputType type)
        {
            int c;
            switch (type)
            {
                case InputType.Touchscreen:
                    c = this.touchConvertIndex;
                    break;
                case InputType.GamePad:
                    c = this.padConvertIndex;
                    break;
                case InputType.Mouse:
                    c = this.mouseConvertIndex;
                    break;
                case InputType.Keyboard:
                    c = this.keyConvertIndex;
                    break;
                default:
                    return;
            }
            if (((c & 0x7FFFFFF8) >> 3) == 0)
            {
                return;
            }
            InputConversionHandlerInternal conHand = this.conversionHandlers[c & 0x7];
            foreach (InputConverter con in this.converters)
            {
                if (con.ConvertTo == type)
                {
                    if ((this.handles & con.ConvertFrom) != con.ConvertFrom)
                    {
                        continue;
                    }
                    int i;
                    switch (con.ConvertFrom)
                    {
                        case InputType.Touchscreen:
                            i = this.touchIndex;
                            break;
                        case InputType.GamePad:
                            i = this.padIndex;
                            break;
                        case InputType.Mouse:
                            i = this.mouseIndex;
                            break;
                        case InputType.Keyboard:
                            i = this.keyIndex;
                            break;
                        default:
                            continue;
                    }

                    conHand.Commit(con.Convert(conHand, this.components[i]));
                }
            }
            conHand.Process();
        }
    }

    #endregion

    #region InputHandler

    /// <summary>
    /// Handle input operations.
    /// </summary>
    public interface InputHandler
    {
        /// <summary>
        /// Get what input type this handler can process.
        /// </summary>
        InputType Handles { get; }

        /// <summary>
        /// Get if this input handler is enabled.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Handle input operations. If this InputHandler supports more then one input device then it will be called more then once, one for each input device.
        /// </summary>
        /// <param name="game">The current, calling, game.</param>
        /// <param name="input">The input component that can be used to handle input.</param>
        void HandleInput(Game game, InputComponent input);

        /// <summary>
        /// Is the input handler input specific. Only used GamePad support.
        /// </summary>
        bool IndexSpecific { get; }

        /// <summary>
        /// What index this input handler supports. Used only when IndexSpecific is <code>true</code>.
        /// </summary>
        PlayerIndex Index { get; }
    }

    #endregion

    #region InputConverter

    /// <summary>
    /// Convert from one input format to another.
    /// </summary>
    public interface InputConverter
    {
        /// <summary>
        /// Get what input type this converter converts from. You cannot append more then one type. Only a basic comparison is used ot check "from" types.
        /// </summary>
        InputType ConvertFrom { get; }
        /// <summary>
        /// Get what input this converter converts to. You cannot append more then one type. Only a basic comparison is used ot check "to" types.
        /// </summary>
        InputType ConvertTo { get; }

        /// <summary>
        /// Convert input from one format to another. The input converter will handle each input individually. For example, if InputFrom is GamePad and only controller 1, and 3 are used; only a GamePad input component 
        /// for GamePad 1 and 3 will be passed in as input for 2 consecutive calls.
        /// </summary>
        /// <param name="handler">The conversion handler that is used to set the converted input.</param>
        /// <param name="input">The input component that has the input before conversion.</param>
        /// <returns><code>true</code> if the data was converted, <code>false</code> if otherwise.</returns>
        bool Convert(InputConversionHandler handler, InputComponent input);
    }

    #endregion

    #region InputConversionHandler

    /// <summary>
    /// Handles any conversion for input.
    /// </summary>
    public interface InputConversionHandler
    {
        /// <summary>
        /// Specify the converted value for input.
        /// </summary>
        /// <param name="input">The converted input value; such as Keys, Buttons, TouchLocations, etc.</param>
        /// <param name="context">Mainly for use by GamePad, the name of the component that input is for.</param>
        /// <param name="replace">
        /// If the value should replace the existing value. By default, only if the value is non-default in input is it set. For example a Key that is Unpressed will not be set, even if the current (or previously 
        /// converted input) has it as pressed. If this is <code>true</code> then it will replace it.
        /// </param>
        /// <returns><code>true</code> if the value was set, <code>false</code> if otherwise.</returns>
        /// <example>
        /// //Basic demonstration with conversion to GamePad input
        /// handler.SetValue(new Vector2(x, y), GamePadComponent.RightStick, false);    //Set the position of the Right stick
        /// handler.SetValue(0.994f, GamePadComponent.RightTrigger, false);             //Set the value for Right trigger
        /// handler.SetValue(GamePadComponent.LeftStick, null, false);                  //Set that left stick was clicked
        /// handler.SetValue(GamePadComponent.A, null, false);                          //Set that 'A' was clicked
        /// 
        /// //Basic demonstration with mouse
        /// handler.SetValue(120, "x", false);                                          //Set the x value of the mouse position
        /// handler.SetValue(MouseButton.LeftButton, null, false);                      //Set the left mouse button
        /// </example>
        bool SetValue(object input, object context, bool replace);
    }

    /// <summary>
    /// Internal conversion handler
    /// </summary>
    internal abstract class InputConversionHandlerInternal : InputConversionHandler
    {
        public abstract bool SetValue(object input, object context, bool replace);

        /// <summary>
        /// Process all the converted results and commit them to the input system.
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Commit the last converted input to the total commit values.
        /// </summary>
        /// <param name="commit"><code>true</code> if the conversion should be commited, <code>false</code> if otherwise.</param>
        public abstract void Commit(bool commit);

        protected void SetValue<T>(T? oT, ref T? v) where T : struct
        {
            if (oT.HasValue)
            {
                v = oT;
            }
        }

        protected void SetValue<T>(T? thisT, T oT, out T v) where T : struct
        {
            if (thisT.HasValue)
            {
                v = thisT.Value;
            }
            else
            {
                v = oT;
            }
        }
    }

    #endregion

    /// <summary>
    /// An input component from InputType.
    /// </summary>
    public interface InputComponent
    {
        /// <summary>
        /// Get what type of input this input component comprises.
        /// </summary>
        InputType Type { get; }
    }

    /// <summary>
    /// The input type in use
    /// </summary>
    [Flags]
    public enum InputType
    {
        /// <summary>
        /// Any game controller
        /// </summary>
        GamePad = 0x1,
        /// <summary>
        /// A Mouse
        /// </summary>
        Mouse = 0x2,
        /// <summary>
        /// A keyboard
        /// </summary>
        Keyboard = 0x4,
        /// <summary>
        /// A touchscreen device
        /// </summary>
        Touchscreen = 0x8,
        /* //TODO Kinect ;)
        /// <summary>
        /// A Kinect device.
        /// </summary>
        Kinect = 0x10
         */
    }
}

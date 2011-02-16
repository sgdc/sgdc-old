using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        private List<InputComponent> components;

        //Keyboard
        internal KeyboardState c_key_state, o_key_state;
        private int keyIndex;

#if WINDOWS_PHONE
        //Touchscreen
        //TODO
        private int touchIndex;
#else
        //Gamepad controller
        internal GamePadState[] c_pad_states, o_pad_states;
        private int padIndex;
        private static GamePadState defPad = default(GamePadState);
#endif

#if WINDOWS
        //Mouse
        internal MouseState c_mouse_state, o_mouse_state;
        private int mouseIndex;
#endif

        internal InputManager()
        {
        }

        internal void AddNewHandler(InputHandler handler)
        {
            if (handler != null)
            {
                if (handlers == null)
                {
                    handlers = new List<InputHandler>();
                }
                handlers.Add(handler);
                if ((handler.Handles & this.handles) != handler.Handles)
                {
                    components = new List<InputComponent>(4);

                    //Add new handlers
#if WINDOWS_PHONE
                    if (((handler.Handles & InputType.Touchscreen) == InputType.Touchscreen) && ((this.handles & InputType.Touchscreen) != InputType.Touchscreen))
                    {
                        //TODO
                    }
#else
                    if (((handler.Handles & InputType.GamePad) == InputType.GamePad) && ((this.handles & InputType.GamePad) != InputType.GamePad))
                    {
                        this.handles |= InputType.GamePad;
                        padIndex = components.Count;
                        //components.Add(new GamePad(this));
                    }
#endif
#if WINDOWS
                    if (((handler.Handles & InputType.Mouse) == InputType.Mouse) && ((this.handles & InputType.Mouse) != InputType.Mouse))
                    {
                        //TODO
                    }
#endif
                    if (((handler.Handles & InputType.Keyboard) == InputType.Keyboard) && ((this.handles & InputType.Keyboard) != InputType.Keyboard))
                    {
                        this.handles |= InputType.Keyboard;
                        keyIndex = components.Count;
                        components.Add(new Keyboard(this));
                        o_key_state = c_key_state = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                    }
                }
                if ((handler.Handles & InputType.GamePad) == InputType.GamePad)
                {
                    if (handler.IndexSpecific)
                    {
                        int index = (int)handler.Index;
                        if (c_pad_states[index].Equals(defPad))
                        {
                            c_pad_states[index] = o_pad_states[index] = Microsoft.Xna.Framework.Input.GamePad.GetState(handler.Index);
                        }
                    }
                    else if (c_pad_states[0].Equals(defPad))
                    {
                        c_pad_states[0] = o_pad_states[0] = Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One);
                    }
                }
            }
        }

        /// <summary>
        /// Add a new input converter.
        /// </summary>
        /// <param name="converter">The input converter to add.</param>
        public void AddNewConverter(InputConverter converter)
        {
            if (converter != null)
            {
                if (converters == null)
                {
                    converters = new List<InputConverter>();
                }
                converters.Add(converter);
            }
        }

        /// <summary>
        /// Remove a input converter.
        /// </summary>
        /// <param name="converter">The input converter to remove.</param>
        /// <returns><code>true</code> if the converter was removed, <code>false</code> if otherwise.</returns>
        public bool RemoveNewConverter(InputConverter converter)
        {
            if (converter != null)
            {
                if (converters != null)
                {
                    return converters.Remove(converter);
                }
            }
            return false;
        }

        internal void Update(Game game, GameTime gameTime)
        {
            if (this.handles != 0)
            {
                //Handle conversions
                //TODO

                //Handle input
#if WINDOWS_PHONE
                if ((this.handles & InputType.Touchscreen) == InputType.Touchscreen)
                {
                    //TODO
                }
#else
                if ((this.handles & InputType.GamePad) == InputType.GamePad)
                {
                    HandleGameInput(PlayerIndex.One, game);
                    HandleGameInput(PlayerIndex.Two, game);
                    HandleGameInput(PlayerIndex.Three, game);
                    HandleGameInput(PlayerIndex.Four, game);
                }
#endif
#if WINDOWS
                if ((this.handles & InputType.Mouse) == InputType.Mouse)
                {
                    //TOOD
                }
#endif
                if ((this.handles & InputType.Keyboard) == InputType.Keyboard)
                {
                    c_key_state = Microsoft.Xna.Framework.Input.Keyboard.GetState();

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
            if (!c_pad_states[ind].Equals(defPad))
            {
                bool indexOne = index == PlayerIndex.One;
                c_pad_states[ind] = GamePad.GetState(index);

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

        //TODO
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

        //TODO
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
        Touchscreen = 0x8
    }
}

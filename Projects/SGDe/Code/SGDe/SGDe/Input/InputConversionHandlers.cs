using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace SGDE.Input
{
    #region KeyConversionHandler

    internal sealed class KeyConversionHandler : InputConversionHandlerInternal
    {
        private InputManager man;

        private Dictionary<Keys, KeyState> tBuf;
        private Dictionary<Keys, KeyState> buf;
        private Dictionary<Keys, bool> pref;

        public KeyConversionHandler(InputManager man)
        {
            this.man = man;
        }

        public override bool SetValue(object input, object context, bool replace)
        {
            if (input is Keys)
            {
                if (this.buf == null)
                {
                    this.buf = new Dictionary<Keys, KeyState>();
                    this.pref = new Dictionary<Keys, bool>();
                }
                Keys k = (Keys)input;
                this.buf.Add(k, KeyState.Down);
                this.pref.Add(k, replace);
                return true;
            }
            else if (input is KeyState)
            {
                if (context is Keys)
                {
                    if (this.buf == null)
                    {
                        this.buf = new Dictionary<Keys, KeyState>();
                        this.pref = new Dictionary<Keys, bool>();
                    }
                    Keys k = (Keys)context;
                    this.buf.Add(k, (KeyState)input);
                    this.pref.Add(k, replace);
                    return true;
                }
            }
            return false;
        }

        public override void Commit(bool commit)
        {
            if (this.buf != null && this.buf.Count > 0)
            {
                if (commit)
                {
                    if (this.tBuf == null)
                    {
                        this.tBuf = new Dictionary<Keys, KeyState>();
                    }
                    foreach (KeyValuePair<Keys, KeyState> kb in this.buf)
                    {
                        if (this.tBuf.ContainsKey(kb.Key) && this.pref.ContainsKey(kb.Key))
                        {
                            if (this.pref[kb.Key])
                            {
                                this.tBuf[kb.Key] = kb.Value;
                            }
                        }
                        else
                        {
                            this.tBuf.Add(kb.Key, kb.Value);
                        }
                    }
                }
                this.buf.Clear();
                this.pref.Clear();
            }
        }

        public override void Process()
        {
            if (this.tBuf != null && this.tBuf.Count > 0)
            {
                List<Keys> keys = new List<Keys>(this.man.c_key_state.GetPressedKeys());
                foreach (KeyValuePair<Keys, KeyState> kb in this.tBuf)
                {
                    if (keys.Contains(kb.Key))
                    {
                        if (kb.Value == KeyState.Up)
                        {
                            keys.Remove(kb.Key);
                        }
                    }
                    else
                    {
                        if (kb.Value == KeyState.Down)
                        {
                            keys.Add(kb.Key);
                        }
                    }
                }
                this.tBuf.Clear();
                this.man.c_key_state = new KeyboardState(keys.ToArray());
            }
        }
    }

    #endregion

    #region MouseConversionHandler

    internal sealed class MouseConversionHandler : InputConversionHandlerInternal
    {
        private InputManager man;

        private int? x, y, wheel;
        private ButtonState? left, right, middle, x1, x2;

        private int? tx, ty, twheel;
        private ButtonState? tleft, tright, tmiddle, tx1, tx2;

        public MouseConversionHandler(InputManager man)
        {
            this.man = man;
        }

        public override bool SetValue(object input, object context, bool replace)
        {
            if (input is Vector2)
            {
                Vector2 v = (Vector2)input;
                this.tx = (int)Math.Round(v.X);
                this.ty = (int)Math.Round(v.Y);
                return true;
            }
            else if (input is int)
            {
                if (context is string)
                {
                    switch (((string)context).ToLower())
                    {
                        case "x":
                            this.tx = (int)input;
                            return true;
                        case "y":
                            this.ty = (int)input;
                            return true;
                        case "wheel":
                            this.twheel = (int)input;
                            return true;
                    }
                }
                else
                {
                    this.twheel = (int)input;
                    return true;
                }
            }
            else if (input is MouseButton || context is MouseButton)
            {
                MouseButton m;
                ButtonState state;
                if (input is MouseButton)
                {
                    m = (MouseButton)input;
                    state = ButtonState.Pressed;
                }
                else
                {
                    m = (MouseButton)context;
                    if (input is ButtonState)
                    {
                        state = (ButtonState)input;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (state == ButtonState.Released && !replace)
                {
                    return false;
                }
                switch (m)
                {
                    case MouseButton.LeftButton:
                        this.tleft = state;
                        return true;
                    case MouseButton.MiddleButton:
                        this.tmiddle = state;
                        return true;
                    case MouseButton.RightButton:
                        this.tright = state;
                        return true;
                    case MouseButton.XButton1:
                        this.tx1 = state;
                        return true;
                    case MouseButton.XButton2:
                        this.tx2 = state;
                        return true;
                }
            }
            return false;
        }

        public override void Commit(bool commit)
        {
            if (this.tx.HasValue || this.ty.HasValue || this.twheel.HasValue || this.tleft.HasValue || this.tright.HasValue || this.tmiddle.HasValue || this.tx1.HasValue || this.tx2.HasValue)
            {
                //Set values
                if (commit)
                {
                    SetValue(this.tx, ref this.x);
                    SetValue(this.ty, ref this.y);
                    SetValue(this.twheel, ref this.wheel);
                    SetValue(this.tleft, ref this.left);
                    SetValue(this.tright, ref this.right);
                    SetValue(this.tmiddle, ref this.middle);
                    SetValue(this.tx1, ref this.x1);
                    SetValue(this.tx2, ref this.x2);
                }

                //Clear the values
                this.tx = null;
                this.ty = null;
                this.twheel = null;
                this.tleft = null;
                this.tright = null;
                this.tmiddle = null;
                this.tx1 = null;
                this.tx2 = null;
            }
        }

        public override void Process()
        {
            if (this.x.HasValue || this.y.HasValue || this.wheel.HasValue || this.left.HasValue || this.right.HasValue || this.middle.HasValue || this.x1.HasValue || this.x2.HasValue)
            {
                int x, y, wheel;
                ButtonState left, right, middle, x1, x2;

                SetValue(this.x, this.man.c_mouse_state.X, out x);
                SetValue(this.y, this.man.c_mouse_state.Y, out y);
                SetValue(this.wheel, this.man.c_mouse_state.ScrollWheelValue, out wheel);
                SetValue(this.left, this.man.c_mouse_state.LeftButton, out left);
                SetValue(this.right, this.man.c_mouse_state.RightButton, out right);
                SetValue(this.middle, this.man.c_mouse_state.MiddleButton, out middle);
                SetValue(this.x1, this.man.c_mouse_state.XButton1, out x1);
                SetValue(this.x2, this.man.c_mouse_state.XButton2, out x2);

                this.man.c_mouse_state = new MouseState(x, y, wheel, left, middle, right, x1, x2);
            }
        }
    }

    #endregion

    #region GamePadConversionHandler

    internal sealed class GamePadConversionHandler : InputConversionHandlerInternal
    {
        private InputManager man;

        private Vector2? ltr, rtr;
        private float? lt, rt;
        private Dictionary<Buttons, ButtonState> tBuf;

        private Vector2? tltr, trtr;
        private float? tlt, trt;
        private Dictionary<Buttons, ButtonState> buf;
        private Dictionary<Buttons, bool> pref;

        public GamePadConversionHandler(InputManager man)
        {
            this.man = man;
        }

        public override bool SetValue(object input, object context, bool replace)
        {
            if (input is float)
            {
                if (context is GamePadComponent)
                {
                    switch ((GamePadComponent)context)
                    {
                        case GamePadComponent.LeftTrigger:
                            this.tlt = (float)input;
                            return true;
                        case GamePadComponent.RightStick:
                            this.trt = (float)input;
                            return true;
                    }
                }
            }
            else if (input is Vector2)
            {
                if (context is GamePadComponent)
                {
                    switch ((GamePadComponent)context)
                    {
                        case GamePadComponent.LeftStick:
                            this.tltr = (Vector2)input;
                            return true;
                        case GamePadComponent.RightStick:
                            this.trtr = (Vector2)input;
                            return true;
                    }
                }
            }
            else if (input is GamePadComponent)
            {
                if (this.buf == null)
                {
                    this.buf = new Dictionary<Buttons, ButtonState>();
                    this.pref = new Dictionary<Buttons, bool>();
                }
                Buttons b = GamePad.Component2Button((GamePadComponent)input);
                this.buf.Add(b, ButtonState.Pressed);
                this.pref.Add(b, replace);
                return true;
            }
            else if (input is ButtonState)
            {
                if (context is GamePadComponent)
                {
                    if (this.buf == null)
                    {
                        this.buf = new Dictionary<Buttons, ButtonState>();
                        this.pref = new Dictionary<Buttons, bool>();
                    }
                    Buttons b = GamePad.Component2Button((GamePadComponent)context);
                    this.buf.Add(b, (ButtonState)input);
                    this.pref.Add(b, replace);
                    return true;
                }
            }
            return false;
        }

        public override void Commit(bool commit)
        {
            if (tltr.HasValue || trtr.HasValue || tlt.HasValue || trt.HasValue)
            {
                //Set values
                if (commit)
                {
                    SetValue(this.tltr, ref this.ltr);
                    SetValue(this.trtr, ref this.rtr);
                    SetValue(this.tlt, ref this.lt);
                    SetValue(this.trt, ref this.rt);
                }

                //Clear the values
                this.tltr = null;
                this.trtr = null;
                this.tlt = null;
                this.trt = null;
            }
            if (this.buf != null && this.buf.Count > 0)
            {
                if (commit)
                {
                    if (this.tBuf == null)
                    {
                        this.tBuf = new Dictionary<Buttons, ButtonState>();
                    }
                    foreach (KeyValuePair<Buttons, ButtonState> kb in this.buf)
                    {
                        if (this.tBuf.ContainsKey(kb.Key) && this.pref.ContainsKey(kb.Key))
                        {
                            if (this.pref[kb.Key])
                            {
                                this.tBuf[kb.Key] = kb.Value;
                            }
                        }
                        else
                        {
                            this.tBuf.Add(kb.Key, kb.Value);
                        }
                    }
                }
                this.buf.Clear();
                this.pref.Clear();
            }
        }

        public override void Process()
        {
            if (ltr.HasValue || rtr.HasValue || lt.HasValue || rt.HasValue || (tBuf != null && tBuf.Count > 0))
            {
                GamePad pad = (GamePad)this.man.components[this.man.padIndex];
                List<Buttons> buttons = GetPressedButtons(pad);
                if (this.tBuf != null)
                {
                    foreach (KeyValuePair<Buttons, ButtonState> kb in this.tBuf)
                    {
                        if (buttons.Contains(kb.Key))
                        {
                            if (kb.Value == ButtonState.Released)
                            {
                                buttons.Remove(kb.Key);
                            }
                        }
                        else
                        {
                            if (kb.Value == ButtonState.Pressed)
                            {
                                buttons.Add(kb.Key);
                            }
                        }
                    }
                    tBuf.Clear();
                }
                Vector2 leftTh, rigthTh;
                float leftT, rightT;

                if (ltr.HasValue)
                {
                    leftTh = ltr.Value;
                }
                else
                {
                    leftTh = pad.GetThumbstickPosition(GamePadComponent.LeftStick);
                }

                if (rtr.HasValue)
                {
                    rigthTh = rtr.Value;
                }
                else
                {
                    rigthTh = pad.GetThumbstickPosition(GamePadComponent.RightStick);
                }

                if (lt.HasValue)
                {
                    leftT = lt.Value;
                }
                else
                {
                    leftT = pad.GetTrigger(GamePadComponent.LeftTrigger);
                }

                if (rt.HasValue)
                {
                    rightT = rt.Value;
                }
                else
                {
                    rightT = pad.GetTrigger(GamePadComponent.RightTrigger);
                }
                this.man.c_pad_states[(int)pad.index] = new GamePadState(leftTh, rigthTh, leftT, rightT, buttons.ToArray());
            }
        }

        private List<Buttons> GetPressedButtons(GamePad pad)
        {
            GamePadComponent[] comps = GamePadComponent.RightTrigger.GetEnumValues<GamePadComponent>();
            List<Buttons> but = new List<Buttons>();
            foreach (GamePadComponent com in comps)
            {
                if (pad.IsButtonPressed(com))
                {
                    but.Add(GamePad.Component2Button(com));
                }
            }
            return but;
        }
    }

    #endregion

    #region TouchConversionHandler

    internal sealed class TouchConversionHandler : InputConversionHandlerInternal
    {
        private InputManager man;

        private List<TouchLocation> tBuf;
        private List<TouchLocation> buf;
        private Dictionary<TouchLocation, bool> pref;

        public TouchConversionHandler(InputManager man)
        {
            this.man = man;
        }

        public override bool SetValue(object input, object context, bool replace)
        {
            if (input is Vector2)
            {
                if (buf == null)
                {
                    buf = new List<TouchLocation>();
                    pref = new Dictionary<TouchLocation, bool>();
                }
                int id = 0;
                if (context is int)
                {
                    id = (int)context;
                }
                TouchLocation loc = new TouchLocation(id, TouchLocationState.Pressed, (Vector2)input);
                buf.Add(loc);
                pref.Add(loc, replace);
                return true;
            }
            else if (input is TouchLocation)
            {
                if (buf == null)
                {
                    buf = new List<TouchLocation>();
                    pref = new Dictionary<TouchLocation, bool>();
                }
                TouchLocation loc = (TouchLocation)input;
                buf.Add(loc);
                pref.Add(loc, replace);
                return true;
            }
            return false;
        }

        public override void Commit(bool commit)
        {
            if (this.buf != null && this.buf.Count > 0)
            {
                if (commit)
                {
                    if (this.tBuf == null)
                    {
                        this.tBuf = new List<TouchLocation>();
                    }
                    foreach (TouchLocation kb in this.buf)
                    {
                        if (this.pref[kb])
                        {
                            bool found = false;
                            for (int i = 0; i < this.tBuf.Count; i++)
                            {
                                if (this.tBuf[i].Id == kb.Id)
                                {
                                    found = true;
                                    this.tBuf[i] = kb;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                this.tBuf.Add(kb);
                            }
                        }
                        else
                        {
                            this.tBuf.Add(kb);
                        }
                    }
                }
                this.buf.Clear();
                this.pref.Clear();
            }
        }

        public override void Process()
        {
            if (this.tBuf != null && this.tBuf.Count > 0)
            {
                TouchCollection col = this.man.c_touch_state;
                List<TouchLocation> loc = new List<TouchLocation>(col.ToArray());
                loc.AddRange(this.tBuf);
                this.tBuf.Clear();
                this.man.c_touch_state = new TouchCollection(loc.ToArray());
            }
        }
    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;

namespace SGDE.Input
{
    /// <summary>
    /// A touchscreen input device.
    /// </summary>
    public sealed class Touchscreen : InputComponent
    {
        private InputManager manager;

        internal Touchscreen(InputManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Always returns Touchscreen.
        /// </summary>
        public InputType Type
        {
            get
            {
                return InputType.Touchscreen;
            }
        }

        /// <summary>
        /// Get the current Mouse input state.
        /// </summary>
        /// <returns>The current, native, input state of the Mouse.</returns>
        public TouchCollection GetCurrentState()
        {
            return this.manager.c_touch_state;
        }

        /// <summary>
        /// Get the past Mouse input state.
        /// </summary>
        /// <returns>The past, native, input state of the Mouse.</returns>
        public TouchCollection GetPastState()
        {
            return this.manager.o_touch_state;
        }

        /// <summary>
        /// Get if the touchscreen device is currently connected.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return this.manager.c_touch_state.IsConnected;
            }
        }

        /// <summary>
        /// Get the currently enabled gestures.
        /// </summary>
        public GestureType EnabledGestures
        {
            get
            {
                return TouchPanel.EnabledGestures;
            }
        }

        /// <summary>
        /// Get if touch a gesture is avalible to be read.
        /// </summary>
        public bool IsGesturesAvalible
        {
            get
            {
                return TouchPanel.IsGestureAvailable;
            }
        }

        /// <summary>
        /// Get's a gesture if one exists. <see cref="IsGesturesAvalible"/>
        /// </summary>
        /// <returns>A gesture, if one exists.</returns>
        public GestureSample GetGesture()
        {
            return TouchPanel.ReadGesture();
        }

        /// <summary>
        /// Get the maximum number of supported touch points.
        /// </summary>
        public int MaxSupportedTouchCount
        {
            get
            {
                return TouchPanel.GetCapabilities().MaximumTouchCount;
            }
        }

        /// <summary>
        /// Get the number of touch points.
        /// </summary>
        public int TouchCount
        {
            get
            {
                return this.manager.c_touch_state.Count;
            }
        }

        /// <summary>
        /// Get the current touch locations.
        /// </summary>
        /// <returns>The current touch locations.</returns>
        public TouchLocation[] GetTouchLocations()
        {
            return this.manager.c_touch_state.ToArray();
        }

        /// <summary>
        /// Get the past touch locations.
        /// </summary>
        /// <returns>The past touch locations.</returns>
        public TouchLocation[] GetPastTouchLocations()
        {
            return this.manager.o_touch_state.ToArray();
        }

        /// <summary>
        /// Get the difference between touch locations. This matches up TouchLocations by ID and returns the difference between them. Only points that exist on both are returned. All fields (besides ID and Position) are ignored.
        /// </summary>
        /// <returns>The difference between touch locations.</returns>
        public TouchLocation[] GetTouchLocationsDIff()
        {
            List<TouchLocation> location = new List<TouchLocation>();
            TouchCollection.Enumerator en = this.manager.c_touch_state.GetEnumerator();
            while (en.MoveNext())
            {
                TouchLocation cur = en.Current;
                TouchLocation past;
                if (this.manager.o_touch_state.FindById(cur.Id, out past))
                {
                    location.Add(new TouchLocation(cur.Id, TouchLocationState.Invalid, cur.Position - past.Position));
                }
            }
            return location.ToArray();
        }

        /// <summary>
        /// Get the current touch location for the specified ID.
        /// </summary>
        /// <param name="id">The ID of the touch location.</param>
        /// <returns>The touch location, if it exists.</returns>
        public TouchLocation GetLocationByID(int id)
        {
            TouchLocation loc;
            if (this.manager.c_touch_state.FindById(id, out loc))
            {
                return loc;
            }
            return default(TouchLocation);
        }

        /// <summary>
        /// Get the past touch location for the specified ID.
        /// </summary>
        /// <param name="id">The ID of the past touch location.</param>
        /// <returns>The past touch location, if it exists.</returns>
        public TouchLocation GetPastLocationByID(int id)
        {
            TouchLocation loc;
            if (this.manager.o_touch_state.FindById(id, out loc))
            {
                return loc;
            }
            return default(TouchLocation);
        }

        /// <summary>
        /// Get the difference between present and past touch locations for the specified ID. Only points that exist on both are returned. All fields (besides ID and Position) are ignored.
        /// </summary>
        /// <param name="id">The ID of the touch locations to get the difference of.</param>
        /// <returns>The touch location difference, if it exists. If it exists the TouchState will be Pressed regardless of it's actual state or difference in state.</returns>
        public TouchLocation GetDiffLocationByID(int id)
        {
            TouchLocation cur;
            if (this.manager.c_touch_state.FindById(id, out cur))
            {
                TouchLocation past;
                if (this.manager.o_touch_state.FindById(id, out past))
                {
                    return new TouchLocation(id, TouchLocationState.Pressed, cur.Position - past.Position);
                }
            }
            return default(TouchLocation);
        }
    }
}

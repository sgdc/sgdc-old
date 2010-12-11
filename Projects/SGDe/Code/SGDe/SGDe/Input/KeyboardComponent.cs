using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SGDE.SceneManagement;
using SGDE.Content;

namespace SGDE
{
    partial class Entity
    {
        public delegate void KeyboardEvent(Game thisGame);

        protected class KeyboardComponent
        {
            private Entity mObjReference;
            private KeyboardEvent[] KeyboardEventMap = new KeyboardEvent[256];

            public KeyboardComponent(Entity mRef)
            {
                mObjReference = mRef;
                if (ContentUtil.LoadingBuilders)
                {
                    return;
                }
                SceneManager.GetInstance().RegisterKeyboardListener(mRef);
            }

            public void RegisterEvent(Keys key, KeyboardEvent callback)
            {
                KeyboardEventMap[(byte)key] = callback;
            }

            public void HandleEvents(KeyboardState keyboardState, Game thisGame)
            {
                foreach (Keys key in keyboardState.GetPressedKeys())
                {
                    if (KeyboardEventMap[(byte)key] != null)
                    {
                        KeyboardEventMap[(byte)key](thisGame);
                    }
                }
            }
        }
    }
}
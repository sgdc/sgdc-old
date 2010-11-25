using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SGDE
{
   partial class Entity
   {
      public delegate void KeyboardEvent(ContentManager content);

      protected class KeyboardComponent
      {
         private Entity mObjReference;
         private KeyboardEvent[] KeyboardEventMap = new KeyboardEvent[256];

         public KeyboardComponent(Entity mRef)
         {
            mObjReference = mRef;
            SceneManager.GetInstance().RegisterKeyboardListener(mRef);
         }

         public void RegisterEvent( Keys key, KeyboardEvent callback )
         {
            KeyboardEventMap[(byte)key] = callback;
         }

         public void HandleEvents(KeyboardState keyboardState, ContentManager content)
         {
            foreach( Keys key in keyboardState.GetPressedKeys( ) )
            {
               if( KeyboardEventMap[(byte)key] != null ) KeyboardEventMap[(byte)key](content);
            }
         }
      }
   }
}
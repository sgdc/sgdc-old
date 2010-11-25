using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TestDemo
{
   class TestPlayerEntity : SGDE.Entity
   {
      public TestPlayerEntity( float x, float y )
         : base( x, y )
      {
         mPhysBaby.SetStatic(false);

         keyboardListener = new KeyboardComponent(this);
         keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Left, Keyboard_LEFT);
         keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Right, Keyboard_RIGHT);
         keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Up, Keyboard_UP);
         keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Down, Keyboard_DOWN);
         keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Escape, Keyboard_ESC);
      }

      private void Keyboard_LEFT(ContentManager content)
      {
         this.Translate(new Microsoft.Xna.Framework.Vector2(-5, 0));
      }

      private void Keyboard_RIGHT(ContentManager content)
      {
         this.Translate(new Microsoft.Xna.Framework.Vector2(5, 0));
      }

      private void Keyboard_UP(ContentManager content)
      {
         this.Translate(new Microsoft.Xna.Framework.Vector2(0, -5));
      }

      private void Keyboard_DOWN(ContentManager content)
      {
         this.Translate(new Microsoft.Xna.Framework.Vector2(0, 5));
      }

      private void Keyboard_ESC(ContentManager content)
      {
         Environment.Exit(1);
      }
   }
}
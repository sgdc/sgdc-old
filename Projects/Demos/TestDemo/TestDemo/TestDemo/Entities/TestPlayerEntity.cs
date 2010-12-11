using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SGDE;

namespace TestDemo
{
    public class TestPlayerEntity : SGDE.Entity
    {
        public TestPlayerEntity(float x, float y)
            : base(x, y)
        {
            keyboardListener = new KeyboardComponent(this);
            keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Left, Keyboard_LEFT);
            keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Right, Keyboard_RIGHT);
            keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Up, Keyboard_UP);
            keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Down, Keyboard_DOWN);
            keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.Escape, Keyboard_Escape);
            keyboardListener.RegisterEvent(Microsoft.Xna.Framework.Input.Keys.C, ((Game1)Game1.CurrentGame).ToggleCollision);
        }

        private void Keyboard_LEFT(Game thisGame)
        {
            this.Translate(-5, 0);
        }

        private void Keyboard_RIGHT(Game thisGame)
        {
            this.Translate(5, 0);
        }

        private void Keyboard_UP(Game thisGame)
        {
            this.Translate(0, -5);
        }

        private void Keyboard_DOWN(Game thisGame)
        {
            this.Translate(0, 5);
        }

        private void Keyboard_Escape(Game thisGame)
        {
            thisGame.Exit();
        }
    }
}
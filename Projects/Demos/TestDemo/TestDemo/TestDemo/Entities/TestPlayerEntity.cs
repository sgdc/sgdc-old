using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SGDE;
using SGDE.Input;

namespace TestDemo
{
    public class TestPlayerEntity : Entity, InputHandler
    {
        public TestPlayerEntity(float x, float y)
            : base(x, y)
        {
        }

        public InputType Handles
        {
            get
            {
                return InputType.Keyboard;
            }
        }

        public void HandleInput(Game game, InputComponent input)
        {
            SGDE.Input.Keyboard keyboard = (SGDE.Input.Keyboard)input;
            if (keyboard.IsKeyPressed(Keys.Left))
            {
                this.Translate(-5, 0);
            }
            if (keyboard.IsKeyPressed(Keys.Right))
            {
                this.Translate(5, 0);
            }
            if (keyboard.IsKeyPressed(Keys.Up))
            {
                this.Translate(0, -5);
            }
            if (keyboard.IsKeyPressed(Keys.Down))
            {
                this.Translate(0, 5);
            }
            if (keyboard.IsKeyClicked(Keys.Escape))
            {
                game.Exit();
            }
            if (keyboard.IsKeyClicked(Keys.C))
            {
                ((Game1)game).ToggleCollision();
            }
        }
    }
}
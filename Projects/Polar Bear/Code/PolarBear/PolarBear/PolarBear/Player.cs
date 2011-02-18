using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

using SGDE;
using SGDE.Input;

namespace PolarBear
{
    public class Player : Entity, InputHandler
    {

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
        }

        public InputType Handles
        {
            get { return InputType.Keyboard; }
        }

        public Microsoft.Xna.Framework.PlayerIndex Index
        {
            get { return Microsoft.Xna.Framework.PlayerIndex.One; }
        }

        public bool IndexSpecific
        {
            get { return false; }
        }
    }
}

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SGDE;
using SGDE.Input;
using Microsoft.Xna.Framework;

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
                return InputType.Keyboard | InputType.GamePad;
            }
        }

        public void HandleInput(SGDE.Game game, InputComponent input)
        {
            switch (input.Type)
            {
                case InputType.Keyboard:
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
                    break;
                case InputType.GamePad:
                    SGDE.Input.GamePad gamePad = (SGDE.Input.GamePad)input;
                    Vector2 rightThumb = gamePad.GetThumbstickPosition(GamePadComponent.RightStick);
                    if (rightThumb != Vector2.Zero)
                    {
                        this.Translate(rightThumb.X * 5, rightThumb.Y * 5);
                    }
                    if (gamePad.IsButtonClicked(GamePadComponent.Back))
                    {
                        game.Exit();
                    }
                    if (gamePad.IsButtonClicked(GamePadComponent.A))
                    {
                        ((Game1)game).ToggleCollision();
                    }
                    break;
                case InputType.Mouse:
                    //Little buggy, otherwise it would be enabled
                    SGDE.Input.Mouse mouse = (SGDE.Input.Mouse)input;
                    Vector2 diff = mouse.PositionDiff;
                    this.Translate(diff.X, diff.Y);
                    break;
            }
        }

        public bool IndexSpecific
        {
            get
            {
                return false;
            }
        }

        public Microsoft.Xna.Framework.PlayerIndex Index
        {
            get
            {
                return Microsoft.Xna.Framework.PlayerIndex.One;
            }
        }
    }
}
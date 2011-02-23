﻿using System;
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
                return InputType.Keyboard | InputType.GamePad | InputType.Mouse;
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
                        if (keyboard.IsKeyPressed(Keys.LeftShift))
                        {
                            game.CameraControl.Position += new Vector2(5, 0);
                        }
                        else
                        {
                            this.Translate(-5, 0);
                        }
                    }
                    if (keyboard.IsKeyPressed(Keys.Right))
                    {
                        if (keyboard.IsKeyPressed(Keys.LeftShift))
                        {
                            game.CameraControl.Position -= new Vector2(5, 0);
                        }
                        else
                        {
                            this.Translate(5, 0);
                        }
                    }
                    if (keyboard.IsKeyPressed(Keys.Up))
                    {
                        if (keyboard.IsKeyPressed(Keys.LeftShift))
                        {
                            game.CameraControl.Position += new Vector2(0, 5);
                        }
                        else
                        {
                            this.Translate(0, -5);
                        }
                    }
                    if (keyboard.IsKeyPressed(Keys.Down))
                    {
                        if (keyboard.IsKeyPressed(Keys.LeftShift))
                        {
                            game.CameraControl.Position -= new Vector2(0, 5);
                        }
                        else
                        {
                            this.Translate(0, 5);
                        }
                    }

                    if (keyboard.IsKeyPressed(Keys.O))
                    {
                        if (keyboard.IsKeyPressed(Keys.LeftShift))
                        {
                            this.Scale(new Vector2(-0.1f, -0.1f));
                        }
                        else
                        {
                            game.CameraControl.Scale -= 0.1f;
                        }
                    }
                    if (keyboard.IsKeyPressed(Keys.I))
                    {
                        if (keyboard.IsKeyPressed(Keys.LeftShift))
                        {
                            this.Scale(new Vector2(0.1f, 0.1f));
                        }
                        else
                        {
                            game.CameraControl.Scale += 0.1f;
                        }
                    }
                    if (keyboard.IsKeyPressed(Keys.R))
                    {
                        game.CameraControl.Rotation += (float)(Math.PI / 32);
                    }
                    if (keyboard.IsKeyPressed(Keys.L))
                    {
                        game.CameraControl.Rotation -= (float)(Math.PI / 32);
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
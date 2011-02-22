using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

using SGDE;
using SGDE.Input;
using SGDE.Audio;
using Microsoft.Xna.Framework;
using SGDE.Physics.Collision;

namespace PolarBear
{
    public class Player : Entity, InputHandler
    {
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
                    if (keyboard.IsKeyPressed(Keys.A))
                    {
                        this.Rotate(-1.0f);
                    }
                    if (keyboard.IsKeyPressed(Keys.D))
                    {
                        this.Rotate(1.0f);
                    }
                    if (keyboard.IsKeyPressed(Keys.W))
                    {
                        this.Scale(new Vector2(0.01f, 0.01f));
                    }
                    if (keyboard.IsKeyPressed(Keys.X))
                    {
                        this.Scale(new Vector2(-0.01f, -0.01f));
                    }

                    break;
                case InputType.GamePad:
                    SGDE.Input.GamePad gamePad = (SGDE.Input.GamePad)input;
                    Vector2 leftThumb = gamePad.GetThumbstickPosition(GamePadComponent.LeftStick);

                    if (leftThumb != Vector2.Zero)
                    {
                        this.Translate(leftThumb.X * 5, leftThumb.Y * -5);
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
            }            
        }

        public override void Update(GameTime gameTime)
        {

            if (mCollisionUnit.HasCollisions())
            {
                this.SpriteImage.Tint = Color.Red;
            }
            else
            {
                this.SpriteImage.Tint = Color.White;
            }

            base.Update(gameTime);
        }

        public InputType Handles
        {
            get { return InputType.Keyboard | InputType.GamePad; }
        }

        public PlayerIndex Index
        {
            get { return PlayerIndex.One; }
        }

        public bool IndexSpecific
        {
            get { return false; }
        }
    }
}

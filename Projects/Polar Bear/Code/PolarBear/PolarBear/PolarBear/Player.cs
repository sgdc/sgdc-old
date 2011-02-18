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
        Game1 mGame; 

        public Player(float x, float y)
            : base(x, y)
        {
        }

        public void HandleInput(SGDE.Game game, InputComponent input)
        {
            SGDE.Input.Keyboard keyboard = (SGDE.Input.Keyboard)input;
            mGame = (Game1)game;
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
        }

        public override void Update(GameTime gameTime)
        {

            if (mCollisionUnit.HasCollisions())
            {
                this.SetColor(Color.Blue);
            }
            else
            {
                this.SetColor(Color.White);
            }

            base.Update(gameTime);
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

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using SGDE.Physics;

namespace PolarBear
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : SGDE.Game
    {

        Texture2D collisionGrid;
        bool showCollision;

        public Game1()
        {
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            showCollision = false;

            collisionGrid = base.GetContent<Texture2D>("Grid");
        }

        public void ToggleCollision()
        {
            showCollision = !showCollision;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            ////GraphicsDevice.Clear(Color.White);              

            //this.SpriteBatch.Begin();

            //if (showCollision)
            //{
            //    PhysicsPharaoh.GetInstance().DrawCollisionGrid(collisionGrid);
            //}

            ////base.Draw(gameTime);

            //this.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

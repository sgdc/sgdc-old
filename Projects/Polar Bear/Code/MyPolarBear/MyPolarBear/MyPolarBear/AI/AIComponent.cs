using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyPolarBear.AI
{
    class AIComponent
    {
        public enum State
        {
            Good,
            Problem,
            Done
        }

        public State CurrentState;
        protected Texture2D mTexture;

        public AIComponent()
        {
            CurrentState = State.Good;
            LoadContent();
        }

        public virtual void LoadContent()
        {
            mTexture = null;
        }

        public virtual void DoAI(GameTime gameTime)
        {
            //return false;
        }

        public virtual void ResetAI()
        {
            CurrentState = State.Good;
        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(mTexture, Vector2.Zero, Color.White);
        //}

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (mTexture != null)
            {
                //spriteBatch.Draw(mTexture, position, Color.White);
                spriteBatch.Draw(mTexture, new Rectangle((int)position.X, (int)position.Y, 20, 20), Color.White);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE
{
    class Sprite
    {
        public static SpriteBatch spriteBatch;
        private Vector2 position;
        private Texture2D texture;

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            texture = theContentManager.Load<Texture2D>(theAssetName);
        }

        public void Draw()
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public void DrawOffset(int x, int y)
        {
            Vector2 offsetPosition = new Vector2(x + position.X, y + position.Y);
            spriteBatch.Draw(texture, offsetPosition, Color.White);
        }
    }
}
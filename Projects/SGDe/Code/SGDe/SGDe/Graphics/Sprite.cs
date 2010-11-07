using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SGDE
{
    public class Sprite : SceneNode
    {
        public static SpriteBatch spriteBatch;
        private Texture2D texture;
        private Color mBackGroundColor;

        public Sprite()
        {
            mBackGroundColor = Color.White;
        }

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            texture = theContentManager.Load<Texture2D>(theAssetName);
        }

        public void Draw()
        {
            spriteBatch.Draw(texture, GetTranslation(), mBackGroundColor);
        }

        public void DrawOffset(int x, int y)
        {
            Vector2 offsetPosition = new Vector2(x + GetTranslation().X, y + GetTranslation().Y);
            spriteBatch.Draw(texture, offsetPosition, mBackGroundColor);
        }

        public Vector2 GetCenter()
        {
            Vector2 center = GetTranslation();

            center.X += texture.Width / 2;
            center.Y += texture.Height / 2;

            return center;
        }

        public int GetWidth()
        {
            return texture.Width;
        }

        public int GetHeight()
        {
            return texture.Height;
        }

        public Color GetBackGroundColor()
        {
            return mBackGroundColor;
        }

        public void SetBackgroundColor(Color backColor)
        {
            mBackGroundColor = backColor;
        }
    }
}
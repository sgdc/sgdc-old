using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SGDE;

namespace SGDE.Graphics
{
   /// <summary>
   /// A drawable sprite object that can be displayed on screen.
   /// </summary>
   public class Sprite : SceneNode
   {
      /* Questions:
       * 1. Should position be to for the center of the image or not? ANS: Upper-left
       * 2. How should non-standard image animations be handled? (If the first frame is 64x64, second 32x32, etc.) ANS: Don't allow
       * 3. Should animations be manually adjusted (dev chooses when next animation frame should be displayed) or internal (done internally, just GameTime passed in)? ANS: Dev can adjust animation rate
       * 4. Should effects be supported? (Shadows, glow, blur, etc.) ANS: Play around and see if easily possible
       * 5. Should drawing be done internally (done internally, just GameTime passed in), should it be done manually (by dev, only location being internal), or overloaded so it is done internal
       * but tinting, rotation, etc. can be passed in. ANS: Overloaded functions
       * 
       * Recomendation:
       * Update and draw, there are base interfaces for it.
       */

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
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

namespace Tyrus_and_Randall
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : SGDE.Game
    {
        private static SpriteFont foodfont;
        public static String foodText;

        protected override void Initialize()
        {
            base.Initialize();
            this.CameraControl.HorizontalBounds = new Vector2(float.PositiveInfinity, 0);
            this.CameraControl.VerticalBounds = new Vector2(this.Window.ClientBounds.Height, 0);
            foodText = "Food: 0";
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            foodfont = Content.Load<SpriteFont>("FoodFont");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void UIDraw(GameTime gameTime)
        {
            base.UIDraw(gameTime);
            this.Graphics2D.DrawString(foodfont, foodText, Vector2.Zero, Color.Black);
        }

        public void SetFoodText(String s)
        {
            foodText = s;
        }
    }
}
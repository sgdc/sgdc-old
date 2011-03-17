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
        public static bool levelonecomplete;
        private static Texture2D superJumpIcon;
        public static bool superJumpActive;
        private static Texture2D continueArrow;
        public static bool continueArrowActive;
        private static Texture2D finishedScreen;
        public static bool finished;

        protected override void Initialize()
        {
            base.Initialize();
            this.CameraControl.HorizontalBounds = new Vector2(5472, 0);
            this.CameraControl.VerticalBounds = new Vector2(this.Window.ClientBounds.Height, 0);
            foodText = "Food: 0 / 20";
            levelonecomplete = false;
            superJumpActive = false;
            continueArrowActive = false;
            finished = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            foodfont = Content.Load<SpriteFont>("FoodFont");
            superJumpIcon = Content.Load<Texture2D>("superjumpicon");
            continueArrow = Content.Load<Texture2D>("continuearrow");
            finishedScreen = Content.Load<Texture2D>("finishedscreen");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void UIDraw(GameTime gameTime)
        {
            base.UIDraw(gameTime);
            this.Graphics2D.DrawString(foodfont, foodText, Vector2.Zero, Color.Black);
            if(superJumpActive)
                this.Graphics2D.Draw(superJumpIcon, new Vector2(300,0), Color.White);
            if(continueArrowActive)
                this.Graphics2D.Draw(continueArrow, new Vector2(738, 216), Color.White);
            if (finished)
                this.Graphics2D.Draw(finishedScreen, Vector2.Zero, Color.White);
        }

        public void SetFoodText(String s)
        {
            foodText = s;
        }

        public static void CompleteLevelOne()
        {
            levelonecomplete = true;
            continueArrowActive = true;
        }
    }
}
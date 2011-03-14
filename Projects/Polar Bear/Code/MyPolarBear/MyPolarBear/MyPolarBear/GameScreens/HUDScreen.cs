using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyPolarBear.Input;
using MyPolarBear.Content;
using MyPolarBear.GameObjects;

namespace MyPolarBear.GameScreens
{
    class HUDScreen: Screen
    {                            
        public HUDScreen(ScreenType screenType)
            : base(screenType)
        {                        
        }

        public void DrawMeter(SpriteBatch spriteBatch)
        {
            Texture2D meter = ContentManager.GetTexture("Meter");
            
            //Draw empty bar
            spriteBatch.Draw(meter, new Vector2(0.0f, 50), new Rectangle(0, 0, meter.Width, meter.Height), Color.LightGreen);
            //Draw filled bar
            spriteBatch.Draw(meter, new Vector2(0.0f, 50), new Rectangle(0, 0, (int)(meter.Width * ((double)GameScreen.CurWorldHealth / 100)), meter.Height), Color.Green);                         

        }

        public void DrawHealth(SpriteBatch spriteBatch)
        {
            Texture2D fullHeart = ContentManager.GetTexture("FullHeart");
            Texture2D emptyHeart = ContentManager.GetTexture("EmptyHeart");

            for (int i = 0; i < PolarBear.MaxHitPoints; i++)
            {
                //Draw empty hearts
                spriteBatch.Draw(emptyHeart, new Vector2(i * emptyHeart.Width, 0.0f), Color.White);
                if (i < PolarBear.CurHitPoints)
                {
                    //Draw filled hearts
                    spriteBatch.Draw(fullHeart, new Vector2(i * fullHeart.Width, 0.0f), Color.White);
                }
            }

        }

        public void DrawPowerSelector(SpriteBatch spriteBatch)
        {
            Texture2D powerSelected = ContentManager.GetTexture("NormalSelected");

            switch (PolarBear.power)
            {
                case PolarBear.Power.Normal:
                    powerSelected = ContentManager.GetTexture("NormalSelected"); break;
                case PolarBear.Power.Ice:
                    powerSelected = ContentManager.GetTexture("IceSelected"); break;
                case PolarBear.Power.Fire:
                    powerSelected = ContentManager.GetTexture("FireSelected"); break;
            }

            spriteBatch.Draw(powerSelected, new Vector2(50, 500), Color.White);                       
        }

        public void DrawNumberOfSeeds(SpriteBatch spriteBatch)
        {            
            spriteBatch.DrawString(ContentManager.GetFont("Calibri"), "Seeds: " + PolarBear.NumSeeds.ToString(), new Vector2(ScreenManager.SCREENWIDTH - 160.0f, ScreenManager.SCREENHEIGHT - 150.0f), Color.White); 
        }

        public void DrawNumberOfEnemies(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.GetFont("Calibri"), "Enemies: " + GameScreen.NumEnemies.ToString(), new Vector2(ScreenManager.SCREENWIDTH - 200.0f, ScreenManager.SCREENHEIGHT - 100.0f), Color.White);
        }
        
        public void DrawNumberOfAnimals(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ContentManager.GetFont("Calibri"), "Animals: " + GameScreen.NumAnimals.ToString(), new Vector2(ScreenManager.SCREENWIDTH - 194.0f, ScreenManager.SCREENHEIGHT - 50.0f), Color.White);
        }

        public void DrawReticule(SpriteBatch spriteBatch)
        {
            Texture2D reticule = ContentManager.GetTexture("Reticule");

            spriteBatch.Draw(reticule, InputManager.Mouse.GetCurrentMousePosition(), null, Color.White, 0.0f, EntityHelper.OriginFromTexture(reticule), 0.5f, SpriteEffects.None, 0.0f);
        }

        public void DrawDisplay(SpriteBatch spriteBatch)
        {
            DrawReticule(spriteBatch);
            DrawMeter(spriteBatch);
            DrawHealth(spriteBatch);
            DrawPowerSelector(spriteBatch);
            DrawNumberOfSeeds(spriteBatch);
            DrawNumberOfEnemies(spriteBatch);
            DrawNumberOfAnimals(spriteBatch);
        }

    }
}

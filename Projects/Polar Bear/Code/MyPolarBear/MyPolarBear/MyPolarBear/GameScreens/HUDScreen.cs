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
        public Texture2D meter;
        public Texture2D fullHeart;
        public Texture2D emptyHeart;
        public Texture2D powerSelected;
        
        public int currentHealth = 50;        

        public HUDScreen(ScreenType screenType)
            : base(screenType)
        {
            meter = ContentManager.GetTexture("Meter");
            fullHeart = ContentManager.GetTexture("FullHeart");
            emptyHeart = ContentManager.GetTexture("EmptyHeart");
            powerSelected = ContentManager.GetTexture("NormalSelected");
        }

        public void DrawMeter(SpriteBatch spriteBatch)
        {
            Vector2 Position = ScreenManager.camera.TopLeft;
            
            //Draw empty bar
            spriteBatch.Draw(meter, new Vector2(Position.X, Position.Y + 50), new Rectangle(0, 0, meter.Width, meter.Height), Color.LightGreen);
            //Draw filled bar
            spriteBatch.Draw(meter, new Vector2(Position.X, Position.Y + 50), new Rectangle(0, 0, (int)(meter.Width * ((double)PolarBear.CurHealth / 100)), meter.Height), Color.Green);                         

        }

        public void DrawHealth(SpriteBatch spriteBatch)
        {
            Vector2 Position = ScreenManager.camera.TopLeft;

            for (int i = 0; i < PolarBear.MaxHitPoints; i++)
            {
                //Draw empty hearts
                spriteBatch.Draw(emptyHeart, new Vector2(Position.X + i * emptyHeart.Width, Position.Y), Color.White);
            }
            for (int i = 0; i < PolarBear.CurHitPoints; i++)
            {
                //Draw filled hearts
                spriteBatch.Draw(fullHeart, new Vector2(Position.X + i * fullHeart.Width, Position.Y), Color.White);
            }

        }

        public void DrawPowerSelector(SpriteBatch spriteBatch)
        {
            Vector2 Origin = ScreenManager.camera.TopLeft;

            switch (PolarBear.power)
            {
                case PolarBear.Power.Normal:
                    powerSelected = ContentManager.GetTexture("NormalSelected"); break;
                case PolarBear.Power.Ice:
                    powerSelected = ContentManager.GetTexture("IceSelected"); break;
                case PolarBear.Power.Fire:
                    powerSelected = ContentManager.GetTexture("FireSelected"); break;
            }

            spriteBatch.Draw(powerSelected, new Vector2(Origin.X + 50, Origin.Y + 500), Color.White);           
            
        }

        public void DrawDisplay(SpriteBatch spriteBatch)
        {
            DrawMeter(spriteBatch);
            DrawHealth(spriteBatch);
            DrawPowerSelector(spriteBatch);
        }

    }
}

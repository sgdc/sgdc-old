using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MyPolarBear.GameScreens
{  
    class Screen
    {
        private List<Text> _entries;
        public List<Text> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }
        
        private ScreenType _screenType;
        public ScreenType ScreenType
        {
            get { return _screenType; }
            set { _screenType = value; }
        }

        private int _selection;
        public int Selection
        {
            get { return _selection; }
            set { _selection = value; }
        }

        public Screen(ScreenType screenType)
        {
            Selection = 1;
            ScreenType = screenType;
            Entries = new List<Text>();
        }

        public void AddEntry(Text entry)
        {
            Entries.Add(entry);
        }

        public void FormatEntries()
        {
            for (int i = 1; i < Entries.Count; i++)
                Entries[i].Position += new Vector2(0, Entries[i - 1].Position.Y + Entries[i - 1].Size.Y);
        }

        public void UpdateEntries()
        {                                 
            SelectEntry();

            for (int i = 0; i < Entries.Count; i++) 
            { 
                if (i == Selection) 
                { 
                    if (!Entries[i].Active) 
                        Entries[i].Active = true; 
                } 
                else 
                { 
                    if (Entries[i].Active) 
                        Entries[i].Active = false; 
                } 
                Entries[i].Update(); 
            } 
        } 

        public virtual void SelectEntry()
        {
            if (ScreenManager.gamepad.IsButtonReleased(Buttons.DPadDown) || ScreenManager.keyboard.IsKeyReleased(Keys.Down))
            {
                if (Selection < Entries.Count - 1)
                    Selection++;
                else
                    Selection = 1;
            }
            else if (ScreenManager.gamepad.IsButtonReleased(Buttons.DPadUp) || ScreenManager.keyboard.IsKeyReleased(Keys.Up))
            {
                if (Selection > 1)
                    Selection--;
                else
                    Selection = Entries.Count - 1;
            } 
        }

        public void DrawEntries(SpriteBatch spriteBatch)
        {
            ScreenManager.camera.ShowMenu();

            foreach (Text entry in Entries)
                entry.Draw(spriteBatch);           
        }

        public virtual void DrawGame(SpriteBatch spriteBatch)
        {
            ScreenManager.camera.FollowEntity();
        }
    }
}

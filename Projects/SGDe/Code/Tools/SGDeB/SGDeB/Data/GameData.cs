using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

using SGDeContent;
using SGDeContent.DataTypes;

using SGDeB.ContentPipelineFake;
using SGDeB.Forms;
using SGDeB.Properties;

namespace SGDeB.Data
{
    public sealed class GameData
    {
        private string title;
        private int width, height;

        /* TODO:
         * Fullscreen
         * VSync
         * Multisample
         * Fixed Time/Time step
         * Orientation
         * Window Resizable
         * Mouse Visible
         * Title (This doesn't apply to Xbox but keep it for using for the name on the MdiChild windows)
         * 
         * SpriteSheet
         * 
         * Maps/MapOrder
         */

        public bool Saved { get; private set; }

        public GameData(Builder builder, string file)
        {
            if (file != null)
            {
                LoadGame(file);
                this.Saved = true;
            }
            else
            {
                //TODO: Create new game data
                this.Saved = false;
                this.title = "New Game";
            }
            //Open game properties
            GamePropertiesForm properties = new GamePropertiesForm();
            MDIUtil.AddControlInGeneralLocation(builder, properties, FormLocation.UpperRight);
            MDIUtil.SetTitle(this.title, Resources.GAME_PROPERTIES_TITLE, properties);
            //TODO
        }

        public DialogResult Save()
        {
            return Save(false);
        }

        public DialogResult Save(bool beForceful)
        {
            if (Saved)
            {
                //No need to worry, everythings A'OK
                return DialogResult.OK;
            }
            //TODO: Non-forceful: Do you want to save?, Forceful: You should save, if you don't you will use your data.
            //BLA BLA
            SaveGame("some path");
            //BLA BLA
            return DialogResult.OK;
        }

        #region IO

        private void LoadGame(string file)
        {
            FakeContentLogger logger = new FakeContentLogger(2);
            //TODO: Take into account the other target platforms
            Game gameContent = (Game)DataUtil.LoadContent(file, logger, ContentTypes.Game);
            //Load the game content
            this.width = gameContent.Width;
            this.height = gameContent.Height;
            //TODO
            this.title = gameContent.Title;
            //TODO
        }

        private void SaveGame(string file)
        {
            FakeContentLogger logger = new FakeContentLogger(2);
            //TODO
        }

        #endregion
    }
}

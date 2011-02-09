using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SGDeB.Forms;
using SGDeB.Data;
using Microsoft.Xna.Framework.Content.Pipeline;
using SGDeB.Properties;
using System.IO;

namespace SGDeB
{
    public partial class Builder : Form
    {
        #region Variables

        private List<GameData> games;
        private ContentPipelineFake.FakeContentLogger log;

        private TargetPlatform platform;

        #endregion

        public Builder()
        {
            InitializeComponent();

            //Setup other components
            viewDeviceTypeClick(windowsToolStripMenuItem, null);

            //Prepare for game system
            log = new ContentPipelineFake.FakeContentLogger(1);
            games = new List<GameData>();
        }

        #region Menu Handlers

        #region File

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewGame();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Setup the dialog
            openFileDialog.Title = Resources.BUILDER_LOAD_DIALOG_TITLE;
            openFileDialog.DefaultExt = "sgde";
            openFileDialog.Filter = "SGDe Content (*.sgde)|*.sgde";
            //Ask user
            DialogResult result = System.Windows.Forms.DialogResult.Cancel;
            while ((result = openFileDialog.ShowDialog()) != System.Windows.Forms.DialogResult.Cancel)
            {
                if (DataUtil.LoadContent(openFileDialog.FileName, log, SGDeContent.DataTypes.ContentTypes.Game, false) == null)
                {
                    MessageBox.Show("That is not a valid SGDe Game. Please try again or create a new Game.", "Invalid SGDe Game", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    Environment.CurrentDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                    AddNewGame(openFileDialog.FileName);
                    break;
                }
            }
        }

        private void AddNewGame(string file = null)
        {
            games.Add(new GameData(this, file));
            //TODO: Prepare/display forms
            if (games.Count == 1)
            {
                //First game
                editToolStripMenuItem.Enabled = true;
                viewToolStripMenuItem.Enabled = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region View

        private void viewDeviceTypeClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                //Erase previous checks
                foreach (ToolStripMenuItem m in currentDeviceToolStripMenuItem.DropDownItems)
                {
                    m.Checked = false;
                }
                //Set the new check
                item.Checked = true;
                //Find what type it is
                string type = item.Text.Replace(" ", string.Empty);
                string[] types = Enum.GetNames(typeof(TargetPlatform));
                TargetPlatform[] typeValues = (TargetPlatform[])Enum.GetValues(typeof(TargetPlatform));
                for (int i = 0; i < types.Length; i++)
                {
                    if (types[i].Equals(type))
                    {
                        this.platform = typeValues[i];
                        break;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Form Handler

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!e.Cancel)
            {
                foreach (GameData data in games)
                {
                    if (!data.Saved)
                    {
                        switch (data.Save())
                        {
                            case DialogResult.No:
                            case DialogResult.Cancel:
                                //User canceled; abort, abort, abort...
                                if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.TaskManagerClosing)
                                {
                                    //Be forceful. Windows is shutting down, or the user really wants this to close. Make sure it closes
                                    data.Save(true);
                                    //Continue closing regardless, of user's response
                                }
                                else
                                {
                                    e.Cancel = true;
                                    return;
                                }
                                break;
                        }
                    }
                }
            }
            base.OnFormClosing(e);
        }

        #endregion
    }
}

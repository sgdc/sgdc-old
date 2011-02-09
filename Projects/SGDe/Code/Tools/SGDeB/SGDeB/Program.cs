using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using SGDeB.ContentPipelineFake;

namespace SGDeB
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            FakeContentLogger logger = new FakeContentLogger(0);
            logger.LogMessage("Starting SGDeB");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Process.GetProcessesByName("SGDeB").Length > 1)
            {
                logger.LogMessage("SGDeB is already running");
                MessageBox.Show("SGDeB is already running", "Already Running", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                logger.LogMessage("Opening SGDeB");
                Application.Run(new Builder());
            }
            logger.LogMessage("Closing SGDeB");
        }
    }
}

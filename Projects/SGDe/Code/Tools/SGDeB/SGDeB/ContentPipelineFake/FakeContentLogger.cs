using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SGDeB.Properties;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SGDeB.ContentPipelineFake
{
    public class FakeContentLogger : ContentBuildLogger
    {
        private EventLog log;
        private int id;

        public FakeContentLogger(int id)
        {
            if (!EventLog.SourceExists(Resources.LOG_FRIENDLY_NAME))
            {
                EventLog.CreateEventSource(Resources.LOG_FRIENDLY_NAME, "SGDeB");
                System.Threading.Thread.Sleep(1000);
            }
            this.log = new EventLog("SGDeB");
            this.log.Source = Resources.LOG_FRIENDLY_NAME;
            this.id = id;
        }

        public override void LogImportantMessage(string message, params object[] messageArgs)
        {
            LogMessage(message, messageArgs);
        }

        public override void LogMessage(string message, params object[] messageArgs)
        {
            this.log.WriteEntry(string.Format(message, messageArgs), EventLogEntryType.Information, id);
        }

        public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs)
        {
            this.log.WriteEntry(string.Format(message, messageArgs), EventLogEntryType.Warning, id);
        }
    }
}

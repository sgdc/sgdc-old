using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework.Content.Pipeline;

using SGDeContent;
using SGDeContent.DataTypes;

using SGDeB.ContentPipelineFake;
using SGDeB.Properties;

namespace SGDeB.Data
{
    public static class DataUtil
    {
        public static ProcessedContent LoadContent(string file, ContentBuildLogger logger, ContentTypes desiredType, bool process = true, TargetPlatform plat = TargetPlatform.Windows)
        {
            Content content = new SGDEImport().Import(file, new FakeContentImporterContext(Path.GetFullPath(file), logger));
            if (content.Type != desiredType)
            {
                logger.LogImportantMessage(Resources.DATA_UTIL_LOAD_BADTYPE, content.Type, desiredType);
                return null;
            }
            if (process)
            {
                FakeContentProcessorContext context = new FakeContentProcessorContext(logger);
                context.TPalt = plat;
                return new SGDEProcessor().Process(content, context);
            }
            return new TempProcessedContent(); //Just to prevent a null outptu
        }

        public static bool SaveContent(string file, ProcessedContent content)
        {
            ContentTypes type = ContentTypes.Unknown;
            if (content is Game)
            {
                type = ContentTypes.Game;
            }
            return new SGDEProcessor().Write(new Content(null, type, SGDEImport.MAX_VERSION), content, file);
        }

        private class TempProcessedContent : ProcessedContent
        {
        }
    }
}

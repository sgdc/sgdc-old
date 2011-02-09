using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;

namespace SGDeB.ContentPipelineFake
{
    public class FakeContentImporterContext : ContentImporterContext
    {
        private string output;
        private ContentBuildLogger logger;

        public FakeContentImporterContext(string outputDir, ContentBuildLogger logger)
        {
            this.output = outputDir;
            this.logger = logger;
        }

        public override void AddDependency(string filename)
        {
            throw new NotImplementedException();
        }

        public override string IntermediateDirectory
        {
            get
            {
                return Path.GetTempPath();
            }
        }

        public override ContentBuildLogger Logger
        {
            get
            {
                return this.logger;
            }
        }

        public override string OutputDirectory
        {
            get
            {
                return this.output;
            }
        }
    }
}

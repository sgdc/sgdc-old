using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SGDeB.ContentPipelineFake
{
    public class FakeContentProcessorContext : ContentProcessorContext
    {
        private ContentBuildLogger logger;
        private List<string[]> filesToProcess;
        private TargetPlatform target;

        public FakeContentProcessorContext(ContentBuildLogger logger)
        {
            this.logger = logger;
        }

        public override void AddDependency(string filename)
        {
            throw new NotImplementedException();
        }

        public override void AddOutputFile(string filename)
        {
            throw new NotImplementedException();
        }

        public override TOutput BuildAndLoadAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName)
        {
            throw new NotImplementedException();
        }

        public override ExternalReference<TOutput> BuildAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName, string assetName)
        {
            if (filesToProcess == null)
            {
                filesToProcess = new List<string[]>();
            }
            filesToProcess.Add(new string[] { sourceAsset.Filename, importerName, processorName });
            return new ExternalReference<TOutput>(sourceAsset.Filename);
        }

        public override string BuildConfiguration
        {
            get { throw new NotImplementedException(); }
        }

        public override TOutput Convert<TInput, TOutput>(TInput input, string processorName, OpaqueDataDictionary processorParameters)
        {
            throw new NotImplementedException();
        }

        public override string IntermediateDirectory
        {
            get { throw new NotImplementedException(); }
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
            get { throw new NotImplementedException(); }
        }

        public override string OutputFilename
        {
            get { throw new NotImplementedException(); }
        }

        public override OpaqueDataDictionary Parameters
        {
            get { throw new NotImplementedException(); }
        }

        public override TargetPlatform TargetPlatform
        {
            get
            {
                return this.target;
            }
        }

        public TargetPlatform TPalt
        {
            get
            {
                return this.target;
            }
            set
            {
                this.target = value;
            }
        }

        public override Microsoft.Xna.Framework.Graphics.GraphicsProfile TargetProfile
        {
            get
            {
                //Do Reach so it works on all systems.
                return Microsoft.Xna.Framework.Graphics.GraphicsProfile.Reach;
            }
        }
    }
}

using System;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SGDeContent
{
    [Serializable]
    internal sealed class LocalizedContentImporterAttribute : ContentImporterAttribute
    {
        // Methods
        public LocalizedContentImporterAttribute(string fileExtension, string resourceName)
            : base(fileExtension)
        {
            this.DisplayName = Messages.ResourceManager.GetString(resourceName);
        }

        public LocalizedContentImporterAttribute(string[] fileExtension, string resourceName)
            : base(fileExtension)
        {
            this.DisplayName = Messages.ResourceManager.GetString(resourceName);
        }
    }

    [Serializable]
    internal sealed class LocalizedContentProcessorAttribute : ContentProcessorAttribute
    {
        private string resourceName;

        public LocalizedContentProcessorAttribute(string resourceName)
        {
            this.resourceName = resourceName;
        }

        public override string DisplayName
        {
            get
            {
                return Messages.ResourceManager.GetString(this.resourceName);
            }
        }
    }
}

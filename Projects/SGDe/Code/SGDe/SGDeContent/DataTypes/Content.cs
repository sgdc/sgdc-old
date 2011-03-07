using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SGDeContent.DataTypes
{
    public class Content : ContentItem
    {
        public XmlDocument document;
        public ContentTypes Type;
        public double Version;

        public Content(XmlDocument doc, ContentTypes type, double version)
        {
            this.document = doc;
            this.Type = type;
            this.Version = version;
        }
    }

    public abstract class ProcessedContent
    {
    }
}

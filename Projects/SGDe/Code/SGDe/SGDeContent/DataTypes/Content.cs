using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SGDeContent.DataTypes
{
    public class Content
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
}

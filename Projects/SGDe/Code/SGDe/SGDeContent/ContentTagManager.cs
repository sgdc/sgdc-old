using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;

namespace SGDeContent
{
    public static class ContentTagManager
    {
        private const double VERSION_INCREMENT = 0.1;

        private static System.Resources.ResourceManager resourceMan;
        private static System.Globalization.CultureInfo resourceCulture;

        private static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SGDeContent.ContentTags", typeof(ContentTagManager).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        private static System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        public static string GetTagValue(string tagID, double version)
        {
            string tagValue = null;
            while (version >= SGDEImport.MIN_VERSION)
            {
                tagValue = GetTagValue(ResourceManager, tagID, version);
                if (tagValue != null)
                {
                    break;
                }
                version -= VERSION_INCREMENT;
            }
            return tagValue;
        }

        private static string GetTagValue(System.Resources.ResourceManager manager, string tagID, double version)
        {
            return manager.GetString(FormatTagName(tagID, version), resourceCulture);
        }

        public static XmlAttribute GetXMLAtt(string tagID, double version, XmlElement el)
        {
            return GetXMLAtt(ResourceManager, tagID, version, el);
        }

        public static XmlAttribute GetXMLAtt(System.Resources.ResourceManager manager, string tagID, double version, XmlElement el)
        {
            return GetXMLAtt(manager, tagID, version, (XmlNode)el);
        }

        public static XmlAttribute GetXMLAtt(string tagID, double version, XmlNode node)
        {
            return GetXMLAtt(ResourceManager, tagID, version, node);
        }

        public static XmlAttribute GetXMLAtt(System.Resources.ResourceManager manager, string tagID, double version, XmlNode node)
        {
            if (node != null)
            {
                string tagValue;
                while (version >= SGDEImport.MIN_VERSION)
                {
                    tagValue = GetTagValue(manager, tagID, version);
                    if (tagValue != null)
                    {
                        XmlAttribute nAt = node.Attributes[tagValue];
                        if (nAt != null)
                        {
                            return nAt;
                        }
                    }
                    version -= VERSION_INCREMENT;
                }
            }
            return null;
        }

        public static XmlElement GetXMLElem(string tagID, double version, XmlElement el)
        {
            return GetXMLElem(ResourceManager, tagID, version, el);
        }

        public static XmlElement GetXMLElem(System.Resources.ResourceManager manager, string tagID, double version, XmlElement el)
        {
            return (XmlElement)GetXMLNode(manager, tagID, version, el);
        }

        public static XmlNode GetXMLNode(string tagID, double version, XmlNode node)
        {
            return GetXMLNode(ResourceManager, tagID, version, node);
        }

        public static XmlNode GetXMLNode(System.Resources.ResourceManager manager, string tagID, double version, XmlNode node)
        {
            if (node != null)
            {
                string tagValue;
                while (version >= SGDEImport.MIN_VERSION)
                {
                    tagValue = GetTagValue(manager, tagID, version);
                    if (tagValue != null)
                    {
                        XmlNode nNode = node[tagValue];
                        if (nNode != null)
                        {
                            return nNode;
                        }
                    }
                    version -= VERSION_INCREMENT;
                }
            }
            return null;
        }

        public static bool TagMatches(string tagID, string tag, double version)
        {
            return TagMatches(ResourceManager, tagID, tag, version);
        }

        public static bool TagMatches(string tagID, string tag, double version, StringComparison comparisonType)
        {
            return TagMatches(ResourceManager, tagID, tag, version, comparisonType);
        }

        public static bool TagMatches(System.Resources.ResourceManager manager, string tagID, string tag, double version)
        {
            return TagMatches(manager, tagID, tag, version, StringComparison.Ordinal);
        }

        public static bool TagMatches(System.Resources.ResourceManager manager, string tagID, string tag, double version, StringComparison comparisonType)
        {
            if (tag != null)
            {
                string tagValue;
                while (version >= SGDEImport.MIN_VERSION)
                {
                    tagValue = GetTagValue(manager, tagID, version);
                    if (tagValue != null)
                    {
                        if (tagValue.Equals(tag, comparisonType))
                        {
                            return true;
                        }
                    }
                    version -= VERSION_INCREMENT;
                }
            }
            return false;
        }

        private static string FormatTagName(string tagID, double version)
        {
            string versionStr = version.ToString();
            if (versionStr.IndexOf("e", 0, versionStr.Length, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                throw new ArgumentOutOfRangeException("version");
            }
            StringBuilder bu = new StringBuilder(versionStr);
            bool found = false;
            for (int i = 0; i < versionStr.Length; i++)
            {
                if (!char.IsDigit(bu[i]))
                {
                    bu[i] = '_';
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                bu.Append("_0");
            }
            return string.Format("{0}_{1}", tagID, bu.ToString());
        }
    }
}

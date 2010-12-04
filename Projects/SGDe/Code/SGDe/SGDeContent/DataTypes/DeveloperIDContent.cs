using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SGDeContent.DataTypes
{
    public abstract class DeveloperIDContent : ProcessedContent
    {
        public Dictionary<object, string> Did;

        protected DeveloperIDContent()
        {
            Did = new Dictionary<object, string>();
        }

        //Simple helper method for handling developer IDs
        public void DeveloperIDWriter(ContentWriter output, object obj)
        {
            if (obj != null && Did.ContainsKey(obj))
            {
                output.Write(true);
                output.Write(Did[obj]);
            }
            else
            {
                output.Write(false);
            }
        }

        public void DevID(XmlAttribute at, object obj)
        {
            if (at != null)
            {
                if (Did.ContainsValue(at.Value))
                {
                    throw new InvalidContentException(string.Format(Messages.DevIDAlreadyExists, at.Value));
                }
                else
                {
                    Did.Add(obj, at.Value);
                }
            }
        }
    }
}

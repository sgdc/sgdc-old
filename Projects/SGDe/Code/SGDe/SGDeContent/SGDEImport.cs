using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using SGDeContent.DataTypes;
using System.Xml;

namespace SGDeContent
{
    [LocalizedContentImporter(".sgde", "SGDEImporter", DefaultProcessor = "SGDEProcessor")]
    public class SGDEImport : ContentImporter<Content>
    {
        private const double MIN_VERSION = 1.0;
        private const double MAX_VERSION = MIN_VERSION;

        public override Content Import(string filename, ContentImporterContext context)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            ContentTypes type = ContentTypes.Unknown;
            XmlElement root = doc.DocumentElement;
            if (!root.Name.Equals("SGDE"))
            {
                throw new InvalidContentException(Messages.InvalidSGDEElement);
            }
            double version = double.Parse(root.Attributes[0].Value);
            if (version < MIN_VERSION || version > MAX_VERSION)
            {
                throw new InvalidContentException(string.Format(Messages.InvalidSGDEVersion, MIN_VERSION, MAX_VERSION));
            }
            if (!root.HasChildNodes)
            {
                throw new InvalidContentException(Messages.NoChildNodes);
            }
            XmlElement mainElement = (XmlElement)root.ChildNodes[0];
            switch (mainElement.Name)
            {
                case "Game":
                    type = ContentTypes.Game;
                    break;
                case "Map":
                    type = ContentTypes.Map;
                    break;
                case "Entity":
                    type = ContentTypes.Entity;
                    break;
                case "SpriteMaps":
                    type = ContentTypes.SpriteMap;
                    break;
                    //TODO: Other types
            }
            return new Content(doc, type);
        }
    }
}

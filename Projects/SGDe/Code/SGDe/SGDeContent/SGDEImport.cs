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
        internal const double MIN_VERSION = 1.0;
        private const double MAX_VERSION = 1.1;

        public override Content Import(string filename, ContentImporterContext context)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            ContentTypes type = ContentTypes.Unknown;
            XmlElement root = doc.DocumentElement;
            if (!ContentTagManager.TagMatches("IMPORT_ROOT_DOCUMENT", root.Name, MIN_VERSION))
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
            if (ContentTagManager.TagMatches("IMPORT_GAME_ELEMENT", mainElement.Name, version))
            {
                type = ContentTypes.Game;
            }
            else if (ContentTagManager.TagMatches("IMPORT_MAP_ELEMENT", mainElement.Name, version))
            {
                type = ContentTypes.Map;
            }
            else if (ContentTagManager.TagMatches("IMPORT_ENTITIY_ELEMENT", mainElement.Name, version))
            {
                type = ContentTypes.Entity;
            }
            else if (ContentTagManager.TagMatches("IMPORT_SPRITESHEET_ELEMENT", mainElement.Name, version))
            {
                type = ContentTypes.SpriteSheet;
            }
            return new Content(doc, type, version);
        }
    }
}

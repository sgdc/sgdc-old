using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using SGDeContent.DataTypes;
using SGDeContent.Processors;
using System.Xml;

namespace SGDeContent
{
    [LocalizedContentProcessor("SGDEProcessor")]
    public class SGDEProcessor : ContentProcessor<Content, ProcessedContent>
    {
        public override ProcessedContent Process(Content input, ContentProcessorContext context)
        {
            switch (input.Type)
            {
                case ContentTypes.Game:
                    return GameProcessor.Process(input, context);
                case ContentTypes.Map:
                    return MapProcessor.Process(input, context);
                case ContentTypes.Entity:
                    return EntityProcessor.Process(input, context);
                case ContentTypes.SpriteSheet:
                    return SpriteSheetProcessor.Process(input, context);
            }
            // TODO: process the input object, and return the modified data.
            throw new NotImplementedException(string.Format(Messages.SGDETypeNotImplemented, input.document.DocumentElement.ChildNodes[0].Name));
        }

        //TODO: Add generic writer so data can be written out to content format

        //What XmlNode.InnerText should really be doing.
        public static string GetInnerText(XmlNode node)
        {
            foreach (XmlNode inode in node)
            {
                if (inode.Name.Equals("#text"))
                {
                    return inode.Value;
                }
            }
            return string.Empty;
        }
    }
}
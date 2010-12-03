using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using SGDeContent.DataTypes;
using SGDeContent.Processors;
using System.Xml;

namespace SGDeContent
{
    [ContentProcessor(DisplayName = "SGDE Element Processor")]
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
                case ContentTypes.SpriteMap:
                    return SpriteMapProcessor.Process(input, context);
            }
            // TODO: process the input object, and return the modified data.
            throw new NotImplementedException(string.Format("Type: \"{0}\" is not implemented.", input.document.DocumentElement.ChildNodes[0].Name));
        }

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
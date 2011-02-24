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

        public bool Write(Content output, ProcessedContent content, string file)
        {
            if (string.IsNullOrEmpty(file) || output.Version < SGDEImport.MIN_VERSION || output.Version > SGDEImport.MAX_VERSION)
            {
                return false;
            }
            bool success = false;
            output.document = new XmlDocument();
            output.document.AppendChild(output.document.CreateXmlDeclaration("1.0", "utf-8", null));
            output.document.AppendChild(output.document.CreateElement("SGDE"));
            output.document.DocumentElement.Attributes.Append(output.document.CreateAttribute("Version"));
            output.document.DocumentElement.Attributes["Version"].Value = output.Version.ToString();
            switch (output.Type)
            {
                case ContentTypes.Game:
                    success = GameProcessor.Write(output.Version, output.document.DocumentElement, content);
                    break;
                case ContentTypes.Map:
                case ContentTypes.Entity:
                case ContentTypes.SpriteSheet:
                    //TODO
                    break;
            }
            if (success)
            {
                output.document.Save(file);
            }
            return success;
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
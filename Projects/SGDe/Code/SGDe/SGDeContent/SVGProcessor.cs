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
using SGDeContent.DataTypes.Sprites.SVG;

namespace SGDeContent
{
    [LocalizedContentProcessor("SVGProcessor")]
    public class SVGProcessor : ContentProcessor<Content, ProcessedContent>
    {
        public enum SVGType
        {
            Full,
            Basic,
            Tiny
        }

        public override ProcessedContent Process(Content input, ContentProcessorContext context)
        {
            return SVGProcessor.Process(input.document.DocumentElement, input.Version, context);
        }

        public static ProcessedContent Process(XmlElement svg, double version, ContentProcessorContext context)
        {
            SVGType type = SVGType.Full;
            XmlAttribute at = svg.Attributes["baseProfile"];
            if (at != null)
            {
                switch (at.Value.Trim())
                {
                    case "tiny":
                        type = SVGType.Tiny;
                        break;
                    case "basic":
                        type = SVGType.Basic;
                        break;
                    //"full" is the default value
                    //"none" is encompessed in full
                }
            }
            if (type != SVGType.Tiny)
            {
                context.Logger.LogWarning(null, null, "SVG is an unsupported type. Supports SVG Tiny, found {0}. Will attempt to process anyway.", type);
            }
            if (version != 1.1)
            {
                context.Logger.LogWarning(null, null, "SVG is an unsupported version. Supports SVG 1.1, found {0}. Will attempt to process anyway.", version);
            }
            SVGfx svgImg = new SVGfx();
            //TODO
            return svgImg;
        }
    }
}
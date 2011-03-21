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
            return SVGProcessor.Process(input.document.DocumentElement, input.Version, context, null);
        }

        public static ProcessedContent Process(XmlNode svg, double version, ContentProcessorContext context, SVGfx parent)
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

            #region Attributes

            //Core.attrib-TODO
            at = svg.Attributes["id"];
            if (at != null)
            {
                svgImg.ID = at.Value;
            }

            //Conditional.attrib-TODO

            //Style.attrib-TODO

            at = svg.Attributes["x"];
            if (at != null)
            {
                svgImg.X = new AnimatableComponent();
                svgImg.X.Add(SpecificNumber.Parse(at.Value), TimeSpan.Zero);
            }
            else
            {
                svgImg.X = new AnimatableComponent();
                svgImg.X.Add(new SpecificNumber(0, NumberType.Precent), TimeSpan.Zero);
            }
            at = svg.Attributes["y"];
            if (at != null)
            {
                svgImg.Y = new AnimatableComponent();
                svgImg.Y.Add(SpecificNumber.Parse(at.Value), TimeSpan.Zero);
            }
            else
            {
                svgImg.Y = new AnimatableComponent();
                svgImg.Y.Add(new SpecificNumber(0, NumberType.Precent), TimeSpan.Zero);
            }
            at = svg.Attributes["width"];
            if (at != null)
            {
                svgImg.Width = new AnimatableComponent();
                SpecificNumber num = SpecificNumber.Parse(at.Value);
                if (num.Number < 0)
                {
                    throw new ArgumentOutOfRangeException("Width < 0");
                }
                svgImg.Width.Add(num, TimeSpan.Zero);
            }
            else
            {
                svgImg.Width = new AnimatableComponent();
                svgImg.Width.Add(new SpecificNumber(100, NumberType.Precent), TimeSpan.Zero);
            }
            at = svg.Attributes["height"];
            if (at != null)
            {
                svgImg.Height = new AnimatableComponent();
                SpecificNumber num = SpecificNumber.Parse(at.Value);
                if (num.Number < 0)
                {
                    throw new ArgumentOutOfRangeException("Height < 0");
                }
                svgImg.Height.Add(num, TimeSpan.Zero);
            }
            else
            {
                svgImg.Height = new AnimatableComponent();
                svgImg.Height.Add(new SpecificNumber(100, NumberType.Precent), TimeSpan.Zero);
            }
            at = svg.Attributes["viewBox"];
            if (at != null)
            {
                //TODO: if not found, ignore. If 0 (for width/height), sprite is not visible. If negative (for width/height), error.
            }
            //preserveAspectRatio, zoomAndPan, contentScriptType, contentStyleType, External.attrib, Presentation.attrib, GraphicalEvents.attrib, DocumentEvents.attrib

            #endregion

            //TODO

            return svgImg;
        }
    }
}
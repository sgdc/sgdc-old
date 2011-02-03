using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline;
using SGDeContent.DataTypes;
using SGDeContent.DataTypes.Code;

namespace SGDeContent.Processors
{
    public class CodeProcessor
    {
        public static SGDeContent.DataTypes.Code.Code Process(XmlElement input, double version, ContentProcessorContext context)
        {
            return Process((XmlNode)input, version, context);
        }

        public static SGDeContent.DataTypes.Code.Code Process(XmlNode input, double version, ContentProcessorContext context)
        {
            SGDeContent.DataTypes.Code.Code code = new SGDeContent.DataTypes.Code.Code();

            string innerText = SGDEProcessor.GetInnerText(input);
            //First see if this is a simple constant
            if (innerText.IndexOf('.') >= 0)
            {
                float fv;
                double dv;
                decimal ddv;
                if (float.TryParse(innerText, out fv))
                {
                    code.Constant = true;
                    code.ConstantValue = fv;
                }
                else if (double.TryParse(innerText, out dv))
                {
                    code.Constant = true;
                    code.ConstantValue = dv;
                }
                else if (decimal.TryParse(innerText, out ddv))
                {
                    code.Constant = true;
                    code.ConstantValue = ddv;
                }
            }
            else
            {
                int iv;
                long lv;
                uint uiv;
                ulong ulv;
                decimal dv;
                if (int.TryParse(innerText, out iv))
                {
                    code.Constant = true;
                    code.ConstantValue = iv;
                }
                else if (uint.TryParse(innerText, out uiv))
                {
                    code.Constant = true;
                    code.ConstantValue = uiv;
                }
                else if (long.TryParse(innerText, out lv))
                {
                    code.Constant = true;
                    code.ConstantValue = lv;
                }
                else if (ulong.TryParse(innerText, out ulv))
                {
                    code.Constant = true;
                    code.ConstantValue = ulv;
                }
                else if (decimal.TryParse(innerText, out dv))
                {
                    code.Constant = true;
                    code.ConstantValue = dv;
                }
            }

            if (!code.Constant)
            {
                try
                {
                    CodeProcessor processor = new CodeProcessor();
                    //Need to actually process the code
                    if (input.ChildNodes.Count > 0 && input.ChildNodes[0].Name.Equals("Code"))
                    {
                        //Full code system
                        processor.Compile((XmlElement)input, version, ref code, context);
                    }
                    else
                    {
                        //K.I.S.S. code system
                        processor.Compile(innerText, version, ref code, context);
                    }
                }
                catch
                {
                    return null;
                }
            }

            return code;
        }

        //This is for simple, one line code elements
        private void Compile(string sourceCode, double version, ref SGDeContent.DataTypes.Code.Code code, ContentProcessorContext context)
        {
            if (sourceCode.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                throw new NullReferenceException(); //A simple work around to make this work.
            }
            //TODO
        }

        //This is for complex, multi line/class code systems
        private void Compile(XmlElement sourceCodeGroup, double version, ref SGDeContent.DataTypes.Code.Code code, ContentProcessorContext context)
        {
            //TODO
        }

        internal static string AbsReference(string reference)
        {
            //TODO: Convert a relitive path (such as Math.PI) to an Absoulte Path that will be used by SGDE (SGDE.Content.Code.Library.SGDE.Math.PI). Relitive path can also be "Abs" in relation to library such as "SGDE.Math.PI"
            return "";
        }
    }
}

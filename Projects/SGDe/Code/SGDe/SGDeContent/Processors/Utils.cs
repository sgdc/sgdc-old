using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SGDeContent.Processors
{
    internal class Utils
    {
        public static T ParseEnum<T>(string value, T defaultValue, ContentBuildLogger log) where T : struct
        {
            string[] values = value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            T evalue = defaultValue;
            T temp;
            for (int i = 0; i < values.Length; i++)
            {
                if (!Enum.TryParse<T>(values[i].Trim(), out temp))
                {
                    log.LogWarning(null, null, Messages.Utils_InvalidEnum, typeof(T).Name, values[i].Trim());
                }
                else
                {
                    int val = ((int)((object)evalue));
                    val |= ((int)((object)temp));
                    evalue = (T)((object)val);
                }
            }
            return evalue;
        }
    }
}

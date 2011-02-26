using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE
{
    internal static class Extensions
    {
        public static T[] SeperatedFlags<T>(this Enum en)
        {
            List<T> items = new List<T>();
            T[] flags = (T[])Enum.GetValues(typeof(T));
            for (int i = 0; i < flags.Length; i++)
            {
                if (((Enum)((object)en)).HasFlag((Enum)((object)flags[i])))
                {
                    items.Add(flags[i]);
                }
            }
            return items.ToArray();
        }
    }
}

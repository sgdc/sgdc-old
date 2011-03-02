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

        public static K GetValue<K, V>(this Dictionary<K, V> lst, V value)
        {
            if (!lst.ContainsValue(value))
            {
                return default(K);
            }
            Dictionary<K, V>.ValueCollection values = lst.Values;
            V[] vals = new V[values.Count];
            values.CopyTo(vals, 0);
            int index = Array.IndexOf(vals, value);
            Dictionary<K, V>.KeyCollection keys = lst.Keys;
            K[] kes = new K[keys.Count];
            keys.CopyTo(kes, 0);
            return kes[index];
        }
    }
}

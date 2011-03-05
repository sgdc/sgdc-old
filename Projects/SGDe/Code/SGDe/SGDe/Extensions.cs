using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !WINDOWS
using System.Reflection;
#endif

namespace SGDE
{
    internal static class Extensions
    {
        public static T[] GetEnumValues<T>(this Enum en)
        {
#if !WINDOWS
            return (T[])EnumGetValues(typeof(T));
#else
            return (T[])Enum.GetValues(typeof(T));
#endif
        }

        public static T[] SeperatedFlags<T>(this Enum en)
        {
            List<T> items = new List<T>();
            T[] flags = en.GetEnumValues<T>();
            for (int i = 0; i < flags.Length; i++)
            {
                if (en.HasFlag((Enum)((object)flags[i])))
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

#if !WINDOWS
        public static Array EnumGetValues(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException();
            }
            if (!enumType.IsEnum)
            {
                throw new ArgumentException();
            }
            FieldInfo[] info = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            Enum[] values = new Enum[info.Length];
            for (int i = 0; i < values.Length; ++i)
            {
                values[i] = (Enum)info[i].GetValue(null);
            }
            return values;
        }

        public static bool HasFlag(this Enum en, Enum flag)
        {
            if (en.GetType() != flag.GetType())
            {
                throw new ArgumentException();
            }
            ulong num = Convert.ToUInt64(flag);
            return ((Convert.ToUInt64(en) & num) == num);
        }
#endif
    }
}

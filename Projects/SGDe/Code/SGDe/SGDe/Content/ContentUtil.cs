using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using SGDE.Content.DataTypes;

namespace SGDE.Content
{
    internal class ContentUtil
    {
        private static Dictionary<string, object> did;

        public static Dictionary<string, object> temp = new Dictionary<string,object>();
        public static GameContent CurrentGameContent;
        /// <summary>
        /// Loading entity builders.
        /// </summary>
        public static bool LoadingBuilders = false;

        public static void PrepTempDID()
        {
            if (did == null)
            {
                did = new Dictionary<string, object>();
            }
            else
            {
                if (did.Count != 0)
                {
                    throw new InvalidOperationException("Concurrency error");
                }
            }
        }

        public static void TempDeveloperID(ContentReader reader, object obj)
        {
            DeveloperID(ref did, reader, obj);
        }

        public static void FinishTempDID(ref Dictionary<string, object> devTypes)
        {
            if (did.Count > 0)
            {
                Dictionary<string, object>.Enumerator en = did.GetEnumerator();
                while (en.MoveNext())
                {
                    KeyValuePair<string, object> value = en.Current;
                    devTypes.Add(value.Key, value.Value);
                }
            }
            did = null;
        }

        public static bool TempDIDExists
        {
            get
            {
                return did != null;
            }
        }

        public static void DeveloperID(ref Dictionary<string, object> devTypes, ContentReader reader, object obj)
        {
            if (reader.ReadBoolean())
            {
                string key = reader.ReadString();
                if (devTypes != null)
                {
                    devTypes.Add(key, obj);
                }
            }
        }
    }
}

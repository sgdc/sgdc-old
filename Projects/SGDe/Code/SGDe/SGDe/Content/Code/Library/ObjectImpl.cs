using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

namespace SGDE.Content.Code.Library
{
    internal static class ObjectImpl
    {
        public static object prototype(object obj)
        {
            FieldInfo fieldInfo = obj.GetType().GetField("prototype", BindingFlags.NonPublic | BindingFlags.Static);
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(obj);
            }
            return obj;
        }
    }
}

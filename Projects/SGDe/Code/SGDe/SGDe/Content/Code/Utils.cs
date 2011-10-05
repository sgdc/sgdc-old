using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGDE.Content.Code.Library;

namespace SGDE.Content.Code
{
    internal static class Utils
    {
        public static void SetDynamic(SGDE.Content.Code.Library.Object obj, bool dyn)
        {
            obj.dynamic = dyn;
        }

        public static void Trace(SGDE.Content.Code.Library.Object obj)
        {
            Console.WriteLine(obj.toString().value);
        }

        public static bool IsInt(Number n)
        {
            return Truncate(n.value) == n.value;
        }

        internal static double Truncate(double value)
        {
#if WINDOWS
            return global::System.Math.Truncate(value);
#else
            //Not correct
            return global::System.Math.Ceiling(value);
#endif
        }

        public static SGDE.Content.Code.Library.Object CreatePrimitive(object obj)
        {
            return new SGDE.Content.Code.Library.Object.PrimitiveWrapper(obj);
        }
    }
}

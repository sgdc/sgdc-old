using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Content.Code
{
    /// <summary>
    /// Used Internally
    /// </summary>
    public class Code
    {
        internal static List<Code> repository;

        internal object ConstantValue;

        internal double Evaluate()
        {
            if (ConstantValue != null)
            {
                return Convert.ToDouble(ConstantValue);
            }
            if (!TryProcess())
            {
                throw new InvalidOperationException("Cannot evaluate code. Arguments could be required.");
            }
            return Convert.ToDouble(ConstantValue);
        }

        private bool TryProcess()
        {
            return false;
        }

        internal object Evaluate(params object[] args)
        {
            return null;
        }
    }
}

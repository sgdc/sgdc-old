using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Content.Code.Library.SGDE
{
    /// <summary>
    /// A data type representing an IEEE-754 double-precision floating-point number.
    /// </summary>
    public sealed class Number
    {
        private static readonly Number prototype = new Number(double.NaN);

        internal double value;

#if WINDOWS
        /// <summary>
        /// Creates a Number object with the specified value.
        /// </summary>
        /// <param name="num">The numeric value of the Number instance being created or a value to be converted to a Number.</param>
        public Number(double num = 0)
#else
        public Number()
            : this(0)
        {
        }

        /// <summary>
        /// Creates a Number object with the specified value.
        /// </summary>
        /// <param name="num">The numeric value of the Number instance being created or a value to be converted to a Number.</param>
        public Number(double num)
#endif
        {
            this.value = num;
        }

        /// <summary>
        /// Convert a native double to a Number.
        /// </summary>
        /// <param name="d">The double value to create the Number with.</param>
        /// <returns>The created Number.</returns>
        public static implicit operator Number(double d)
        {
            return new Number(d);
        }

        /// <summary>
        /// Convert a native float to a Number.
        /// </summary>
        /// <param name="d">The float value to create the Number with.</param>
        /// <returns>The created Number.</returns>
        public static implicit operator Number(float d)
        {
            return new Number(d);
        }
    }
}

/* Author: Vincent Simonetti
 * Date: 2/3/2011
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Content.Code.Library
{
    /// <summary>
    /// A data type representing an IEEE-754 double-precision floating-point number.
    /// </summary>
    public sealed class Number : Object
    {
        internal double value;

        /// <summary>
        /// The largest representable number (double-precision IEEE-754).
        /// </summary>
        public static readonly Number MAX_VALUE = double.MaxValue;
        /// <summary>
        /// The smallest representable non-negative, non-zero, number (double-precision IEEE-754).
        /// </summary>
        public static readonly Number MIN_VALUE = double.Epsilon;
        /// <summary>
        /// The IEEE-754 value representing Not a Number (NaN).
        /// </summary>
        public static readonly Number NaN = double.NaN;
        /// <summary>
        /// Specifies the IEEE-754 value representing negative infinity.
        /// </summary>
        public static readonly Number NEGATIVE_INFINITY = double.NegativeInfinity;
        /// <summary>
        /// Specifies the IEEE-754 value representing positive infinity.
        /// </summary>
        public static readonly Number POSITIVE_INFINITY = double.PositiveInfinity;

        //TODO: Make sure that when instatiated but not set the balue is NaN. Example: "var i:Number", i would be NaN. "var i:Number = 0", "var i:Number = new Number()", "var i:Number = new Number(0)", i would be 0.

#if WINDOWS
        /// <summary>
        /// Creates a Number object with the specified value.
        /// </summary>
        /// <param name="num">The numeric value of the Number instance being created or a value to be converted to a Number.</param>
        public Number(Object num = null)
#else
        /// <summary>
        /// Creates a Number object with the value of 0.
        /// </summary>
        public Number()
            : this(null)
        {
        }

        /// <summary>
        /// Creates a Number object with the specified value.
        /// </summary>
        /// <param name="num">The numeric value of the Number instance being created or a value to be converted to a Number.</param>
        public Number(Object num)
#endif
        {
            this.value = 0;
            if (num != null)
            {
                if (num is Global.UndefinedClass)
                {
                    this.value = double.NaN;
                }
                else if (num is Number)
                {
                    this.value = ((Number)num).value;
                }
                else if (num is Boolean)
                {
                    this.value = ((Boolean)num).value ? 1 : 0;
                }
                else if (num is String)
                {
                    string str = ((String)num).value;
                    this.value = 0;
                    if (!string.IsNullOrEmpty(str))
                    {
                        double.TryParse(str, out this.value);
                    }
                }
            }
        }

        /* TODO
        Number.protoype.toString
        Number.prototype.toLocaleString
        Number.prototype.valueOf
        Number.prototype.toFixed
        Number.prototype.toExponential
        Number.prototype.toPrecision
         */

        #region Primitive handlers

        /// <summary>
        /// Convert a primitive double to a Number.
        /// </summary>
        /// <param name="d">The double value to create the Number with.</param>
        /// <returns>The created Number.</returns>
        public static implicit operator Number(double d)
        {
            Number n = new Number();
            n.value = d;
            return n;
        }

        /// <summary>
        /// Convert a Number to a primitive double.
        /// </summary>
        /// <param name="n">The Number to convert to a double.</param>
        /// <returns>The retrieved double.</returns>
        public static implicit operator double(Number n)
        {
            return n.value;
        }

        #endregion
    }
}

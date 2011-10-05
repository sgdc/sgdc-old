/* Author: Vincent Simonetti
 * Date: 2/3/2011
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

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

        //TODO: Make sure that when instatiated but not set the value is NaN. Example: "var i:Number", i would be NaN. "var i:Number = 0", "var i:Number = new Number()", "var i:Number = new Number(0)", i would be 0.

        internal Number(double d)
        {
            this.value = d;
        }

#if WINDOWS
        /// <summary>
        /// Creates a Number object with the specified value.
        /// </summary>
        /// <param name="num">The numeric value of the Number instance being created or a value to be converted to a Number.</param>
        public Number(Object num = null)
            : this(0.0)
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
            : this(0.0)
#endif
        {
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
                    this.value = double.NaN;
                    if (!string.IsNullOrEmpty(str))
                    {
                        double.TryParse(str, out this.value);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the string representation of the specified Number object (myNumber).
        /// </summary>
        /// <returns>The numeric representation of the Number object as a string.</returns>
        public override String toString()
        {
            return toString(10);
        }

        /// <summary>
        /// Returns the string representation of the specified Number object (myNumber).
        /// </summary>
        /// <param name="radix">Specifies the numeric base (from 2 to 36) to use for the number-to-string conversion. If you do not specify the radix parameter, the default value is 10.</param>
        /// <returns>The numeric representation of the Number object as a string.</returns>
        public String toString(Number radix)
        {
            return toString(radix, false);
        }

        /// <summary>
        /// Returns the local string representation of the specified Number object (myNumber).
        /// </summary>
        /// <returns>The numeric representation of the Number object as a string.</returns>
        public override String toLocaleString()
        {
            return toString(10, true);
        }

        private String toString(Number radix, bool local)
        {
            int r = (int)global::System.Math.Floor(radix.value);
            if (r < 2 || r > 36)
            {
                r = 10;
            }
            double v = this.value;
            if (r != 10)
            {
                v = Utils.Truncate(v);
            }
            CultureInfo info = local ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
            StringBuilder bu = new StringBuilder();
            if (r == 10 || double.IsInfinity(v) || double.IsNaN(v))
            {
                bu.Append(v.ToString(info));
            }
            else
            {
                bool neg = v < 0;
                v = global::System.Math.Abs(v);
                while (v >= r)
                {
#if WINDOWS
                    bu.Insert(0, forDigit((int)(v % r), r));
#else
                    bu.Insert(0, new char[] { forDigit((int)(v % r), r) });
#endif
                    v = Utils.Truncate(v / r);
                }
                if (v != 0)
                {
#if WINDOWS
                    bu.Insert(0, forDigit((int)v, r));
#else
                    bu.Insert(0, new char[] { forDigit((int)v, r) });
#endif
                }
                if (neg)
                {
#if WINDOWS
                    bu.Insert(0, '-');
#else
                    bu.Insert(0, new char[] { '-' });
#endif
                }
            }
            return new String(bu.ToString());
        }

        private static char forDigit(int digit, int radix)
        {
            if (digit < 10)
            {
                return (char)('0' + digit);
            }
            return (char)('a' + digit - 10);
        }

        /// <summary>
        /// Returns a string representation of the number in fixed-point notation.
        /// </summary>
        /// <param name="fractionDigits">An integer between 0 and 20, inclusive, that represents the desired number of decimal places.</param>
        public String toFixed(uint fractionDigits)
        {
            return toFormat('F', fractionDigits, 0);
        }

        /// <summary>
        /// Returns a string representation of the number in exponential notation.
        /// </summary>
        /// <param name="fractionDigits">An integer between 0 and 20, inclusive, that represents the desired number of decimal places.</param>
        public String toExponential(uint fractionDigits)
        {
            return toFormat('e', fractionDigits, 0);
        }

        /// <summary>
        /// Returns a string representation of the number either in exponential notation or in fixed-point notation.
        /// </summary>
        /// <param name="precision">An integer between 1 and 21, inclusive, that represents the desired number of digits to represent in the resulting string.</param>
        public String toPrecision(uint precision)
        {
            return toFormat('g', precision - 1, 1);
        }

        private String toFormat(char format, uint digits, uint offset)
        {
            if (digits > 20)
            {
                //TODO: Throw range exeption
            }
            digits += offset;
            return new String(this.value.ToString(string.Format("{0}{1}", format, digits)));
        }

        #region Primitive handlers

        /// <summary>
        /// Convert a primitive double to a Number.
        /// </summary>
        /// <param name="d">The double value to create the Number with.</param>
        /// <returns>The created Number.</returns>
        public static implicit operator Number(double d)
        {
            return new Number(d);
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

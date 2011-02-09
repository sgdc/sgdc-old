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
    /// Top level or global functions are available in any place where ActionScript is used, or in any user-defined class.
    /// </summary>
    public sealed class Global
    {
        /// <summary>
        /// A special value that applies to untyped variables that have not been initialized or dynamic object properties that are not initialized.
        /// </summary>
        public static readonly Object undefined = new UndefinedClass();
        //Infinity, -Infinity, and NaN will simply be forwarded by the compiler to the Number constants.

        /*
        /// <summary>
        /// Converts a given value to a Number value.
        /// </summary>
        /// <param name="expression">A value to be converted to a number.</param>
        /// <returns>The converted number value</returns>
        public static Number Number(Object expression)
        {
            return new Number(expression);
        }

        /// <summary>
        /// Converts the expression parameter to a Boolean value and returns the value.
        /// </summary>
        /// <param name="expression">An expression or object to convert to Boolean.</param>
        /// <returns>The result of the conversion to Boolean.</returns>
        public static Boolean Boolean(Object expression)
        {
            return new Boolean(expression);
        }

        /// <summary>
        /// Returns a string representation of the specified parameter.
        /// </summary>
        /// <param name="expression">An expression to convert to a string.</param>
        /// <returns> A string representation of the value passed for the expression parameter.</returns>
        public static String String(Object expression)
        {
            String str;
            if (expression == null)
            {
                str = new String();
                str.value = "null";
            }
            else
            {
                if (expression is String)
                {
                    str = (String)expression;
                }
                else
                {
                    str = expression.toString();
                }
            }
            return str;
        }
         */

        /* TODO
        parseInt
        parseFloat
        isNaN
        isFinite
        decodeURI
        decodeURIComponent
        encodeURI
        encodeURIComponent
         */

        internal class UndefinedClass : Object
        {
            public UndefinedClass()
                : base(false, false, false)
            {
            }

            public override String toString()
            {
                return "undefined";
            }
        }
    }
}

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

#if WINDOWS
        /// <summary>
        /// Converts a string to an integer.
        /// </summary>
        /// <param name="str">A string to convert to an integer.</param>
        /// <param name="radix">An integer representing the radix (base) of the number to parse. Legal values are from 2 to 36.</param>
        /// <returns>A number or NaN (not a number).</returns>
        public static Number parseInt(String str, uint radix = 0)
#else
        /// <summary>
        /// Converts a string to an integer.
        /// </summary>
        /// <param name="str">A string to convert to an integer.</param>
        /// <returns>A number or NaN (not a number).</returns>
        public static Number parseInt(String str)
        {
            return parseInt(str, 0);
        }

        /// <summary>
        /// Converts a string to an integer.
        /// </summary>
        /// <param name="str">A string to convert to an integer.</param>
        /// <param name="radix">An integer representing the radix (base) of the number to parse. Legal values are from 2 to 36.</param>
        /// <returns>A number or NaN (not a number).</returns>
        public static Number parseInt(String str, uint radix)
#endif
        {
            string raw = str.value.Trim();
            //Check for hex marker
            if (raw.StartsWith("-0x", StringComparison.InvariantCulture))
            {
                radix = 16;
                raw = '-' + raw.Substring(3);
            }
            else if (raw.StartsWith("0x", StringComparison.InvariantCulture))
            {
                radix = 16;
                raw = raw.Substring(2);
            }
            //Make sure within valid radix and parse
            if (radix >= 2 && radix <= 36)
            {
                if (validForNum(raw, 0, radix, false))
                {
                    long v = 0;
                    bool neg = false;
                    bool good = true;
                    int i;
                    if (raw[0] == '-')
                    {
                        neg = true;
                        i = 1;
                    }
                    else
                    {
                        i = 0;
                    }
                    //Actual parse runtion
                    for (; i < raw.Length && validForNum(raw, i, radix, false); i++)
                    {
                        v *= radix;
                        char c = raw[i];
                        int tmp;
                        if (c > '9')
                        {
                            if (c > 'Z')
                            {
                                tmp = c - 'a';
                            }
                            else
                            {
                                tmp = c - 'A';
                            }
                            tmp += 10;
                        }
                        else
                        {
                            tmp = c - '0';
                        }
                        v += tmp;
                        if (v > uint.MaxValue)
                        {
                            good = false;
                            break;
                        }
                    }
                    if (neg)
                    {
                        v = -v;
                    }
                    if (good)
                    {
                        return new Number((int)v);
                    }
                }
            }
            return Number.NaN;
        }

        /// <summary>
        /// Converts a string to a floating-point number.
        /// </summary>
        /// <param name="str">The string to read and convert to a floating-point number.</param>
        /// <returns>A number or NaN (not a number).</returns>
        public static Number parseFloat(String str)
        {
            string raw = str.value.Trim();
            double d;
            //Try an easy parse
            if (double.TryParse(raw, out d))
            {
                return new Number(d);
            }
            //Find a non-valid char and try to substring it
            if (validForNum(raw, 0, 10, true))
            {
                int i;
                for (i = 0; i < raw.Length && validForNum(raw, i, 10, true); i++) ;
                if (double.TryParse(raw.Substring(0, i), out d))
                {
                    return new Number(d);
                }
            }
            //Error
            return Number.NaN;
        }

        private static bool validForNum(string str, int index, uint radix, bool flt)
        {
            return validForNum(str[index], radix, flt);
        }

        private static bool validForNum(char c, uint radix, bool flt)
        {
            c = char.ToLower(c);
            if (flt)
            {
                if (c == '.')
                {
                    return true;
                }
                else if (c == 'e')
                {
                    return true;
                }
            }
            if (c == '-' || c == '+')
            {
                return true;
            }
            else if (c >= '0' && c <= '9')
            {
                return true;
            }
            else if (!flt)
            {
                c -= 'a';
                return c >= 0 && c <= radix;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the value is NaN(not a number).
        /// </summary>
        /// <param name="num">A numeric value or mathematical expression to evaluate.</param>
        /// <returns>Returns true if the value is NaN(not a number) and false otherwise.</returns>
        public static Boolean isNaN(Number num)
        {
            return new Boolean(double.IsNaN(num.value));
        }

        /// <summary>
        /// Returns true if the value is a finite number, or false if the value is Infinity or -Infinity.
        /// </summary>
        /// <param name="num">A number to evaluate as finite or infinite.</param>
        /// <returns>Returns true if it is a finite number or false if it is infinity or negative infinity</returns>
        public static Boolean isFinite(Number num)
        {
            return new Boolean(!double.IsInfinity(num.value));
        }

        /// <summary>
        /// Decodes an encoded URI into a string.
        /// </summary>
        /// <param name="uri">A string encoded with the encodeURI function.</param>
        /// <returns>A string in which all characters previously escaped by the encodeURI function are restored to their unescaped representation.</returns>
        public static String decodeURI(String uri)
        {
            return new String(new Uri(uri.value).ToString());
        }

        //TODO: decodeURIComponent

        /// <summary>
        /// Encodes a string into a valid URI (Uniform Resource Identifier).
        /// </summary>
        /// <param name="uri">A string representing a complete URI.</param>
        /// <returns>A string with certain characters encoded as UTF-8 escape sequences.</returns>
        public static String encodeURI(String uri)
        {
            return new String(new Uri(uri.value).AbsoluteUri);
        }

        //TOOD: encodeURIComponent

        internal class UndefinedClass : Object
        {
            public UndefinedClass()
                : base(false, false)
            {
            }

            public override String toString()
            {
                return new String("undefined");
            }
        }
    }
}

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
    /// The Math class contains methods and constants that represent common mathematical functions and values.
    /// </summary>
    public sealed class Math : Object
    {
        /// <summary>
        /// A mathematical constant for the ratio of the circumference of a circle to its diameter, expressed as pi, with a value of 3.141592653589793.
        /// </summary>
        public static readonly Number PI = global::System.Math.PI;

        /// <summary>
        /// A mathematical constant for the base of natural logarithms, expressed as e.
        /// </summary>
        public static readonly Number E = global::System.Math.E;

        /// <summary>
        /// A mathematical constant for the natural logarithm of 10, expressed as loge10, with an approximate value of 2.302585092994046.
        /// </summary>
        public static readonly Number LN10 = 2.3025850929940456840179914546844; //Excessive precision is fine

        /// <summary>
        /// A mathematical constant for the natural logarithm of 2, expressed as loge2, with an approximate value of 0.6931471805599453.
        /// </summary>
        public static readonly Number LN2 = 0.69314718055994530941723212145818;

        /// <summary>
        /// A mathematical constant for the base-2 logarithm of the constant e, expressed as log2e, with an approximate value of 1.442695040888963387.
        /// </summary>
        public static readonly Number LOG2E = 1.4426950408889634073599246810019;

        /// <summary>
        /// A mathematical constant for the base-10 logarithm of the constant e (Math.E), expressed as log10e, with an approximate value of 0.4342944819032518.
        /// </summary>
        public static readonly Number LOG10E = 0.43429448190325182765112891891661;

        /// <summary>
        /// A mathematical constant for the square root of one-half, with an approximate value of 0.7071067811865476.
        /// </summary>
        public static readonly Number SQRT1_2 = 0.70710678118654752440084436210485;

        /// <summary>
        /// A mathematical constant for the square root of 2, with an approximate value of 1.4142135623730951.
        /// </summary>
        public static readonly Number SQRT2 = 1.4142135623730950488016887242097;

        /// <summary>
        /// Computes and returns an absolute value for the number specified by the parameter val.
        /// </summary>
        /// <param name="val">The number whose absolute value is returned.</param>
        /// <returns>The absolute value of the specified paramater.</returns>
        public static Number abs(Number val)
        {
            return new Number(global::System.Math.Abs(val.value));
        }

        /// <summary>
        /// Computes and returns the arc cosine of the number specified in the parameter val, in radians.
        /// </summary>
        /// <param name="val">A number from -1.0 to 1.0.</param>
        /// <returns>The arc cosine of the parameter val.</returns>
        public static Number acos(Number val)
        {
            return new Number(global::System.Math.Acos(val.value));
        }

        /// <summary>
        /// Computes and returns the arc sine for the number specified in the parameter val, in radians.
        /// </summary>
        /// <param name="val">A number from -1.0 to 1.0.</param>
        /// <returns>A number between negative pi divided by 2 and positive pi divided by 2.</returns>
        public static Number asin(Number val)
        {
            return new Number(global::System.Math.Asin(val.value));
        }

        /// <summary>
        /// Computes and returns the value, in radians, of the angle whose tangent is specified in the parameter val.
        /// </summary>
        /// <param name="val">A number that represents the tangent of an angle.</param>
        /// <returns>A number between negative pi divided by 2 and positive pi divided by 2.</returns>
        public static Number atan(Number val)
        {
            return new Number(global::System.Math.Atan(val.value));
        }

        /// <summary>
        /// Computes and returns the angle of the point y/x in radians, when measured counterclockwise from a circle's x axis (where 0,0 represents the center of the circle).
        /// </summary>
        /// <param name="y">The y coordinate of the point.</param>
        /// <param name="x">The x coordinate of the point.</param>
        /// <returns>A number.</returns>
        public static Number atan2(Number y, Number x)
        {
            return new Number(global::System.Math.Atan2(y.value, x.value));
        }

        /// <summary>
        /// Returns the ceiling of the specified number or expression.
        /// </summary>
        /// <param name="val">A number or expression.</param>
        /// <returns>An integer that is both closest to, and greater than or equal to, the parameter val.</returns>
        public static Number ceil(Number val)
        {
            return new Number(global::System.Math.Ceiling(val.value));
        }

        /// <summary>
        /// Computes and returns the cosine of the specified angle in radians.
        /// </summary>
        /// <param name="angleRadians">A number that represents an angle measured in radians.</param>
        /// <returns>A number from -1.0 to 1.0.</returns>
        public static Number cos(Number angleRadians)
        {
            return new Number(global::System.Math.Cos(angleRadians.value));
        }

        /// <summary>
        /// Returns the value of the base of the natural logarithm (e), to the power of the exponent specified in the parameter x.
        /// </summary>
        /// <param name="val">The exponent; a number or expression.</param>
        /// <returns>e to the power of the parameter val.</returns>
        public static Number exp(Number val)
        {
            return new Number(global::System.Math.Exp(val.value));
        }

        /// <summary>
        /// Returns the floor of the number or expression specified in the parameter val.
        /// </summary>
        /// <param name="val">A number or expression.</param>
        /// <returns>The integer that is both closest to, and less than or equal to, the parameter val.</returns>
        public static Number floor(Number val)
        {
            return new Number(global::System.Math.Floor(val.value));
        }

        /// <summary>
        /// Returns the natural logarithm of the parameter val.
        /// </summary>
        /// <param name="val">A number or expression with a value greater than 0. </param>
        /// <returns>The natural logarithm of parameter val.</returns>
        public static Number log(Number val)
        {
            return new Number(global::System.Math.Log(val.value));
        }

        /// <summary>
        /// Evaluates val1 and val2 (or more values) and returns the largest value.
        /// </summary>
        /// <param name="val1">A number or expression.</param>
        /// <param name="val2">A number or expression.</param>
        /// <param name="rest">A number or expression. Math.max() can accept multiple arguments.</param>
        /// <returns>The largest of the parameters val1 and val2 (or more values).</returns>
        public static Number max(Number val1, Number val2, params Number[] rest)
        {
            double val = global::System.Math.Max(val1.value, val2.value);
            if (rest != null)
            {
                foreach (Number num in rest)
                {
                    val = global::System.Math.Max(val, num.value);
                }
            }
            return new Number(val);
        }

        /// <summary>
        /// Evaluates val1 and val2 (or more values) and returns the smallest value.
        /// </summary>
        /// <param name="val1">A number or expression.</param>
        /// <param name="val2">A number or expression.</param>
        /// <param name="rest">A number or expression. Math.min() can accept multiple arguments.</param>
        /// <returns>The smallest of the parameters val1 and val2 (or more values).</returns>
        public static Number min(Number val1, Number val2, params Number[] rest)
        {
            double val = global::System.Math.Min(val1.value, val2.value);
            if (rest != null)
            {
                foreach (Number num in rest)
                {
                    val = global::System.Math.Min(val, num.value);
                }
            }
            return new Number(val);
        }

        /// <summary>
        /// Computes and returns base to the power of pow.
        /// </summary>
        /// <param name="base">A number to be raised by the power of the parameter pow.</param>
        /// <param name="pow">A number specifying the power that the parameter base is raised by.</param>
        /// <returns>The value of base raised to the power of pow.</returns>
        public static Number pow(Number @base, Number pow)
        {
            return new Number(global::System.Math.Pow(@base.value, pow));
        }

        private static Random _rand;

        /// <summary>
        /// Returns a pseudo-random number n, where 0 &gt;= n &gt; 1.
        /// </summary>
        /// <returns>A pseudo-random number.</returns>
        public static Number random()
        {
            if (_rand == null)
            {
                _rand = new Random();
            }
            return new Number(_rand.NextDouble());
        }

        /// <summary>
        /// Rounds the value of the parameter val up or down to the nearest integer and returns the value.
        /// </summary>
        /// <param name="val">The number to round.</param>
        /// <returns>The parameter val rounded to the nearest whole number.</returns>
        public static Number round(Number val)
        {
            return new Number(global::System.Math.Round(val.value));
        }

        /// <summary>
        /// Computes and returns the sine of the specified angle in radians.
        /// </summary>
        /// <param name="angleRadians">A number that represents an angle measured in radians.</param>
        /// <returns>A number; the sine of the specified angle (between -1.0 and 1.0).</returns>
        public static Number sin(Number angleRadians)
        {
            return new Number(global::System.Math.Sin(angleRadians.value));
        }

        /// <summary>
        /// Computes and returns the square root of the specified number.
        /// </summary>
        /// <param name="val">A number or expression greater than or equal to 0.</param>
        /// <returns>If the parameter val is greater than or equal to zero, a number; otherwise NaN (not a number).</returns>
        public static Number sqrt(Number val)
        {
            return new Number(global::System.Math.Sqrt(val.value));
        }

        /// <summary>
        /// Computes and returns the tangent of the specified angle.
        /// </summary>
        /// <param name="angleRadians">A number that represents an angle measured in radians.</param>
        /// <returns>The tangent of the parameter angleRadians.</returns>
        public static Number tan(Number angleRadians)
        {
            return new Number(global::System.Math.Tan(angleRadians.value));
        }
    }
}

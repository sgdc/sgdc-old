using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Content.Code.Library
{
    /// <summary>
    /// A Boolean object is a data type that can have one of two values, either true or false, used for logical operations.
    /// </summary>
    public sealed class Boolean : Object
    {
        internal bool value;

        internal Boolean(bool b)
        {
            this.value = b;
        }

#if WINDOWS
        /// <summary>
        /// Creates a Boolean object with the specified value.
        /// </summary>
        /// <param name="expression">Any expression.</param>
        public Boolean(Object expression = null)
            : this(false)
#else
        /// <summary>
        /// Creates a Boolean object with the value of false.
        /// </summary>
        public Boolean()
            : this(null)
        {
        }

        /// <summary>
        /// Creates a Boolean object with the specified value.
        /// </summary>
        /// <param name="expression">Any expression.</param>
        public Boolean(Object expression)
            : this(false)
#endif
        {
            if (expression != null)
            {
                if (expression is Number)
                {
                    Number num = (Number)expression;
                    this.value = num.value != 0 && !double.IsNaN(num.value);
                }
                else if (expression is Boolean)
                {
                    this.value = ((Boolean)expression).value;
                }
                else if (expression is String)
                {
                    this.value = !string.IsNullOrEmpty(((String)expression).value);
                }
                else if (!(expression is Global.UndefinedClass))
                {
                    this.value = true;
                }
            }
        }

        /// <summary>
        /// Returns the string representation ("true" or "false") of the Boolean object.
        /// </summary>
        /// <returns>The string "true" or "false".</returns>
        public override String toString()
        {
            return new String(this.value.ToString());
        }

        #region Primitive handlers

        /// <summary>
        /// Convert a primitive bool to a Boolean.
        /// </summary>
        /// <param name="b">The bool value to create the Boolean with.</param>
        /// <returns>The created Boolean.</returns>
        public static implicit operator Boolean(bool b)
        {
            return new Boolean(b);
        }

        /// <summary>
        /// Convert a Boolean to a primitive bool.
        /// </summary>
        /// <param name="b">The Boolean to convert to a bool.</param>
        /// <returns>The retrieved bool.</returns>
        public static implicit operator bool(Boolean b)
        {
            return b.value;
        }

        #endregion
    }
}

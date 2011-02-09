using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Content.Code.Library
{
    /// <summary>
    /// The String class is a data type that represents a string of characters.
    /// </summary>
    public sealed class String : Object
    {
        internal string value;

        internal String()
        {
        }

        /// <summary>
        /// Creates a new String object initialized to the specified string.
        /// </summary>
        /// <param name="val">The initial value of the new String object.</param>
        public String(String val)
        {
            this.value = val.value;
        }

        /* TODO
        String.fromCharCode
        String.prototype.toString
        String.prototype.valueOf
        String.prototype.charAt
        String.prototype.charCodeAt
        String.prototype.concat
        String.prototype.indexOf
        String.prototype.lastIndexOf
        String.prototype.localeCompare
        String.prototype.match
        String.prototype.replace
        String.prototype.search
        String.prototype.slice
        String.prototype.split
        String.prototype.substring
        String.prototype.toLowerCase
        String.prototype.toLocaleLowerCase
        String.protoype.toUpperCase
        String.protoype.toLocaleUpperCase
        String.[[Value]]
        String.length
         */

        #region Primitive handlers

        /// <summary>
        /// Convert a primitive string to a String.
        /// </summary>
        /// <param name="s">The string value to create the String with.</param>
        /// <returns>The created String.</returns>
        public static implicit operator String(string s)
        {
            String st = new String();
            st.value = s;
            return st;
        }

        /// <summary>
        /// Convert a String to a primitive string.
        /// </summary>
        /// <param name="s">The String to convert to a string.</param>
        /// <returns>The retrieved string.</returns>
        public static implicit operator string(String s)
        {
            return s.value;
        }

        #endregion
    }
}

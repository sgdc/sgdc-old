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

        internal String(string str)
        {
            this.value = str;
        }

        /// <summary>
        /// Creates a new String object initialized to the specified string.
        /// </summary>
        /// <param name="val">The initial value of the new String object.</param>
        public String(String val)
            : this(val.value)
        {
        }

        /// <summary>
        /// Returns a string comprising the characters represented by the Unicode character codes in the parameters.
        /// </summary>
        /// <param name="charCodes">A series of decimal integers that represent Unicode values.</param>
        /// <returns>The string value of the specified Unicode character codes.</returns>
        public static String fromCharCode(params Number[] charCodes)
        {
            StringBuilder bu = new StringBuilder();
            foreach (Number c in charCodes)
            {
                bu.Append((char)((int)global::System.Math.Floor(c.value)));
            }
            return new String(bu.ToString());
        }

        /// <summary>
        /// Returns the string representation of the specified object.
        /// </summary>
        /// <returns>A string representation of the object.</returns>
        public override String toString()
        {
            return new String(this.value);
        }

        /// <summary>
        /// Returns the character in the position specified by the index parameter.
        /// </summary>
        /// <returns>The character at the specified index. Or an empty string if the specified index is outside the range of this string's indices.</returns>
        public String charAt()
        {
            return charAt(new Number());
        }

        /// <summary>
        /// Returns the character in the position specified by the index parameter.
        /// </summary>
        /// <param name="index">An integer specifying the position of a character in the string.</param>
        /// <returns>The character at the specified index. Or an empty string if the specified index is outside the range of this string's indices.</returns>
        public String charAt(Number index)
        {
            int ind = (int)global::System.Math.Floor(index.value);
            if (ind < 0 || ind >= this.value.Length)
            {
                return new String(string.Empty);
            }
            return new String(this.value[ind].ToString());
        }

        /// <summary>
        /// Returns the numeric Unicode character code of the character at the specified index.
        /// </summary>
        /// <returns>The Unicode character code of the character at the specified index.</returns>
        public Number charCodeAt()
        {
            return charCodeAt(new Number());
        }

        /// <summary>
        /// Returns the numeric Unicode character code of the character at the specified index.
        /// </summary>
        /// <param name="index">An integer that specifies the position of a character in the string.</param>
        /// <returns>The Unicode character code of the character at the specified index.</returns>
        public Number charCodeAt(Number index)
        {
            return new Number((int)charAt(index).value[0]);
        }

        /// <summary>
        /// Appends the supplied arguments to the end of the String object, converting them to strings if necessary, and returns the resulting string.
        /// </summary>
        /// <param name="args">Zero or more values to be concatenated.</param>
        /// <returns>A new string consisting of this string concatenated with the specified parameters.</returns>
        public String concat(params Object[] args)
        {
            StringBuilder bu = new StringBuilder(this.value);
            if (args != null)
            {
                foreach (Object obj in args)
                {
                    if (obj is String)
                    {
                        bu.Append(((String)obj).value);
                    }
                    else
                    {
                        bu.Append(obj.toString().value);
                    }
                }
            }
            return new String(bu.ToString());
        }

        /// <summary>
        /// Searches the string and returns the position of the first occurrence of val found at or after startIndex within the calling string.
        /// </summary>
        /// <param name="val">The substring for which to search.</param>
        /// <returns>The index of the first occurrence of the specified substring or -1.</returns>
        public int indexOf(String val)
        {
            return indexOf(val, new Number());
        }

        /// <summary>
        /// Searches the string and returns the position of the first occurrence of val found at or after startIndex within the calling string.
        /// </summary>
        /// <param name="val">The substring for which to search.</param>
        /// <param name="startIndex">An optional integer specifying the starting index of the search.</param>
        /// <returns>The index of the first occurrence of the specified substring or -1.</returns>
        public int indexOf(String val, Number startIndex)
        {
            return this.value.IndexOf(val.value, (int)global::System.Math.Floor(startIndex.value));
        }

        /// <summary>
        /// Searches the string from right to left and returns the index of the last occurrence of val found before startIndex.
        /// </summary>
        /// <param name="val">The string for which to search.</param>
        /// <returns>The position of the last occurrence of the specified substring or -1 if not found.</returns>
        public int lastIndexOf(String val)
        {
            return lastIndexOf(val, 0x7FFFFFFF);
        }

        /// <summary>
        /// Searches the string from right to left and returns the index of the last occurrence of val found before startIndex.
        /// </summary>
        /// <param name="val">The string for which to search.</param>
        /// <param name="startIndex">An optional integer specifying the starting index from which to search for val.</param>
        /// <returns>The position of the last occurrence of the specified substring or -1 if not found.</returns>
        public int lastIndexOf(String val, Number startIndex)
        {
            int len = (int)global::System.Math.Floor(startIndex.value);
            if (len > this.value.Length)
            {
                len = this.value.Length - 1;
            }
            len -= val.value.Length;
            return this.value.LastIndexOf(val.value, 0, len);
        }

        /* For one reason or another, the Actionscript documentation states that there are extra parameters. But the actual implementation does not have them. Base off of implementation
        /// <summary>
        /// Compares the sort order of two or more strings and returns the result of the comparison as an integer.
        /// </summary>
        /// <param name="other">A string value to compare.</param>
        /// <param name="values">Optional set of more strings to compare.</param>
        /// <returns>
        /// The value 0 if the strings are equal. Otherwise, a negative integer if the original string precedes the string argument and a positive integer if the string argument precedes the original string. In both 
        /// cases the absolute value of the number represents the difference between the two strings.
        /// </returns>
        public int localeCompare(String other, params String[] values)
         */
#if WINDOWS
        /// <summary>
        /// Compares the sort order of two or more strings and returns the result of the comparison as an integer.
        /// </summary>
        /// <param name="other">A string value to compare.</param>
        /// <returns>
        /// The value 0 if the strings are equal. Otherwise, a negative integer if the original string precedes the string argument and a positive integer if the string argument precedes the original string. In both 
        /// cases the absolute value of the number represents the difference between the two strings.
        /// </returns>
        public int localeCompare(String other = null)
#else
        /// <summary>
        /// Compares the sort order of two or more strings and returns the result of the comparison as an integer.
        /// </summary>
        /// <returns>
        /// The value 0 if the strings are equal. Otherwise, a negative integer if the original string precedes the string argument and a positive integer if the string argument precedes the original string. In both 
        /// cases the absolute value of the number represents the difference between the two strings.
        /// </returns>
        public int localeCompare()
        {
            return localeCompare(null);
        }

        /// <summary>
        /// Compares the sort order of two or more strings and returns the result of the comparison as an integer.
        /// </summary>
        /// <param name="other">A string value to compare.</param>
        /// <returns>
        /// The value 0 if the strings are equal. Otherwise, a negative integer if the original string precedes the string argument and a positive integer if the string argument precedes the original string. In both 
        /// cases the absolute value of the number represents the difference between the two strings.
        /// </returns>
        public int localeCompare(String other)
#endif
        {
            return this.value.CompareTo(other.value);
        }

        /* TODO
        String.prototype.match
        String.prototype.replace
        String.prototype.search
         */

        /// <summary>
        /// Returns a string that includes the startIndex character and all characters up to, but not including, the endIndex character.
        /// </summary>
        /// <returns>A substring based on the specified indices.</returns>
        public String slice()
        {
            //This uses default indexes, which would simply cause the String to be returned.
            return this;
        }

        /// <summary>
        /// Returns a string that includes the startIndex character and all characters up to, but not including, the endIndex character.
        /// </summary>
        /// <param name="startIndex">The zero-based index of the starting point for the slice. If startIndex is a negative number, the slice is created from right-to-left, where -1 is the last character.</param>
        /// <returns>A substring based on the specified indices.</returns>
        public String slice(Number startIndex)
        {
            return slice(startIndex, new Number(0x7fffffff));
        }

        /// <summary>
        /// Returns a string that includes the startIndex character and all characters up to, but not including, the endIndex character.
        /// </summary>
        /// <param name="startIndex">The zero-based index of the starting point for the slice. If startIndex is a negative number, the slice is created from right-to-left, where -1 is the last character.</param>
        /// <param name="endIndex">
        /// An integer that is one greater than the index of the ending point for the slice. The character indexed by the endIndex parameter is not included in the extracted string. If endIndex is a negative number, the 
        /// ending point is determined by counting back from the end of the string, where -1 is the last character. The default is the maximum value allowed for an index. If this parameter is omitted, String.length is used. 
        /// </param>
        /// <returns>A substring based on the specified indices.</returns>
        public String slice(Number startIndex, Number endIndex)
        {
            return strSliceOp(startIndex, endIndex, false);
        }

        private String strSliceOp(Number startIndex, Number endIndex, bool swap)
        {
            int s = (int)global::System.Math.Floor(startIndex.value);
            int e = (int)global::System.Math.Floor(endIndex.value);

            if (swap)
            {
                if (s > e)
                {
                    int t = s;
                    s = e;
                    e = t;
                }
            }

            //Handle the start index
            int origin;
            if (s >= 0)
            {
                origin = s;
            }
            else
            {
                origin = this.value.Length + s;
            }
            //Handle the end index
            int len;
            if (e < 0)
            {
                e = this.value.Length + e;
            }
            else if (e > this.value.Length)
            {
                e = this.value.Length;
            }
            if (e < origin)
            {
                int t = e;
                e = origin;
                origin = t;
            }
            len = e - origin;
            return new String(this.value.Substring(origin, len));
        }

        //TODO: String.prototype.split

        /// <summary>
        /// Returns a string consisting of the character specified by startIndex and all characters up to endIndex - 1.
        /// </summary>
        /// <returns>A substring based on the specified parameters.</returns>
        public String substring()
        {
            //This uses default indexes, which would simply cause the String to be returned.
            return this;
        }

        /// <summary>
        /// Returns a string consisting of the character specified by startIndex and all characters up to endIndex - 1.
        /// </summary>
        /// <param name="startIndex">
        /// An integer specifying the index of the first character used to create the substring. Valid values for startIndex are 0 through String.length. If startIndex is a negative value, 0 is used.
        /// </param>
        /// <returns>A substring based on the specified parameters.</returns>
        public String substring(Number startIndex)
        {
            return substring(startIndex, new Number(0x7fffffff));
        }

        /// <summary>
        /// Returns a string consisting of the character specified by startIndex and all characters up to endIndex - 1.
        /// </summary>
        /// <param name="startIndex">
        /// An integer specifying the index of the first character used to create the substring. Valid values for startIndex are 0 through String.length. If startIndex is a negative value, 0 is used.
        /// </param>
        /// <param name="endIndex">
        /// An integer that is one greater than the index of the last character in the extracted substring. Valid values for endIndex are 0 through String.length. The character at endIndex is not included in the substring. 
        /// The default is the maximum value allowed for an index. If this parameter is omitted, String.length is used. If this parameter is a negative value, 0 is used.
        /// </param>
        /// <returns>A substring based on the specified parameters.</returns>
        public String substring(Number startIndex, Number endIndex)
        {
            return strSliceOp(startIndex, endIndex, true);
        }

        /// <summary>
        /// Returns a copy of this string, with all uppercase characters converted to lowercase.
        /// </summary>
        /// <returns>A copy of this string with all uppercase characters converted to lowercase.</returns>
        public String toLowerCase()
        {
            return new String(this.value.ToLower());
        }

        /// <summary>
        /// Returns a copy of this string, with all uppercase characters converted to lowercase.
        /// </summary>
        /// <returns>A copy of this string with all uppercase characters converted to lowercase.</returns>
        public String toLocaleLowerCase()
        {
            return toLowerCase();
        }

        /// <summary>
        /// Returns a copy of this string, with all lowercase characters converted to uppercase.
        /// </summary>
        /// <returns>A copy of this string with all lowercase characters converted to uppercase.</returns>
        public String toUpperCase()
        {
            return new String(this.value.ToUpper());
        }

        /// <summary>
        /// Returns a copy of this string, with all lowercase characters converted to uppercase.
        /// </summary>
        /// <returns>A copy of this string with all lowercase characters converted to uppercase.</returns>
        public String toLocaleUpperCase()
        {
            return toUpperCase();
        }

        //Has this in "open source" spec but can't find any sign of it: "String.[[Value]]"

        /// <summary>
        /// An integer specifying the number of characters in the specified String object.
        /// </summary>
        public int length
        {
            get
            {
                return this.value.Length;
            }
        }

        #region Primitive handlers

        /// <summary>
        /// Convert a primitive string to a String.
        /// </summary>
        /// <param name="s">The string value to create the String with.</param>
        /// <returns>The created String.</returns>
        public static implicit operator String(string s)
        {
            return new String(s);
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

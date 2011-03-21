using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Content.Code.Library
{
    /// <summary>
    /// The Error class contains information about an error that occurred in a script.
    /// </summary>
    public class Error : Object
    {
        /// <summary>
        /// Contains the message associated with the Error object.
        /// </summary>
        public String message;

        /// <summary>
        /// Contains the name of the Error object.
        /// </summary>
        public String name;

        /// <summary>
        /// Creates a new Error object.
        /// </summary>
        public Error()
            : this(null)
        {
        }

        /// <summary>
        /// Creates a new Error object.
        /// </summary>
        /// <param name="message">A string associated with the Error object; this parameter is optional.</param>
        public Error(String message)
            : base(true, true)
        {
            this.message = message;
            this.dynamic = this.__proto__.__proto__.__proto__ == null;
        }

        /// <summary>
        /// Returns the string "Error" by default or the value contained in the Error.message property, if defined.
        /// </summary>
        /// <returns>The error message.</returns>
        public override String toString()
        {
            StringBuilder bu = new StringBuilder();
            if (name == null)
            {
                bu.Append("Error");
            }
            else
            {
                bu.Append(this.name.value);
            }
            if (message != null)
            {
                bu.Append(": ");
                bu.Append(this.message.value);
            }
            return new String(bu.ToString());
        }
    }
}

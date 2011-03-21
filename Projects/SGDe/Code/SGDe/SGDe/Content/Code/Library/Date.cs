using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGDE.Content.Code.Library
{
    /// <summary>
    /// The Date class represents date and time information.
    /// </summary>
    public sealed class Date : Object
    {
        //The properties are not part of the open source standard

        private DateTime dateTime;

        /// <summary>
        /// Constructs a new Date object that holds the specified date and time.
        /// </summary>
        /// <param name="yearOrTimevalue">
        /// If other parameters are specified, this number represents a year (such as 1965); otherwise, it represents a time value. If the number represents a year, a value of 0 to 99 indicates 1900 through 1999; otherwise 
        /// all four digits of the year must be specified. If the number represents a time value (no other parameters are specified), it is the number of milliseconds before or after 0:00:00 GMT January 1, 1970; a 
        /// negative values represents a time before 0:00:00 GMT January 1, 1970, and a positive value represents a time after.
        /// </param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        public Date(Object yearOrTimevalue, Number month)
            : this(yearOrTimevalue, month, null)
        {
        }

        /// <summary>
        /// Constructs a new Date object that holds the specified date and time.
        /// </summary>
        /// <param name="yearOrTimevalue">
        /// If other parameters are specified, this number represents a year (such as 1965); otherwise, it represents a time value. If the number represents a year, a value of 0 to 99 indicates 1900 through 1999; otherwise 
        /// all four digits of the year must be specified. If the number represents a time value (no other parameters are specified), it is the number of milliseconds before or after 0:00:00 GMT January 1, 1970; a 
        /// negative values represents a time before 0:00:00 GMT January 1, 1970, and a positive value represents a time after.
        /// </param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        public Date(Object yearOrTimevalue, Number month, Number date)
            : this(yearOrTimevalue, month, date, null)
        {
        }

        /// <summary>
        /// Constructs a new Date object that holds the specified date and time.
        /// </summary>
        /// <param name="yearOrTimevalue">
        /// If other parameters are specified, this number represents a year (such as 1965); otherwise, it represents a time value. If the number represents a year, a value of 0 to 99 indicates 1900 through 1999; otherwise 
        /// all four digits of the year must be specified. If the number represents a time value (no other parameters are specified), it is the number of milliseconds before or after 0:00:00 GMT January 1, 1970; a 
        /// negative values represents a time before 0:00:00 GMT January 1, 1970, and a positive value represents a time after.
        /// </param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        /// <param name="hour">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        public Date(Object yearOrTimevalue, Number month, Number date, Number hour)
            : this(yearOrTimevalue, month, date, hour, null)
        {
        }

        /// <summary>
        /// Constructs a new Date object that holds the specified date and time.
        /// </summary>
        /// <param name="yearOrTimevalue">
        /// If other parameters are specified, this number represents a year (such as 1965); otherwise, it represents a time value. If the number represents a year, a value of 0 to 99 indicates 1900 through 1999; otherwise 
        /// all four digits of the year must be specified. If the number represents a time value (no other parameters are specified), it is the number of milliseconds before or after 0:00:00 GMT January 1, 1970; a 
        /// negative values represents a time before 0:00:00 GMT January 1, 1970, and a positive value represents a time after.
        /// </param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        /// <param name="hour">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <param name="minute">An integer from 0 to 59.</param>
        public Date(Object yearOrTimevalue, Number month, Number date, Number hour, Number minute)
            : this(yearOrTimevalue, month, date, hour, minute, null)
        {
        }

        /// <summary>
        /// Constructs a new Date object that holds the specified date and time.
        /// </summary>
        /// <param name="yearOrTimevalue">
        /// If other parameters are specified, this number represents a year (such as 1965); otherwise, it represents a time value. If the number represents a year, a value of 0 to 99 indicates 1900 through 1999; otherwise 
        /// all four digits of the year must be specified. If the number represents a time value (no other parameters are specified), it is the number of milliseconds before or after 0:00:00 GMT January 1, 1970; a 
        /// negative values represents a time before 0:00:00 GMT January 1, 1970, and a positive value represents a time after.
        /// </param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        /// <param name="hour">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <param name="minute">An integer from 0 to 59.</param>
        /// <param name="second">An integer from 0 to 59.</param>
        public Date(Object yearOrTimevalue, Number month, Number date, Number hour, Number minute, Number second)
            : this(yearOrTimevalue, month, date, hour, minute, second, null)
        {
        }

        /// <summary>
        /// Constructs a new Date object that holds the specified date and time.
        /// </summary>
        /// <param name="yearOrTimevalue">
        /// If other parameters are specified, this number represents a year (such as 1965); otherwise, it represents a time value. If the number represents a year, a value of 0 to 99 indicates 1900 through 1999; otherwise 
        /// all four digits of the year must be specified. If the number represents a time value (no other parameters are specified), it is the number of milliseconds before or after 0:00:00 GMT January 1, 1970; a 
        /// negative values represents a time before 0:00:00 GMT January 1, 1970, and a positive value represents a time after.
        /// </param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        /// <param name="hour">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <param name="minute">An integer from 0 to 59.</param>
        /// <param name="second">An integer from 0 to 59.</param>
        /// <param name="millisecond">An integer from 0 to 999 of milliseconds.</param>
        public Date(Object yearOrTimevalue, Number month, Number date, Number hour, Number minute, Number second, Number millisecond)
            : base(true, true)
        {
            //TODO: Process all the values into the correct formats and create the dateTime
            this.dynamic = true;
        }

        /*
        Date.parse
        Date.UTC
        Date.prototype.toString
        Date.prototype.toDateString
        Date.prototype.toTimeString
        Date.prototype.toLocaleString
        Date.prototype.toLocaleDateString
        Date.prototype.toLocaletimeString
        Date.prototype.getTime
        Date.prototype.getFullYear
        Date.prototype.getUTCFullYear
        Date.prototype.getMonth
        Date.prototype.getUTCMonth
        Date.prototype.getDate
        Date.prototype.getUTCDate
        Date.prototype.getDay
        Date.prototype.getUTCDay
        Date.prototype.getHours
        Date.prototype.getUTCHours
        Date.prototype.getMinutes
        Date.prototype.getUTCMinutes
        Date.prototype.getSeconds
        Date.prototype.getUTCSeconds
        Date.prototype.getMilliseconds
        Date.prototype.getUTCMilliseconds
        Date.prototype.getTimezoneOffset
        Date.prototype.setTime
        Date.prototype.setMilliseconds
        Date.prototype.setUTCMilliseconds
        Date.prototype.setSeconds
        Date.prototype.setUTCSeconds
        Date.prototype.setMinutes
        Date.prototype.setUTCMinutes
        Date.prototype.setHours
        Date.prototype.setUTCHours
        Date.prototype.setDate
        Date.prototype.setUTCDate
        Date.prototype.setMonth
        Date.prototype.setUTCMonth
        Date.prototype.setFullYear
        Date.prototype.setUTCFullYear
        Date.prototype.toUTCString
         */
    }
}

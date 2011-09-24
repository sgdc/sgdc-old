using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SGDE.Content.Code.Library
{
    /// <summary>
    /// The Date class represents date and time information.
    /// </summary>
    public sealed class Date : Object
    {
        //The properties are not part of the open source standard

        private const string FULL_FORMAT = "{0:ddd MMM dd HH':'mm':'ss} GMT{1}{2:0000} {0:yyyy}";
        private const string DATE_FORMAT = "{0:ddd MMM dd yyyy}";
        private const string TIME_FORMAT = "{0:HH':'mm':'ss} GMT{1}{2:0000}";
        private const string UTC_FORMAT = "{0:ddd MMM dd HH':'mm':'ss yyyy UTC}";

        private DateTime dateTime;

        #region Constructor

        /// <summary>
        /// Constructs a new Date object that holds the specified date and time.
        /// </summary>
        public Date()
            : this(null)
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
        public Date(Object yearOrTimevalue)
            : this(yearOrTimevalue, null)
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
            if (yearOrTimevalue == null)
            {
                this.dateTime = DateTime.Now;
            }
            else
            {
                if (month == null)
                {
                    if (yearOrTimevalue is Number)
                    {
                        this.dateTime = DateTime.FromBinary(-0x7760800A084A8000);
                        this.dateTime = this.dateTime.AddMilliseconds(((Number)yearOrTimevalue).value).ToLocalTime();
                    }
                    else if (yearOrTimevalue is String)
                    {
                        this.dateTime = parseDate(((String)yearOrTimevalue).value).ToLocalTime();
                    }
                    else
                    {
                        this.dateTime = DateTime.MinValue;
                    }
                }
                else
                {
                    int y = 0, m, d, h, mm, s, mmm;
                    m = h = mm = s = mmm = 0;
                    d = 1;
                    //Year
                    if (yearOrTimevalue is Number)
                    {
                        y = (int)global::System.Math.Floor(((Number)yearOrTimevalue).value);
                    }
                    else if (yearOrTimevalue is String)
                    {
                        y = int.Parse(((String)yearOrTimevalue).value);
                    }
                    if (y <= 0)
                    {
                        y = 1970;
                    }
                    else
                    {
                        if (y <= 99)
                        {
                            y += 1900;
                        }
                    }
                    //Month
                    if (month != null)
                    {
                        m = (int)(global::System.Math.Floor(month.value) + 1);
                    }
                    //Day (date)
                    if (date != null)
                    {
                        d = (int)global::System.Math.Floor(date.value);
                    }
                    //Hour
                    if (hour != null)
                    {
                        h = (int)global::System.Math.Floor(hour.value);
                    }
                    //Minute
                    if (minute != null)
                    {
                        mm = (int)global::System.Math.Floor(minute.value);
                    }
                    //Seconds
                    if (second != null)
                    {
                        s = (int)global::System.Math.Floor(second.value);
                    }
                    //Milliseconds
                    if (millisecond != null)
                    {
                        mmm = (int)global::System.Math.Floor(millisecond.value);
                    }
                    this.dateTime = new DateTime(y, m, d, h, mm, s, mmm).ToLocalTime();
                }
            }
            this.dynamic = true;
        }

        private static DateTime parseDate(string str)
        {
            //TODO: Make sure that toString can be parsed (once time zones are implemented)
            DateTime dt;
            if (!DateTime.TryParse(str, out dt))
            {
                str = str.Trim();
                StringBuilder bu = new StringBuilder(str);
                int tmp, index;
                DateTimeStyles style = DateTimeStyles.None;

                //Timezone
                index = str.IndexOfAny(new char[] { '-', '+' });
                if (index >= 0)
                {
                    //Adjust timezone
                    if (int.TryParse(str.Substring(index, 5), out tmp))
                    {
                        //Remove name, leave offset
                        bu.Remove(index - 3, 3);
                        str = bu.ToString();
                    }
                }
                if (str.EndsWith("UTC"))
                {
                    style = DateTimeStyles.AssumeUniversal;
                    bu.Remove(str.Length - 4, 4);
                    str = bu.ToString();
                }
                //Year
                if (int.TryParse(str.Substring(str.Length - 5), out tmp) && (index + 2) != str.Length)
                {
                    //This always goes into third place
                    bu.Remove(str.Length - 5, 5);
                    index = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        index = str.IndexOf(' ', index + 1);
                    }
                    if (index < 0)
                    {
                        bu.Append(' ').Append(tmp);
                    }
                    else
                    {
                        bu.Insert(index, tmp);
                        bu.Insert(index, ' ');
                    }
                    str = bu.ToString();
                }
                //Try again
                dt = DateTime.Parse(str, DateTimeFormatInfo.CurrentInfo, style);
            }
            return dt;
        }

        #endregion

        /// <summary>
        /// Converts a string representing a date into a number equaling the number of milliseconds elapsed since January 1, 1970, UTC.
        /// </summary>
        /// <param name="date">A string representation of a date, which conforms to the format for the output of Date.toString().</param>
        /// <returns>A number representing the milliseconds elapsed since January 1, 1970, UTC.</returns>
        public static Number parse(String date)
        {
            return new Date(date).getTime();
        }

        #region UTC

        /// <summary>
        /// Returns the number of milliseconds between midnight on January 1, 1970, universal time, and the time specified in the parameters.
        /// </summary>
        /// <param name="year">A four-digit integer that represents the year (for example, 2000).</param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <returns>The number of milliseconds since January 1, 1970 and the specified date and time.</returns>
        public static Number UTC(Number year, Number month)
        {
            return UTC(year, month, null);
        }

        /// <summary>
        /// Returns the number of milliseconds between midnight on January 1, 1970, universal time, and the time specified in the parameters.
        /// </summary>
        /// <param name="year">A four-digit integer that represents the year (for example, 2000).</param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        /// <returns>The number of milliseconds since January 1, 1970 and the specified date and time.</returns>
        public static Number UTC(Number year, Number month, Number date)
        {
            return UTC(year, month, date, null);
        }

        /// <summary>
        /// Returns the number of milliseconds between midnight on January 1, 1970, universal time, and the time specified in the parameters.
        /// </summary>
        /// <param name="year">A four-digit integer that represents the year (for example, 2000).</param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        /// <param name="hour">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <returns>The number of milliseconds since January 1, 1970 and the specified date and time.</returns>
        public static Number UTC(Number year, Number month, Number date, Number hour)
        {
            return UTC(year, month, date, hour, null);
        }

        /// <summary>
        /// Returns the number of milliseconds between midnight on January 1, 1970, universal time, and the time specified in the parameters.
        /// </summary>
        /// <param name="year">A four-digit integer that represents the year (for example, 2000).</param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        /// <param name="hour">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <param name="minute">An integer from 0 to 59.</param>
        /// <returns>The number of milliseconds since January 1, 1970 and the specified date and time.</returns>
        public static Number UTC(Number year, Number month, Number date, Number hour, Number minute)
        {
            return UTC(year, month, date, hour, minute, null);
        }

        /// <summary>
        /// Returns the number of milliseconds between midnight on January 1, 1970, universal time, and the time specified in the parameters.
        /// </summary>
        /// <param name="year">A four-digit integer that represents the year (for example, 2000).</param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        /// <param name="hour">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <param name="minute">An integer from 0 to 59.</param>
        /// <param name="second">An integer from 0 to 59.</param>
        /// <returns>The number of milliseconds since January 1, 1970 and the specified date and time.</returns>
        public static Number UTC(Number year, Number month, Number date, Number hour, Number minute, Number second)
        {
            return UTC(year, month, date, hour, minute, second, null);
        }

        /// <summary>
        /// Returns the number of milliseconds between midnight on January 1, 1970, universal time, and the time specified in the parameters.
        /// </summary>
        /// <param name="year">A four-digit integer that represents the year (for example, 2000).</param>
        /// <param name="month">An integer from 0 (January) to 11 (December).</param>
        /// <param name="date">An integer from 1 to 31.</param>
        /// <param name="hour">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <param name="minute">An integer from 0 to 59.</param>
        /// <param name="second">An integer from 0 to 59.</param>
        /// <param name="millisecond">An integer from 0 to 999.</param>
        /// <returns>The number of milliseconds since January 1, 1970 and the specified date and time.</returns>
        public static Number UTC(Number year, Number month, Number date, Number hour, Number minute, Number second, Number millisecond)
        {
            return new Date(year, month, date, hour, minute, second, millisecond).getTime();
        }

        #endregion

        private String toString(string format, bool local, bool utc)
        {
            char sign = '+';
            long off = (TimeZoneInfo.Local.BaseUtcOffset.Ticks / TimeSpan.TicksPerMinute) - 100;
            if (off < 0)
            {
                sign = '-';
                off = -off;
            }
            return new String(string.Format(local ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture, format, utc ? this.dateTime.ToUniversalTime() : this.dateTime, sign, off));
        }

        /// <summary>
        /// Returns a String representation of the day, date, time, and timezone.
        /// </summary>
        /// <returns>The string representation of a Date object.</returns>
        public override String toString()
        {
            return toString(FULL_FORMAT, false, false);
        }

        /// <summary>
        /// Returns a string representation of the day and date only, and does not include the time or timezone.
        /// </summary>
        /// <returns>The string representation of day and date only.</returns>
        public String toDateString()
        {
            return toString(DATE_FORMAT, false, false);
        }

        /// <summary>
        /// Returns a String representation of the time and timezone only, and does not include the day and date.
        /// </summary>
        /// <returns>The string representation of time and timezone only.</returns>
        public String toTimeString()
        {
            return toString(TIME_FORMAT, false, false);
        }

        /// <summary>
        /// Returns a String representation of the day, date, time, given in local time.
        /// </summary>
        /// <returns>A string representation of a Date object in the local timezone.</returns>
        public override String toLocaleString()
        {
            return toString(FULL_FORMAT, true, false);
        }

        /// <summary>
        /// Returns a String representation of the day and date only, and does not include the time or timezone.
        /// </summary>
        /// <returns>The String representation of day and date only.</returns>
        public String toLocaleDateString()
        {
            return toString(DATE_FORMAT, true, false);
        }

        /// <summary>
        /// Returns a String representation of the time only, and does not include the day, date, year, or timezone.
        /// </summary>
        /// <returns>The string representation of time and timezone only.</returns>
        public String toLocaletimeString()
        {
            return toString(TIME_FORMAT, true, false);
        }

        /// <summary>
        /// Returns the number of milliseconds since midnight January 1, 1970, universal time, for a Date object.
        /// </summary>
        /// <returns>The number of milliseconds since Jan 1, 1970 that a Date object represents.</returns>
        public Number getTime()
        {
            //DateTime.FromBinary(-0x7760800A084A8000).Ticks = 0x89F7FCC0EDF7800L
            return new Number((this.dateTime.Ticks - 0x89F7FCC0EDF7800L) / TimeSpan.TicksPerMillisecond);
        }

        /// <summary>
        /// Returns the full year (a four-digit number, such as 2000) of a Date object according to local time.
        /// </summary>
        /// <returns>The full year a Date object represents.</returns>
        public Number getFullYear()
        {
            return new Number(this.dateTime.Year);
        }

        /// <summary>
        /// Returns the four-digit year of a Date object according to universal time (UTC).
        /// </summary>
        /// <returns>The UTC four-digit year a Date object represents.</returns>
        public Number getUTCFullYear()
        {
            return new Number(this.dateTime.ToUniversalTime().Year);
        }

        /*
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
         */

        /// <summary>
        /// Returns a String representation of the day, date, and time in universal time (UTC).
        /// </summary>
        /// <returns>The string representation of a Date object in UTC time.</returns>
        public String toUTCString()
        {
            return toString(UTC_FORMAT, false, true);
        }
    }
}

using System;

namespace Horseshoe.NET.DateAndTime
{
    public static class DateTimeConstants
    {
        /// <summary>
        /// When true, Horseshoe.NET will try to wrangle dates into business dates system-wide where possible.
        /// </summary>
        public static bool PreferBusinessDates { get; set; }

        /// <summary>
        /// The business low date i.e. 1/1/1900.  Most business dates are not expected to be earlier than this.
        /// </summary>
        public static DateTime LowDate = new DateTime(1900, 1, 1);

        /// <summary>
        /// The business high date i.e. 12/31/2199.  Most business dates are not expected to be later than this.
        /// </summary>
        public static DateTime HighDate = new DateTime(2199, 12, 31);
    }
}

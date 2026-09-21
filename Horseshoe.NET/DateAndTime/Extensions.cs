using System;
using System.Collections.Generic;
using System.Linq;

using static Horseshoe.NET.DateAndTime.DateTimeConstants;
using Horseshoe.NET.Globalization;
using Horseshoe.NET.Text;

namespace Horseshoe.NET.DateAndTime
{
    public static class Extensions
    {
        /* * * * * * * * * * * *
         * 
         * DateTime Extensions
         * 
         * * * * * * * * * * * */

        /// <summary>
        /// Calculates the age of the supplied date as of a second supplied date, if supplied, otherwise as of now, optionally ignoring the time portions.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate age.</param>
        /// <param name="asOf">The date to which age is calculated, default is the current date.</param>
        /// <param name="ignoreTime">Whether to ignore the time portion of the dates.</param>
        /// <returns>The age as a TimeSpan.</returns>
        public static TimeSpan Age(this DateTime dateTime, DateTime asOf = default, bool ignoreTime = false) =>
            Wrangle(asOf, ignoreTime: ignoreTime, wrangleToCurrent: true) - (ignoreTime ? dateTime.Date : dateTime);

        /// <summary>
        /// Calculates the age in years of the supplied date as of a second supplied date, if supplied, otherwise as of now, optionally ignoring the time portions.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate age.</param>
        /// <param name="asOf">The date to which age is calculated, default is the current date.</param>
        /// <param name="ignoreTime">Whether to ignore the time portion of the dates.</param>
        /// <returns>The age in years.</returns>
        public static double AgeInYears(this DateTime dateTime, DateTime asOf = default, bool ignoreTime = false) =>
            ExtTotalYears(Age(dateTime, asOf: asOf, ignoreTime: ignoreTime));

        /// <summary>
        /// Calculates the age in months of the supplied date as of a second supplied date, if supplied, otherwise as of now, optionally ignoring the time portions.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate age.</param>
        /// <param name="asOf">The date to which age is calculated, default is the current date.</param>
        /// <param name="ignoreTime">Whether to ignore the time portion of the dates.</param>
        /// <returns>The age in months.</returns>
        public static double AgeInMonths(this DateTime dateTime, DateTime asOf = default, bool ignoreTime = false) =>
            ExtTotalMonths(Age(dateTime, asOf: asOf, ignoreTime: ignoreTime));

        /// <summary>
        /// Calculates the age in weeks of the supplied date as of a second supplied date, if supplied, otherwise as of now, optionally ignoring the time portions.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate age.</param>
        /// <param name="asOf">The date to which age is calculated, default is the current date.</param>
        /// <param name="ignoreTime">Whether to ignore the time portion of the dates.</param>
        /// <returns>The age in weeks.</returns>
        public static double AgeInWeeks(this DateTime dateTime, DateTime asOf = default, bool ignoreTime = false) =>
            ExtTotalWeeks(Age(dateTime, asOf: asOf, ignoreTime: ignoreTime));

        /// <summary>
        /// Calculates the age in days of the supplied date as of a second supplied date, if supplied, otherwise as of now, optionally ignoring the time portions.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate age.</param>
        /// <param name="asOf">The date to which age is calculated, default is the current date.</param>
        /// <param name="ignoreTime">Whether to ignore the time portion of the dates.</param>
        /// <returns>The age in days.</returns>
        public static double AgeInDays(this DateTime dateTime, DateTime asOf = default, bool ignoreTime = false) =>
            Age(dateTime, asOf: asOf, ignoreTime: ignoreTime).TotalDays;

        /// <summary>
        /// Calculates the age in hours of the supplied date as of a second supplied date, if supplied, otherwise as of now, optionally ignoring the time portions.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate age.</param>
        /// <param name="asOf">The date to which age is calculated, default is the current date.</param>
        /// <param name="ignoreTime">Whether to ignore the time portion of the dates.</param>
        /// <returns>The age in hours.</returns>
        public static double AgeInHours(this DateTime dateTime, DateTime asOf = default, bool ignoreTime = false) =>
            Age(dateTime, asOf: asOf, ignoreTime: ignoreTime).TotalHours;

        /// <summary>
        /// Calculates the age in minutes of the supplied date as of a second supplied date, if supplied, otherwise as of now, optionally ignoring the time portions.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate age.</param>
        /// <param name="asOf">The date to which age is calculated, default is the current date.</param>
        /// <param name="ignoreTime">Whether to ignore the time portion of the dates.</param>
        /// <returns>The age in minutes.</returns>
        public static double AgeInMinutes(this DateTime dateTime, DateTime asOf = default, bool ignoreTime = false) =>
            Age(dateTime, asOf: asOf, ignoreTime: ignoreTime).TotalMinutes;

        /// <summary>
        /// Calculates the age in seconds of the supplied date as of a second supplied date, if supplied, otherwise as of now, optionally ignoring the time portions.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate age.</param>
        /// <param name="asOf">The date to which age is calculated, default is the current date.</param>
        /// <param name="ignoreTime">Whether to ignore the time portion of the dates.</param>
        /// <returns>The age in seconds.</returns>
        public static double AgeInSeconds(this DateTime dateTime, DateTime asOf = default, bool ignoreTime = false) =>
            Age(dateTime, asOf: asOf, ignoreTime: ignoreTime).TotalSeconds;

        /// <summary>
        /// Calculates the age in milliseconds of the supplied date as of a second supplied date, if supplied, otherwise as of now, optionally ignoring the time portions.
        /// </summary>
        /// <param name="dateTime">The date for which to calculate age.</param>
        /// <param name="asOf">The date to which age is calculated, default is the current date.</param>
        /// <param name="ignoreTime">Whether to ignore the time portion of the dates.</param>
        /// <returns>The age in milliseconds.</returns>
        public static double AgeInMilliseconds(this DateTime dateTime, DateTime asOf = default, bool ignoreTime = false) =>
            Age(dateTime, asOf: asOf, ignoreTime: ignoreTime).TotalMilliseconds;

        /// <summary>
        /// Move the supplied date into the business date range (i.e. lowDate to highDate), you can optionally override <see cref="LowDate"/> and <see cref="HighDate"/> or ignore the time portion.
        /// </summary>
        /// <param name="dateTime">A date / time</param>
        /// <param name="ignoreTime">Whether to truncate the time portion of the dates.</param>
        /// <param name="lowDate">The lower bound of the date range, default is <see cref="LowDate"/>.</param>
        /// <param name="highDate">The upper bound of the date range, <see cref="HighDate"/>.</param>
        /// <param name="wrangleToCurrent">Whether to wrangle the date to the current date.</param>
        /// <returns>The wrangled date.</returns>
        public static DateTime Wrangle
        (
            this DateTime dateTime, 
            bool ignoreTime = false, 
            DateTime lowDate = default, 
            DateTime highDate = default,
            bool wrangleToCurrent = false
        )
        {
            // handle special cases
            if (wrangleToCurrent)
                return dateTime == default
                    ? (ignoreTime ? DateTime.Today : DateTime.Now)
                    : (ignoreTime ? dateTime.Date : dateTime);

            // parameter validation
            if (lowDate == default)
                lowDate = LowDate;
            else 
                lowDate = lowDate.Date;

            if (highDate == default) 
                highDate = HighDate;
            else
                highDate = highDate.Date;

            // only return a value within the range of lowDate and highDate
            if (dateTime < lowDate)
                return lowDate;

            if (dateTime > highDate)
                return highDate;

            return ignoreTime ? dateTime.Date : dateTime;
        }

        /* * * * * * * * * * * *
         * 
         * TimeSpan Extensions
         * 
         * * * * * * * * * * * */

        private const double daysInYear = 365.25;

        private static double CalcYears(TimeSpan timeSpan) =>
            timeSpan.TotalDays / daysInYear;

        private static double CalcMonths(TimeSpan timeSpan) =>
            (timeSpan.TotalDays - ((int)CalcYears(timeSpan) * daysInYear)) / daysInYear * 12.0;

        private static double CalcWeeks(TimeSpan timeSpan) =>
            (timeSpan.TotalDays - ((int)CalcYears(timeSpan) * daysInYear) - ((int)CalcMonths(timeSpan) * daysInYear / 12.0)) / 7.0;

        private static double CalcDays(TimeSpan timeSpan) =>
            timeSpan.TotalDays - ((int)CalcYears(timeSpan) * daysInYear) - ((int)CalcMonths(timeSpan) * daysInYear / 12.0) - ((int)CalcWeeks(timeSpan) * 7.0);

        public static int ExtYears(this TimeSpan timeSpan) =>
            (int)CalcYears(timeSpan);

        public static int ExtMonths(this TimeSpan timeSpan) =>
            (int)CalcMonths(timeSpan);

        public static int ExtWeeks(this TimeSpan timeSpan) =>
            (int)CalcWeeks(timeSpan);

        public static int ExtDays(this TimeSpan timeSpan) =>
            (int)CalcDays(timeSpan);

        public static double ExtTotalWeeks(this TimeSpan timeSpan) =>
            timeSpan.TotalDays / 7.0;

        public static double ExtTotalMonths(this TimeSpan timeSpan) =>
            timeSpan.TotalDays / daysInYear * 12.0;

        public static double ExtTotalYears(this TimeSpan timeSpan) =>
            timeSpan.TotalDays / daysInYear;

        /// <summary>
        /// Renders a TimeSpan as a string adding years, months and weeks to the output.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="locale"></param>
        /// <param name="letterCase"></param>
        /// <param name="abbreviationLength"></param>
        /// <param name="elementFormat"></param>
        /// <param name="separator"></param>
        /// <param name="renderHint"></param>
        /// <param name="nonZeroOnly"></param>
        /// <returns></returns>
        /// <exception cref="ThisShouldNeverHappenException"></exception>
        public static string ExtToString(this TimeSpan timeSpan, string locale = null, LetterCase letterCase = default, int abbreviationLength = 0, /*int elementLimit = 0,*/ string elementFormat = "{0} {1}", string separator = ", ", TimeSpanRenderHint renderHint = default, bool nonZeroOnly = false)
        {
            var years = ExtYears(timeSpan);
            var months = ExtMonths(timeSpan);
            var weeks = ExtWeeks(timeSpan);
            var days = ExtDays(timeSpan);

            var renderedElements = new List<string>();

            switch (renderHint)
            {
                case TimeSpanRenderHint.All:

                    if (years + months + weeks + days + timeSpan.Hours + timeSpan.Minutes + timeSpan.Seconds + timeSpan.Milliseconds == 0)
                    {
                        renderedElements.Add(_extToString("TimeSpan.Day", 0, locale, letterCase, abbreviationLength, elementFormat));
                        break;
                    }

                    if (years != 0 || !nonZeroOnly)
                        renderedElements.Add(_extToString("TimeSpan.Year", years, locale, letterCase, abbreviationLength, elementFormat));
                    if (months != 0 || !nonZeroOnly)
                        renderedElements.Add(_extToString("TimeSpan.Month", months, locale, letterCase, abbreviationLength, elementFormat));
                    if (weeks != 0 || !nonZeroOnly)
                        renderedElements.Add(_extToString("TimeSpan.Week", weeks, locale, letterCase, abbreviationLength, elementFormat));
                    if (days != 0 || !nonZeroOnly)
                        renderedElements.Add(_extToString("TimeSpan.Day", days, locale, letterCase, abbreviationLength, elementFormat));
                    if (timeSpan.Hours != 0 || !nonZeroOnly)
                        renderedElements.Add(_extToString("TimeSpan.Hour", timeSpan.Hours, locale, letterCase, abbreviationLength, elementFormat));
                    if (timeSpan.Minutes != 0 || !nonZeroOnly)
                        renderedElements.Add(_extToString("TimeSpan.Minute", timeSpan.Minutes, locale, letterCase, abbreviationLength, elementFormat));
                    if (timeSpan.Seconds != 0 || !nonZeroOnly)
                        renderedElements.Add(_extToString("TimeSpan.Second", timeSpan.Seconds, locale, letterCase, abbreviationLength, elementFormat));
                    if (timeSpan.Milliseconds != 0 || !nonZeroOnly)
                        renderedElements.Add(_extToString("TimeSpan.Millisecond", timeSpan.Milliseconds, locale, letterCase, abbreviationLength, elementFormat));
                    break;


                case TimeSpanRenderHint.TrimZeroes:

                    if (years + months + weeks + days + timeSpan.Hours + timeSpan.Minutes + timeSpan.Seconds + timeSpan.Milliseconds == 0)
                    {
                        renderedElements.Add(_extToString("TimeSpan.Day", 0, locale, letterCase, abbreviationLength, elementFormat));
                        break;
                    }

                    // display years if non-zero
                    if (years != 0)
                        renderedElements.Add(_extToString("TimeSpan.Year", years, locale, letterCase, abbreviationLength, elementFormat));

                    // display months if non-zero or if not a leading or trailing zero (i.e., if there are any previously rendered elements and weeks + days + hours + minutes + seconds + milliseconds is non-zero)
                    if (months != 0 || (renderedElements.Any() && weeks + days + timeSpan.Hours + timeSpan.Minutes + timeSpan.Seconds + timeSpan.Milliseconds != 0))
                        renderedElements.Add(_extToString("TimeSpan.Month", months, locale, letterCase, abbreviationLength, elementFormat));

                    // display weeks if non-zero or if not a leading or trailing zero (i.e., if there are any previously rendered elements and days + hours + minutes + seconds + milliseconds is non-zero)
                    if (weeks != 0 || (renderedElements.Any() && days + timeSpan.Hours + timeSpan.Minutes + timeSpan.Seconds + timeSpan.Milliseconds != 0))
                        renderedElements.Add(_extToString("TimeSpan.Week", weeks, locale, letterCase, abbreviationLength, elementFormat));

                    // display days if non-zero or if not a leading or trailing zero (i.e., if there are any previously rendered elements and hours + minutes + seconds + milliseconds is non-zero)
                    if (days != 0 || (renderedElements.Any() && timeSpan.Hours + timeSpan.Minutes + timeSpan.Seconds + timeSpan.Milliseconds != 0))
                        renderedElements.Add(_extToString("TimeSpan.Day", days, locale, letterCase, abbreviationLength, elementFormat));

                    // display hours if non-zero or if not a leading or trailing zero (i.e., if there are any previously rendered elements and minutes + seconds + milliseconds is non-zero)
                    if (timeSpan.Hours != 0 || (renderedElements.Any() && timeSpan.Minutes + timeSpan.Seconds + timeSpan.Milliseconds != 0))
                        renderedElements.Add(_extToString("TimeSpan.Hour", timeSpan.Hours, locale, letterCase, abbreviationLength, elementFormat));

                    // display minutes if non-zero or if not a leading or trailing zero (i.e., if there are any previously rendered elements and seconds + milliseconds is non-zero)
                    if (timeSpan.Minutes != 0 || (renderedElements.Any() && timeSpan.Seconds + timeSpan.Milliseconds != 0))
                        renderedElements.Add(_extToString("TimeSpan.Minute", timeSpan.Minutes, locale, letterCase, abbreviationLength, elementFormat));

                    // display seconds if non-zero or if not a leading or trailing zero (i.e., if there are any previously rendered elements and milliseconds is non-zero)
                    if (timeSpan.Seconds != 0 || (renderedElements.Any() && timeSpan.Milliseconds != 0))
                        renderedElements.Add(_extToString("TimeSpan.Second", timeSpan.Seconds, locale, letterCase, abbreviationLength, elementFormat));

                    // display milliseconds if non-zero
                    if (timeSpan.Milliseconds != 0)
                        renderedElements.Add(_extToString("TimeSpan.Millisecond", timeSpan.Milliseconds, locale, letterCase, abbreviationLength, elementFormat));

                    break;

                case TimeSpanRenderHint.RuleOfTwo:
                case TimeSpanRenderHint.RuleOfThree:
                case TimeSpanRenderHint.RuleOfFour:

                    int maxElements = renderHint switch
                    {
                        TimeSpanRenderHint.RuleOfTwo => 2,
                        TimeSpanRenderHint.RuleOfThree => 3,
                        TimeSpanRenderHint.RuleOfFour => 4,
                        _ => throw new ThisShouldNeverHappenException()
                    };

                    // display years if non-zero
                    if (years != 0)
                        renderedElements.Add(_extToString("TimeSpan.Year", years, locale, letterCase, abbreviationLength, elementFormat));

                    // display months if non-zero
                    if (renderedElements.Count < maxElements && (months != 0 || (renderedElements.Any() && !nonZeroOnly)))
                        renderedElements.Add(_extToString("TimeSpan.Month", months, locale, letterCase, abbreviationLength, elementFormat));

                    // display weeks if non-zero
                    if (renderedElements.Count < maxElements && (weeks != 0 || (renderedElements.Any() && !nonZeroOnly)))
                        renderedElements.Add(_extToString("TimeSpan.Week", weeks, locale, letterCase, abbreviationLength, elementFormat));

                    // display days if non-zero
                    if (renderedElements.Count < maxElements && (days != 0 || (renderedElements.Any() && !nonZeroOnly)))
                        renderedElements.Add(_extToString("TimeSpan.Day", days, locale, letterCase, abbreviationLength, elementFormat));

                    // display hours if non-zero
                    if (renderedElements.Count < maxElements && (timeSpan.Hours != 0 || (renderedElements.Any() && !nonZeroOnly)))
                        renderedElements.Add(_extToString("TimeSpan.Hour", timeSpan.Hours, locale, letterCase, abbreviationLength, elementFormat));

                    // display minutes if non-zero
                    if (renderedElements.Count < maxElements && (timeSpan.Minutes != 0 || (renderedElements.Any() && !nonZeroOnly)))
                        renderedElements.Add(_extToString("TimeSpan.Minute", timeSpan.Minutes, locale, letterCase, abbreviationLength, elementFormat));

                    // display seconds if non-zero
                    if (renderedElements.Count < maxElements && (timeSpan.Seconds != 0 || (renderedElements.Any() && !nonZeroOnly)))
                        renderedElements.Add(_extToString("TimeSpan.Second", timeSpan.Seconds, locale, letterCase, abbreviationLength, elementFormat));

                    // display milliseconds if non-zero
                    if (renderedElements.Count < maxElements && (timeSpan.Milliseconds != 0 || (renderedElements.Any() && !nonZeroOnly)))
                        renderedElements.Add(_extToString("TimeSpan.Millisecond", timeSpan.Milliseconds, locale, letterCase, abbreviationLength, elementFormat));

                    break;

                default:

                    renderedElements.Add(_extToString("TimeSpan.Day", 0, locale, letterCase, abbreviationLength, elementFormat));
                    break;
            }

            return string.Join(separator, renderedElements);
        }

        private static string _extToString(string key, int value, string locale, LetterCase letterCase, int abbreviationLength, string elementFormat)
        {
            // Step 1: set time element label
            var label = Lang.Get(key, locale: locale);
            if (label.Contains('|'))
                label = label.Split('|')[value == 1 ? 0 : 1];
            else if(value != 1)
                label += "s";

            // Step 2: abbrieviate label, if applicable
            if (abbreviationLength > 0 && abbreviationLength < label.Length)
                label = label.Substring(0, abbreviationLength);

            // Step 3: handle letter case
            label = TextUtil.ApplyLetterCase(label, letterCase, locale: locale);

            // Step 4: put it all together
            return string.Format(elementFormat, value, label);
        }

        private static Languages Lang { get; } = new Languages
        {
            { "TimeSpan.Year", "Year" },
            { "TimeSpan.Month", "Month" },
            { "TimeSpan.Week", "Week" },
            { "TimeSpan.Day", "Day" },
            { "TimeSpan.Hour", "Hour" },
            { "TimeSpan.Minute", "Minute" },
            { "TimeSpan.Second", "Second" },
            { "TimeSpan.Millisecond", "Millisecond" }
        }
        .AddLanguages
        (
            new Language("es") 
            {
                { "TimeSpan.Year", "Año" },
                { "TimeSpan.Month", "Mes|Meses" },
                { "TimeSpan.Week", "Semana" },
                { "TimeSpan.Day", "Día" },
                { "TimeSpan.Hour", "Hora" },
                { "TimeSpan.Minute", "Minuto" },
                { "TimeSpan.Second", "Segundo" },
                { "TimeSpan.Millisecond", "Milisegundo" }
            }
        );
    }
}

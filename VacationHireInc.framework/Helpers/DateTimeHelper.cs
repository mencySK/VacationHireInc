// <copyright file="DateTimeHelper.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.framework.Helpers
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Extensions for the native .net date time methods
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Represents the format of a valid date and time request
        /// </summary>
        private static string validFullDateTimeRequestFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Represent the format of a valid date request
        /// </summary>
        private static string validDateRequestFormat = "yyyy-MM-dd";

        /// <summary>
        /// Gets the server time based on the Eastern European time as azure does not adjust
        /// </summary>
        /// <returns>The current time in the UK</returns>
        public static DateTime ServerTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GTB Standard Time"));
        }

        /// <summary>
        /// Try to parse date with a full date time format
        /// </summary>
        /// <param name="date">The date with time trying to convert</param>
        /// <param name="dateTimeConverted">If successful the converted date time will be returned</param>
        /// <returns>If it is successful it will return the converted date time</returns>
        public static bool FullDateTimeConversion(string date, out DateTime? dateTimeConverted)
        {
            if (DateTime.TryParseExact(date, validFullDateTimeRequestFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeOut))
            {
                dateTimeConverted = dateTimeOut;
                return true;
            }
            else
            {
                dateTimeConverted = null;
                return false;
            }
        }

        /// <summary>
        /// Try to parse date with a date format without time
        /// </summary>
        /// <param name="date">The date with time trying to convert</param>
        /// <param name="dateTimeConverted">If successful the converted date time will be returned</param>
        /// <returns>If it is successful it will return the converted date without time</returns>
        public static bool DateConversion(string date, out DateTime? dateTimeConverted)
        {
            if (DateTime.TryParseExact(date, validDateRequestFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeOut))
            {
                dateTimeConverted = dateTimeOut;
                return true;
            }
            else
            {
                dateTimeConverted = null;
                return false;
            }
        }
    }
}

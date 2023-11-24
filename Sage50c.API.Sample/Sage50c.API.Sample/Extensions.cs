using System;

namespace Sage50c.API.Sample {
    internal static class Extensions {
        internal static int ToInt(this string Value) {
            int result = 0;
            int.TryParse(Value, out result);
            return result;
        }

        internal static DateTime FirstDayOfMonth(this DateTime CurrentDate) {
            var firstDay = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            return firstDay;
        }

        internal static DateTime LastDayOfLastMonth(this DateTime CurrentDate) {
            var lastDay = new DateTime(CurrentDate.Year, CurrentDate.Month, DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month));
            return lastDay;
        }
    }
}

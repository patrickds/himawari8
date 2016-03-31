using System;

namespace himawari8.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime NormalizeMinutesAccordingToHimawari8UpdateCycle(this DateTime dateTime)
        {
            var minutesSinceLastUpdate = new TimeSpan(0, dateTime.Minute % 10, 0);
            return dateTime - minutesSinceLastUpdate;
        }

        public static DateTime NormalizeSecondsAccordingToHimawari8UpdateCycle(this DateTime dateTime)
        {
            var secondsSinceLastUpdate = new TimeSpan(0, 0, dateTime.Second);
            return dateTime - secondsSinceLastUpdate;
        }
    }
}

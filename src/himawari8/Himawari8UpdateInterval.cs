using System;
using himawari8.Extensions;

namespace himawari8
{
    internal static class Himawari8UpdateInterval
    {
        public static DateTime GetLasHimawari8UpdateTime()
        {
            var timeSpan = new TimeSpan(01, 40, 00);
            var nowInJapan = DateTime.Now + timeSpan;

            return nowInJapan
                .NormalizeMinutesAccordingToHimawari8UpdateCycle()
                .NormalizeSecondsAccordingToHimawari8UpdateCycle();
        }

        public static DateTime GetSynchronizedSunLightTime()
        {
            var timeSpan = new TimeSpan(09, 00, 00);
            var synchronizedSunLightDate = DateTime.Now - timeSpan;

            return synchronizedSunLightDate
                .NormalizeMinutesAccordingToHimawari8UpdateCycle()
                .NormalizeSecondsAccordingToHimawari8UpdateCycle();
        }

        public static DateTime GetLastDayHimawariTime()
        {
            var oneDaySpan = new TimeSpan(24, 00, 00);
            var yesterday = DateTime.Now - oneDaySpan;

            return yesterday
                .NormalizeMinutesAccordingToHimawari8UpdateCycle()
                .NormalizeSecondsAccordingToHimawari8UpdateCycle();
        }
    }
}

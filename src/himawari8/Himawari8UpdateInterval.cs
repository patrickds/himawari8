using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace himawari8
{
    internal static class Himawari8UpdateInterval
    {
        private static DateTime GetJapanDateTimeNow()
        {
            var timeSpan = new TimeSpan(-2, -30, 0);
            var nowInJapan = DateTime.Now + timeSpan;

            return nowInJapan;
        }

        private static DateTime NormalizeMinutes(DateTime nowInJapan)
        {
            var minutesSinceLastUpdate = nowInJapan.Minute % 10;
            return nowInJapan.AddMinutes(-minutesSinceLastUpdate);
        }

        private static DateTime NormalizeSeconds(DateTime nowInJapan)
        {
            var secondsSinceLastUpdate = nowInJapan.Second;
            return nowInJapan.AddSeconds(-secondsSinceLastUpdate);
        }

        public static DateTime GetLasHimawari8UpdateTime()
        {
            var nowInJapan = GetJapanDateTimeNow();
            var nowInJapanNormlizedMinutes = NormalizeMinutes(nowInJapan);
            var lastHimawariUpdate = NormalizeSeconds(nowInJapanNormlizedMinutes);

            return lastHimawariUpdate;
        }
    }
}

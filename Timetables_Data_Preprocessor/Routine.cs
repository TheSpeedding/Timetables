using System;
using System.Threading;
using System.IO;

namespace Timetables.Preprocessor
{
    public static class Routine
    {
        private static DateTime GetDateTimeOfNextRun(DateTime expiration)
        {
            // Few hours before expiration of data or ~ 3 days after the last run, everytime at 3.00 am.

            // If the data expires today, we will try to redownload them every hour until we get the actual ones.
            
            if (expiration.Date == DateTime.Today.Date || DateTime.Now.AddHours(8) > expiration) // Expires today? Refresh in one hour.
                return DateTime.Now.AddHours(1);

            if (DateTime.Now.Date.AddDays(3) > expiration.Date) // Expires in less than 3 days? Refresh 6 hours before expiration.
                return expiration.Subtract(new TimeSpan(6, 0, 0));

            else // Expires in more than 3 days? Refresh in 3 days sometime at night.
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 3, 0, 0).AddDays(3);
        }
        private static DateTime GetExpirationDateTime()
        {
            DateTime expiration;
            using (var sr = new StreamReader("data/expires.txt"))
            {
                string validUntil = sr.ReadLine();

                int year = int.Parse(validUntil.Substring(0, 4));
                int month = int.Parse(validUntil.Substring(4, 2));
                int day = int.Parse(validUntil.Substring(6, 2));

                expiration = new DateTime(year, month, day, 23, 59, 59);
            }
            return expiration;
        }
        public static void PerformRoutine()
        {
            // First run of the application or loss of data. Data does not exist or they are in the invalid state, we have to download and transform them.

            if (!DataFeed.AreDataPresent)
                 DataFeed.GetAndTransformDataFeed<GtfsDataFeed>();


            // Data may become outdated soon. Redownload them.

            DateTime expiration = GetExpirationDateTime();

            if (expiration.Date == DateTime.Today.Date)
                DataFeed.GetAndTransformDataFeed<GtfsDataFeed>();

            while (true)
            {
                // Download data only when they are about to become outdated.

                expiration = GetDateTimeOfNextRun(GetExpirationDateTime());

                int secondsToSleep = (int)(expiration - DateTime.Now).TotalSeconds;

                Thread.Sleep(secondsToSleep);

                DataFeed.GetAndTransformDataFeed<GtfsDataFeed>();
            }

            throw new InvalidOperationException("Preprocessor thread has unexpectedly stopped.");
        }
    }
}

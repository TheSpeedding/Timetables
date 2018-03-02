using System;
using System.IO;

namespace Timetables.Preprocessor
{
    /// <summary>
    /// Interface that all the formats of feeds for timetables should implement.
    /// </summary>
    public interface IDataFeed
    {
        Calendar Calendar { get; }
        CalendarDates CalendarDates { get; }
        RoutesInfo RoutesInfo { get; }
        Stops Stops { get; }
        Stations Stations { get; }
        Footpaths Footpaths { get; }
        Trips Trips { get; }
        StopTimes StopTimes { get; }
        Routes Routes { get; }
        string ExpirationDate { get; }
        void CreateDataFeed(string path);
    }

    public static class DataFeed
    {
        public static void GetAndTransformDataFeed<T>() where T : IDataFeed
        {
            Downloader.GetDataFeed("temp_data/");

            IDataFeed data = (T)Activator.CreateInstance(typeof(T), (string)"temp_data/");

            data.CreateDataFeed("data/");

            Downloader.DeleteTrash("temp_data/");
        }
        public static bool AreDataPresent => Directory.Exists("data") &&
            File.Exists("data/calendar.txt") && File.Exists("data/calendar_dates.txt") && File.Exists("data/expires.txt") &&
            File.Exists("data/footpaths.txt") && File.Exists("data/routes.txt") && File.Exists("data/routes_info.txt") &&
            File.Exists("data/stations.txt") && File.Exists("data/stop_times.txt") && File.Exists("data/stops.txt") &&
            File.Exists("data/trips.txt");
    }
}

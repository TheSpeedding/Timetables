using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void CreateDataFeed(string path);
    }

    public static class DataFeed
    {
        public static void GetAndTransformDataFeed<T>() where T : IDataFeed
        {
            Downloader.GetDataFeed("gtfs_data/");

            IDataFeed data = (T)Activator.CreateInstance(typeof(T), (string)"gtfs_data/");

            data.CreateDataFeed("data/");

            Downloader.DeleteTrash("gtfs_data/");
        }
    }
}

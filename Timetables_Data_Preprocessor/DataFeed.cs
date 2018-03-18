using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        string ExpirationDate { set; get; }
        void CreateDataFeed(string path);
    }
	/// <summary>
	/// Static class including everything necessary for data feed generation.
	/// </summary>
    public static class DataFeed
    {
		private static Random random = new Random();
		/// <summary>
		/// Returns random string of length 10. Probability of getting the same string is 37^10, more than 4 quadrillions.
		/// </summary>
		public static string RandomString() => new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10).Select(s => s[random.Next(s.Length)]).ToArray());

		/// <summary>
		/// Downloads and creates data feed.
		/// </summary>
		public static void GetAndTransformDataFeed<T>(params string[] urls) where T : IDataFeed
        {
			List<IDataFeed> dataList = new List<IDataFeed>();

			for (int i = 0; i < urls.Length; i++)
			{
				Downloader.GetDataFeed($"{ i }_temp_data/", urls[i]);
				dataList.Add((T)Activator.CreateInstance(typeof(T), (string)$"{ i }_temp_data/"));
			}

			IDataFeed mergedData = MergeMultipleDataFeeds(dataList);

			mergedData.CreateDataFeed("data/");

			for (int i = 0; i < urls.Length; i++)
				Downloader.DeleteTrash($"{ i }_temp_data/");
        }

		/// <summary>
		/// Merges multiple data feeds into one data feed.
		/// </summary>
		/// <param name="dataList">List of data feeds to be merged.</param>
		/// <returns>Merged data feed.</returns>
		private static IDataFeed MergeMultipleDataFeeds(List<IDataFeed> dataList)
		{
			if (dataList.Count == 0)
				return default(IDataFeed);

			IDataFeed mergedData = dataList[0];
			
			for (int i = 1; i < dataList.Count; i++)
				mergedData.AddToDataFeed(dataList[i]);

			return mergedData;
		}

		/// <summary>
		/// Adds something into data feed.
		/// </summary>
		/// <param name="dataFeed">Data feed.</param>
		/// <param name="toBeAdded">What should be added to data feed.</param>
		/// <returns>Merged data feed.</returns>

		private static void AddToDataFeed(this IDataFeed dataFeed, IDataFeed toBeAdded)
		{
			dataFeed.Calendar.MergeCollections(toBeAdded.Calendar);
			dataFeed.CalendarDates.MergeCollections(toBeAdded.CalendarDates);
			dataFeed.RoutesInfo.MergeCollections(toBeAdded.RoutesInfo);
			dataFeed.Routes.MergeCollections(toBeAdded.Routes);
			dataFeed.Stations.MergeCollections(toBeAdded.Stations);
			dataFeed.Stops.MergeCollections(toBeAdded.Stops);
			dataFeed.StopTimes.MergeCollections(toBeAdded.StopTimes);
			dataFeed.Trips.MergeCollections(toBeAdded.Trips);
			dataFeed.Footpaths.MergeCollections(toBeAdded.Footpaths);
			dataFeed.ExpirationDate = int.Parse(dataFeed.ExpirationDate) < int.Parse(toBeAdded.ExpirationDate) ? dataFeed.ExpirationDate : toBeAdded.ExpirationDate;
		}

		/// <summary>
		/// Checks if the data are present.
		/// </summary>
		public static bool AreDataPresent => Directory.Exists("data") &&
            File.Exists("data/calendar.txt") && File.Exists("data/calendar_dates.txt") && File.Exists("data/expires.txt") &&
            File.Exists("data/footpaths.txt") && File.Exists("data/routes.txt") && File.Exists("data/routes_info.txt") &&
            File.Exists("data/stations.txt") && File.Exists("data/stop_times.txt") && File.Exists("data/stops.txt") &&
            File.Exists("data/trips.txt");
    }
}

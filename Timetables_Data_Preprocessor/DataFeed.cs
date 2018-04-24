using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        Trips Trips { get; }
        StopTimes StopTimes { get; }
        Routes Routes { get; }
		Footpaths Footpaths { set; get; }
		string ExpirationDate { set; get; }
        void CreateDataFeed(string path);
		void CreateBasicData(string path);
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
		/// Delegate serving information about data processing.
		/// </summary>
		/// <param name="message">Message to be shown.</param>
		public delegate void DataProcessingEventHandler(string message);
		/// <summary>
		/// The event acknowleding the server administrator about work of the preprocessor.
		/// </summary>
		public static event DataProcessingEventHandler DataProcessing;
		/// <summary>
		/// Delegate serving information about loading progress.
		/// </summary>
		/// <param name="message">Message to be shown.</param>
		/// <param name="progress">Progress in percent.</param>
		public delegate void LoadingProgressEventHandler(string message, int progress);
		/// <summary>
		/// The event acknowleding user about the progress of loading the data.
		/// </summary>
		public static event LoadingProgressEventHandler LoadingProgress;
		/// <summary>
		/// Processes one data source.
		/// </summary>
		/// <typeparam name="T">Type of data.</typeparam>
		/// <param name="dataList">List where should data be added.</param>
		/// <param name="url">URL to data.</param>
		/// <param name="index">Internal identifier for creating temporary folders.</param>
		/// <param name="urlsLength">Count of data feeds to process.</param>
		private static void ProcessData<T>(IList<IDataFeed> dataList, string url, int index = 0, int urlsLength = 1) where T : IDataFeed
		{
			try
			{

				DataProcessing?.Invoke($"Trying to download data from URL { url }.");

				Downloader.GetDataFeed($"{ index }_temp_data/", url, index);
				
				LoadingProgress?.Invoke("Part of the data successfully downloaded.", 20 / urlsLength);

				DataProcessing?.Invoke($"Data from URL { url } downloaded successfully.");

				DataProcessing?.Invoke($"Trying to parse data downloaded from { url }.");

				IDataFeed data = (T)Activator.CreateInstance(typeof(T), (string)$"{ index }_temp_data/");

				lock (dataList)
				{
					dataList.Add(data);
				}

				LoadingProgress?.Invoke("Part of the data successfully parsed.", 40 / urlsLength);

				DataProcessing?.Invoke($"The data downloaded from { url } parsed successfully.");
			}
			catch (Exception ex)
			{
				if (ex is System.Reflection.TargetInvocationException)
					ex = ex.InnerException;

				if (ex is UriFormatException)
					DataProcessing?.Invoke($"The URL { url } is invalid.");

				else if (ex is System.Net.WebException)
					DataProcessing?.Invoke($"There was a problem downloading file { url }. Server may be inacessible or you are missing internet connection.");

				else if (ex is FormatException)
					DataProcessing?.Invoke($"The data downloaded from { url } are not well-formed.");
#if !DEBUG
				else
					DataProcessing?.Invoke($@"Parsing of data located in { url } ended with an unknown error.
Error: { ex.Message } Type of { ex.GetType() }.");
#else
					else
						throw;
#endif
			}
		}
		/// <summary>
		/// Downloads and creates data feed.
		/// </summary>
		public static void GetAndTransformDataFeed<T>(params string[] urls) where T : IDataFeed
        {
			LoadingProgress?.Invoke("Preparing for the first launch.", 0);

			List<IDataFeed> dataList = new List<IDataFeed>(); // Data are added into the list only if they are (downloaded and) parsed successfully.
			
			Parallel.For(0, urls.Length, (int i) => ProcessData<T>(dataList, urls[i], i, urls.Length)); // Try to process the data in parallel mode.
						
			if (dataList.Count == 0)
			{
				DataProcessing?.Invoke($"There are no data to process.");
				return;
			}

			DataProcessing?.Invoke($"Trying to merge the data.");
			
			IDataFeed mergedData = MergeMultipleDataFeeds(dataList);

			LoadingProgress?.Invoke("Data merged successfully.", 5);

			DataProcessing?.Invoke($"Data merged successfully.");
			
			DataProcessing?.Invoke($"Trying to create new data feed.");
			
			mergedData.CreateDataFeed("data/");

			mergedData.CreateBasicData("basic/");

			LoadingProgress?.Invoke($"Data feed created successfully.", 30);

			DataProcessing?.Invoke($"Data feed created successfully.");

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
			dataFeed.Footpaths.MergeCollections(toBeAdded.Footpaths, dataFeed.Stops, toBeAdded.Stops); // This should come before the stops merging.
			dataFeed.Calendar.MergeCollections(toBeAdded.Calendar);
			dataFeed.CalendarDates.MergeCollections(toBeAdded.CalendarDates);
			dataFeed.RoutesInfo.MergeCollections(toBeAdded.RoutesInfo);
			dataFeed.Routes.MergeCollections(toBeAdded.Routes);
			dataFeed.Stations.MergeCollections(toBeAdded.Stations);
			dataFeed.Stops.MergeCollections(toBeAdded.Stops);
			dataFeed.StopTimes.MergeCollections(toBeAdded.StopTimes);
			dataFeed.Trips.MergeCollections(toBeAdded.Trips);
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

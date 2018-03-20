using System;
using System.Collections.Generic;
using System.IO;

namespace Timetables.Preprocessor
{
    /// <summary>
    /// Gtfs data parser.
    /// </summary>
    public class GtfsDataFeed : IDataFeed
    {
        /// <summary>
        /// Includes information about services.
        /// </summary>
        public Calendar Calendar { get; }
        /// <summary>
        /// Includes information about extraordinary event in the transport.
        /// </summary>
        public CalendarDates CalendarDates { get; }
        /// <summary>
        /// Includes some basic information about routes.
        /// </summary>
        public RoutesInfo RoutesInfo { get; }
        /// <summary>
        /// Includes information about stops.
        /// </summary>
        public Stops Stops { get; }
        /// <summary>
        /// Includes information about stations.
        /// </summary>
        public Stations Stations { get; }
        /// <summary>
        /// Includes information about footpaths.
        /// </summary>
        public Footpaths Footpaths { set; get; }
        /// <summary>
        /// Includes information about trips.
        /// </summary>
        public Trips Trips { get; }
        /// <summary>
        /// Includes information about stop times.
        /// </summary>
        public StopTimes StopTimes { get; }
        /// <summary>
        /// Includes information about routes.
        /// </summary>
        public Routes Routes { get; }
        /// <summary>
        /// Date that the timetables expires in.
        /// </summary>
        public string ExpirationDate { set; get; }
        /// <summary>
        /// Loads Gtfs data feed to memory.
        /// </summary>
        /// <param name="path">Path to the folder with feed.</param>
        public GtfsDataFeed(string path)
        {
            Calendar = new GtfsCalendar(new StreamReader(path + "/calendar.txt"));

			if (File.Exists(path + "/calendar_dates.txt")) // This is fully optional file in GTFS format.
				CalendarDates = new GtfsCalendarDates(new StreamReader(path + "/calendar_dates.txt"), Calendar);
			else
				CalendarDates = new GtfsCalendarDates(); // No file → no extraordinary events.

            RoutesInfo = new GtfsRoutesInfo(new StreamReader(path + "/routes.txt"));
            Stops = new GtfsStops(new StreamReader(path + "/stops.txt"));
            Stations = new GtfsStations(Stops);

            Footpaths = new GtfsFootpaths(Stops); // Even though walking time is an optional field in GTFS format, we will compute it on our own everytime - even though the data for transfers may exist. It ensures us better consistency.

			Trips = new GtfsTrips(new StreamReader(path + "/trips.txt"), Calendar, RoutesInfo);
            StopTimes = new GtfsStopTimes(new StreamReader(path + "/stop_times.txt"), Trips, Stops);
            Routes = new GtfsRoutes(Trips, RoutesInfo);
            ExpirationDate = Calendar.GetExpirationDate().ToString();
        }
        /// <summary>
        /// Creates data feed that is required for the application.
        /// </summary>
        /// <param name="path">Path to the folder that will be the feed saved to.</param>
        public void CreateDataFeed(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            Directory.CreateDirectory(path);
			
            Trips.Write(new StreamWriter(path + "/trips.txt")); // This MUST come first because of trip reindexation (based on sorting).

            Calendar.Write(new StreamWriter(path + "/calendar.txt"));
            CalendarDates.Write(new StreamWriter(path + "/calendar_dates.txt"));
            RoutesInfo.Write(new StreamWriter(path + "/routes_info.txt"));
            Stops.Write(new StreamWriter(path + "/stops.txt"));
            Stations.Write(new StreamWriter(path + "/stations.txt"));
            Footpaths.Write(new StreamWriter(path + "/footpaths.txt"));
            StopTimes.Write(new StreamWriter(path + "/stop_times.txt"));
            Routes.Write(new StreamWriter(path + "/routes.txt"));
            using (var expiration = new StreamWriter(path + "/expires.txt"))
                expiration.Write(ExpirationDate);
		}
		/// <summary>
		/// Method that splits string according to GTFS rules which are the same as CSV files.
		/// </summary>
		/// <param name="input">Input to be splitted.</param>
		public static IList<string> SplitGtfs(string input)
		{
			// This approach is faster than using regex's.

			// Note that this approach is not bugless and may not work for every GTFS feed.

			Queue<string> q = new Queue<string>(input.Replace("\"\"", "").Split(','));

			// Check if there was a comma within the quotes.

			List<string> tokens = new List<string>();

			bool quotes = false;

			while (q.Count > 0)
			{
				string entry = q.Dequeue();

				bool prevQuotes = quotes;

				if (entry.Length > 0 && entry[0] == '"') // Start of the quotes.
				{
					entry = entry.Substring(1, entry.Length - 1);
					quotes = true;
				}

				if (entry.Length > 0 && entry[entry.Length - 1] == '"') // End of the quotes.
				{
					entry = entry.Substring(0, entry.Length - 1);
					quotes = false;
				}

				if (prevQuotes)
					tokens[tokens.Count - 1] += ',' + entry;
				else
					tokens.Add(entry);
			}

			return tokens;
		}
	}
}

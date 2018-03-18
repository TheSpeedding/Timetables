using System;
using System.Collections;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Abstract class for trips collecting information about trips.
	/// </summary>
	public abstract class Trips : IEnumerable<KeyValuePair<string, Trips.Trip>>
    {
        public class Trip : IComparable<Trip>
        {
            /// <summary>
            /// ID of the trip.
            /// </summary>
            public int ID { get; internal set; }
            /// <summary>
            /// Headsign of the trip.
            /// </summary>
            public string Headsign { get; }
            /// <summary>
            /// Route info for the trip.
            /// </summary>
            public RoutesInfo.RouteInfo RouteInfo { get; }
            /// <summary>
            /// Service for the trip.
            /// </summary>
            public Calendar.Service Service { get; }
            /// <summary>
            /// List of the stop times belonging to this trip.
            /// </summary>
            public List<StopTimes.StopTime> StopTimes { get; }
            /// <summary>
            /// Route for the trip.
            /// </summary>
            public Routes.Route Route { get; internal set; }
			/// <summary>
			/// Relative departure time from the first stop. Seconds since midnight.
			/// </summary>
			public int DepartureTime { get; internal set; }
			/// <summary>
			/// Trip ID, Service ID, Route ID, Departure Time.
			/// </summary>
            public override string ToString() => ID + ";" + Service.ID + ";" + Route.ID + ";" + DepartureTime + ";";
			/// <summary>
			/// Compares this instance to a specified Trip object and returns and indication of their relative values.
			/// </summary>
			/// <param name="other">Trip to compare.</param>
			public int CompareTo(Trip other) => DepartureTime.CompareTo(other.DepartureTime);
			/// <summary>
			/// Initializes object.
			/// </summary>
			/// <param name="id">Trip ID.</param>
			/// <param name="headsign">Headsign.</param>
			/// <param name="routeInfo">Route Info.</param>
			/// <param name="service">Service.</param>
            public Trip(int id, string headsign, RoutesInfo.RouteInfo routeInfo, Calendar.Service service)
            {
                StopTimes = new List<StopTimes.StopTime>();
                ID = id;
                Headsign = headsign;
                RouteInfo = routeInfo;
                Service = service;
            }
        }
        protected Dictionary<string, Trip> list = new Dictionary<string, Trip>();
        /// <summary>
        /// Gets required trip.
        /// </summary>
        /// <param name="index">Identificator of the trip.</param>
        /// <returns>Obtained trip.</returns>
        public Trip this[string index] => list[index];
        /// <summary>
        /// Gets the total number of trip.
        /// </summary>
        public int Count => list.Count;
		/// <summary>
		/// Writes the data into given stream.
		/// </summary>
		/// <param name="trips">Stream that the data should be written in.</param>
		public void Write(System.IO.StreamWriter trips)
		{
			trips.WriteLine(Count);

			List<Trip> sortedList = new List<Trip>(list.Values);
			sortedList.Sort();

			for (int i = 0; i < sortedList.Count; i++) // Change indices after sorting to load the data easily.
				sortedList[i].ID = i;

			foreach (var item in sortedList)
				trips.Write(item);			
                
            trips.Close();
            trips.Dispose();
		}
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<KeyValuePair<string, Trip>> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		/// <summary>
		/// Merges two collections into one.
		/// </summary>
		/// <param name="other">The other collection that should be merged.</param>
		public void MergeCollections(Trips other)
		{
			foreach (var item in other)
			{
				var trip = item.Value;
				trip.ID = Count; // Reindex the item.
				string key;
				while (list.ContainsKey(key = DataFeed.RandomString())) ; // We can index the item using some random string, since this identificator is only used while initialization. Both data are already initialized.
				list.Add(key, trip);
			}
			other = null;
		}
	}
	/// <summary>
	/// Class for trips with a specific parsing from GTFS format.
	/// </summary>
	public sealed class GtfsTrips : Trips
	{
		/// <summary>
		/// Initializes object using GTFS data feed.
		/// </summary>
		/// <param name="routesInfo">Routes Info.</param>
		/// <param name="services">Services.</param>
		/// <param name="trips">Trips.</param>
		public GtfsTrips(System.IO.StreamReader trips, Calendar services, RoutesInfo routesInfo)
        {
            // Get order of field names.
            string[] fieldNames = trips.ReadLine().Split(',');
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < fieldNames.Length; i++)
                dic.Add(fieldNames[i].Replace("\"", ""), i);

            // These fields are required for our purpose.
            if (!dic.ContainsKey("route_id")) throw new FormatException("Route ID field name missing.");
            if (!dic.ContainsKey("service_id")) throw new FormatException("Service ID field name missing.");
            if (!dic.ContainsKey("trip_id")) throw new FormatException("Trip ID field name missing.");
            if (!dic.ContainsKey("trip_headsign")) throw new FormatException("Trip headsign field name missing.");

            while (!trips.EndOfStream)
            {
                Queue<string> q = new Queue<string>(trips.ReadLine().Split(','));

                // Check if there was a comma within the quotes.

                List<string> tokens = new List<string>();

                bool quotes = false;

				while (q.Count > 0)
				{
					string entry = q.Dequeue();

					bool toBeAdded = false;

					if (quotes)
						tokens[tokens.Count - 1] += ',' + entry;
					else
						toBeAdded = true;

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

					if (toBeAdded)
						tokens.Add(entry);
					toBeAdded = false;
				}

				Trip trip = new Trip(Count, tokens[dic["trip_headsign"]], routesInfo[tokens[dic["route_id"]]], services[tokens[dic["service_id"]]]);

                list.Add(tokens[dic["trip_id"]], trip);
            }
            trips.Dispose();
        }
    }
}

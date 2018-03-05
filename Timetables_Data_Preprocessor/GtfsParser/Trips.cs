using System;
using System.Collections;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
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
            public override string ToString() => ID + ";" + RouteInfo.ID + ";" + Service.ID + ";" + Route.ID + ";" + Headsign + ";";

			public int CompareTo(Trip other) => DepartureTime.CompareTo(other.DepartureTime);

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
        public void Write(System.IO.StreamWriter trips)
        {
            List<Trip> sortedList = new List<Trip>(list.Values);
            sortedList.Sort();

            for (int i = 0; i < sortedList.Count; i++) // Change indices after sorting to load the data easily.
                sortedList[i].ID = i;

            trips.WriteLine(Count);
            foreach (var item in sortedList)
                trips.Write(item);
            trips.Close();
            trips.Dispose();
        }
        public IEnumerator<KeyValuePair<string, Trip>> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public sealed class GtfsTrips : Trips
    {
        public GtfsTrips(System.IO.StreamReader trips, Calendar services, RoutesInfo routesInfo)
        {

            // Get order of field names.
            string[] fieldNames = trips.ReadLine().Split(',');
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < fieldNames.Length; i++)
                dic.Add(fieldNames[i], i);

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

                    if (quotes)
                        tokens[tokens.Count - 1] += ',' + entry;
                    else
                        tokens.Add(entry);

                    if (entry.Length > 0 && entry[0] == '"') quotes = true; // Start of the quotes.
                    if (entry.Length > 0 && entry[entry.Length - 1] == '"') quotes = false; // End of the quotes.
                }


                string headsign = tokens[dic["trip_headsign"]];

                if (headsign[0] == '"') headsign = headsign.Substring(1, headsign.Length - 1);

                if (headsign[headsign.Length - 1] == '"') headsign = headsign.Substring(0, headsign.Length - 1);

                Trip trip = new Trip(Count, headsign, routesInfo[tokens[dic["route_id"]]], services[tokens[dic["service_id"]]]);

                list.Add(tokens[dic["trip_id"]], trip);
            }
            trips.Dispose();
        }
    }
}

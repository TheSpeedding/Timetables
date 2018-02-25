using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Preprocessor
{
    public class Trips
    {
        public class Trip
        {
            /// <summary>
            /// ID of the trip.
            /// </summary>
            public int ID { get; }
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
            public override string ToString() => ID + ";" + RouteInfo.ID + ";" + Service.ID + ";" + Headsign + ";";
            public Trip(int id, string headsign, RoutesInfo.RouteInfo routeInfo, Calendar.Service service)
            {
                ID = id;
                Headsign = headsign;
                RouteInfo = routeInfo;
                Service = service;
            }
        }
        private Dictionary<int, Trip> list = new Dictionary<int, Trip>();
        /// <summary>
        /// Gets required trip.
        /// </summary>
        /// <param name="index">Identificator of the trip.</param>
        /// <returns>Obtained trip.</returns>
        public Trip this[int index] => list[index];
        /// <summary>
        /// Gets the total number of trip.
        /// </summary>
        public int Count => list.Count;
        public Trips(System.IO.StreamReader trips, Calendar services, RoutesInfo routesInfo)
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

                Trip trip = new Trip(int.Parse(tokens[dic["trip_id"]]), headsign, routesInfo[tokens[dic["route_id"]]], services[int.Parse(tokens[dic["service_id"]])]);
                
                list.Add(int.Parse(tokens[dic["trip_id"]]), trip);
            }
        }
        public void Write(System.IO.StreamWriter trips)
        {
            trips.WriteLine(Count);
            foreach (var item in list)
                trips.Write(item.Value);
            trips.Close();
        }
    }
}

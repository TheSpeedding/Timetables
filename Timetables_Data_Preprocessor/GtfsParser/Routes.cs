using System;
using System.Collections;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Abstract class for routes collecting information about routes.
	/// </summary>
    public abstract class Routes : IEnumerable<Routes.Route>
    {
		/// <summary>
		/// Collects information about one route.
		/// </summary>
        public class Route : IEquatable<Route>
        {
            /// <summary>
            /// ID of the route.
            /// </summary>
            public int ID { internal set; get; }
            /// <summary>
            /// Basic info for the route.
            /// </summary>
            public RoutesInfo.RouteInfo RouteInfo { get; }
            /// <summary>
            /// Stops for the route in-order.
            /// </summary>
            public List<Stops.Stop> Stops { get; }
			/// <summary>
			/// Headsign of the route.
			/// </summary>
			public string Headsign { get; }
			/// <summary>
			/// Route ID, Route Info ID, Number Of Stops, Headsign.
			/// </summary>
            public override string ToString() => ID + ";" + RouteInfo.ID + ";" + Stops.Count + ";" + Headsign + ";";
			/// <summary>
			/// Compares this instance to a specified Route object and returns and indication of their relative values.
			/// </summary>
			/// <param name="other">Route to compare.</param>
			public bool Equals(Route other)
            {
				// Same routes must have the same route info ID.
                if (RouteInfo.ID != other.RouteInfo.ID) return false;

				// They also must have the same number of stops.
                if (Stops.Count != other.Stops.Count) return false;

				// Lastly, we have to check every stop one by one.
                for (int i = 0; i < Stops.Count; i++)
                    if (Stops[i].ID != other.Stops[i].ID)
                        return false;

                return true;
            }

			/// <summary>
			/// Initializes object.
			/// </summary>
			/// <param name="id">Route ID.</param>
			/// <param name="info">Route Info.</param>
			/// <param name="stops">Stops.</param>
			/// <param name="headsign">Headsign.</param>
            public Route(int id, RoutesInfo.RouteInfo info, List<Stops.Stop> stops, string headsign)
            {
                ID = id;
                RouteInfo = info;
                Stops = stops;
				Headsign = headsign;
            }
        }
        protected List<Route> list = new List<Route>();
        /// <summary>
        /// Gets the total number of routes.
        /// </summary>
        public int Count => list.Count;
		/// <summary>
		/// Writes the data into given stream.
		/// </summary>
		/// <param name="routes">Stream that the data should be written in.</param>
        public void Write(System.IO.StreamWriter routes)
        {
            routes.WriteLine(Count);
            foreach (var item in list)
                routes.Write(item);
            routes.Close();
            routes.Dispose();
        }
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
        public IEnumerator<Route> GetEnumerator() => ((IEnumerable<Route>)list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Route>)list).GetEnumerator();
		/// <summary>
		/// Merges two collections into one.
		/// </summary>
		/// <param name="other">The other collection that should be merged.</param>
		public void MergeCollections(Routes other)
		{
			foreach (var item in other)
			{
				item.ID = Count; // Reindex the item.
				list.Add(item);
			}
			other = null;
		}
	}
	/// <summary>
	/// Class for routes with a specific parsing from GTFS format.
	/// </summary>
    public sealed class GtfsRoutes : Routes
    {
		/// <summary>
		/// Initializes object using GTFS data feed.
		/// </summary>
		/// <param name="trips">Trips.</param>
		/// <param name="routesInfo">Routes Info.</param>
        public GtfsRoutes(Trips trips, RoutesInfo routesInfo)
        {
            foreach (var trip in trips)
            {
                List<Stops.Stop> stopsSequence = new List<Stops.Stop>();

                foreach (var stopTime in trip.Value.StopTimes)
                    stopsSequence.Add(stopTime.Stop);

                Route r = new Route(Count, trip.Value.RouteInfo, stopsSequence, trip.Value.Headsign);

                if (!list.Contains(r))
                    list.Add(r);
                else
                    r = list.Find(x => x.Equals(r));

                trip.Value.Route = r;
            }
        }
    }
}

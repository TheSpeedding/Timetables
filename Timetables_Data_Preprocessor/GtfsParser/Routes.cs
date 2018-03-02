using System;
using System.Collections;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
    public abstract class Routes : IEnumerable<Routes.Route>
    {
        public class Route : IEquatable<Route>
        {
            /// <summary>
            /// ID of the route.
            /// </summary>
            public int ID { get; }
            /// <summary>
            /// Basic info for the route.
            /// </summary>
            public RoutesInfo.RouteInfo RouteInfo { get; }
            /// <summary>
            /// Stops for the route in-order.
            /// </summary>
            public List<Stops.Stop> Stops { get; }
            public override string ToString() => ID + ";" + RouteInfo.ID + ";" + Stops.Count + ";";
            public bool Equals(Route other)
            {
                if (RouteInfo.ID != other.RouteInfo.ID) return false;

                if (Stops.Count != other.Stops.Count) return false;

                for (int i = 0; i < Stops.Count; i++)
                    if (Stops[i].ID != other.Stops[i].ID)
                        return false;

                return true;
            }

            public Route(int id, RoutesInfo.RouteInfo info, List<Stops.Stop> stops)
            {
                ID = id;
                RouteInfo = info;
                Stops = stops;
            }
        }
        protected List<Route> list = new List<Route>();
        /// <summary>
        /// Gets the total number of routes.
        /// </summary>
        public int Count => list.Count;
        public void Write(System.IO.StreamWriter routes)
        {
            routes.WriteLine(Count);
            foreach (var item in list)
                routes.Write(item);
            routes.Close();
            routes.Dispose();
        }
        public IEnumerator<Route> GetEnumerator() => ((IEnumerable<Route>)list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Route>)list).GetEnumerator();
    }
    public sealed class GtfsRoutes : Routes
    {
        public GtfsRoutes(Trips trips, RoutesInfo routesInfo)
        {
            foreach (var trip in trips)
            {
                List<Stops.Stop> stopsSequence = new List<Stops.Stop>();

                foreach (var stopTime in trip.Value.StopTimes)
                    stopsSequence.Add(stopTime.Stop);

                Route r = new Route(Count, trip.Value.RouteInfo, stopsSequence);

                if (!list.Contains(r))
                    list.Add(r);
                else
                    r = list.Find(x => x.Equals(r));

                trip.Value.Route = r;
            }
        }
    }
}

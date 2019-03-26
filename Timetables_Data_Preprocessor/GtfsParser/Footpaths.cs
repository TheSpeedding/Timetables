using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Static class that supplies extensions needed for footpaths.
	/// </summary>
    public static class FootpathsExtensions
    {
        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="degrees">Representation of angle in degrees.</param>
        /// <returns>Representation of angle in radians.</returns>
        public static double DegreesToRadians(this double degrees) => (Math.PI / 180) * degrees;
        /// <summary>
        /// Gets a walking time between two stops.
        /// </summary>
        /// <param name="A">First stop.</param>
        /// <param name="B">Second stop.</param>
        /// <returns>Walking time in seconds.</returns>
        public static int GetWalkingTime(this Stops.Stop A, Stops.Stop B)
		{
			var deltaLatitudeDistance = Math.Abs(Math.Abs(A.Location.Item1) - Math.Abs(B.Location.Item1)) * OneDegreeLatitudeLength; // Meters.
			var deltaLongitudeDistance = Math.Abs(Math.Abs(A.Location.Item2) - Math.Abs(B.Location.Item2)) * OneDegreeLatitudeLength; // Meters.

			return (int) (Math.Sqrt(Math.Pow(deltaLatitudeDistance, 2) + Math.Pow(deltaLongitudeDistance, 2)) / GlobalData.AverageWalkingSpeed); // Pythagorean theoream.
        }
		/// <summary>
		/// Recomputes average 1° distance for specified stops.
		/// </summary>
		public static void RecomputeAverageLatitudeAndLongitudeLength(params Stops[] stopsLists)
		{
			var averageLat = stopsLists.Average(x => x.Average(y => y.Value.Location.Item1));
			var averageLong = stopsLists.Average(x => x.Average(y => y.Value.Location.Item2));
			OneDegreeLongitudeLength = Math.Cos(averageLong.DegreesToRadians()) * 111321; // 111321 is longitude length at the equator.
		}
		/// <summary>
		/// In meters.
		/// </summary>
		private static double OneDegreeLatitudeLength { get; } = 111000; // Given by Earth properties.
		/// <summary>
		/// In meters.
		/// </summary>
		private static double OneDegreeLongitudeLength { get; set; }
    }
	/// <summary>
	/// Abstract class for footpaths collecting information about footpaths.
	/// </summary>
	public abstract class Footpaths : IEnumerable<Footpaths.Footpath>
	{
		/// <summary>
		/// Collects information about one footpath.
		/// </summary>
		public class Footpath
        {
            /// <summary>
            /// Duration in seconds between two stops.
            /// </summary>
            public int Duration { get; }
            /// <summary>
            /// First stop.
            /// </summary>
            public Stops.Stop First { get; }
            /// <summary>
            /// Second stop.
            /// </summary>
            public Stops.Stop Second { get; }
			/// <summary>
			/// Duration In Seconds, First Stop ID, Second Stop ID.
			/// </summary>
            public override string ToString() => Duration + ";" + First.ID + ";" + Second.ID + ";";
			/// <summary>
			/// Initializes object.
			/// </summary>
			/// <param name="duration">Duration in seconds.</param>
			/// <param name="first">First stop.</param>
			/// <param name="second">Second stop.</param>
            public Footpath(int duration, Stops.Stop first, Stops.Stop second)
            {
                Duration = duration;
                First = first;
                Second = second;
            }
        }
        protected List<Footpath> list = new List<Footpath>();
        /// <summary>
        /// Gets the total number of footpaths.
        /// </summary>
        public int Count => list.Count;
		/// <summary>
		/// Writes the data into given stream.
		/// </summary>
		/// <param name="footpaths">Stream that the data should be written in.</param>
		public void Write(System.IO.StreamWriter footpaths)
        {
            footpaths.WriteLine(Count);
            foreach (var item in list)
                footpaths.Write(item);
            footpaths.Close();
            footpaths.Dispose();
		}
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<Footpath> GetEnumerator() => ((IEnumerable<Footpath>)list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Footpath>)list).GetEnumerator();
		/// <summary>
		/// Merges two collections into one.
		/// </summary>
		/// <param name="other">The other collection that should be merged.</param>
		public void MergeCollections(Footpaths other, Stops stopsA, Stops stopsB)
		{
			FootpathsExtensions.RecomputeAverageLatitudeAndLongitudeLength(stopsA, stopsB);

			foreach (var footpath in other)
				list.Add(footpath);

			other = null;

			foreach (var A in stopsA)
				foreach (var B in stopsB)
				{
					int walkingTime = A.Value.GetWalkingTime(B.Value);
					if (walkingTime < GlobalData.MaximalDurationOfTransfer * 1.5 && walkingTime > 0) // While moving to another data feed, will consider only the footpaths with walking time lower than 15 mins by default.
						list.Add(new Footpath(walkingTime, A.Value, B.Value));
				}
		}
	}
	/// <summary>
	/// Class for routes with a specific parsing from GTFS format.
	/// </summary>
	public sealed class GtfsFootpaths : Footpaths
	{
		/// <summary>
		/// Initializes object using GTFS data feed.
		/// </summary>
		/// <param name="stops">Stops.</param>
		public GtfsFootpaths(Stops stops)
		{
			FootpathsExtensions.RecomputeAverageLatitudeAndLongitudeLength(stops);

			foreach (var A in stops)
                foreach (var B in stops)
                {
                    int walkingTime = A.Value.GetWalkingTime(B.Value);
                    if (walkingTime < GlobalData.MaximalDurationOfTransfer && walkingTime > 0) // We will consider only the footpaths with walking time lower than 10 mins.
					{
						// Lets check if it is a transfer from surface to underground transport. If so, triple the walking time.

						if (A.Value.ThroughgoingRoutes.Count != 0 && B.Value.ThroughgoingRoutes.Count != 0 && (
						   (A.Value.ThroughgoingRoutes[0].Type == RoutesInfo.RouteInfo.RouteType.Subway && B.Value.ThroughgoingRoutes[0].Type != RoutesInfo.RouteInfo.RouteType.Subway) ||
						   (A.Value.ThroughgoingRoutes[0].Type != RoutesInfo.RouteInfo.RouteType.Subway && B.Value.ThroughgoingRoutes[0].Type == RoutesInfo.RouteInfo.RouteType.Subway)))
						{
							if (GlobalData.CoefficientUndergroundToSurfaceTransfer * walkingTime < GlobalData.MaximalDurationOfTransfer)
								list.Add(new Footpath((int)GlobalData.CoefficientUndergroundToSurfaceTransfer * walkingTime, A.Value, B.Value));

						}

						else if (A.Value.ThroughgoingRoutes.Count != 0 && B.Value.ThroughgoingRoutes.Count != 0 &&
						         A.Value.ThroughgoingRoutes[0].Type == RoutesInfo.RouteInfo.RouteType.Subway && B.Value.ThroughgoingRoutes[0].Type == RoutesInfo.RouteInfo.RouteType.Subway)
						{
							// Transfer from subway to subway. 
														
							if (A.Value.ThroughgoingRoutes[0].ID == B.Value.ThroughgoingRoutes[0].ID)

							// Transfer to same line usually take less time, multiply it by 0.5 by default. This is because distance is measured from the beginning of the platfrom.

								list.Add(new Footpath((int)(GlobalData.CoefficientUndergroundTransfersWithinSameLine * walkingTime), A.Value, B.Value));

							else

							// Transfer between two lines usually take more time, multiply it by 2 by default.

								list.Add(new Footpath((int)(GlobalData.CoefficientUndergroundTransfersWithinDifferentLines * walkingTime), A.Value, B.Value));
						}

						else

							list.Add(new Footpath(walkingTime, A.Value, B.Value));
					}
				}
        }
    }
}

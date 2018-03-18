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
        /// Gets a walking time between two stops using Haversine formula.
        /// </summary>
        /// <param name="A">First stop.</param>
        /// <param name="B">Second stop.</param>
        /// <returns>Walking time in seconds.</returns>
        public static int GetWalkingTime(this Stops.Stop A, Stops.Stop B)
        {
            // Using Haversine formula. 
            double AlatR = A.Location.Item1.DegreesToRadians();
            double AlonR = A.Location.Item2.DegreesToRadians();
            double BlatR = B.Location.Item1.DegreesToRadians();
            double BlonR = B.Location.Item2.DegreesToRadians();
            double u = Math.Sin((BlatR - AlatR) / 2);
            double v = Math.Sin((BlonR - AlonR) / 2);
            return (int)(2.0 * 6371.0 * Math.Asin(Math.Sqrt(u * u + Math.Cos(AlatR) * Math.Cos(BlatR) * v * v)) * 1000 * 1.2);
        }
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
		public void MergeCollections(Footpaths other)
		{
			foreach (var footpath in other)
				list.Add(footpath);
			other = null;
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
            foreach (var A in stops)
                foreach (var B in stops)
                {
                    int walkingTime = A.Value.GetWalkingTime(B.Value);
                    if (walkingTime < 600 && walkingTime > 0) // We will consider only the footpaths with walking time lower than 10 mins.
						if (A.Value.ParentStation == B.Value.ParentStation)
							list.Add(new Footpath(walkingTime, A.Value, B.Value));
				}
        }
    }
}

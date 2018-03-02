using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Preprocessor
{
    public static class FootpathsExtensions
    {
        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="x">Representation of angle in degrees.</param>
        /// <returns>Representation of angle in radians.</returns>
        public static double DegreesToRadians(this double x) => (Math.PI / 180) * x;
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
            return (int)(2.0 * 6371.0 * Math.Asin(Math.Sqrt(u * u + Math.Cos(AlatR) * Math.Cos(BlatR) * v * v)) * 1000 * 2);
        }
    }
    public abstract class Footpaths : IEnumerable<Footpaths.Footpath>
    {
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
            public override string ToString() => Duration + ";" + First.ID + ";" + Second.ID + ";";
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
        public void Write(System.IO.StreamWriter footpaths)
        {
            footpaths.WriteLine(Count);
            foreach (var item in list)
                footpaths.Write(item);
            footpaths.Close();
            footpaths.Dispose();
        }
        public IEnumerator<Footpath> GetEnumerator() => ((IEnumerable<Footpath>)list).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Footpath>)list).GetEnumerator();
    }
    public sealed class GtfsFootpaths : Footpaths
    {
        public GtfsFootpaths(Stops stops)
        {
            foreach (var A in stops)
                foreach (var B in stops)
                {
                    int walkingTime = A.Value.GetWalkingTime(B.Value);
                    if (walkingTime < 600 && walkingTime > 0) // We will consider only the footpaths with walking time lower than 10 mins.
                        list.Add(new Footpath(walkingTime, A.Value, B.Value));
                }
        }
    }
}

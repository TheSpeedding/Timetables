using System;
using System.Collections;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Abstract class for stations collecting information about stations.
	/// </summary>
	public abstract class Stations : IEnumerable<Stations.Station>
    {
        public class Station
        {
            /// <summary>
            /// ID of the station.
            /// </summary>
            public int ID { internal set; get; }
            /// <summary>
            /// Name of the station.
            /// </summary>
            public string Name { get; }
			/// <summary>
			/// Station ID, Name.
			/// </summary>
			/// <returns></returns>
            public override string ToString() => ID + ";" + Name + ";";
			/// <summary>
			/// Initializes object.
			/// </summary>
			/// <param name="id">Station ID.</param>
			/// <param name="name">Name.</param>
            public Station(int id, string name)
            {
                ID = id;
                Name = name;
            }
        }
        protected List<Station> list = new List<Station>();
        /// <summary>
        /// Gets required station.
        /// </summary>
        /// <param name="index">Identificator of the station.</param>
        /// <returns>Obtained station.</returns>
        public Station this[int index] => list[index];
        /// <summary>
        /// Gets the total number of station.
        /// </summary>
        public int Count => list.Count;
		/// <summary>
		/// Writes the data into given stream.
		/// </summary>
		/// <param name="stations">Stream that the data should be written in.</param>
		public void Write(System.IO.StreamWriter stations)
		{
			stations.WriteLine(Count);
			foreach (var item in list)
				stations.Write(item);
			stations.Close();
			stations.Dispose();
		}
		/// <summary>
		/// Writes basic data into given stream.
		/// </summary>
		/// <param name="stations">Stream that the data should be written in.</param>
		public void WriteBasic(System.IO.StreamWriter stations) => Write(stations);
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<Station> GetEnumerator() => ((IEnumerable<Station>)list).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Station>)list).GetEnumerator();
		/// <summary>
		/// Merges two collections into one.
		/// </summary>
		/// <param name="other">The other collection that should be merged.</param>
		public void MergeCollections(Stations other)
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
	/// Class for stations with a specific parsing from GTFS format.
	/// </summary>
	public sealed class GtfsStations : Stations
	{
		/// <summary>
		/// Initializes object using GTFS data feed.
		/// </summary>
		/// <param name="stops">Stops</param>
		public GtfsStations(Stops stops)
        {
            var flags = new Dictionary<string, Station>();

            foreach (var stop in stops)
            {
				// Since this application is optimized for Prague, we have to union stations of type Florenc, Florenc - B, Florenc - C. Otherwise they would become three separate stations.

				string name = stop.Value.Name.Length > 4 && stop.Value.Name[stop.Value.Name.Length - 3] == '-' ? stop.Value.Name.Substring(0, stop.Value.Name.Length - 4) : stop.Value.Name;

                if (!flags.ContainsKey(name))
                {
                    var station = new Station(Count, name);
                    list.Add(station);
                    flags.Add(name, station);
                }

                stop.Value.ParentStation = flags[name];
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Abstract class for stations collecting information about stations.
	/// </summary>
	public abstract class Stations
    {
        public class Station
        {
            /// <summary>
            /// ID of the station.
            /// </summary>
            public int ID { get; }
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
                if (!flags.ContainsKey(stop.Value.Name))
                {
                    var station = new Station(Count, stop.Value.Name);
                    list.Add(station);
                    flags.Add(stop.Value.Name, station);
                }
                stop.Value.ParentStation = flags[stop.Value.Name];
            }
        }
    }
}

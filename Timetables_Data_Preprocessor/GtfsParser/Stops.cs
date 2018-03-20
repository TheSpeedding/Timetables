﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Abstract class for routes collecting information about stops.
	/// </summary>
	public abstract class Stops : IEnumerable<KeyValuePair<string, Stops.Stop>>
    {
        public class Stop
        {
            /// <summary>
            /// ID of the stop.
            /// </summary>
            public int ID { internal set; get; }
            /// <summary>
            /// Name of the stop.
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Gps coords of the stop. Latitude and longitude.
            /// </summary>
            public Tuple<double, double> Location { get; }
            /// <summary>
            /// Reference to the parent station.
            /// </summary>
            public Stations.Station ParentStation { get; internal set; } 
			/// <summary>
			/// Stop ID, Parent Station ID.
			/// </summary>
            public override string ToString() => ID + ";" + ParentStation.ID + ";";
			/// <summary>
			/// Initializes object.
			/// </summary>
			/// <param name="id">Stop ID.</param>
			/// <param name="name">Name.</param>
			/// <param name="latitude">Latitude.</param>
			/// <param name="longitude">Longitude.</param>
            public Stop(int id, string name, double latitude, double longitude)
            {
                ID = id;
                Name = name;
                Location = new Tuple<double, double>(latitude, longitude);
            }
        }
        protected Dictionary<string, Stop> list = new Dictionary<string, Stop>();
        /// <summary>
        /// Gets required stop.
        /// </summary>
        /// <param name="index">Identificator of the stop.</param>
        /// <returns>Obtained stop.</returns>
        public Stop this[string index] => list[index];
        /// <summary>
        /// Gets the total number of stops.
        /// </summary>
        public int Count => list.Count;
		/// <summary>
		/// Writes the data into given stream.
		/// </summary>
		/// <param name="stops">Stream that the data should be written in.</param>
		public void Write(System.IO.StreamWriter stops)
        {
            stops.WriteLine(Count);
            foreach (var item in list)
                stops.Write(item.Value);
            stops.Close();
            stops.Dispose();
		}
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<KeyValuePair<string, Stop>> GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		/// <summary>
		/// Merges two collections into one.
		/// </summary>
		/// <param name="other">The other collection that should be merged.</param>
		public void MergeCollections(Stops other)
		{
			foreach (var item in other)
			{
				var stop = item.Value;
				stop.ID = Count; // Reindex the item.
				string key;
				while (list.ContainsKey(key = DataFeed.RandomString())) ; // We can index the item using some random string, since this identificator is only used while initialization. Both data are already initialized.
				list.Add(key, stop);
			}
			other = null;
		}
	}
	/// <summary>
	/// Class for stops with a specific parsing from GTFS format.
	/// </summary>
	public sealed class GtfsStops : Stops
	{
		/// <summary>
		/// Initializes object using GTFS data feed.
		/// </summary>
		/// <param name="stops">Stops.</param>
		public GtfsStops(System.IO.StreamReader stops)
        {

            // Get order of field names.
            string[] fieldNames = stops.ReadLine().Split(',');
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < fieldNames.Length; i++)
                dic.Add(fieldNames[i].Replace("\"", ""), i);

            // These fields are required for our purpose.
            if (!dic.ContainsKey("stop_id")) throw new FormatException("Stop ID field name missing.");
            if (!dic.ContainsKey("stop_name")) throw new FormatException("Stop name field name missing.");
            if (!dic.ContainsKey("stop_lat")) throw new FormatException("Stop latitude field name missing.");
            if (!dic.ContainsKey("stop_lon")) throw new FormatException("Stop longitude field name missing.");
            // if (!dic.ContainsKey("location_type")) throw new FormatException("Location type field name missing."); Optional, but we "need" it (partionally).

            while (!stops.EndOfStream)
            {
				List<string> tokens = new List<string>(stops.ReadLine().SplitGtfs());

				Stop stop = new Stop(Count, tokens[dic["stop_name"]], double.Parse(tokens[dic["stop_lat"]], CultureInfo.InvariantCulture), double.Parse(tokens[dic["stop_lon"]], CultureInfo.InvariantCulture));

                list.Add(tokens[dic["stop_id"]], stop);
            }

            stops.Dispose();
        }
    }
}

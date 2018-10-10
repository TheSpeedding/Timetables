﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Timetables.Structures.Basic
{
	/// <summary>
	/// Collection of stations.
	/// </summary>
	[Serializable]
	public class StationsBasic : IEnumerable<StationsBasic.StationBasic>
	{
		/// <summary>
		/// Class collecting basic information about station.
		/// </summary>
		[Serializable]
		public class StationBasic
		{
			/// <summary>
			/// Station ID.
			/// </summary>
			public int ID { get; set; }
			/// <summary>
			/// Name of the station.
			/// </summary>
			public string Name { get; set; }
			/// <summary>
			/// Child stops.
			/// </summary>
			public List<StopsBasic.StopBasic> ChildStops { get; set; }
			/// <summary>
			/// Throughgoing routes.
			/// </summary>
			public IEnumerable<RoutesInfoBasic.RouteInfoBasic> GetThroughgoingRoutes()
			{
				foreach (var stop in ChildStops)
					foreach (var routeInfo in stop.ThroughgoingRoutes)
						yield return routeInfo;
			}
			/// <summary>
			/// Station ID, Name.
			/// </summary>
			public override string ToString() => ID + ";" + Name + ";";
			public StationBasic(int id, string name)
			{
				ChildStops = new List<StopsBasic.StopBasic>();
				ID = id;
				Name = name;
			}
		}
		public static implicit operator StationBasic[] (StationsBasic stations) => stations.Items;
		public static explicit operator string[] (StationsBasic stations) => stations.Select(s => s.Name).ToArray();
		private StationBasic[] Items { get; }
		/// <summary>
		/// Gets required station.
		/// </summary>
		/// <param name="index">Identificator of the station.</param>
		/// <returns>Obtained station.</returns>
		public StationBasic this[int index] => Items[index];
		/// <summary>
		/// Gets the total number of stations.
		/// </summary>
		public int Count => Items.Length;
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<StationBasic> GetEnumerator() => ((IEnumerable<StationBasic>)Items).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<StationBasic>)Items).GetEnumerator();
		public StationsBasic(System.IO.StreamReader sr)
		{
			var count = int.Parse(sr.ReadLine());
			Items = new StationBasic[count];
			var tokens = sr.ReadLine().Split(';'); // This could take some time but files are usually small.
			for (int i = 0; i < count; i++)
				Items[i] = new StationBasic(int.Parse(tokens[2 * i]), tokens[2 * i + 1]);
			sr.Dispose();
		}
		/// <summary>
		/// Returns collection of stations matching the pattern.
		/// </summary>
		/// <param name="name">Part of the name.</param>
		public IEnumerable<StationBasic> FindByPartOfName(string name) => from station in Items where station.Name.Contains(name) select station;
		/// <summary>
		/// Returns station represented by the name.
		/// </summary>
		/// <param name="name">Name of the station.</param>
		public StationBasic FindByName(string name) => Array.Find(Items, (StationBasic station) => StringComparer.CurrentCultureIgnoreCase.Compare(station.Name, name) == 0);
		/// <summary>
		/// Returns station represented by the index.
		/// </summary>
		/// <param name="index">Index of the station.</param>
		public StationBasic FindByIndex(int index) => this[index];
		/// <summary>
		/// Writes basic data into given stream.
		/// </summary>
		/// <param name="stations">Stream that the data should be written in.</param>
		public void WriteBasic(System.IO.StreamWriter stations)
		{
			stations.WriteLine(Count);
			foreach (var item in Items)
				stations.Write(item);
			stations.Close();
			stations.Dispose();
		}
	}
}

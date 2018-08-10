using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Timetables.Structures.Basic
{
	/// <summary>
	/// Collection of stations.
	/// </summary>
	public class StationsBasic : Preprocessor.Stations, IEnumerable<StationsBasic.StationBasic>
	{
		/// <summary>
		/// Class collecting basic information about station.
		/// </summary>
		public class StationBasic
		{
			/// <summary>
			/// Station ID.
			/// </summary>
			public uint ID { get; }
			/// <summary>
			/// Name of the station.
			/// </summary>
			public string Name { get; }
			/// <summary>
			/// Child stops.
			/// </summary>
			public List<StopsBasic.StopBasic> ChildStops { get; }
			/// <summary>
			/// Throughgoing routes.
			/// </summary>
			public IEnumerable<RoutesInfoBasic.RouteInfoBasic> GetThroughgoingRoutes()
			{
				foreach (var stop in ChildStops)
					foreach (var routeInfo in stop.ThroughgoingRoutes)
						yield return routeInfo;
			}
			public override string ToString() => Name;
			public StationBasic(uint id, string name)
			{
				ChildStops = new List<StopsBasic.StopBasic>();
				ID = id;
				Name = name;
			}
		}
		private new List<StationBasic> list = new List<StationBasic>();
		/// <summary>
		/// Gets required station.
		/// </summary>
		/// <param name="index">Identificator of the station.</param>
		/// <returns>Obtained station.</returns>
		public new StationBasic this[int index] => list[index];
		/// <summary>
		/// Gets the total number of stations.
		/// </summary>
		public new int Count => list.Count;
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public new IEnumerator<StationBasic> GetEnumerator() => ((IEnumerable<StationBasic>)list).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<StationBasic>)list).GetEnumerator();
		public StationsBasic(System.IO.StreamReader sr)
		{
			var count = int.Parse(sr.ReadLine());
			var tokens = sr.ReadLine().Split(';'); // This could take some time but files are usually small.
			for (int i = 0; i < count; i++)
				list.Add(new StationBasic((uint)Count, tokens[2 * i + 1]));
		}
		/// <summary>
		/// Returns collection of stations matching the pattern.
		/// </summary>
		/// <param name="name">Part of the name.</param>
		public IEnumerable<StationBasic> FindByPartOfName(string name) => from station in list where station.Name.Contains(name) select station;
		/// <summary>
		/// Returns station represented by the name.
		/// </summary>
		/// <param name="name">Name of the station.</param>
		public StationBasic FindByName(string name) => list.Find((StationBasic station) => StringComparer.CurrentCultureIgnoreCase.Compare(station.Name, name) == 0);
		/// <summary>
		/// Returns station represented by the index.
		/// </summary>
		/// <param name="index">Index of the station.</param>
		public StationBasic FindByIndex(int index) => this[index];
	}
}

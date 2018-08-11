using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Timetables.Structures.Basic
{
	/// <summary>
	/// Collection of stops.
	/// </summary>
	[Serializable]
	public class StopsBasic : IEnumerable<StopsBasic.StopBasic>
	{
		/// <summary>
		/// Class collecting basic information about stop.
		/// </summary>
		[Serializable]
		public class StopBasic
		{
			/// <summary>
			/// Stop ID.
			/// </summary>
			public uint ID { get; set; }
			/// <summary>
			/// Parent station.
			/// </summary>
			public StationsBasic.StationBasic ParentStation { get; }
			/// <summary>
			/// Name of the stop.
			/// </summary>
			public string Name { get { return ParentStation.Name; } }
			/// <summary>
			/// Latitude of the stop.
			/// </summary>
			public double Latitude { get; set; }
			/// <summary>
			/// Longitude of the stop.
			/// </summary>
			public double Longitude { get; set; }
			/// <summary>
			/// List of all routes that goes through this stop.
			/// </summary>
			public List<RoutesInfoBasic.RouteInfoBasic> ThroughgoingRoutes { get; set; }
			public StopBasic(StationsBasic.StationBasic station, double lat, double lon, List<RoutesInfoBasic.RouteInfoBasic> routes)
			{
				ParentStation = station;
				Latitude = lat;
				Longitude = lon;
				ThroughgoingRoutes = routes;
			}
			/// <summary>
			/// Stop ID, Parent Station ID, Latitude, Longitude, List Of All Throughgoing Routes (separated by a backtick).
			/// </summary>
			public override string ToString()
			{
				System.Text.StringBuilder result = new System.Text.StringBuilder();
				result.Append(ID + ";" + ParentStation.ID + ";" + Latitude.ToString(CultureInfo.InvariantCulture) + ";" + Longitude.ToString(CultureInfo.InvariantCulture) + ";");

				foreach (var route in ThroughgoingRoutes)
					result.Append(route.ID + "`");

				result.Append(";");
				return result.ToString();
			}
		}
		private List<StopBasic> list = new List<StopBasic>();
		/// <summary>
		/// Gets required stop.
		/// </summary>
		/// <param name="index">Identificator of the stop.</param>
		/// <returns>Obtained stop.</returns>
		public StopBasic this[int index] => list[index];
		/// <summary>
		/// Gets the total number of stops.
		/// </summary>
		public int Count => list.Count;
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<StopBasic> GetEnumerator() => ((IEnumerable<StopBasic>)list).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<StopBasic>)list).GetEnumerator();
		public StopsBasic(System.IO.StreamReader sr, StationsBasic stations, RoutesInfoBasic routes)
		{
			var count = int.Parse(sr.ReadLine());
			var tokens = sr.ReadLine().Split(';'); // This could take some time but files are usually small.
			for (int i = 0; i < count; i ++)
			{
				var r = new List<RoutesInfoBasic.RouteInfoBasic>();
				foreach (var route in tokens[5 * i + 4].Split('`'))
					if (route != "")
						r.Add(routes[int.Parse(route)]);
				var stop = new StopBasic(stations[int.Parse(tokens[5 * i + 1])], double.Parse(tokens[5 * i + 2], System.Globalization.CultureInfo.InvariantCulture), double.Parse(tokens[5 * i + 3], System.Globalization.CultureInfo.InvariantCulture), r);
				list.Add(stop);
				stations[int.Parse(tokens[5 * i + 1])].ChildStops.Add(stop);
			}
		}
		/// <summary>
		/// Returns stop represented by the name.
		/// </summary>
		/// <param name="name">Name of the stop.</param>
		public StopBasic FindByName(string name) => list.Find((StopBasic station) => StringComparer.CurrentCultureIgnoreCase.Compare(station.Name, name) == 0);
		/// <summary>
		/// Finds stop by its coordinates.
		/// </summary>
		/// <param name="lat">Latitude.</param>
		/// <param name="lon">Longitude.</param>
		public StopBasic FindByCoordinates(double lat, double lon) => list.Find((StopBasic s) => { return s.Latitude == lat && s.Longitude == lon; });
		/// <summary>
		/// Returns stop represented by the index.
		/// </summary>
		/// <param name="index">Index of the stop.</param>
		public StopBasic FindByIndex(uint index) => this[(int)index];
		/// <summary>
		/// Writes basic data into given stream.
		/// </summary>
		/// <param name="stops">Stream that the data should be written in.</param>
		public void WriteBasic(System.IO.StreamWriter stops)
		{
			stops.WriteLine(Count);
			foreach (var item in list)
				stops.Write(item);
			stops.Close();
			stops.Dispose();
		}
	}
}

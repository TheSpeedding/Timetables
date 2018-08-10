using System;
using System.IO;

namespace Timetables.Structures.Basic
{
	/// <summary>
	/// Basic data feed class.
	/// </summary>
	[Serializable]
	public class DataFeedBasic
	{
		public StationsBasic Stations { get; set; }
		public RoutesInfoBasic RoutesInfo { get; set; }
		public StopsBasic Stops { get; set; }
		public DataFeedBasic(string path)
		{
			Stations = new StationsBasic(new StreamReader(path + "/stations.tfb"));
			RoutesInfo = new RoutesInfoBasic(new StreamReader(path + "/routes_info.tfb"));
			Stops = new StopsBasic(new StreamReader(path + "/stops.tfb"), Stations, RoutesInfo);
		}
		public DataFeedBasic() : this("basic") { }
		/// <summary>
		/// Saves the date into desired folder.
		/// <paramref name="folder">Desired folder.</paramref>
		/// </summary>
		public void Save(string folder = "basic")
		{
			if (Directory.Exists(folder))
				Directory.Delete(folder, true);
			Directory.CreateDirectory(folder);

			RoutesInfo.WriteBasic(new StreamWriter(folder + "/routes_info.tfb"));
			Stops.WriteBasic(new StreamWriter(folder + "/stops.tfb"));
			Stations.WriteBasic(new StreamWriter(folder + "/stations.tfb"));
		}
	}
}

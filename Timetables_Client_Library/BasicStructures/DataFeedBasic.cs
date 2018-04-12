using System;
using System.IO;

namespace Timetables.Structures.Basic
{
	public class DataFeedBasic
	{
		public StationsBasic Stations { get; }
		public RoutesInfoBasic RoutesInfo { get; }
		public StopsBasic Stops { get; }
		public DataFeedBasic(string path)
		{
			Stations = new StationsBasic(new StreamReader(path + "/stations.tfb"));
			RoutesInfo = new RoutesInfoBasic(new StreamReader(path + "/routes_info.tfb"));
			Stops = new StopsBasic(new StreamReader(path + "/stops.tfb"), Stations, RoutesInfo);
		}
		public DataFeedBasic() : this("basic") { }
	}
}

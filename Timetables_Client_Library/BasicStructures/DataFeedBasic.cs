using System;
using System.IO;

namespace Timetables.Structures.Basic
{
	/// <summary>
	/// Request for download of the basic data feed.
	/// </summary>
	[Serializable]
	public class DataFeedBasicRequest
	{
		/// <summary>
		/// Version of the data feed at the client side.
		/// </summary>
		public string Version { get; set; }
		public DataFeedBasicRequest(string version) => Version = version;
		public DataFeedBasicRequest() => Version = "ForceDownload";
	}
	/// <summary>
	/// Response from the server to the DataFeedBasicRequest.
	/// </summary>
	[Serializable]
	public class DataFeedBasicResponse
	{
		/// <summary>
		/// Version of the data feed at the server side. The client should refresh this.
		/// </summary>
		public string Version { get; set; }
		/// <summary>
		/// Basic data.
		/// </summary>
		public DataFeedBasic Data { get; set; }
		/// <summary>
		/// Indicated whether the client should update the data or not.
		/// </summary>
		public bool ShouldBeUpdated => Data != null;
	}

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
		/// Saves the data into desired folder.
		/// </summary>
		/// <param name="folder">Desired folder.</param>
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

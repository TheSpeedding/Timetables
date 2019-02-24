using System;
using System.IO;
using System.Linq;

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
		public override string ToString() => $"Version: { Version }.";
	}
	/// <summary>
	/// Response from the server to the DataFeedBasicRequest.
	/// </summary>
	[Serializable]
	public class DataFeedBasicResponse
	{
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
		public string Version { get; set; }
		public DataFeedBasic(string path = "./")
		{
			Stations = new StationsBasic(new StreamReader(path + "basic/stations.tfb"));
			RoutesInfo = new RoutesInfoBasic(new StreamReader(path + "basic/routes_info.tfb"));
			Stops = new StopsBasic(new StreamReader(path + "basic/stops.tfb"), Stations, RoutesInfo);
			using (var sr = new System.IO.StreamReader(path + "basic/.version"))
				Version = sr.ReadLine();
		}
		/// <summary>
		/// Saves the data into desired folder.
		/// </summary>
		/// <param name="basePath">Base path to the folder.</param>
		/// <param name="folder">Desired folder.</param>
		public void Save(string basePath = "./")
		{
			if (Directory.Exists(basePath + "basic"))
				Directory.Delete(basePath + "/basic", true);
			Directory.CreateDirectory(basePath + "/basic");

			RoutesInfo.WriteBasic(new StreamWriter(basePath + "basic/routes_info.tfb"));
			Stops.WriteBasic(new StreamWriter(basePath + "basic/stops.tfb"));
			Stations.WriteBasic(new StreamWriter(basePath + "basic/stations.tfb"));
			using (var sw = new StreamWriter(basePath + "basic/.version"))
				sw.WriteLine(Version);
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Timetables.Structures.Basic;

namespace Timetables.Client
{
	public abstract class CachedData<T>
	{
		/// <summary>
		/// Directory where the cached files are saved.
		/// </summary>
		protected static string cachedFilesDirectory = "cached/";
		/// <summary>
		/// Path to the file with cached data.
		/// </summary>
		protected string pathToFile;
		/// <summary>
		/// Parses the tokens from the filename.
		/// </summary>
		protected static IEnumerable<int> GetTokensFromFileName(string fileName) => fileName.Split('-').Skip(1).Select(x => int.Parse(x));
		/// <summary>
		/// Loads cached journeys from the file.
		/// </summary>
		/// <returns>Requested journeys.</returns>
		public T LoadResults()
		{
			using (FileStream fileStream = new FileStream(pathToFile, FileMode.Open))
				return (T)new XmlSerializer(typeof(T)).Deserialize(fileStream);
		}
		/// <summary>
		/// Caches the results to the specified path and returns true on success.
		/// </summary>
		protected static bool SaveResults(T res, string path)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(path))
					new XmlSerializer(typeof(T)).Serialize(sw, res);
			}

			catch
			{
				return false;
			}

			return true;
		}
	}

	public class StationInfoCached : CachedData<DepartureBoardResponse>
	{
		private static string stationInfoPrefix = "si";
		private static readonly Regex stationInfoFilePattern = new Regex($@"{ stationInfoPrefix }-[0-9]+\.fav"); // Format: "si-<station id>.fav"
		/// <summary>
		/// Station of cached departures.
		/// </summary>
		public StationsBasic.StationBasic Station { get; }
		public StationInfoCached(int stationId, string path)
		{
			Station = DataFeedClient.Basic.Stations.FindByIndex(stationId);
			pathToFile = path;
		}
		/// <summary>
		/// Enumerates files in specified directory and returns a collection of cached data.
		/// </summary>
		public static IEnumerable<StationInfoCached> FetchStationInfoData(string directoryPath) =>
			Directory.EnumerateFiles(directoryPath).Where(x => stationInfoFilePattern.IsMatch(x)).Select(x => new StationInfoCached(GetTokensFromFileName(x).ElementAt(0), x));
		/// <summary>
		/// Saves results as expected.
		/// </summary>
		public static bool SaveResults(DepartureBoardResponse res) =>
			SaveResults(res, cachedFilesDirectory + stationInfoPrefix + "-" + DataFeedClient.Basic.Stops.FindByIndex(res.Departures[0].StopID).ParentStation.ID + ".fav");
		/// <summary>
		/// Constructs new request so it is ready to be updated.
		/// </summary>
		public StationInfoRequest ConstructNewRequest() => new StationInfoRequest(Station.ID, DateTime.Now, DateTime.Now.Add(DataFeedClient.CachingTime), true);
	}

	public class LineInfoCached : CachedData<DepartureBoardResponse>
	{
		private static string lineInfoPrefix = "li";
		private static readonly Regex lineInfoFilePattern = new Regex($@"{ lineInfoPrefix }-[0-9]+\.fav"); // Format: "li-<route info id>-fav"
		/// <summary>
		/// Cached line.
		/// </summary>
		public RoutesInfoBasic.RouteInfoBasic Route { get; }
		public LineInfoCached(int routeInfoId, string path)
		{
			Route = DataFeedClient.Basic.RoutesInfo.FindByIndex(routeInfoId);
			pathToFile = path;
		}
		/// <summary>
		/// Enumerates files in specified directory and returns a collection of cached data.
		/// </summary>
		public static IEnumerable<LineInfoCached> FetchLineInfoData(string directoryPath) =>
			Directory.EnumerateFiles(directoryPath).Where(x => lineInfoFilePattern.IsMatch(x)).Select(x => new LineInfoCached(GetTokensFromFileName(x).ElementAt(0), x));
		/// <summary>
		/// Saves results as expected.
		/// </summary>
		public static bool SaveResults(DepartureBoardResponse res) =>
			SaveResults(res, cachedFilesDirectory + lineInfoPrefix + "-" + DataFeedClient.Basic.RoutesInfo.FindByLabel(res.Departures[0].LineLabel).ID + ".fav");
		/// <summary>
		/// Constructs new request so it is ready to be updated.
		/// </summary>
		public LineInfoRequest ConstructNewRequest() => new LineInfoRequest(DateTime.Now, DateTime.Now.Add(DataFeedClient.CachingTime), Route.ID);
	}

	public class JourneyCached : CachedData<RouterResponse>
	{
		private static string journeyPrefix = "jo";
		private static readonly Regex journeyFilePattern = new Regex($@"{ journeyPrefix }-[0-9]+-[0-9]+\.fav"); // Format: "jo-<source station id>-<target station id>.fav"
		/// <summary>
		/// Source station of cached journey.
		/// </summary>
		public StationsBasic.StationBasic SourceStation { get; }
		/// <summary>
		/// Target station of cached journey.
		/// </summary>
		public StationsBasic.StationBasic TargetStation { get; }
		public JourneyCached(int sourceStationId, int targetStationId, string path)
		{
			SourceStation = DataFeedClient.Basic.Stations.FindByIndex(sourceStationId);
			TargetStation = DataFeedClient.Basic.Stations.FindByIndex(targetStationId);
			pathToFile = path;
		}
		/// <summary>
		/// Enumerates files in specified directory and returns a collection of cached data.
		/// </summary>
		public static IEnumerable<JourneyCached> FetchJourneyData(string directoryPath) =>
			Directory.EnumerateFiles(directoryPath).Where(x => journeyFilePattern.IsMatch(x)).Select(x =>
			{
				var tokens = GetTokensFromFileName(x);
				return new JourneyCached(tokens.ElementAt(0), tokens.ElementAt(1), x);
			});
		/// <summary>
		/// Saves results as expected.
		/// </summary>
		public static bool SaveResults(RouterResponse res) =>
			SaveResults(res, cachedFilesDirectory + journeyPrefix + "-" + res.Journeys[0].JourneySegments.First().SourceStopID + "-" + res.Journeys[0].JourneySegments.Last().TargetStopID + ".fav");
	}
}

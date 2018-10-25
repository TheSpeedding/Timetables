using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Timetables.Structures.Basic;

namespace Timetables.Client
{
	/// <summary>
	/// Base class for cached data.
	/// </summary>
	/// <typeparam name="Res">Data type to cache.</typeparam>
	/// <typeparam name="Req">Data type to create new request for update.</typeparam>
	public abstract class CachedData<Res, Req> where Res : ResponseBase where Req : RequestBase
	{
		/// <summary>
		/// Directory where the cached files are saved.
		/// </summary>
		protected static readonly string cachedFilesDirectory = "cached/";
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
		public Res LoadResults()
		{
			using (FileStream fileStream = new FileStream(cachedFilesDirectory + pathToFile, FileMode.Open))
				return (Res)new XmlSerializer(typeof(Res)).Deserialize(fileStream);
		}
		/// <summary>
		/// Decides whether the cached data should be updated or not yet.
		/// </summary>
		public bool ShouldBeUpdated => DateTime.Parse(XDocument.Load(pathToFile).Element("CreatedAt")?.Value).Add(DataFeedClient.TimeToCacheFor).Subtract(DataFeedClient.TimeToUpdateCachedBeforeExpiration) >= DateTime.Now;
		/// <summary>
		/// Caches the results to the specified path.
		/// </summary>
		protected static void SaveResults(Res res, string path)
		{
			using (StreamWriter sw = new StreamWriter(path))
				new XmlSerializer(typeof(Res)).Serialize(sw, res);
		}
		/// <summary>
		/// Constructs new request so it is ready to be updated.
		/// </summary>
		public abstract Req ConstructNewRequest();
		static CachedData()
		{
			if (!Directory.Exists(cachedFilesDirectory))
				Directory.CreateDirectory(cachedFilesDirectory);
		}
	}

	public class StationInfoCached : CachedData<DepartureBoardResponse, StationInfoRequest>
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
		public override string ToString() => Station.Name;
		/// <summary>
		/// Enumerates files in specified directory and returns a collection of cached data.
		/// </summary>
		public static IEnumerable<StationInfoCached> FetchStationInfoData() =>
			Directory.EnumerateFiles(cachedFilesDirectory).Where(x => stationInfoFilePattern.IsMatch(x)).Select(x => new StationInfoCached(GetTokensFromFileName(x).ElementAt(0), x));
		/// <summary>
		/// Saves results as expected.
		/// </summary>
		public static bool SaveResults(DepartureBoardResponse res)
		{
			try
			{
				SaveResults(res, cachedFilesDirectory + stationInfoPrefix + "-" + DataFeedClient.Basic.Stops.FindByIndex(res.Departures[0].StopID).ParentStation.ID + ".fav");
			}

			catch
			{
				return false;
			}

			return true;
		} 
		/// <summary>
		/// Constructs new request so it is ready to be updated.
		/// </summary>
		public override StationInfoRequest ConstructNewRequest() => new StationInfoRequest(Station.ID, DateTime.Now, DateTime.Now.Add(DataFeedClient.TimeToCacheFor), true);
	}

	public class LineInfoCached : CachedData<DepartureBoardResponse, LineInfoRequest>
	{
		private static string lineInfoPrefix = "li";
		private static readonly Regex lineInfoFilePattern = new Regex($@"{ lineInfoPrefix }-[0-9]+\.fav"); // Format: "li-<route info id>.fav"
		/// <summary>
		/// Cached line.
		/// </summary>
		public RoutesInfoBasic.RouteInfoBasic Route { get; }
		public LineInfoCached(int routeInfoId, string path)
		{
			Route = DataFeedClient.Basic.RoutesInfo.FindByIndex(routeInfoId);
			pathToFile = path;
		}
		public override string ToString() => Route.Label;
		/// <summary>
		/// Enumerates files in specified directory and returns a collection of cached data.
		/// </summary>
		public static IEnumerable<LineInfoCached> FetchLineInfoData() =>
			Directory.EnumerateFiles(cachedFilesDirectory).Where(x => lineInfoFilePattern.IsMatch(x)).Select(x => new LineInfoCached(GetTokensFromFileName(x).ElementAt(0), x));
		/// <summary>
		/// Saves results as expected.
		/// </summary>
		public static bool SaveResults(DepartureBoardResponse res)
		{
			try
			{
				SaveResults(res, cachedFilesDirectory + lineInfoPrefix + "-" + DataFeedClient.Basic.RoutesInfo.FindByLabel(res.Departures[0].LineLabel).ID + ".fav");
			}

			catch
			{
				return false;
			}

			return true;
		}			
		/// <summary>
		/// Constructs new request so it is ready to be updated.
		/// </summary>
		public override LineInfoRequest ConstructNewRequest() => new LineInfoRequest(DateTime.Now, DateTime.Now.Add(DataFeedClient.TimeToCacheFor), Route.ID);
	}

	public class JourneyCached : CachedData<RouterResponse, RouterRequest>
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
		public override string ToString() => $"{ SourceStation.Name } - { TargetStation }";
		/// <summary>
		/// Enumerates files in specified directory and returns a collection of cached data.
		/// </summary>
		public static IEnumerable<JourneyCached> FetchJourneyData() =>
			Directory.EnumerateFiles(cachedFilesDirectory).Where(x => journeyFilePattern.IsMatch(x)).Select(x =>
			{
				var tokens = GetTokensFromFileName(x);
				return new JourneyCached(tokens.ElementAt(0), tokens.ElementAt(1), x);
			});
		/// <summary>
		/// Saves results as expected.
		/// </summary>
		public static bool SaveResults(RouterResponse res)
		{
			try
			{
				SaveResults(res, cachedFilesDirectory + journeyPrefix + "-" + res.Journeys[0].JourneySegments.First().SourceStopID + "-" + res.Journeys[0].JourneySegments.Last().TargetStopID + ".fav");
			}

			catch
			{
				return false;
			}

			return true;
		}			
		/// <summary>
		/// Constructs new request so it is ready to be updated.
		/// </summary>
		public override RouterRequest ConstructNewRequest() => new RouterRequest(SourceStation.ID, TargetStation.ID, DateTime.Now, 100, DateTime.Now.Add(DataFeedClient.TimeToCacheFor), 1.0, (MeanOfTransport)255);
	}
}

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
		protected static IEnumerable<int> GetTokensFromFileName(string fileName) => fileName.Split('.')[0].Split('-').Skip(1).Select(x => int.Parse(x));
		/// <summary>
		/// Loads cached data from the file.
		/// </summary>
		/// <returns>Requested journeys.</returns>
		public Res LoadResults()
		{
			using (FileStream fileStream = new FileStream(cachedFilesDirectory + pathToFile, FileMode.Open))
				return (Res)new XmlSerializer(typeof(Res)).Deserialize(fileStream);
		}
		/// <summary>
		/// Remove this item from the favorites list.
		/// </summary>
		public void Remove() => File.Delete(pathToFile);
		/// <summary>
		/// Decides whether the cached data should be updated or not yet.
		/// </summary>
		public bool ShouldBeUpdated => DateTime.Parse(XDocument.Load(pathToFile).Descendants("CreatedAt").First().Value).Add(DataFeedClient.TimeToCacheFor).Subtract(DataFeedClient.TimeToUpdateCachedBeforeExpiration) >= DateTime.Now;
		/// <summary>
		/// Caches the results to the specified path.
		/// </summary>
		protected static void CacheResults(Res res, string path)
		{
			using (StreamWriter sw = new StreamWriter(path))
				new XmlSerializer(typeof(Res)).Serialize(sw, res);
		}
		/// <summary>
		/// Constructs new request so it is ready to be updated.
		/// </summary>
		public abstract Req ConstructNewRequest();
		/// <summary>
		/// Find the data that satisfies given request.
		/// </summary>
		public abstract Res FindResultsSatisfyingRequest(Req request);
		static CachedData()
		{
			if (!Directory.Exists(cachedFilesDirectory))
				Directory.CreateDirectory(cachedFilesDirectory);
		}
	}

	public sealed class StationInfoCached : CachedData<DepartureBoardResponse, StationInfoRequest>
	{
		private static string stationInfoPrefix = "si";
		private static readonly Regex stationInfoFilePattern = new Regex($@"{ stationInfoPrefix }-[0-9]+\.fav"); // Format: "si-<station id>.fav"
		/// <summary>
		/// Station of cached departures.
		/// </summary>
		public StationsBasic.StationBasic Station { get; }
		public StationInfoCached(int stationId)
		{
			Station = DataFeedClient.Basic.Stations.FindByIndex(stationId);
			CacheResults(Station, new DepartureBoardResponse(new List<Departure>()));
		}
		private StationInfoCached(int stationId, string path)
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
		/// Selects desired cached data, if exists. Otherwise returns null.
		/// </summary>
		public static StationInfoCached Select(StationsBasic.StationBasic station) => FetchStationInfoData().FirstOrDefault(x => x.Station == station);
		public static StationInfoCached Select(int id) => FetchStationInfoData().FirstOrDefault(x => x.Station.ID == id);
		/// <summary>
		/// Saves results as expected.
		/// </summary>
		public static bool CacheResults(StationsBasic.StationBasic station, DepartureBoardResponse res)
		{
			try
			{
				CacheResults(res, cachedFilesDirectory + stationInfoPrefix + "-" + station.ID + ".fav");
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
		public override DepartureBoardResponse FindResultsSatisfyingRequest(StationInfoRequest request)
		{
			var results = LoadResults().Departures.SkipWhile(x => RequestBase.ConvertDateTimeToUnixTimestamp(x.DepartureDateTime) > request.MaximalArrivalDateTime);

			if (request.RouteInfoID != -1)
			{
				var routeInfo = DataFeedClient.Basic.RoutesInfo.FindByIndex(request.RouteInfoID);
				results = results.Where(x => x.LineLabel == routeInfo.Label);
			}

			return new DepartureBoardResponse(results.Take(request.Count).ToList());
		}
	}

	public sealed class LineInfoCached : CachedData<DepartureBoardResponse, LineInfoRequest>
	{
		private static string lineInfoPrefix = "li";
		private static readonly Regex lineInfoFilePattern = new Regex($@"{ lineInfoPrefix }-[0-9]+\.fav"); // Format: "li-<route info id>.fav"
		/// <summary>
		/// Cached line.
		/// </summary>
		public RoutesInfoBasic.RouteInfoBasic Route { get; }
		public LineInfoCached(int routeInfoId)
		{
			Route = DataFeedClient.Basic.RoutesInfo.FindByIndex(routeInfoId);
			CacheResults(Route, new DepartureBoardResponse(new List<Departure>()));
		}
		private LineInfoCached(int routeInfoId, string path)
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
		/// Selects desired cached data, if exists. Otherwise returns null.
		/// </summary>
		public static LineInfoCached Select(RoutesInfoBasic.RouteInfoBasic route) => FetchLineInfoData().FirstOrDefault(x => x.Route == route);
		public static LineInfoCached Select(int id) => FetchLineInfoData().FirstOrDefault(x => x.Route.ID == id);
		/// <summary>
		/// Saves results as expected.
		/// </summary>
		public static bool CacheResults(RoutesInfoBasic.RouteInfoBasic route, DepartureBoardResponse res)
		{
			try
			{
				CacheResults(res, cachedFilesDirectory + lineInfoPrefix + "-" + route.ID + ".fav");
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
		public override DepartureBoardResponse FindResultsSatisfyingRequest(LineInfoRequest request) =>
			new DepartureBoardResponse(LoadResults().Departures.
				SkipWhile(x => RequestBase.ConvertDateTimeToUnixTimestamp(x.DepartureDateTime) > request.MaximalArrivalDateTime).
				Take(request.Count).ToList());
	}

	public sealed class JourneyCached : CachedData<RouterResponse, RouterRequest>
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
		public JourneyCached(int sourceStationId, int targetStationId)
		{
			SourceStation = DataFeedClient.Basic.Stations.FindByIndex(sourceStationId);
			TargetStation = DataFeedClient.Basic.Stations.FindByIndex(targetStationId);
			CacheResults(SourceStation, TargetStation, new RouterResponse(new List<Journey>()));
		}
		private JourneyCached(int sourceStationId, int targetStationId, string path)
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
		/// Selects desired cached data, if exists. Otherwise returns null.
		/// </summary>
		public static JourneyCached Select(StationsBasic.StationBasic source, StationsBasic.StationBasic target) => FetchJourneyData().FirstOrDefault(x => x.SourceStation == source && x.TargetStation == target);
		public static JourneyCached Select(int sourceID, int targetID) => FetchJourneyData().FirstOrDefault(x => x.SourceStation.ID == sourceID && x.TargetStation.ID == targetID);
		/// <summary>
		/// Saves results as expected.
		/// </summary>
		public static bool CacheResults(StationsBasic.StationBasic source, StationsBasic.StationBasic target, RouterResponse res)
		{
			try
			{
				CacheResults(res, cachedFilesDirectory + journeyPrefix + "-" + source.ID + "-" + target.ID + ".fav");
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
		public override RouterResponse FindResultsSatisfyingRequest(RouterRequest request)
		{
			List<Journey> results = new List<Journey>();

			foreach (var journey in LoadResults().Journeys.
				SkipWhile(x => RequestBase.ConvertDateTimeToUnixTimestamp(x.DepartureDateTime) > request.MaximalArrivalDateTime).
				Where(x => x.TransfersCount <= request.MaxTransfers))
			{
				bool isSuitable = true;

				foreach (var js in journey.JourneySegments)
				{
					if (js is TripSegment && (((TripSegment)js).MeanOfTransport & request.MeansOfTransport) == 0)// This journey contains mean of transport which is not in the request.
					{
						isSuitable = false;
						break;
					}
				}

				if (isSuitable)
					results.Add(journey);

				if (results.Count == request.Count)
					break;
			}

			return new RouterResponse(results);
		}
	}
}

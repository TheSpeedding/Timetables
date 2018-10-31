// All of these methods must be in this project, because of localization settings and global settings (timeout duration etc.). Requests cannot be in Client library, since they need some native methods (C++/CLI), unreachable from mobile app.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timetables.Client;

namespace Timetables.Application.Desktop
{
	/// <summary>
	/// Class that supplies methods to send requests.
	/// </summary>
	internal static class Requests
	{
		/// <summary>
		/// Gets everything ready for textbox auto-completion.
		/// </summary>
		/// <param name="tb">Textbox.</param>
		/// <param name="content">Content.</param>
		internal static void AutoCompleteTextBox(TextBox tb, string[] content)
		{
			tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
			tb.AutoCompleteCustomSource.AddRange(content);
			tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		}
		/// <summary>
		/// Returns station object with given name. If not found, returns error message within MessageBox.
		/// </summary>
		/// <param name="name">Name of the station.</param>
		/// <returns>Station object.</returns>
		public static Structures.Basic.StationsBasic.StationBasic GetStationFromString(string name)
		{
			Structures.Basic.StationsBasic.StationBasic source = DataFeedDesktop.Basic.Stations.FindByName(name);

			if (source == null)
				MessageBox.Show(Settings.Localization.UnableToFindStation + ": " + name, Settings.Localization.StationNotFound, MessageBoxButtons.OK, MessageBoxIcon.Warning);

			return source;
		}
		/// <summary>
		/// Returns route info object with given name. If not found, returns error message within MessageBox.	
		/// </summary>
		/// <param name="label">Route label.</param>
		/// <returns>Route info object.</returns>
		public static Structures.Basic.RoutesInfoBasic.RouteInfoBasic GetRouteInfoFromLabel(string label)
		{
			if (string.IsNullOrEmpty(label))
				return null;

			Structures.Basic.RoutesInfoBasic.RouteInfoBasic route = DataFeedDesktop.Basic.RoutesInfo.FindByLabel(label);

			if (route == null)
				MessageBox.Show(Settings.Localization.UnableToFindRouteInfo + ": " + label, Settings.Localization.RouteInfoNotFound, MessageBoxButtons.OK, MessageBoxIcon.Warning);

			return route;
		}
		/// <summary>
		/// Returns true if everything is OK.
		/// </summary>
		public static async Task<bool> CheckBasicDataValidity()
		{
			if (!DataFeedDesktop.OfflineMode)
				try
				{
					await DataFeedClient.DownloadAsync(true); // Checks basic data validity.
				}
				catch (System.Net.WebException)
				{
					MessageBox.Show(Settings.Localization.UnreachableHost, Settings.Localization.Offline, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			return true;
		}
		/// <summary>
		/// Tries to obtain journeys and returns window with results.
		/// </summary>
		/// <param name="sourceName">Source station name.</param>
		/// <param name="targetName">Target station name.</param>
		/// <param name="dt">Datetime.</param>
		/// <param name="transfers">Max transfers.</param>
		/// <param name="count">Number of journeys.</param>
		/// <param name="coefficient">Coefficient for the footpaths.</param>
		/// <param name="mot">Mean of transport.</param>
		/// <param name="win">Window with request.</param>
		/// <param name="comp">Comparer for journeys.</param>
		/// <returns>Window with results</returns>
		public static async Task<JourneyResultsWindow> GetRouterWindowAsync(string sourceName, string targetName, DateTime dt, int transfers, int count, double coefficient, MeanOfTransport mot, NewJourneyWindow win, IComparer<Journey> comp = null)
		{
			if (!await CheckBasicDataValidity()) return null;

			Structures.Basic.StationsBasic.StationBasic source = GetStationFromString(sourceName);
			Structures.Basic.StationsBasic.StationBasic target = GetStationFromString(targetName);

			if (source == null || target == null) return null;

			var routerRequest = new RouterRequest(source.ID, target.ID, dt, transfers, count, coefficient, mot);
			var routerResponse = await SendRouterRequestAsync(routerRequest);

			if (comp != null && routerResponse != null)
				routerResponse.Journeys.Sort(comp);

			return routerResponse == null ? null : new JourneyResultsWindow(routerResponse, source.Name, target.Name, dt, win);
		}
		/// <summary>
		/// Tries to obtain station info and return window with results.
		/// </summary>
		/// <param name="stationName">Station name.</param>
		/// <param name="dt">Datetime.</param>
		/// <param name="count">Number of departures.</param>
		/// <param name="isStation">Indicates whether it is station or stop.</param>
		/// <param name="routeLabel">Route label.</param>
		/// <param name="win">Window with request.</param>
		/// <returns>Window with results.</returns>
		public static async Task<DepartureBoardResultsWindow> GetStationInfoWindowAsync(string stationName, DateTime dt, int count, bool isStation, string routeLabel, NewStationInfoWindow win)
		{
			if (!await CheckBasicDataValidity()) return null;

			Structures.Basic.StationsBasic.StationBasic station = GetStationFromString(stationName);
			Structures.Basic.RoutesInfoBasic.RouteInfoBasic route = GetRouteInfoFromLabel(routeLabel);

			if (station == null || (route == null && !string.IsNullOrWhiteSpace(routeLabel))) return null;

			var dbRequest = new StationInfoRequest(station.ID, dt, count, isStation, route == null ? -1 : route.ID);
			var dbResponse = await SendDepartureBoardRequestAsync(dbRequest);

			return dbResponse == null ? null : new DepartureBoardResultsWindow(dbResponse, station.Name, dt, true, win);
		}
		/// <summary>
		/// Tries to obtain station info and return window with results.
		/// </summary>
		/// <param name="dt">Datetime.</param>
		/// <param name="count">Number of departures.</param>
		/// <param name="routeLabel">Route label.</param>
		/// <param name="win">Window with request.</param>
		/// <returns>Window with results.</returns>
		public static async Task<DepartureBoardResultsWindow> GetLineInfoWindowAsync(DateTime dt, int count, string routeLabel, NewLineInfoWindow win)
		{
			if (!await CheckBasicDataValidity()) return null;
			
			Structures.Basic.RoutesInfoBasic.RouteInfoBasic route = GetRouteInfoFromLabel(routeLabel);

			if (route == null) return null;

			var dbRequest = new LineInfoRequest(dt, count, route.ID);
			var dbResponse = await SendDepartureBoardRequestAsync(dbRequest);

			return dbResponse == null ? null : new DepartureBoardResultsWindow(dbResponse, routeLabel, dt, false, win);
		}
		public static async Task<DepartureBoardResponse> SendDepartureBoardRequestAsync(StationInfoRequest dbRequest)
		{
			DepartureBoardResponse dbResponse = null;

			if (DataFeedDesktop.OfflineMode)
			{
				await Task.Run(() =>
				{
					using (var dbProcessing = new Interop.DepartureBoardManaged(DataFeedDesktop.Full, dbRequest))
					{
						dbProcessing.ObtainDepartureBoard();
						dbResponse = dbProcessing.ShowDepartureBoard();
					}

				});
			}

			else
			{
				using (var dbProcessing = new DepartureBoardProcessing())
				{
					var cached = StationInfoCached.Select(dbRequest.StopID);

					if (cached == null || cached.ShouldBeUpdated)
					{
						try
						{
							dbResponse = await dbProcessing.ProcessAsync(dbRequest, Settings.TimeoutDuration);
						}
						catch (System.Net.WebException)
						{
							MessageBox.Show(Settings.Localization.UnreachableHost, Settings.Localization.Offline, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}

					else
					{
						dbResponse = new DepartureBoardResponse(cached.LoadResults().Departures.SkipWhile(x => RequestBase.ConvertDateTimeToUnixTimestamp(x.DepartureDateTime) > dbRequest.EarliestDepartureDateTime).Take(dbRequest.Count).ToList());
					}
				}
			}
			return dbResponse;
		}

		public static async Task<DepartureBoardResponse> SendDepartureBoardRequestAsync(LineInfoRequest dbRequest)
		{
			DepartureBoardResponse dbResponse = null;

			if (DataFeedDesktop.OfflineMode)
			{
				await Task.Run(() =>
				{
					using (var dbProcessing = new Interop.DepartureBoardManaged(DataFeedDesktop.Full, dbRequest))
					{
						dbProcessing.ObtainDepartureBoard();
						dbResponse = dbProcessing.ShowDepartureBoard();
					}

				});
			}

			else
			{
				using (var dbProcessing = new DepartureBoardProcessing())
				{
					var cached = LineInfoCached.Select(dbRequest.RouteInfoID);

					if (cached == null || cached.ShouldBeUpdated)
					{
						try
						{
							dbResponse = await dbProcessing.ProcessAsync(dbRequest, Settings.TimeoutDuration);
						}
						catch (System.Net.WebException)
						{
							MessageBox.Show(Settings.Localization.UnreachableHost, Settings.Localization.Offline, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}

					else
					{
						dbResponse = new DepartureBoardResponse(cached.LoadResults().Departures.SkipWhile(x => RequestBase.ConvertDateTimeToUnixTimestamp(x.DepartureDateTime) > dbRequest.EarliestDepartureDateTime).Take(dbRequest.Count).ToList());
					}
				}
			}
			return dbResponse;
		}
		public static async Task<RouterResponse> SendRouterRequestAsync(RouterRequest routerRequest)
		{
			RouterResponse routerResponse = null;

			if (DataFeedDesktop.OfflineMode)
			{
				await Task.Run(() =>
				{
					using (var routerProcessing = new Interop.RouterManaged(DataFeedDesktop.Full, routerRequest))
					{
						routerProcessing.ObtainJourneys();
						routerResponse = routerProcessing.ShowJourneys();
					}
				});
			}

			else
			{
				var cached = JourneyCached.Select(routerRequest.SourceStationID, routerRequest.TargetStationID);

				if (cached == null || cached.ShouldBeUpdated)
				{
					using (var routerProcessing = new RouterProcessing())
					{
						try
						{
							routerResponse = await routerProcessing.ProcessAsync(routerRequest, Settings.TimeoutDuration);
						}
						catch (System.Net.WebException)
						{
							MessageBox.Show(Settings.Localization.UnreachableHost, Settings.Localization.Offline, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}

				else
				{
					routerResponse = new RouterResponse(cached.LoadResults().Journeys.SkipWhile(x => RequestBase.ConvertDateTimeToUnixTimestamp(x.DepartureDateTime) > routerRequest.MaximalArrivalDateTime).Take(routerRequest.Count).ToList());					
				}
			}

			return routerResponse;
		}
		/// <summary>
		/// Caches the departures according to departure board request.
		/// </summary>
		public static async Task<bool> CacheDepartureBoardAsync(StationInfoRequest dbRequest) => StationInfoCached.CacheResults(await SendDepartureBoardRequestAsync(dbRequest));
		/// <summary>
		/// Caches the departures according to departure board request.
		/// </summary>
		public static async Task<bool> CacheDepartureBoardAsync(LineInfoRequest dbRequest) => LineInfoCached.CacheResults(await SendDepartureBoardRequestAsync(dbRequest));
		/// <summary>
		/// Caches the journeys according to router request.
		/// </summary>
		public static async Task<bool> CacheJourneyAsync(RouterRequest routerRequest) => JourneyCached.CacheResults(await SendRouterRequestAsync(routerRequest));
		/// <summary>
		/// Updates all the cached results.
		/// </summary>
		public static async Task UpdateCachedResultsAsync()
		{
			async Task forEachFetchedResult<Res, Req>(IEnumerable<CachedData<Res, Req>> collection, Func<Req, Task> processAsync) where Res : ResponseBase where Req : RequestBase
			{
				foreach (var fetched in collection)
					if (fetched.ShouldBeUpdated)
						await processAsync(fetched.ConstructNewRequest());
			}

			await Task.WhenAll(
				forEachFetchedResult(StationInfoCached.FetchStationInfoData(), CacheDepartureBoardAsync),
				forEachFetchedResult(LineInfoCached.FetchLineInfoData(), CacheDepartureBoardAsync),
				forEachFetchedResult(JourneyCached.FetchJourneyData(), CacheJourneyAsync)
				);
		}
		/// <summary>
		/// Returns loading HTML string with customized text.
		/// </summary>
		/// <param name="text">Customized text.</param>
		/// <returns>HTML document.</returns>
		public static string LoadingHtml(string text) => @"<!DOCTYPE html>
<html>
<head>
<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
<meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1""/>
<style>
body {
  text-align: center;
  margin-top: 10%;
  color: grey;
}
.loader {
  display: inline-block;
  margin-bottom: 5%;
  border: 16px solid #f3f3f3;
  border-radius: 50%;
  border-top: 16px solid #3498db;
  width: 120px;
  height: 120px;
  -webkit-animation: spin 2s linear infinite; 
  animation: spin 2s linear infinite;
}
@-webkit-keyframes spin {
  0% { -webkit-transform: rotate(0deg); }
  100% { -webkit-transform: rotate(360deg); }
}
@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
</style>
</head>
<body>
<div class=""loader""></div>
<h2>" + text + @"</h2>
</body>
</html>";
	}
}

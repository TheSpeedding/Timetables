// All of these methods must be in this project, because of localization settings and global settings (timeout duration etc.).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Application.Mobile;
using Timetables.Client;

namespace Timetables.Application.Mobile
{
	/// <summary>
	/// Class that supplies methods to send requests.
	/// </summary>
	internal static class Request
	{
		/// <summary>
		/// Returns station object with given name. If not found, returns error message.
		/// </summary>
		/// <param name="name">Name of the station.</param>
		/// <returns>Station object.</returns>
		public static Structures.Basic.StationsBasic.StationBasic GetStationFromString(string name)
		{
			Structures.Basic.StationsBasic.StationBasic source = DataFeedClient.Basic.Stations.FindByName(name);

			if (source == null)
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnableToFindStation + ": " + name);

			return source;
		}
		/// <summary>
		/// Returns route info object with given name. If not found, returns error message.
		/// </summary>
		/// <param name="label">Route label.</param>
		/// <returns>Route info object.</returns>
		public static Structures.Basic.RoutesInfoBasic.RouteInfoBasic GetRouteInfoFromLabel(string label)
		{
			if (string.IsNullOrEmpty(label))
				return null;

			Structures.Basic.RoutesInfoBasic.RouteInfoBasic route = DataFeedClient.Basic.RoutesInfo.FindByLabel(label);

			if (route == null)
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnableToFindRouteInfo + ": " + label);

			return route;
		}
		/// <summary>
		/// Returns true if everything is OK.
		/// </summary>
		public static async Task<bool> CheckBasicDataValidity()
		{
			try
			{
				await DataFeedClient.DownloadAsync(true); // Checks basic data validity.
			}
			catch (System.Net.WebException)
			{
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnreachableHost);
				return false;
			}
			return true;
		}

		public static async Task<RouterResponse> SendRouterRequestAsync(RouterRequest routerRequest)
		{
			RouterResponse routerResponse = null;
			var cached = JourneyCached.Select(routerRequest.SourceStationID, routerRequest.TargetStationID);

			if (cached == null || cached.ShouldBeUpdated)
			{
				using (var routerProcessing = new RouterProcessing())
				{
					try
					{
						if (!await CheckBasicDataValidity()) return null;

						// Process the request immediately so the user does not have to wait until the caching is completed.

						routerResponse = await routerProcessing.ProcessAsync(routerRequest, routerRequest.Count == -1 ? int.MaxValue : Settings.TimeoutDuration);

						// Then update the cache.

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
						if (cached != null && cached.ShouldBeUpdated && routerRequest.Count != -1)
							Task.Run(async () => cached.UpdateCache(await routerProcessing.ProcessAsync(cached.ConstructNewRequest(), int.MaxValue)));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

					}
					catch (System.Net.WebException)
					{
						PlatformDependentSettings.ShowMessage(Settings.Localization.UnreachableHost);
					}
				}
			}

			else
			{
				routerResponse = cached.FindResultsSatisfyingRequest(routerRequest);
			}

			return routerResponse;
		}


		public static async Task<DepartureBoardResponse> SendDepartureBoardRequestAsync(DepartureBoardRequest dbRequest)
		{
			// This cannot be done better since in each of the method we need information about specialized classes (caching, algorithm). 
			// Actually, it could be done better (without code copies), but then the code would be unreadable and less efficient, too.

			if (dbRequest.GetType() == typeof(StationInfoRequest))
				return await SendDepartureBoardRequestAsync((StationInfoRequest)dbRequest);

			else
				return await SendDepartureBoardRequestAsync((LineInfoRequest)dbRequest);
		}
		private static async Task<DepartureBoardResponse> SendDepartureBoardRequestAsync(StationInfoRequest dbRequest)
		{
			DepartureBoardResponse dbResponse = null;

			using (var dbProcessing = new DepartureBoardProcessing())
			{
				var cached = StationInfoCached.Select(dbRequest.StopID);

				if (cached == null || cached.ShouldBeUpdated)
				{
					try
					{
						if (!await CheckBasicDataValidity()) return null;

						// Process the request immediately so the user does not have to wait until the caching is completed.

						dbResponse = await dbProcessing.ProcessAsync(dbRequest, dbRequest.Count == -1 ? int.MaxValue : Settings.TimeoutDuration);

						// Then update the cache.

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
						if (cached != null && cached.ShouldBeUpdated && dbRequest.Count != -1)
							Task.Run(async () => cached.UpdateCache(await dbProcessing.ProcessAsync(cached.ConstructNewRequest(), int.MaxValue)));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
					}
					catch (System.Net.WebException)
					{
						PlatformDependentSettings.ShowMessage(Settings.Localization.UnreachableHost);
					}
				}

				else
				{
					dbResponse = cached.FindResultsSatisfyingRequest(dbRequest);
				}
			}

			return dbResponse;
		}
		private static async Task<DepartureBoardResponse> SendDepartureBoardRequestAsync(LineInfoRequest dbRequest)
		{
			DepartureBoardResponse dbResponse = null;

			using (var dbProcessing = new DepartureBoardProcessing())
			{
				var cached = LineInfoCached.Select(dbRequest.RouteInfoID);

				if (cached == null || cached.ShouldBeUpdated)
				{
					try
					{
						if (!await CheckBasicDataValidity()) return null;

						// Process the request immediately so the user does not have to wait until the caching is completed.

						dbResponse = await dbProcessing.ProcessAsync(dbRequest, dbRequest.Count == -1 ? int.MaxValue : Settings.TimeoutDuration);

						// Then update the cache.

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
						if (cached != null && cached.ShouldBeUpdated && dbRequest.Count != -1)
							Task.Run(async () => cached.UpdateCache(await dbProcessing.ProcessAsync(cached.ConstructNewRequest(), int.MaxValue)));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

					}
					catch (System.Net.WebException)
					{
						PlatformDependentSettings.ShowMessage(Settings.Localization.UnreachableHost);
					}
				}

				else
				{
					dbResponse = cached.FindResultsSatisfyingRequest(dbRequest);
				}
			}

			return dbResponse;
		}

		
		/// <summary>
		/// Caches the departures according to departure board request.
		/// </summary>
		public static async Task<bool> CacheDepartureBoardAsync(DepartureBoardRequest dbRequest)
		{
			if (dbRequest.GetType() == typeof(StationInfoRequest))
				return await CacheDepartureBoardAsync((StationInfoRequest)dbRequest);

			else
				return await CacheDepartureBoardAsync((LineInfoRequest)dbRequest);
		}
		/// <summary>
		/// Caches the departures according to departure board request.
		/// </summary>
		private static async Task<bool> CacheDepartureBoardAsync(StationInfoRequest dbRequest) => 
			StationInfoCached.CacheResults(DataFeedClient.Basic.Stations.FindByIndex(dbRequest.StopID), await SendDepartureBoardRequestAsync(dbRequest)) != null;
		/// <summary>
		/// Caches the departures according to departure board request.
		/// </summary>
		private static async Task<bool> CacheDepartureBoardAsync(LineInfoRequest dbRequest) => 
			LineInfoCached.CacheResults(DataFeedClient.Basic.RoutesInfo.FindByIndex(dbRequest.RouteInfoID), await SendDepartureBoardRequestAsync(dbRequest)) != null;
		/// <summary>
		/// Caches the journeys according to router request.
		/// </summary>
		public static async Task<bool> CacheJourneyAsync(RouterRequest routerRequest) => 
			JourneyCached.CacheResults(DataFeedClient.Basic.Stations.FindByIndex(routerRequest.SourceStationID), DataFeedClient.Basic.Stations.FindByIndex(routerRequest.TargetStationID), await SendRouterRequestAsync(routerRequest)) != null;
		/// <summary>
		/// Updates all the cached results.
		/// </summary>
		public static async Task UpdateCachedResultsAsync(bool forceUpdate = false)
		{
			async Task ForEachFetchedResult<Res, Req>(IEnumerable<CachedData<Res, Req>> collection, Func<Req, Task> processAsync) where Res : ResponseBase where Req : RequestBase
			{
				foreach (var fetched in collection)
					if (forceUpdate || fetched.ShouldBeUpdated)
						await processAsync(fetched.ConstructNewRequest());
			}

			await Task.WhenAll(
				ForEachFetchedResult(StationInfoCached.FetchStationInfoData(), CacheDepartureBoardAsync),
				ForEachFetchedResult(LineInfoCached.FetchLineInfoData(), CacheDepartureBoardAsync),
				ForEachFetchedResult(JourneyCached.FetchJourneyData(), CacheJourneyAsync)
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
<h3>" + text + @"</h3>
</body>
</html>";
	
	}
}

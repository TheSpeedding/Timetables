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
	static class Requests
	{
		/// <summary>
		/// Returns station object with given name. If not found, returns error message within MessageBox.
		/// </summary>
		/// <param name="name">Name of the station.</param>
		/// <returns>Station object.</returns>
		public static Structures.Basic.StationsBasic.StationBasic GetStationFromString(string name)
		{
			Structures.Basic.StationsBasic.StationBasic source = DataFeed.Basic.Stations.FindByName(name);

			if (source == null)
			{
				MessageBox.Show(Settings.Localization.UnableToFindStation + ": " + name, Settings.Localization.StationNotFound, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return null;
			}

			return source;
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
		/// <returns>Window with results</returns>
		public static async Task<JourneyResultsWindow> SendRouterRequestAsync(string sourceName, string targetName, DateTime dt, uint transfers, uint count, double coefficient)
		{
			Structures.Basic.StationsBasic.StationBasic source = GetStationFromString(sourceName);
			Structures.Basic.StationsBasic.StationBasic target = GetStationFromString(targetName);

			if (source == null || target == null)
				return null;

			var routerRequest = new RouterRequest(source.ID, target.ID, dt, transfers, count, coefficient);
			RouterResponse routerResponse = null;

			if (DataFeed.OfflineMode)
			{
				await Task.Run(() =>
				{
					using (var routerProcessing = new Interop.RouterManaged(DataFeed.Full, routerRequest))
					{
						routerProcessing.ObtainJourneys();
						routerResponse = routerProcessing.ShowJourneys();
					}
				});
			}

			else
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

			return routerResponse == null ? null : new JourneyResultsWindow(routerResponse, source.Name, target.Name, dt);
		}

		/// <summary>
		/// Tries to obtain departures and return window with results
		/// </summary>
		/// <param name="stationName">Station name.</param>
		/// <param name="dt">Datetime.</param>
		/// <param name="count">Number of departures.</param>
		/// <param name="isStation">Indicates whether it is station or stop.</param>
		/// <returns>Window with results.</returns>
		public static async Task<DepartureBoardResultsWindow> SendDepartureBoardRequestAsync(string stationName, DateTime dt, uint count, bool isStation)
		{
			Structures.Basic.StationsBasic.StationBasic station = GetStationFromString(stationName);

			if (station == null)
				return null;

			var dbRequest = new DepartureBoardRequest(station.ID, dt, count, isStation);
			DepartureBoardResponse dbResponse = null;

			if (DataFeed.OfflineMode)
			{
				await Task.Run(() =>
				{
					using (var dbProcessing = new Interop.DepartureBoardManaged(DataFeed.Full, dbRequest))
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
					try
					{
						dbResponse = await dbProcessing.ProcessAsync(dbRequest, Settings.TimeoutDuration);
					}
					catch (System.Net.WebException)
					{
						MessageBox.Show(Settings.Localization.UnreachableHost, Settings.Localization.Offline, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}

			return dbResponse == null ? null : new DepartureBoardResultsWindow(dbResponse, stationName, dt);
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

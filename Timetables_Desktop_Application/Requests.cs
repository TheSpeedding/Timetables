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
		/// Gets everything ready for textbox auto-completion.
		/// </summary>
		/// <param name="tb">Textbox.</param>
		/// <param name="content">Content.</param>
		public static void AutoCompleteTextBox(TextBox tb, string[] content)
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
			if (name == null)
				return null;

			Structures.Basic.StationsBasic.StationBasic source = DataFeed.Basic.Stations.FindByName(name);

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
			if (label == null || string.IsNullOrWhiteSpace(label))
				return null;

			Structures.Basic.RoutesInfoBasic.RouteInfoBasic route = DataFeed.Basic.RoutesInfo.FindByLabel(label);

			if (route == null)
				MessageBox.Show(Settings.Localization.UnableToFindRouteInfo + ": " + label, Settings.Localization.RouteInfoNotFound, MessageBoxButtons.OK, MessageBoxIcon.Warning);

			return route;
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
		/// <param name="routeLabel">Route label.</param>
		/// <returns>Window with results.</returns>
		public static async Task<DepartureBoardResultsWindow> SendDepartureBoardRequestAsync(string stationName, DateTime dt, uint count, bool isStation, string routeLabel)
		{
			Structures.Basic.StationsBasic.StationBasic station = GetStationFromString(stationName);
			Structures.Basic.RoutesInfoBasic.RouteInfoBasic route = GetRouteInfoFromLabel(routeLabel);

			if (station == null)
				return null;

			var dbRequest = new DepartureBoardRequest(station.ID, dt, count, isStation, route == null ? -1 : route.ID);
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

			return dbResponse == null ? null : new DepartureBoardResultsWindow(dbResponse, station.Name, dt);
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

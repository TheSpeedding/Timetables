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
	}
}

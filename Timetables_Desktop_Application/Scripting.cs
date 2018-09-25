using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timetables.Application.Desktop;
using Timetables.Client;
using Timetables.Structures.Basic;

namespace Timetables.Interop
{

	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[System.Runtime.InteropServices.ComVisible(true)]
	public abstract class GoogleMapsScripting : Scripting
	{
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
		[System.Runtime.InteropServices.ComVisible(true)]
		public sealed class General : GoogleMapsScripting
		{
			protected override DateTime GetEarliestDepartureDateTime(StopsBasic.StopBasic stop) => DateTime.Now;
			public override string ShowArrivalTime(uint stopId) => string.Empty;
			public override string ShowArrivalConstant() => string.Empty;
			protected override bool ShowDeparturesFromStation() => false;
		}
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
		[System.Runtime.InteropServices.ComVisible(true)]
		public sealed class Journey : GoogleMapsScripting
		{
			private Client.Journey journey;
			public Journey(Client.Journey journey) => this.journey = journey;
			protected override DateTime GetEarliestDepartureDateTime(StopsBasic.StopBasic stop)
			{
				foreach (var js in journey.JourneySegments)
				{
					if (DataFeed.Basic.Stops.FindByIndex(js.SourceStopID).ParentStation.ID == stop.ParentStation.ID) return js.DepartureDateTime;

					if (js is TripSegment)
						foreach (var @is in (js as TripSegment).IntermediateStops)
						{
							if (DataFeed.Basic.Stops.FindByIndex(@is.StopID).ParentStation.ID == stop.ParentStation.ID) return @is.Arrival;
						}

					if (DataFeed.Basic.Stops.FindByIndex(js.TargetStopID).ParentStation.ID == stop.ParentStation.ID) return js.ArrivalDateTime;
				}

				throw new ArgumentException("Stop ID not found in the journey.");
			}
		}
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
		[System.Runtime.InteropServices.ComVisible(true)]
		public sealed class Departure : GoogleMapsScripting
		{
			private Client.Departure departure;
			public Departure(Client.Departure departure) => this.departure = departure;
			protected override DateTime GetEarliestDepartureDateTime(StopsBasic.StopBasic stop)
			{
				if (stop.ParentStation.ID == DataFeed.Basic.Stops.FindByIndex(departure.StopID).ParentStation.ID)
					return departure.DepartureDateTime;
				else
					return departure.IntermediateStops.Find(s => DataFeed.Basic.Stops.FindByIndex(s.StopID).ParentStation.ID == stop.ParentStation.ID).Arrival;
			}
		}
		protected abstract DateTime GetEarliestDepartureDateTime(StopsBasic.StopBasic stop);		
		public string ShowDepartures(uint stopId)
		{
			DateTime dt = GetEarliestDepartureDateTime(DataFeed.Basic.Stops.FindByIndex(stopId));

			bool isStation = ShowDeparturesFromStation();
			uint newId = isStation ? DataFeed.Basic.Stops.FindByIndex(stopId).ParentStation.ID : stopId;

			DepartureBoardResponse results = AsyncHelpers.RunSync(() => Requests.SendDepartureBoardRequestAsync(new DepartureBoardRequest(newId, dt, 5, isStation)));

			return results.TransformToHtml(Settings.DepartureBoardInMapXslt.FullName, Settings.DepartureBoardInMapCss.FullName).RenderJavascriptToHtml(this);
		}
		protected virtual bool ShowDeparturesFromStation() => true;
		public virtual string ShowArrivalConstant() => Settings.Localization.ArrivalAt + ": ";
		public virtual string ShowArrivalTime(uint stopId) => GetEarliestDepartureDateTime(DataFeed.Basic.Stops.FindByIndex(stopId)).ToShortTimeString();
		public string NoDepartures() => Settings.Localization.NoDeparturesFromThisStop;
	}
	/// <summary>
	/// Offers scripting for journeys window.
	/// </summary>ž
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[System.Runtime.InteropServices.ComVisible(true)]
	public class JourneyScripting : Scripting
	{
		private JourneyResultsWindow window;
		/// <summary>
		/// Initializes the object.
		/// </summary>
		/// <param name="window">Window that is relevant for this object.</param>
		public JourneyScripting(JourneyResultsWindow window) => this.window = window;
		/// <summary>
		/// Shows detail of the journey.
		/// </summary>
		/// <param name="index">Index of the journey.</param>
		public void ShowJourneyDetail(int index) => new JourneyResultsWindow(window.Results.Journeys[index]).Show(window.DockPanel, window.DockState);
		/// <summary>
		/// Shows map of the journey.
		/// </summary>
		/// <param name="index">Index of the journey.</param>
		public void ShowMap(int index = 0) => new ShowMapWindow(window.Results.Journeys[index]).Show(window.DockPanel, window.DockState);
		/// <summary>
		/// Shows journey printing dialog.
		/// </summary>
		/// <param name="index">Index of the journey.</param>
		public void PrintJourneyDetail(int index = 0)
		{
			var wb = new WebBrowser
			{
				ObjectForScripting = new JourneyScripting(window),
				ScriptErrorsSuppressed = true,
				DocumentText = window.Results.Journeys[index].TransformToHtml(Settings.JourneyDetailPrintXslt.FullName, Settings.JourneyDetailPrintCss.FullName)
			};

			wb.DocumentCompleted += (object sender, WebBrowserDocumentCompletedEventArgs e) => (sender as WebBrowser).ShowPrintDialog();
		}
		public void PrintJourneyList()
		{
			var wb = new WebBrowser
			{
				ObjectForScripting = new JourneyScripting(window),
				ScriptErrorsSuppressed = true,
				DocumentText = window.Results.TransformToHtml(Settings.JourneySimplePrintXslt.FullName, Settings.JourneySimplePrintCss.FullName)
			};

			wb.DocumentCompleted += (object sender, WebBrowserDocumentCompletedEventArgs e) => (sender as WebBrowser).ShowPrintDialog();
		}
		public void EditJourneysParameters() => window.CloseThisAndReopenPrevious();
		public string ShowJourneyText() => window.Text;
	}

	/// <summary>
	/// Offers scripting for departure boards window.
	/// </summary>
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[System.Runtime.InteropServices.ComVisible(true)]
	public class DepartureBoardScripting : Scripting
	{
		private DepartureBoardResultsWindow window;

		/// <summary>
		/// Initializes the object.
		/// </summary>
		/// <param name="window">Window that is relevant for this object.</param>
		public DepartureBoardScripting(DepartureBoardResultsWindow window) => this.window = window;
		/// <summary>
		/// Shows detail of the departure.
		/// </summary>
		/// <param name="index">Index of the departure.</param>
		public void ShowDepartureDetail(int index) => new DepartureBoardResultsWindow(window.Results.Departures[index]).Show(window.DockPanel, window.DockState);
		/// <summary>
		/// Shows map of the departure.
		/// </summary>
		/// <param name="index">Index of the departure.</param>
		public void ShowMap(int index = 0) => new ShowMapWindow(window.Results.Departures[index]).Show(window.DockPanel, window.DockState);
		/// <summary>
		/// Shows departure printing dialog.
		/// </summary>
		/// <param name="index">Index of the departure.</param>
		public void PrintDepartureDetail(int index = 0)
		{
			var wb = new WebBrowser
			{
				ObjectForScripting = new DepartureBoardScripting(window),
				ScriptErrorsSuppressed = true,
				DocumentText = window.Results.Departures[index].TransformToHtml(Settings.DepartureBoardDetailPrintXslt.FullName, Settings.DepartureBoardDetailPrintCss.FullName)
			};

			wb.DocumentCompleted += (object sender, WebBrowserDocumentCompletedEventArgs e) => (sender as WebBrowser).ShowPrintDialog();
		}
		public void PrintDepartureBoardList()
		{
			var wb = new WebBrowser
			{
				ObjectForScripting = new DepartureBoardScripting(window),
				ScriptErrorsSuppressed = true,
				DocumentText = window.Results.TransformToHtml(Settings.DepartureBoardSimplePrintXslt.FullName, Settings.DepartureBoardSimplePrintCss.FullName)
			};

			wb.DocumentCompleted += (object sender, WebBrowserDocumentCompletedEventArgs e) => (sender as WebBrowser).ShowPrintDialog();
		}
		public void EditDeparturesParameters() => window.CloseThisAndReopenPrevious();
		public string ShowDepartureText() => window.Text;
	}
}

// This would be better in Timetables Client Library project, but then there would be a problem with cross dependencies.

namespace Timetables.Interop
{
	/// <summary>
	/// Offers functions that can be called from Javascript code using ObjectForScripting property.
	/// </summary>
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Scripting
	{
		public static readonly Scripting ObjectForScripting = new Scripting();
		protected Scripting() { }

		/// <summary>
		/// Returns total number of transfers in the journey represented as a string.
		/// </summary>
		/// <param name="totalTripSegments">Number of trip segments.</param>
		/// <returns>String representation of transfers.</returns>
		public string TotalTransfersToString(int totalTripSegments)
		{
			--totalTripSegments;
			if (totalTripSegments <= 0) return Settings.Localization.NoTransfers;
			else if (totalTripSegments == 1) return Settings.Localization.OneTransfer;
			else return totalTripSegments + " " + Settings.Localization.Transfers;
		}

		/// <summary>
		/// Converts timespan to string that suits our needs.
		/// </summary>
		/// <param name="t">Timespan to convert.</param>
		/// <returns>Timespan as a string.</returns>
		private string TimeSpanToString(TimeSpan t) => 
			(t.Days > 0 ? t.Days + (t.Days == 1 ? " " + Settings.Localization.Day + " " : " " + Settings.Localization.Days + " ") : "") + 
			(t.Hours > 0 ? t.Hours + (t.Hours == 1 ? " " + Settings.Localization.Hour + " " : " " + Settings.Localization.Hours + " ") : "") + 
			t.Minutes + (t.Minutes == 1 ? " " + Settings.Localization.Minute + " " : " " + Settings.Localization.Minutes + " ");

		/// <summary>
		/// Computes difference of two datetimes and returns total duration.
		/// </summary>
		/// <param name="iso8601a">First datetime.</param>
		/// <param name="iso8601b">Second datetime.</param>
		/// <returns>Total duration of the journey.</returns>
		public string TotalDurationToString(string iso8601a, string iso8601b) => TimeSpanToString(DateTime.Parse(iso8601b) - DateTime.Parse(iso8601a));

		/// <summary>
		/// Converts ISO8601 formatted datetime to timespan in which the journey leaves.
		/// </summary>
		/// <param name="iso8601">ISO8601 formatted datetime.</param>
		/// <returns>Timespan in which the journey leaves.</returns>
		public string LeavingTimeToString(string iso8601)
		{
			TimeSpan diff = DateTime.Parse(iso8601) - DateTime.Now;

			return diff.TotalSeconds < 0 ? Settings.Localization.Left + " " + TimeSpanToString(diff.Negate()) + " " + Settings.Localization.Ago : Settings.Localization.LeavesIn + " " + TimeSpanToString(diff);
		}

		/// <summary>
		/// Converts ISO8601 formatted datetime to simple string.
		/// </summary>
		/// <param name="iso8601">ISO8601 formatted datetime.</param>
		/// <returns>Simple time representation.</returns>
		public string Iso8601ToSimpleString(string iso8601)
		{
			var d = DateTime.Parse(iso8601);
			return d.Hour + ":" + d.Minute.ToString("00");
		}

		/// <summary>
		/// Replaces stop ID with coressponding name.
		/// </summary>
		/// <param name="id">ID of the stop.</param>
		/// <returns>Name of the stop.</returns>
		public string ReplaceIdWithName(uint id) => DataFeed.Basic.Stops.FindByIndex(id).Name;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string MapStringConstant() => Settings.Localization.Map;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string DetailStringConstant() => Settings.Localization.Detail;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string PrintStringConstant() => Settings.Localization.Print;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string TransferStringConstant() => Settings.Localization.Transfer;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string OutdatedStringConstant() => Settings.Localization.Outdated;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string EditParametersStringConstant() => Settings.Localization.EditParameters;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string PrintListStringConstant() => Settings.Localization.PrintList;
	}
}
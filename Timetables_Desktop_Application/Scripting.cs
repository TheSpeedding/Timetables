using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Application.Desktop;
using Timetables.Client;

namespace Timetables.Interop
{
	/// <summary>
	/// Offers scripting for journeys window.
	/// </summary>
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
		public void ShowJourneyDetail(int index) => new JourneyResultsWindow(window.Journeys[index]).Show(window.DockPanel, window.DockState);
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
		public void ShowDepartureDetail(int index) => new DepartureBoardResultsWindow(window.Departures[index]).Show(window.DockPanel, window.DockState);
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
	}
}
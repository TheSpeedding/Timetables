using System;
using System.Collections.Generic;
using System.Text;
using Timetables.Client;

namespace Timetables.Interop
{
	/// <summary>
	/// Offers functions that can be called from Javascript code. In this project because of cross dependencies.
	/// </summary>
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Scripting
	{
		protected readonly Localization localization;
		private readonly bool isMobile;
		
		public Scripting(Localization loc, bool mobile)
		{
			localization = loc;
			isMobile = mobile;
		}

		/// <summary>
		/// This is used in JavaScript file so we don't need duplicates for mobile and desktop version.
		/// </summary>
		public bool IsMobileVersion() => isMobile;

		/// <summary>
		/// Returns total number of transfers in the journey represented as a string.
		/// </summary>
		/// <param name="totalTripSegments">Number of trip segments.</param>
		/// <returns>String representation of transfers.</returns>
		public string TotalTransfersToString(int totalTripSegments)
		{
			--totalTripSegments;
			if (totalTripSegments <= 0) return localization.NoTransfers;
			else if (totalTripSegments == 1) return localization.OneTransfer;
			else return totalTripSegments + " " + localization.Transfers;
		}

		/// <summary>
		/// Converts timespan to string that suits our needs.
		/// </summary>
		/// <param name="t">Timespan to convert.</param>
		/// <returns>Timespan as a string.</returns>
		private string TimeSpanToString(TimeSpan t) =>
			(t.Days > 0 ? t.Days + (t.Days == 1 ? " " + localization.Day + " " : " " + localization.Days + " ") : "") +
			(t.Hours > 0 ? t.Hours + (t.Hours == 1 ? " " + localization.Hour + " " : " " + localization.Hours + " ") : "") +
			t.Minutes + (t.Minutes == 1 ? " " + localization.Minute + " " : " " + localization.Minutes + " ");

		/// <summary>
		/// Computes difference of two datetimes and returns total duration.
		/// </summary>
		/// <param name="iso8601">Two datetimes separated by a comma.</param>
		/// <returns>Total duration of the journey.</returns>
		public string TotalDurationToString(string iso8601) => TimeSpanToString(DateTime.Parse(iso8601.Split(',')[1]) - DateTime.Parse(iso8601.Split(',')[0]));

		/// <summary>
		/// Converts ISO8601 formatted datetime to timespan in which the journey leaves.
		/// </summary>
		/// <param name="iso8601">ISO8601 formatted datetime.</param>
		/// <returns>Timespan in which the journey leaves.</returns>
		public string LeavingTimeToString(string iso8601)
		{
			TimeSpan diff = DateTime.Parse(iso8601) - DateTime.Now;

			return diff.TotalSeconds < 0 ? localization.Left + " " + TimeSpanToString(diff.Negate()) + " " + localization.Ago : localization.LeavesIn + " " + TimeSpanToString(diff);
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
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string MapStringConstant() => localization.Map;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string DetailStringConstant() => localization.Detail;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string PrintStringConstant() => localization.Print;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string TransferStringConstant() => localization.Transfer;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string OutdatedStringConstant() => localization.Outdated;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string EditParametersStringConstant() => localization.EditParameters;

		/// <summary>
		/// Returns localized string constant.
		/// </summary>
		/// <returns>Localized string constant.</returns>
		public string PrintListStringConstant() => localization.PrintList;
	}
}
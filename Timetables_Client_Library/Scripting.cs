using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			if (totalTripSegments <= 0) return "No transfer";
			else if (totalTripSegments == 1) return "1 transfer";
			else return totalTripSegments + " transfers";
		}

		/// <summary>
		/// Converts timespan to string that suits our needs.
		/// </summary>
		/// <param name="t">Timespan to convert.</param>
		/// <returns>Timespan as a string.</returns>
		private string TimeSpanToString(TimeSpan t) => (t.Hours > 0 ? t.Hours + (t.Hours == 1 ? " hour " : " hours ") : "") + t.Minutes + (t.Minutes == 1 ? " minute " : " minutes ");

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

			return diff.TotalSeconds < 0 ? "Left " + TimeSpanToString(diff) + " ago" : "Leaves in " + TimeSpanToString(diff); 
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
		/// Loads CSS stylesheet from the given file and returns its content.
		/// </summary>
		/// <param name="path">Path to the stylesheet.</param>
		/// <returns>Content of the file.</returns>
		public string LoadCssStylesheet(string path) => "<style>" + new System.IO.StreamReader(path).ReadToEnd() + "</style>";

		/// <summary>
		/// Gets absolute path.
		/// </summary>
		/// <param name="path">Relative path.</param>
		/// <returns>A path to the active directory.</returns>
		public string GetAbsolutePath(string path) => System.IO.Directory.GetCurrentDirectory() + "/" + path;
	}
}
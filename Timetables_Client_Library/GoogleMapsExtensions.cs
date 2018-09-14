using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Structures.Basic;

namespace Timetables.Client
{
	/// <summary>
	/// Offers helper methods for Google Maps API.
	/// </summary>
	public static class GoogleMapsExtensions
	{
		public static string CreateArrayForMarkers(this List<StopsBasic.StopBasic> stops, string arrName)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"var { arrName } = [");

			sb.Append($@" ['{ stops[0].Name }', { stops[0].Latitude }, { stops[0].Longitude }, 0]");

			for (int i = 1; i < stops.Count; i++)
				sb.Append($@", ['{ stops[i].Name }', { stops[i].Latitude }, { stops[i].Longitude }, { i }]");

			sb.Append(" ];");

			return sb.ToString();
		}

		public static double GetMidpointLatitude(this List<StopsBasic.StopBasic> stops) => stops.Select(s => s.Latitude).Average();
		public static double GetMidpointLongitude(this List<StopsBasic.StopBasic> stops) => stops.Select(s => s.Longitude).Average();
	}
}

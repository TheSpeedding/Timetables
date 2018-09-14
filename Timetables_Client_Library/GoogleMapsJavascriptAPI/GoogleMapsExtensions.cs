using System;
using System.Collections.Generic;
using System.Globalization;
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
		public static JavascriptVariable<JavascriptObject> CreateMap(this IEnumerable<StopsBasic.StopBasic> stops, string map, double lat, double lon, int zoom) => 
			new JavascriptVariable<JavascriptObject>(map, new JavascriptObject("google.maps.Map",
				new JavascriptFunction.Call("document.getElementById", "'map'"),
				new JavascriptAnonymousObject(
						new KeyValuePair<string, object>("zoom", zoom),
						new KeyValuePair<string, object>("center", new JavascriptAnonymousObject(
							new KeyValuePair<string, object>("lat", lat),
							new KeyValuePair<string, object>("lng", lon))))));

		public static string CreateMarkers(this IEnumerable<StopsBasic.StopBasic> stops, JavascriptVariable<JavascriptObject> map)
		{
			var markersInfo =
				new JavascriptVariable<JavascriptArray<JavascriptArray<object>>>("markersInfo", 
					new JavascriptArray<JavascriptArray<object>>(stops.Select(stop => new JavascriptArray<object> { "'" + stop.Name + "'", stop.Latitude, stop.Longitude })));

			var cycle = new JavascriptControlStructures.For(new JavascriptVariable<uint>("i", 0), (uint)markersInfo.Content.Count);

			cycle.AddInstruction(() => new JavascriptObject("google.maps.Marker",
				new JavascriptAnonymousObject(
					new KeyValuePair<string, object>("title", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][0]"),
					new KeyValuePair<string, object>("position", 
						new JavascriptAnonymousObject(
							new KeyValuePair<string, object>("lat", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][1]"),
							new KeyValuePair<string, object>("lng", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][2]"))),
					new KeyValuePair<string, object>("map", map.Name))).ToString() + ";");

			return markersInfo.VariableAssignment() + Environment.NewLine + cycle.ToString();
		}

		public static double GetAverageLatitude(this IEnumerable<StopsBasic.StopBasic> stops) => stops.Select(s => s.Latitude).Average();
		public static double GetAverageLongitude(this IEnumerable<StopsBasic.StopBasic> stops) => stops.Select(s => s.Longitude).Average();
	}
}

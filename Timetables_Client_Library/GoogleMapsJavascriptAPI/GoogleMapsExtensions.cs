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
				new JavascriptFunction.Call("document.getElementById", new JavascriptString("map")),
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

			var marker = new JavascriptVariable<JavascriptObject>("marker", new JavascriptObject("google.maps.Marker",
				new JavascriptAnonymousObject(
					new KeyValuePair<string, object>("title", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][0]"),
					new KeyValuePair<string, object>("position",
						new JavascriptAnonymousObject(
							new KeyValuePair<string, object>("lat", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][1]"),
							new KeyValuePair<string, object>("lng", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][2]"))),
					new KeyValuePair<string, object>("map", map.Name))));			
			
			var content = new JavascriptString("SEM VLOŽIT TEN DEBILNÍ KÓD, STEJNÝ PRO VŠECHNY MARKERY"); 
			var popupWindow = new JavascriptVariable<JavascriptObject>("popupWindow",
				new JavascriptObject("google.maps.InfoWindow",
					new JavascriptAnonymousObject(new KeyValuePair<string, object>("content", content))));

			var onClickFn = new JavascriptFunction.Anonymous();
			onClickFn.AddInstruction(() => popupWindow.Call(new JavascriptFunction.Call("open", map.Name, marker.Name)).ToString() + ";");

			cycle.AddInstruction(marker.VariableAssignment);
			cycle.AddInstruction(() => marker.Call(new JavascriptFunction.Call("addListener", new JavascriptString("click"), onClickFn)).ToString() + ";");

			return markersInfo.VariableAssignment() + Environment.NewLine + popupWindow.VariableAssignment() + Environment.NewLine + cycle.ToString();
		}

		public static double GetAverageLatitude(this IEnumerable<StopsBasic.StopBasic> stops) => stops.Select(s => s.Latitude).Average();
		public static double GetAverageLongitude(this IEnumerable<StopsBasic.StopBasic> stops) => stops.Select(s => s.Longitude).Average();
	}
}

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
				new JavascriptObject.Anonymous(
						new KeyValuePair<string, object>("zoom", zoom),
						new KeyValuePair<string, object>("center", new JavascriptObject.Anonymous(
							new KeyValuePair<string, object>("lat", lat),
							new KeyValuePair<string, object>("lng", lon))))));

		public static string CreateMarkers(this IEnumerable<StopsBasic.StopBasic> stops, JavascriptVariable<JavascriptObject> map)
		{
			var markersInfo =
				new JavascriptVariable<JavascriptArray<JavascriptArray<object>>>("markersInfo", 
					new JavascriptArray<JavascriptArray<object>>(stops.Select(stop => new JavascriptArray<object> { "'" + stop.Name + "'", stop.Latitude, stop.Longitude })));
			
			var cycle = new JavascriptControlStructures.For(new JavascriptVariable<uint>("i", 0), (uint)markersInfo.Content.Count);

			var marker = new JavascriptVariable<JavascriptObject>("marker", new JavascriptObject("google.maps.Marker",
				new JavascriptObject.Anonymous(
					new KeyValuePair<string, object>("title", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][0]"),
					new KeyValuePair<string, object>("position",
						new JavascriptObject.Anonymous(
							new KeyValuePair<string, object>("lat", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][1]"),
							new KeyValuePair<string, object>("lng", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][2]"))),
					new KeyValuePair<string, object>("zIndex", cycle.ControlVariable.Name),
					new KeyValuePair<string, object>("map", map.Name))));			
			
			var popupWindow = new JavascriptVariable<JavascriptObject>("popupWindow", new JavascriptObject("google.maps.InfoWindow"));

			var content = new JavascriptVariable<string>("content", "marker.getZIndex().toString()");

			var innerFn = new JavascriptFunction.Anonymous();
			innerFn.AddInstruction(popupWindow.Call(new JavascriptFunction.Call("setContent", content.Name)).ToString);
			innerFn.AddInstruction(popupWindow.Call(new JavascriptFunction.Call("open", map.Name, marker.Name)).ToString);

			var onClickAnonymousFn = new JavascriptFunction.Anonymous(marker.Name, content.Name, popupWindow.Name);
			onClickAnonymousFn.AddInstruction(new JavascriptFunction.Return<JavascriptFunction.Anonymous>(innerFn).ToString);

			var onClickListener = new JavascriptFunction.Call("google.maps.event.addListener",
				marker.Name,
				new JavascriptString("click"),
				new JavascriptFunction.Anonymous.Application(onClickAnonymousFn, marker.Name, content.Name, popupWindow.Name)
				);

			cycle.AddInstruction(popupWindow.VariableAssignment);
			cycle.AddInstruction(marker.VariableAssignment);
			cycle.AddInstruction(content.VariableAssignment);
			cycle.AddInstruction(onClickListener.ToString);

			return markersInfo.VariableAssignment() + Environment.NewLine + cycle.ToString();
		}

		public static double GetAverageLatitude(this IEnumerable<StopsBasic.StopBasic> stops) => stops.Select(s => s.Latitude).Average();
		public static double GetAverageLongitude(this IEnumerable<StopsBasic.StopBasic> stops) => stops.Select(s => s.Longitude).Average();
	}
}

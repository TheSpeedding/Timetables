using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Structures.Basic;
using Timetables.Utilities;

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

		public static string CreateSimplePolyline(this IEnumerable<StopsBasic.StopBasic> stops, JavascriptVariable<JavascriptObject> map, double strokeWeight, CPColor strokeColor, bool strokeDashed = false, int index = 0)
		{
			var arr = new JavascriptArray<JavascriptObject.Anonymous>(stops.Select(s => new JavascriptObject.Anonymous(
				new KeyValuePair<string, object>("lat", s.Latitude),
				new KeyValuePair<string, object>("lng", s.Longitude))));

			var coordinates = new JavascriptVariable<JavascriptArray<JavascriptObject.Anonymous>>($"coordinates{ index }", arr);

			var lineSymbol = new JavascriptVariable<JavascriptObject.Anonymous>($"lineSymbol{ index }", new JavascriptObject.Anonymous(
				new KeyValuePair<string, object>("path", new JavascriptString("M 0,-1 0,1")),
				new KeyValuePair<string, object>("strokeOpacity", 1),
				new KeyValuePair<string, object>("scale", 4)));

			var polyline = strokeDashed ? new JavascriptVariable<JavascriptObject>($"polyline{ index }", new JavascriptObject("google.maps.Polyline",
				new JavascriptObject.Anonymous(
					new KeyValuePair<string, object>("path", coordinates.Name),
					new KeyValuePair<string, object>("strokeColor", new JavascriptString("#" + strokeColor.R.ToString("X2") + strokeColor.G.ToString("X2") + strokeColor.B.ToString("X2"))),
					new KeyValuePair<string, object>("strokeOpacity", 0),
					new KeyValuePair<string, object>("icons", new JavascriptArray<JavascriptObject.Anonymous> { new JavascriptObject.Anonymous(
						new KeyValuePair<string, object>("icon", lineSymbol.Name),
						new KeyValuePair<string, object>("offset", new JavascriptString("0")),
						new KeyValuePair<string, object>("repeat", new JavascriptString("20px"))) }))))
					:
					new JavascriptVariable<JavascriptObject>($"polyline{ index }", new JavascriptObject("google.maps.Polyline",
				new JavascriptObject.Anonymous(
					new KeyValuePair<string, object>("path", coordinates.Name),
					new KeyValuePair<string, object>("strokeColor", new JavascriptString("#" + strokeColor.R.ToString("X2") + strokeColor.G.ToString("X2") + strokeColor.B.ToString("X2"))),
					new KeyValuePair<string, object>("strokeOpacity", 1.0),
					new KeyValuePair<string, object>("strokeWeight", strokeWeight))));

			return coordinates.VariableAssignment() + Environment.NewLine + (strokeDashed ? (lineSymbol.VariableAssignment() + Environment.NewLine) : string.Empty) + 
				polyline.VariableAssignment() + Environment.NewLine + polyline.Call(new JavascriptFunction.Call("setMap", map.Name));
		}

		public static string CreateMarkers(this IEnumerable<StopsBasic.StopBasic> stops, JavascriptVariable<JavascriptObject> map, bool zoomToCurrentPosition = false)
		{
			var markersInfo = new JavascriptVariable<JavascriptArray<JavascriptArray<object>>>("markersInfo", 
					new JavascriptArray<JavascriptArray<object>>(stops.Select(stop => new JavascriptArray<object> { "'" + stop.Name + "'", stop.Latitude, stop.Longitude, stop.ID })));

			var bounds = new JavascriptVariable<JavascriptObject>("bounds", new JavascriptObject("google.maps.LatLngBounds"));

			var cycle = new JavascriptControlStructures.For(new JavascriptVariable<int>("i", 0), (int)markersInfo.Content.Count);

			var marker = new JavascriptVariable<JavascriptObject>("marker", new JavascriptObject("google.maps.Marker",
				new JavascriptObject.Anonymous(
					new KeyValuePair<string, object>("title", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][0]"),
					new KeyValuePair<string, object>("position",
						new JavascriptObject.Anonymous(
							new KeyValuePair<string, object>("lat", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][1]"),
							new KeyValuePair<string, object>("lng", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][2]"))),
					new KeyValuePair<string, object>("map", map.Name))));			
			
			var popupWindow = new JavascriptVariable<JavascriptObject>("popupWindow", new JavascriptObject("google.maps.InfoWindow"));

			var content = new JavascriptVariable<string>("content", $"{ markersInfo.Name }[{ cycle.ControlVariable.Name }][3]");

			var innerFn = new JavascriptFunction.Anonymous();
			innerFn.AddInstruction(popupWindow.Call(new JavascriptFunction.Call("setContent", 
				new JavascriptFunction.Call("window.external.ShowDepartures", content.Name))).ToString);
			innerFn.AddInstruction(popupWindow.Call(new JavascriptFunction.Call("open", map.Name, marker.Name)).ToString);

			var onClickAnonymousFn = new JavascriptFunction.Anonymous(marker.Name, content.Name, popupWindow.Name);
			onClickAnonymousFn.AddInstruction(new JavascriptFunction.Return<JavascriptFunction.Anonymous>(innerFn).ToString);

			var onClickListener = new JavascriptFunction.Call("google.maps.event.addListener",
				marker.Name,
				new JavascriptString("click"),
				new JavascriptFunction.Anonymous.Application(onClickAnonymousFn, marker.Name, content.Content, popupWindow.Name)
				);

			cycle.AddInstruction(popupWindow.VariableAssignment);
			cycle.AddInstruction(marker.VariableAssignment);
			cycle.AddInstruction(onClickListener.ToString);
			if (!zoomToCurrentPosition)
				cycle.AddInstruction(bounds.Call(new JavascriptFunction.Call("extend", marker.Call(new JavascriptFunction.Call("getPosition")))).ToString);

			return zoomToCurrentPosition ? 
				markersInfo.VariableAssignment() + Environment.NewLine + cycle.ToString() : 
				markersInfo.VariableAssignment() + Environment.NewLine + bounds.VariableAssignment() + Environment.NewLine + cycle.ToString() + Environment.NewLine + map.Call(new JavascriptFunction.Call("fitBounds", bounds.Name));
		}

		public static double GetAverageLatitude(this IEnumerable<StopsBasic.StopBasic> stops) => stops.Select(s => s.Latitude).Average();
		public static double GetAverageLongitude(this IEnumerable<StopsBasic.StopBasic> stops) => stops.Select(s => s.Longitude).Average();
	}
}

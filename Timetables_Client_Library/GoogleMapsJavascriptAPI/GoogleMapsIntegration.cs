using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Structures.Basic;
using Timetables.Utilities;

namespace Timetables.Client
{
	public static class GoogleMaps
	{
		private static readonly string GoogleMapsApiKey = "AIzaSyB05TDGoGzSk_jzTvWYocWD_k1CiLuCgoY";
		public static IEnumerable<StopsBasic.StopBasic> GetStops(this Departure departure)
		{
			yield return DataFeedClient.Basic.Stops.FindByIndex(departure.StopID);
			foreach (var @is in departure.IntermediateStops)
				yield return DataFeedClient.Basic.Stops.FindByIndex(@is.StopID);
		}
		private static IEnumerable<StopsBasic.StopBasic> GetStops(this TripSegment ts)
		{
			yield return DataFeedClient.Basic.Stops.FindByIndex(ts.SourceStopID);
			foreach (var @is in ts.IntermediateStops)
				yield return DataFeedClient.Basic.Stops.FindByIndex(@is.StopID);
			yield return DataFeedClient.Basic.Stops.FindByIndex(ts.TargetStopID);
		}
		private static IEnumerable<StopsBasic.StopBasic> GetStops(this FootpathSegment fs)
		{
			yield return DataFeedClient.Basic.Stops.FindByIndex(fs.SourceStopID);
			yield return DataFeedClient.Basic.Stops.FindByIndex(fs.TargetStopID);
		}
		public static IEnumerable<StopsBasic.StopBasic> GetStops(this Journey journey)
		{
			foreach (var js in journey.JourneySegments)
			{
				if (js is TripSegment)
					foreach (var nestedJs in ((TripSegment)js).GetStops())
						yield return nestedJs;
				else
					foreach (var nestedJs in ((FootpathSegment)js).GetStops())
						yield return nestedJs;
			}
		}
		public static string GetMapWithMarkersAndPolylines(Departure departure)
		{
			var stops = departure.GetStops();
			var map = stops.CreateMap("map", stops.GetAverageLatitude(), stops.GetAverageLongitude(), 13);

			var initMap = new JavascriptFunction.Definition("initMap");

			initMap.AddInstruction(map.VariableAssignment);
			initMap.AddInstruction(stops.CreateMarkers(map).ToString);
			initMap.AddInstruction(stops.CreateSimplePolyline(map, 4.0, departure.LineColor).ToString);

			return GetHtmlStringConstant(initMap.ToString(), initMap.FunctionName);
		}
		public static string GetMapWithMarkersAndPolylines(Journey journey)
		{
			var stops = journey.GetStops();
			var map = stops.CreateMap("map", stops.GetAverageLatitude(), stops.GetAverageLongitude(), 15);

			var initMap = new JavascriptFunction.Definition("initMap");

			initMap.AddInstruction(map.VariableAssignment);
			initMap.AddInstruction(stops.CreateMarkers(map).ToString);

			for (int i = 0; i < journey.JourneySegments.Count; i++)
			{
				var js = journey.JourneySegments[i];
				
				if (js is TripSegment)
					initMap.AddInstruction(((TripSegment)js).GetStops().CreateSimplePolyline(map, 4.0, ((TripSegment)js).LineColor, false, i).ToString);

				else
					initMap.AddInstruction(((FootpathSegment)js).GetStops().CreateSimplePolyline(map, 4.0, Timetables.Utilities.CPColor.Gray, true, i).ToString);
			}

			return GetHtmlStringConstant(initMap.ToString(), initMap.FunctionName);
		}
		public static string GetMapWithMarkers(this IEnumerable<StopsBasic.StopBasic> stops)
		{	
			double lat, lon;
			int zoom;

			var loc = AsyncHelpers.RunSync<Position>(DataFeedClient.GeoWatcher.GetCurrentPosition);

			if (double.IsNaN(loc.Latitude) || double.IsNaN(loc.Longitude))
			{
				lat = stops.GetAverageLatitude();
				lon = stops.GetAverageLongitude();
				zoom = 15;
			}
			else
			{
				lat = loc.Latitude;
				lon = loc.Longitude;
				zoom = 17;
			}


			var map = stops.CreateMap("map", lat, lon, zoom);
			var initMap = new JavascriptFunction.Definition("initMap");


			initMap.AddInstruction(map.VariableAssignment);
			initMap.AddInstruction(stops.CreateMarkers(map, true).ToString);

			return GetHtmlStringConstant(initMap.ToString(), initMap.FunctionName);
		}

		private static string GetHtmlStringConstant(string customScriptCode, string functionName) => $@"<!DOCTYPE html>
<html>
  <head>
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1""/>
    <meta name=""viewport"" content=""initial-scale=1.0, user-scalable=no"">
    <meta charset=""utf-8"">
    <style>
      #map {{
        height: 100%;
		width: 100%;
      }}
      html, body {{
        height: 100%;
        margin: 0;
        padding: 0;
      }}
    </style>
  </head>
  <body>
    <div id=""map""></div>
    <script>
	{ customScriptCode }
    </script>
    <script async defer
    src=""https://maps.googleapis.com/maps/api/js?key={ GoogleMapsApiKey }&callback={ functionName }"">
    </script>
  </body>
</html>";
	}
}

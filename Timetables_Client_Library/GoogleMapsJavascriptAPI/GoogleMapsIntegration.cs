using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Structures.Basic;

namespace Timetables.Client
{
	public static class GoogleMaps
	{
		private static readonly string GoogleMapsApiKey = "AIzaSyB05TDGoGzSk_jzTvWYocWD_k1CiLuCgoY";
		private static IEnumerable<StopsBasic.StopBasic> GetStops(this Departure departure)
		{
			yield return DataFeed.Basic.Stops.FindByIndex(departure.StopID);
			foreach (var @is in departure.IntermediateStops)
				yield return DataFeed.Basic.Stops.FindByIndex(@is.StopID);
		}
		public static string GetMapWithMarkersAndPolylines(Departure departure)
		{
			var stops = departure.GetStops();		
			var map = stops.CreateMap("map", stops.GetAverageLatitude(), stops.GetAverageLongitude(), 13);

			var initMap = new JavascriptFunction.Definition("initMap");
			initMap.AddInstruction(map.VariableAssignment);
			initMap.AddInstruction(stops.CreateMarkers(map).ToString);

			return GetHtmlStringConstant(initMap.ToString(), initMap.FunctionName);
		}
		public static string GetMapWithMarkers(this IEnumerable<StopsBasic.StopBasic> stops)
		{	
			double lat, lon;
			int zoom;

			if (double.IsNaN(DataFeed.GeoWatcher.Position.Location.Latitude) || double.IsNaN(DataFeed.GeoWatcher.Position.Location.Longitude))
			{
				lat = stops.GetAverageLatitude();
				lon = stops.GetAverageLongitude();
				zoom = 15;
			}
			else
			{
				lat = DataFeed.GeoWatcher.Position.Location.Latitude;
				lon = DataFeed.GeoWatcher.Position.Location.Longitude;
				zoom = 17;
			}


			var map = stops.CreateMap("map", lat, lon, zoom);
			var initMap = new JavascriptFunction.Definition("initMap");


			initMap.AddInstruction(map.VariableAssignment);
			initMap.AddInstruction(stops.CreateMarkers(map).ToString);

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

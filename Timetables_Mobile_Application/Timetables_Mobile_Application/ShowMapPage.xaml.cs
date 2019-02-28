using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;
using Timetables.Structures.Basic;
using Timetables.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace Timetables.Application.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShowMapPage : ContentPage
	{
		private double GetLargestDistanceBetweenStops(IEnumerable<StopsBasic.StopBasic> stops)
		{
			double GetDistance(double Alat, double Alon, double Blat, double Blon)
			{
				double DegreesToRadians(double degrees) => (Math.PI / 180) * degrees;

				// Using Haversine formula. 
				double AlatR = DegreesToRadians(Alat);
				double AlonR = DegreesToRadians(Alon);
				double BlatR = DegreesToRadians(Blat);
				double BlonR = DegreesToRadians(Blon);
				double u = Math.Sin((BlatR - AlatR) / 2);
				double v = Math.Sin((BlonR - AlonR) / 2);
				return 500 + 2 * 6371 * Math.Asin(Math.Sqrt(u * u + Math.Cos(AlatR) * Math.Cos(BlatR) * v * v)) * 1000;
			}

			return stops.Max(A => stops.Max(B => GetDistance(A.Latitude, A.Longitude, B.Latitude, B.Longitude)));

		}
		private void SetMapScope(IEnumerable<StopsBasic.StopBasic> stops, bool zoomAtCurrentLoc = true, bool adaptiveZoom = false)
		{
			double lat, lon, distance;

			var loc = AsyncHelpers.RunSync(DataFeedClient.GeoWatcher.GetCurrentPosition);

			if (!zoomAtCurrentLoc || double.IsNaN(loc.Latitude) || double.IsNaN(loc.Longitude))
			{
				lat = stops.GetAverageLatitude();
				lon = stops.GetAverageLongitude();
				distance = adaptiveZoom ? GetLargestDistanceBetweenStops(stops) / 2 : 1000;
			}
			else
			{
				lat = loc.Latitude;
				lon = loc.Longitude;
				distance = 350;
			}

			map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.GoogleMaps.Position(lat, lon), new Distance(distance)), false);
		}
		private void DrawPolyline(IEnumerable<StopsBasic.StopBasic> stops, CPColor color)
		{

		}
		private void DrawMarkers(IEnumerable<StopsBasic.StopBasic> stops)
		{
			foreach (var stop in stops)
			{
				map.Pins.Add(new Pin
				{
					Type = PinType.Place,
					Position = new Xamarin.Forms.GoogleMaps.Position(stop.Latitude, stop.Longitude),
					Label = stop.ID.ToString()
				});
			}
		}
		public ShowMapPage()
		{
			InitializeComponent();

			Title = Settings.Localization.Map;

			SetMapScope(DataFeedClient.Basic.Stops);

			DrawMarkers(DataFeedClient.Basic.Stops);
		}
		public ShowMapPage(Departure dep)
		{
			InitializeComponent();

			Title = Settings.Localization.Map;

			var stops = dep.GetStops();

			SetMapScope(stops, false, true);

			DrawMarkers(stops);
		}
		public ShowMapPage(Journey journey)
		{
			InitializeComponent();

			Title = Settings.Localization.Map;

			var stops = journey.GetStops();

			SetMapScope(stops, false, true);

			DrawMarkers(stops);
		}
	}
}
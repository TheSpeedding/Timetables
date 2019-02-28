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
		private class StopPin
		{
			private bool secondClick = false;
			public StopsBasic.StopBasic Stop { get; }
			public bool SecondClick
			{
				get
				{
					var val = secondClick;
					secondClick = !secondClick;
					return val;
				}
			}
			public void Reset() => secondClick = false;
			public StopPin(StopsBasic.StopBasic stop) => Stop = stop;
		}
		private Dictionary<Pin, StopPin> markers = new Dictionary<Pin, StopPin>(); // Structure to assign stop to ping so it is possible to obtain earliest departure datetime.
		private Queue<StopPin> queue = new Queue<StopPin>(); // Reset second click if needed.
		private Func<StopsBasic.StopBasic, DateTime> findEarliestDeparture; // This varies throughout kinds of map.
		private bool useStopNotStation; // False if open from journey/departure window.
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
			Polyline line = new Polyline
			{
				StrokeColor = new Color((double)color.R / 255, (double)color.G / 255, (double)color.B / 255),
				StrokeWidth = 4
			};

			foreach (var stop in stops)
				line.Positions.Add(new Xamarin.Forms.GoogleMaps.Position(stop.Latitude, stop.Longitude));

			map.Polylines.Add(line);
		}
		private void DrawMarkers(IEnumerable<StopsBasic.StopBasic> stops)
		{
			foreach (var stop in stops)
			{
				var pin = new Pin
				{
					Type = PinType.Place,
					Position = new Xamarin.Forms.GoogleMaps.Position(stop.Latitude, stop.Longitude),
					Label = stop.Name
				};
				if (!markers.ContainsKey(pin))
				{
					map.Pins.Add(pin);
					markers.Add(pin, new StopPin(stop));
				}
			}
			map.PinClicked += MapPinClicked;
		}

		private async void MapPinClicked(object sender, PinClickedEventArgs e)
		{
			var entry = markers[e.Pin];

			while (queue.Count > 0)
			{
				var item = queue.Dequeue();
				if (item != entry)
					item.Reset();
			}

			queue.Enqueue(entry);

			if (entry.SecondClick)
			{
				var stop = entry.Stop;
				var dt = findEarliestDeparture(stop);
				var res = await Request.SendDepartureBoardRequestAsync(new StationInfoRequest(useStopNotStation ? stop.ID : stop.ParentStation.ID, dt, 5, !useStopNotStation));
				Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new DepartureBoardResultsPage(res, true, stop.ParentStation.Name), true));
			}
		}

		public ShowMapPage()
		{
			InitializeComponent();

			Title = Settings.Localization.Map;

			useStopNotStation = true;

			findEarliestDeparture = _ => DateTime.Now;

			SetMapScope(DataFeedClient.Basic.Stops);

			DrawMarkers(DataFeedClient.Basic.Stops);
		}
		public ShowMapPage(Departure departure)
		{
			InitializeComponent();

			useStopNotStation = false;

			findEarliestDeparture = stop =>
			{
				if (stop.ParentStation.ID == DataFeedClient.Basic.Stops.FindByIndex(departure.StopID).ParentStation.ID)
					return departure.DepartureDateTime;
				else
					return departure.IntermediateStops.Find(s => DataFeedClient.Basic.Stops.FindByIndex(s.StopID).ParentStation.ID == stop.ParentStation.ID).Arrival;
			};

			Title = Settings.Localization.Map;

			var stops = departure.GetStops();

			SetMapScope(stops, false, true);

			DrawMarkers(stops);

			DrawPolyline(stops, departure.LineColor);
		}
		public ShowMapPage(Journey journey)
		{
			InitializeComponent();

			useStopNotStation = false;

			findEarliestDeparture = stop =>
			{
				foreach (var js in journey.JourneySegments)
				{
					if (DataFeedClient.Basic.Stops.FindByIndex(js.SourceStopID).ParentStation.ID == stop.ParentStation.ID) return js.DepartureDateTime;

					if (js is TripSegment)
						foreach (var @is in (js as TripSegment).IntermediateStops)
						{
							if (DataFeedClient.Basic.Stops.FindByIndex(@is.StopID).ParentStation.ID == stop.ParentStation.ID) return @is.Arrival;
						}

					if (DataFeedClient.Basic.Stops.FindByIndex(js.TargetStopID).ParentStation.ID == stop.ParentStation.ID) return js.ArrivalDateTime;
				}

				throw new ArgumentException("Stop ID not found in the journey.");
			};

			Title = Settings.Localization.Map;

			var stops = journey.GetStops();

			SetMapScope(stops, false, true);

			DrawMarkers(stops);

			foreach (var js in journey.JourneySegments)
			{
				if (js is TripSegment)
					DrawPolyline(((TripSegment)js).GetStops(), ((TripSegment)js).LineColor);
				else
					DrawPolyline(((FootpathSegment)js).GetStops(), CPColor.Gray);
			}
		}
	}
}
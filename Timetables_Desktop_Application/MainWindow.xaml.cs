using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using Microsoft.Maps.MapControl.WPF;
using System.Device.Location;

namespace Timetables.Desktop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
			watcher.TryStart(false, TimeSpan.FromSeconds(5));
			
			myMap.Center = new Location(watcher.Position.Location.Latitude, watcher.Position.Location.Longitude);
			myMap.ZoomLevel = 18;

			foreach (var stop in Timetables.Client.DataFeedGlobals.BasicData.Stops)
			{
				var pushpin = new Pushpin();
				pushpin.MouseDoubleClick += Pushpin_MouseDoubleClick;
				pushpin.Location = new Location(stop.Latitude, stop.Longitude);
				myMap.Children.Add(pushpin);
			}
		}

		private void Pushpin_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var s = Timetables.Client.DataFeedGlobals.BasicData.Stops.FindByCoordinates(((Pushpin)sender).Location.Latitude, ((Pushpin)sender).Location.Longitude);
			using (var db = new Interop.DepartureBoardManaged(Timetables.Client.DataFeedGlobals.FullData, new Client.DepartureBoardRequest(s.ID, DateTime.Now, 1, false)))
			{
				db.ObtainDepartureBoard();
				var x = db.ShowDepartureBoard().Departures[0];
				MessageBox.Show($@"Zastávka {s.Name}. Nejbližší odjezd v {x.DepartureDateTime} linkou {x.LineLabel} směrem {x.Headsign}.");
			}
		}
	}
}
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
			var x = new Timetables.Structures.Basic.DataFeedBasic();
			foreach (var stop in x.Stops)
				myMap.Children.Add(new Pushpin
				{
					Location = new Location(stop.Latitude, stop.Longitude)
				});
		}
	}
}
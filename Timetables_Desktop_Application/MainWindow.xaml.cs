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
			using (var feed = new Timetables.Interop.DataFeedManaged())
			{
				var x = Timetables.Interop.DepartureBoardManaged.SendRequestAndGetResponse(feed, new Timetables.Client.DepartureBoardRequest(0, DateTime.Now, 10, true));
			}
		}		
	}
}
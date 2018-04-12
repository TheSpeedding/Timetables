﻿using System;
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
			var x = new Timetables.Structures.Basic.DataFeedBasic();
			using (var feed = new Timetables.Interop.DataFeedManaged())
			{
				using (var router = new Timetables.Interop.RouterManaged(feed, new Client.RouterRequest(0, 69, DateTime.Now, 30, 5, 1)))
				{
					router.ObtainJourneys();
					var xz = router.ShowJourneys();
				}
			}
		}
	}
}
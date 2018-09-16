using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Timetables.Client;
using WeifenLuo.WinFormsUI.Docking;

namespace Timetables.Application.Desktop
{
	public partial class ShowMapWindow : DockContent
	{
		public ShowMapWindow()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);			
			Text = Settings.Localization.Map;
			resultsWebBrowser.ObjectForScripting = new Interop.GoogleMapsScripting.General();
			resultsWebBrowser.DocumentText = GoogleMaps.GetMapWithMarkers(DataFeed.Basic.Stops);
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Timetables.Client;
using WeifenLuo.WinFormsUI.Docking;

namespace Timetables.Application.Desktop
{
	public partial class DepartureBoardResultsWindow : DockContent
	{
		public DepartureBoardResultsWindow(DepartureBoardResponse dbReponse, string stationName, DateTime dateTime)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			resultsWebBrowser.ObjectForScripting = new Timetables.Interop.DepartureBoardScripting(this);

			Text = $"Departures ({ dbReponse.Departures.Count }) - { stationName } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = dbReponse.TransformToHtml("xslt/DepartureBoardToHtml.xslt", "css/DepartureBoardToHtml.css");
		}
	}
}
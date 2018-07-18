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
		public List<Departure> Departures { get; set; }
		private DepartureBoardResultsWindow()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			resultsWebBrowser.ObjectForScripting = new Timetables.Interop.DepartureBoardScripting(this);
		}
		public DepartureBoardResultsWindow(DepartureBoardResponse dbReponse, string stationName, DateTime dateTime) : this()
		{
			Departures = dbReponse.Departures;

			Text = $"{ Settings.Localization.Departures } ({ dbReponse.Departures.Count }) - { stationName } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = dbReponse.TransformToHtml(Settings.DepartureBoardSimpleXslt.FullName, Settings.DepartureBoardSimpleCss.FullName);
		}
		public DepartureBoardResultsWindow(Departure departure) : this()
		{
			Departures = new List<Departure> { departure };

			Text = $"{ Settings.Localization.Departure } - {  DataFeed.Basic.Stops.FindByIndex(departure.StopID).Name } - { departure.DepartureDateTime.ToShortTimeString() } { departure.DepartureDateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = Departures[0].TransformToHtml(Settings.DepartureBoardDetailXslt.FullName, Settings.DepartureBoardDetailCss.FullName);
		}
	}
}
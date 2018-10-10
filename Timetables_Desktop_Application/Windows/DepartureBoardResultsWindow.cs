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
		private NewDepartureBoardWindow requestWindow;
		public DepartureBoardResponse Results { get; set; }
		private DepartureBoardResultsWindow(NewDepartureBoardWindow win = null)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			resultsWebBrowser.ObjectForScripting = new Timetables.Interop.DepartureBoardScripting(this);
			requestWindow = win;
		}
		public DepartureBoardResultsWindow(DepartureBoardResponse dbReponse, string stationName, DateTime dateTime, NewDepartureBoardWindow win = null) : this(win)
		{
			Results = dbReponse;

			Text = $"{ Settings.Localization.Departures } ({ dbReponse.Departures.Count }) - { stationName } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = dbReponse.TransformToHtml(Settings.DepartureBoardSimpleXslt.FullName, Settings.DepartureBoardSimpleCss.FullName);
		}
		public DepartureBoardResultsWindow(Departure departure, NewDepartureBoardWindow win = null) : this(win)
		{
			Results = new DepartureBoardResponse(new List<Departure> { departure });

			Text = $"{ Settings.Localization.Departure } - {  DataFeedDesktop.Basic.Stops.FindByIndex(departure.StopID).Name } - { departure.DepartureDateTime.ToShortTimeString() } { departure.DepartureDateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = Results.Departures[0].TransformToHtml(Settings.DepartureBoardDetailXslt.FullName, Settings.DepartureBoardDetailCss.FullName);
		}
		public void CloseThisAndReopenPrevious()
		{
			requestWindow?.Show(DockPanel, DockState);
			Close();
		}
	}
}
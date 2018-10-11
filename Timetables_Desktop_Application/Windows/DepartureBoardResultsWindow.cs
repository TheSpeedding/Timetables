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
		private DockContent requestWindow;
		public DepartureBoardResponse Results { get; set; }
		private DepartureBoardResultsWindow(DockContent win = null)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			resultsWebBrowser.ObjectForScripting = new Timetables.Interop.DepartureBoardScripting(this);
			requestWindow = win;
		}
		public DepartureBoardResultsWindow(DepartureBoardResponse dbReponse, string title, DateTime dateTime, DockContent win = null) : this(win)
		{
			Results = dbReponse;

			Text = $"{ Settings.Localization.Departures } ({ dbReponse.Departures.Count }) - { title } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = dbReponse.TransformToHtml(Settings.DepartureBoardSimpleXslt.FullName, Settings.DepartureBoardSimpleCss.FullName);
		}
		public DepartureBoardResultsWindow(Departure departure, bool stationInfo, DockContent win = null) : this(win)
		{
			Results = new DepartureBoardResponse(new List<Departure> { departure });

			Text = $"{ Settings.Localization.Departure } - { (stationInfo ? DataFeedDesktop.Basic.Stops.FindByIndex(departure.StopID).Name : departure.LineLabel) } - { departure.DepartureDateTime.ToShortTimeString() } { departure.DepartureDateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = Results.Departures[0].TransformToHtml(Settings.DepartureBoardDetailXslt.FullName, Settings.DepartureBoardDetailCss.FullName);
		}
		public void CloseThisAndReopenPrevious()
		{
			requestWindow?.Show(DockPanel, DockState);
			Close();
		}
	}
}
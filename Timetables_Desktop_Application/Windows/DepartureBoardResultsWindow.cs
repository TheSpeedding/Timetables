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
		public bool IsStationInfo { get; }
		public bool IsLineInfo => !IsStationInfo;
		private DepartureBoardResultsWindow(bool isStationInfo, DockContent win = null)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			resultsWebBrowser.ObjectForScripting = new Timetables.Interop.DepartureBoardScripting(this, isStationInfo);
			IsStationInfo = isStationInfo;
			requestWindow = win;
		}
		public DepartureBoardResultsWindow(DepartureBoardResponse dbReponse, string title, DateTime dateTime, bool isStationInfo, DockContent win = null) : this(isStationInfo, win)
		{
			Results = dbReponse;

			Text = $"{ Settings.Localization.Departures } ({ dbReponse.Departures.Count }) - { title } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = dbReponse.TransformToHtml(Settings.DepartureBoardSimpleXslt.FullName, Settings.DepartureBoardSimpleCss.FullName, Settings.OnLoadActionsJavaScript.FullName);
		}
		public DepartureBoardResultsWindow(Departure departure, bool isStationInfo, DockContent win = null) : this(isStationInfo, win)
		{
			Results = new DepartureBoardResponse(new List<Departure> { departure });

			Text = $"{ Settings.Localization.Departure } - { (isStationInfo ? DataFeedDesktop.Basic.Stops.FindByIndex(departure.StopID).Name : departure.LineLabel) } - { departure.DepartureDateTime.ToShortTimeString() } { departure.DepartureDateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = Results.Departures[0].TransformToHtml(Settings.DepartureBoardDetailXslt.FullName, Settings.DepartureBoardDetailCss.FullName, Settings.OnLoadActionsJavaScript.FullName);
		}
		public void CloseThisAndReopenPrevious()
		{
			requestWindow?.Show(DockPanel, DockState);
			Close();
		}
	}
}
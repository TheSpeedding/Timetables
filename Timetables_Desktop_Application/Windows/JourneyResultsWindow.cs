using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Timetables.Client;
using System.IO;
using System.Xml;

namespace Timetables.Application.Desktop
{
	public partial class JourneyResultsWindow : DockContent
	{
		private DockContent requestWindow;
		public RouterResponse Results { get; set; }
		private JourneyResultsWindow(DockContent win = null)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			resultsWebBrowser.ObjectForScripting = new Timetables.Interop.JourneyScripting(this);
			requestWindow = win;
		}
		public JourneyResultsWindow(RouterResponse rResponse, string source, string target, DateTime dateTime, DockContent win = null) : this(win)
		{
			Results = rResponse;

			Text = $"{ Settings.Localization.Journeys } ({ rResponse.Journeys.Count }) - { source } - { target } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = rResponse.TransformToHtml(Settings.JourneySimpleXslt.FullName,  Settings.JourneySimpleCss.FullName, Settings.OnLoadActionsJavaScript.FullName);
		}
		public JourneyResultsWindow(Journey journey, DockContent win = null) : this(win)
		{
			Results = new RouterResponse(new List<Journey> { journey });

			Text = $"{ Settings.Localization.Journey } - { DataFeedDesktop.Basic.Stops.FindByIndex(journey.JourneySegments[0].SourceStopID).Name } - { DataFeedDesktop.Basic.Stops.FindByIndex(journey.JourneySegments[journey.JourneySegments.Count - 1].TargetStopID).Name } - { journey.DepartureDateTime.ToShortTimeString() } { journey.DepartureDateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = Results.Journeys[0].TransformToHtml(Settings.JourneyDetailXslt.FullName, Settings.JourneyDetailCss.FullName, Settings.OnLoadActionsJavaScript.FullName);
		}
		public void CloseThisAndReopenPrevious()
		{
			requestWindow?.Show(DockPanel, DockState);
			Close();
		}
	}
}

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
		public RouterResponse Results { get; set; }
		private JourneyResultsWindow()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			resultsWebBrowser.ObjectForScripting = new Timetables.Interop.JourneyScripting(this);
		}
		public JourneyResultsWindow(RouterResponse rResponse, string source, string target, DateTime dateTime) : this()
		{
			Results = rResponse;	

			Text = $"{ Settings.Localization.Journeys } ({ rResponse.Journeys.Count }) - { source } - { target } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = rResponse.TransformToHtml(Settings.JourneySimpleXslt.FullName,  Settings.JourneySimpleCss.FullName);
		}
		public JourneyResultsWindow(Journey journey) : this()
		{
			Results = new RouterResponse(new List<Journey> { journey });

			Text = $"{ Settings.Localization.Journey } - { DataFeed.Basic.Stops.FindByIndex(journey.JourneySegments[0].SourceStopID).Name } - { DataFeed.Basic.Stops.FindByIndex(journey.JourneySegments[journey.JourneySegments.Count - 1].TargetStopID).Name } - { journey.DepartureDateTime.ToShortTimeString() } { journey.DepartureDateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = Results.Journeys[0].TransformToHtml(Settings.JourneyDetailXslt.FullName, Settings.JourneyDetailCss.FullName);
		}
	}
}

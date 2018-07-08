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
		public List<Journey> Journeys { get; set; }
		public WebBrowser WebBrowser => resultsWebBrowser;
		public JourneyResultsWindow()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			resultsWebBrowser.ObjectForScripting = new Timetables.Interop.JourneyScripting(this);
		}
		public JourneyResultsWindow(RouterResponse rResponse, string source, string target, DateTime dateTime) : this()
		{
			Journeys = rResponse.Journeys;			

			Text = $"Journeys ({ rResponse.Journeys.Count }) - { source } - { target } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = rResponse.TransformToHtml("xslt/JourneysSimpleToHtml.xslt", true);
		}
		public JourneyResultsWindow(Journey journey) : this()
		{
			Journeys = new List<Journey> { journey };

			Text = $"Journey - { DataFeedGlobals.Basic.Stops.FindByIndex(journey.JourneySegments[0].SourceStopID).Name } - { DataFeedGlobals.Basic.Stops.FindByIndex(journey.JourneySegments[journey.JourneySegments.Count - 1].TargetStopID).Name } - { journey.ArrivalDateTime.ToShortTimeString() } { journey.ArrivalDateTime.ToShortDateString() }";

			resultsWebBrowser.DocumentText = Journeys[0].TransformToHtml("xslt/JourneyDetailToHtml.xslt", true);
		}
	}
}

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
			resultsWebBrowser.ObjectForScripting = new GoogleMapsScripting.General();
			resultsWebBrowser.DocumentText = GoogleMaps.GetMapWithMarkers(DataFeedDesktop.Basic.Stops);
		}
		public ShowMapWindow(Departure departure)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			Text = $"{ Settings.Localization.Map } - { Settings.Localization.Departure } - {  DataFeedDesktop.Basic.Stops.FindByIndex(departure.StopID).Name } - { departure.DepartureDateTime.ToShortTimeString() } { departure.DepartureDateTime.ToShortDateString() }";
			resultsWebBrowser.ObjectForScripting = new GoogleMapsScripting.Departure(departure);
			resultsWebBrowser.DocumentText = GoogleMaps.GetMapWithMarkersAndPolylines(departure);
		}
		public ShowMapWindow(Journey journey)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			Text = $"{ Settings.Localization.Map } - { Settings.Localization.Journey } - { DataFeedDesktop.Basic.Stops.FindByIndex(journey.JourneySegments[0].SourceStopID).Name } - { DataFeedDesktop.Basic.Stops.FindByIndex(journey.JourneySegments[journey.JourneySegments.Count - 1].TargetStopID).Name } - { journey.DepartureDateTime.ToShortTimeString() } { journey.DepartureDateTime.ToShortDateString() }";
			resultsWebBrowser.ObjectForScripting = new GoogleMapsScripting.Journey(journey);
			resultsWebBrowser.DocumentText = GoogleMaps.GetMapWithMarkersAndPolylines(journey);
		}
	}
}
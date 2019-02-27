using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;
using Timetables.Interop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timetables.Application.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindJourneyResultsPage : ContentPage
	{
		public RouterResponse Response { get; }
		public FindJourneyResultsPage(Journey journey)
		{
			InitializeComponent();

			Title = Settings.Localization.Journey + " " + 
				DataFeedClient.Basic.Stops.FindByIndex(journey.JourneySegments.First().SourceStopID).ParentStation.Name + " - " +
				DataFeedClient.Basic.Stops.FindByIndex(journey.JourneySegments.Last().TargetStopID).ParentStation.Name;

			Response = new RouterResponse(new List<Journey> { journey });

			resultsWebView.Scripting = new JourneyScripting(resultsWebView, this);

			resultsWebView.Source = new HtmlWebViewSource
			{
				Html = Response.Journeys[0].TransformToHtml(
					PlatformDependentSettings.GetStream(Settings.JourneyDetailXslt),
					PlatformDependentSettings.GetStream(Settings.JourneyDetailCss),
					PlatformDependentSettings.GetStream(Settings.OnLoadActionsJavaScript)
					)
			};
		}
		public FindJourneyResultsPage(RouterResponse res, string source, string target)
		{
			InitializeComponent();

			Title = Settings.Localization.Journey + " " + source + " - " + target;

			Response = res;
			
			resultsWebView.Scripting = new JourneyScripting(resultsWebView, this);

			resultsWebView.Source = new HtmlWebViewSource
			{
				Html = Response.TransformToHtml(
					PlatformDependentSettings.GetStream(Settings.JourneySimpleXslt),
					PlatformDependentSettings.GetStream(Settings.JourneySimpleCss),
					PlatformDependentSettings.GetStream(Settings.OnLoadActionsJavaScript)
					)
			}; 
		}
	}
}
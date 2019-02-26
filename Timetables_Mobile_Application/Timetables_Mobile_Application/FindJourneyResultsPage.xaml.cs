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
	public partial class FindJourneyResults : ContentPage
	{
		public RouterResponse Response { get; }
		public FindJourneyResults(Journey journey)
		{
			InitializeComponent();

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
		public FindJourneyResults(RouterResponse res)
		{
			InitializeComponent();

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
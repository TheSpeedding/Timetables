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
		public FindJourneyResults(RouterResponse res)
		{
			InitializeComponent();

			resultsWebView.Scripting = new Scripting(resultsWebView);

			resultsWebView.Source = new HtmlWebViewSource
			{
				Html = res.TransformToHtml(
					PlatformDependentSettings.GetStream(Settings.JourneySimpleXslt),
					PlatformDependentSettings.GetStream(Settings.JourneySimpleCss),
					PlatformDependentSettings.GetStream(Settings.OnLoadActionsJavaScript)
					)
			}; 
		}
	}
}
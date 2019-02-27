using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timetables.Application.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DepartureBoardResultsPage : ContentPage
	{
		public DepartureBoardResponse Response { get; }
		public DepartureBoardResultsPage(Departure dep)
		{
			InitializeComponent();

			Response = new DepartureBoardResponse(new List<Departure> { dep });

			resultsWebView.Scripting = new DepartureBoardScripting(resultsWebView, this);

			resultsWebView.Source = new HtmlWebViewSource
			{
				Html = Response.Departures[0].TransformToHtml(
					PlatformDependentSettings.GetStream(Settings.DepartureBoardDetailXslt),
					PlatformDependentSettings.GetStream(Settings.DepartureBoardDetailCss),
					PlatformDependentSettings.GetStream(Settings.OnLoadActionsJavaScript)
					)
			};
		}
		public DepartureBoardResultsPage(DepartureBoardResponse res)
		{
			InitializeComponent();

			Response = res;

			resultsWebView.Scripting = new DepartureBoardScripting(resultsWebView, this);

			resultsWebView.Source = new HtmlWebViewSource
			{
				Html = Response.TransformToHtml(
					PlatformDependentSettings.GetStream(Settings.DepartureBoardSimpleXslt),
					PlatformDependentSettings.GetStream(Settings.DepartureBoardSimpleCss),
					PlatformDependentSettings.GetStream(Settings.OnLoadActionsJavaScript)
					)
			};
		}
	}
}
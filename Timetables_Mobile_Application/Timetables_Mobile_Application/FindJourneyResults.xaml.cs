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
	public partial class FindJourneyResults : ContentPage
	{
		public FindJourneyResults (RouterResponse res)
		{
			InitializeComponent();

			resultsWebView.Source = new HtmlWebViewSource
			{
				Html = res.TransformToHtml(
					Settings.GetStream(Settings.JourneySimpleXslt),
					Settings.GetStream(Settings.JourneySimpleCss),
					Settings.GetStream(Settings.OnLoadActionsJavaScript)
					)
			};
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timetables.Application.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InfoPage : ContentPage
	{
		protected InfoPage ()
		{
			InitializeComponent ();
			resultsWebView.Source = new HtmlWebViewSource { Html = Request.LoadingHtml(Settings.Localization.PleaseWaitDownloading) };
		}
		protected async void LoadContent(Uri uri, System.IO.FileInfo xslt, System.IO.FileInfo css)
		{
			try
			{
				if (uri == null) throw new ArgumentException();

				using (var wc = new WebClient())
				{
					wc.Encoding = Encoding.UTF8;

#if true // Async version, not blocking UI.					
					var downloading = wc.DownloadStringTaskAsync(uri);

					if (await Task.WhenAny(downloading, Task.Delay(Settings.TimeoutDuration)) == downloading && downloading.Status == TaskStatus.RanToCompletion)
						resultsWebView.Source = new HtmlWebViewSource { Html = (await downloading).TransformToHtml(
							PlatformDependentSettings.GetStream(xslt), PlatformDependentSettings.GetStream(css)) };
					else
						throw new WebException();
#else
					resultsWebView.Source = new HtmlWebViewSource { Html = wc.DownloadString(uri).TransformToHtml(xslt.FullName, css.FullName) };
#endif
				}
			}

			catch (ArgumentException)
			{
				resultsWebView.Source = new HtmlWebViewSource { Html = "Invalid Uri address." };
			}

			catch (WebException)
			{
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnreachableHost);
			}

		}
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public class LockoutsPage : InfoPage
	{
		public LockoutsPage() : base()
		{
			Title = Settings.Localization.Lockouts;
			LoadContent(Settings.Lockouts, Settings.LockoutsXslt, Settings.LockoutsCss);
		}
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public class ExtraordinaryEventsPage : InfoPage
	{
		public ExtraordinaryEventsPage() : base()
		{
			Title = Settings.Localization.ExtraordinaryEvents;
			LoadContent(Settings.ExtraordinaryEvents, Settings.ExtraordinaryEventsXslt, Settings.ExtraordinaryEventsCss);
		}
	}
}
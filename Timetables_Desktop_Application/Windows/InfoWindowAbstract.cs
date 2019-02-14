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
	public abstract partial class InfoWindowAbstract : DockContent
	{
		protected void Initialize()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);
			
			resultsWebBrowser.DocumentText = Request.LoadingHtml(Settings.Localization.PleaseWaitDownloading);
		}

		protected void SetWebbrowserContent(string content) => resultsWebBrowser.DocumentText = content;

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
						resultsWebBrowser.Document.Write((await downloading).TransformToHtml(xslt.FullName, css.FullName));
					else
						throw new WebException();
#else
					resultsWebBrowser.DocumentText = wc.DownloadString(uri).TransformToHtml(xslt.FullName, css.FullName);
#endif
				}
			}

			catch (ArgumentException)
			{
				resultsWebBrowser.Document.Write("Invalid Uri address.");
			}

			catch (WebException)
			{
				resultsWebBrowser.Document.Write("Error while getting information from remote server. Check your internet connection or try again later.");
			}

		}
	}
}
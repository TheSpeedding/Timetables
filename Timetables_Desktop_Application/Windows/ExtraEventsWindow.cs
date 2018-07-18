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
	public partial class ExtraEventsWindow : DockContent
	{
		public ExtraEventsWindow()
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			Text = Settings.Localization.ExtraordinaryEvents;

			try
			{
				if (Settings.ExtraordinaryEvents == null) throw new ArgumentException();

				using (var wc = new WebClient())
				{
					wc.Encoding = Encoding.UTF8;
					resultsWebBrowser.DocumentText = wc.DownloadString(Settings.ExtraordinaryEvents).TransformToHtml(Settings.ExtraordinaryEventsXslt.FullName, Settings.ExtraordinaryEventsCss.FullName);
				}
			}

			catch (ArgumentException)
			{
				resultsWebBrowser.DocumentText = "Invalid Uri address.";
			}

			catch (WebException)
			{
				resultsWebBrowser.DocumentText = "Error while getting information from remote server. Check your internet connection or try again later.";
			}
			
		}
	}
}
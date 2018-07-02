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
		public JourneyResultsWindow(RouterResponse jResponse, string source, string target, DateTime dateTime)
		{
			InitializeComponent();
			Settings.Theme.Apply(this);

			resultsWebBrowser.ObjectForScripting = Timetables.Interop.Scripting.ObjectForScripting;

			Text = $"Journeys ({ jResponse.Journeys.Count }) - { source } - { target } - { dateTime.ToShortTimeString() } { dateTime.ToShortDateString() }";
			
			resultsWebBrowser.DocumentText = jResponse.ConvertObjectToTransformedString("JourneysSimpleToHtml.xslt", true);
		}
	}
}

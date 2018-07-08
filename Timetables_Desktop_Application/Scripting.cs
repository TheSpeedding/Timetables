using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Application.Desktop;
using Timetables.Client;

namespace Timetables.Interop
{
	/// <summary>
	/// Offers scripting for journeys window.
	/// </summary>
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[System.Runtime.InteropServices.ComVisible(true)]
	public class JourneyScripting : Scripting
	{
		private JourneyResultsWindow window;
		private Stack<string> navigation = new Stack<string>();

		/// <summary>
		/// Initializes the object.
		/// </summary>
		/// <param name="window">Window that is relevant for this object.</param>
		public JourneyScripting(JourneyResultsWindow window) => this.window = window;

		/// <summary>
		/// Navigates back to the previous content.
		/// </summary>
		public void GoBack()
		{
			window.WebBrowser.Document.OpenNew(false);
			window.WebBrowser.Document.Write(navigation.Pop());
		}

		/// <summary>
		/// Navigates forward to the following content.
		/// </summary>
		/// <param name="content">New string content.</param>
		public void GoForward(string content)
		{
			navigation.Push(window.WebBrowser.DocumentText);
			window.WebBrowser.Document.OpenNew(false);
			window.WebBrowser.Document.Write(content);
		}
		
		/// <summary>
		/// Navigates forward to the following content.
		/// </summary>
		/// <param name="index">Index of the journey.</param>
		public void ShowJourneyDetail(int index) => GoForward(window.Journeys[index].TransformToHtml("JourneyDetailToHtml.xslt", true));
	}

	/// <summary>
	/// Offers scripting for departure boards window.
	/// </summary>
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[System.Runtime.InteropServices.ComVisible(true)]
	public class DepartureBoardScripting : Scripting
	{
		private DepartureBoardResultsWindow window;

		/// <summary>
		/// Initializes the object.
		/// </summary>
		/// <param name="window">Window that is relevant for this object.</param>
		public DepartureBoardScripting(DepartureBoardResultsWindow window) => this.window = window;
	}
}

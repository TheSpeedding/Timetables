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
		public void GoBack() => window.resultsWebBrowser.DocumentText = navigation.Pop();

		/// <summary>
		/// Navigates forward to the following content.
		/// </summary>
		/// <param name="content">New string content.</param>
		public void GoForward(string content)
		{
			navigation.Push(window.resultsWebBrowser.DocumentText);
			window.resultsWebBrowser.DocumentText = content;
		}

		/// <summary>
		/// Navigates forward to the following content.
		/// </summary>
		/// <param name="journey">New journey content.</param>
		public void GoForward(Journey journey) => GoForward(journey.TransformToHtml("JourneyDetailToHtml.xslt", true));

		/// <summary>
		/// Navigates forward to the following content.
		/// </summary>
		/// <param name="index">Index of the journey.</param>
		public void GoForward(int index) => GoForward(window.Journeys[index]);
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

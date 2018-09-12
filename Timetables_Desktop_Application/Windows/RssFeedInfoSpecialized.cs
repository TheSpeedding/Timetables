using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Application.Desktop
{
	/// <summary>
	/// Specialized window for extraordinary events.
	/// </summary>
	public class ExtraEventsWindow : RssFeedInfoWindow
	{
		public ExtraEventsWindow()
		{
			Initialize();
			Text = Settings.Localization.ExtraordinaryEvents;
			Load += (object sender, EventArgs e) => LoadContent(Settings.ExtraordinaryEvents, Settings.ExtraordinaryEventsXslt, Settings.ExtraordinaryEventsCss);
		}
	}
	/// <summary>
	/// Specialized window for lockouts.
	/// </summary>
	public class LockoutsWindow : RssFeedInfoWindow
	{
		public LockoutsWindow()
		{
			Initialize();
			Text = Settings.Localization.Lockouts;
			Load += (object sender, EventArgs e) => LoadContent(Settings.Lockouts, Settings.LockoutsXslt, Settings.LockoutsCss);
		}
	}
}

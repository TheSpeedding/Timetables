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
	public class ExtraEventsWindow : AbstractInfoWindow
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
	public class LockoutsWindow : AbstractInfoWindow
	{
		public LockoutsWindow()
		{
			Initialize();
			Text = Settings.Localization.Lockouts;
			Load += (object sender, EventArgs e) => LoadContent(Settings.Lockouts, Settings.LockoutsXslt, Settings.LockoutsCss);
		}
	}

	public class ShowMapWindow : AbstractInfoWindow
	{
		public ShowMapWindow()
		{
			Initialize();
			Text = Settings.Localization.Map;
			Load += (object sender, EventArgs e) => SetWebbrowserContent(Client.GoogleMaps.GetMapWithMarkers(Client.DataFeed.Basic.Stops));

			System.Windows.Forms.Clipboard.SetText(Timetables.Client.GoogleMaps.GetMapWithMarkers(Timetables.Client.DataFeed.Basic.Stops));
		}
	}
}

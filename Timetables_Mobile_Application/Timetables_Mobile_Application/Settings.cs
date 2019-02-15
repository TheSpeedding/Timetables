using System;
using System.IO;
using System.Net;
using System.Xml;
using Timetables.Client;

namespace Timetables.Application.Mobile
{
	/// <summary>
	/// Settings for the application.
	/// </summary>
	static class Settings
	{
		/// <summary>
		/// Language used in the application.
		/// </summary>
		public static Localization Localization { get; set; }
		/// <summary>
		/// Uri to the site that offers RSS feed for extraordinary events in traffic.
		/// </summary>
		public static Uri ExtraordinaryEvents { get; private set; }
		/// <summary>
		/// Uri to the site that offers RSS feed for lockouts in traffic.
		/// </summary>
		public static Uri Lockouts { get; private set; }
		/// <summary>
		/// Path to journey detail XSLT stylesheet.
		/// </summary>
		public static FileInfo JourneyDetailXslt => new FileInfo("xslt/JourneyDetailToHtml.xslt");
		/// <summary>
		/// Path to journey detail CSS stylesheet.
		/// </summary>
		public static FileInfo JourneyDetailCss => new FileInfo("css/JourneyDetailToHtml.css");
		/// <summary>
		/// Path to journey simple XSLT stylesheet.
		/// </summary>
		public static FileInfo JourneySimpleXslt => new FileInfo("xslt/JourneySimpleToHtml.xslt");
		/// <summary>
		/// Path to journey simple CSS stylesheet.
		/// </summary>
		public static FileInfo JourneySimpleCss => new FileInfo("css/JourneySimpleToHtml.css");
		/// <summary>
		/// Path to departure board simple XSLT stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardSimpleXslt => new FileInfo("xslt/DepartureBoardSimpleToHtml.xslt");
		/// <summary>
		/// Path to departure board simple CSS stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardSimpleCss => new FileInfo("css/DepartureBoardSimpleToHtml.css");
		/// <summary>
		/// Path to departure board map XSLT stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardInMapXslt => new FileInfo("xslt/DepartureBoardInMapToHtml.xslt");
		/// <summary>
		/// Path to departure board map CSS stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardInMapCss => new FileInfo("css/DepartureBoardInMapToHtml.css");
		/// <summary>
		/// Path to lockouts XSLT stylesheet.
		/// </summary>
		public static FileInfo LockoutsXslt => new FileInfo("xslt/LockoutsToHtml.xslt");
		/// <summary>
		/// Path to lockouts CSS stylesheet.
		/// </summary>
		public static FileInfo LockoutsCss => new FileInfo("css/LockoutsToHtml.css");
		/// <summary>
		/// Path to extraordinary event XSLT stylesheet.
		/// </summary>
		public static FileInfo ExtraordinaryEventsXslt => new FileInfo("xslt/ExtraordinaryEventsToHtml.xslt");
		/// <summary>
		/// Path to extraordinary events CSS stylesheet.
		/// </summary>
		public static FileInfo ExtraordinaryEventsCss => new FileInfo("css/ExtraordinaryEventsToHtml.css");
		/// <summary>
		/// Path to script file with onLoad actions.
		/// </summary>
		public static FileInfo OnLoadActionsJavaScript => new FileInfo("js/OnLoadActions.js");
		/// <summary>
		/// Timeout duration while waiting for connection with the server.
		/// </summary>
		public static int TimeoutDuration { get; } = 15000;
		/// <summary>
		/// Loads the settings.
		/// </summary>
		public static void Load()
		{
			XmlDocument settings = new XmlDocument();
			settings.Load(".settings");

			Localization = Localization.GetTranslation(settings.GetElementsByTagName("Language")?[0].InnerText);
			
			Client.DataFeedClient.ServerIpAddress = settings.GetElementsByTagName("ServerIp")[0].InnerText == string.Empty ? null : IPAddress.Parse(settings.GetElementsByTagName("ServerIp")[0].InnerText);

			Client.DataFeedClient.RouterPortNumber = settings.GetElementsByTagName("RouterPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("RouterPort")[0].InnerText);

			Client.DataFeedClient.DepartureBoardPortNumber = settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText);

			Client.DataFeedClient.BasicDataPortNumber = settings.GetElementsByTagName("BasicDataPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("BasicDataPort")[0].InnerText);

			Lockouts = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("LockoutsUri")[0].InnerText);

			ExtraordinaryEvents = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText);
		}
		/// <summary>
		/// Saves current settings.
		/// </summary>
		public static void Save(bool afterException = false)
		{
			if (!System.IO.File.Exists(".settings") || afterException)
				using (var sw = new System.IO.StreamWriter(".settings"))
					sw.Write("<Timetables></Timetables>");

			XmlDocument settings = new XmlDocument();
			settings.Load(".settings");							

			void CreateElementIfNotExist(params string[] names)
			{
				foreach (var name in names)
					if (settings.GetElementsByTagName(name).Count == 0)
						settings.DocumentElement.AppendChild(settings.CreateElement(name));
			}

			CreateElementIfNotExist("Language", "ExtraEventsUri", "LockoutsUri", "ServerIp", "RouterPort", "DepartureBoardPort", "BasicDataPort");

			settings.GetElementsByTagName("Language")[0].InnerText = Localization.ToString();
			
			settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText = ExtraordinaryEvents == null ? string.Empty : ExtraordinaryEvents.AbsoluteUri;

			settings.GetElementsByTagName("LockoutsUri")[0].InnerText = Lockouts == null ? string.Empty : Lockouts.AbsoluteUri;
			
			settings.GetElementsByTagName("ServerIp")[0].InnerText = Client.DataFeedClient.ServerIpAddress.ToString();

			settings.GetElementsByTagName("RouterPort")[0].InnerText = Client.DataFeedClient.RouterPortNumber.ToString();

			settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText = Client.DataFeedClient.DepartureBoardPortNumber.ToString();

			settings.GetElementsByTagName("BasicDataPort")[0].InnerText = Client.DataFeedClient.BasicDataPortNumber.ToString();

			settings.Save(".settings");
		}
	}	
}

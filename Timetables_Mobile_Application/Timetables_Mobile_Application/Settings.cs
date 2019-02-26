using System;
using System.IO;
using System.Net;
using System.Xml;
using Timetables.Client;

// Settings class should be accessible from executable application.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Timetables_Mobile_Application.Android")]

namespace Timetables.Application.Mobile
{
	/// <summary>
	/// Class containing settings that varies through the platforms.
	/// </summary>
	internal static class PlatformDependentSettings
	{
		/// <summary>
		/// Opens a filestream with given filename.
		/// </summary>
		/// <param name="file">File.</param>
		/// <returns>Filestream.</returns>
		public delegate Stream GetStreamHandler(System.IO.FileInfo file);
		/// <summary>
		/// Gets stream for the file.
		/// </summary>
		public static GetStreamHandler GetStream { get; set; }
		/// <summary>
		/// Callback to show a message to the user. 
		/// </summary>
		public static Action<string> ShowMessage { get; set; }
		/// <summary>
		/// Sets the base path to the given path.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void SetBasePath(string path) => DataFeedClient.BasePath = path;
	}
	/// <summary>
	/// Settings for the application.
	/// </summary>
	internal static class Settings
	{
		/// <summary>
		/// Reference to the settings file location.
		/// </summary>
		public static FileInfo SettingsFile { get; } = new FileInfo("settings.xml");
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
		public static FileInfo JourneyDetailCss => new FileInfo("css/JourneyDetailToHtmlLR.css");
		/// <summary>
		/// Path to journey simple XSLT stylesheet.
		/// </summary>
		public static FileInfo JourneySimpleXslt => new FileInfo("xslt/JourneySimpleToHtml.xslt");
		/// <summary>
		/// Path to journey simple CSS stylesheet.
		/// </summary>
		public static FileInfo JourneySimpleCss => new FileInfo("css/JourneySimpleToHtmlLR.css");
		/// <summary>
		/// Path to departure board simple XSLT stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardSimpleXslt => new FileInfo("xslt/DepartureBoardSimpleToHtml.xslt");
		/// <summary>
		/// Path to departure board simple CSS stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardSimpleCss => new FileInfo("css/DepartureBoardSimpleToHtmlLR.css");
		/// <summary>
		/// Path to departure board map XSLT stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardInMapXslt => new FileInfo("xslt/DepartureBoardInMapToHtml.xslt");
		/// <summary>
		/// Path to departure board map CSS stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardInMapCss => new FileInfo("css/DepartureBoardInMapToHtmlLR.css");
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
		/// Determines whether subway is allowed in search results.
		/// </summary>
		public static bool AllowSubway { get; set; }
		/// <summary>
		/// Determines whether tram is allowed in search results.
		/// </summary>
		public static bool AllowTram { get; set; }
		/// <summary>
		/// Determines whether bus is allowed in search results.
		/// </summary>
		public static bool AllowBus { get; set; }
		/// <summary>
		/// Determines whether ship is allowed in search results.
		/// </summary>
		public static bool AllowShip { get; set; }
		/// <summary>
		/// Determines whether train is allowed in search results.
		/// </summary>
		public static bool AllowTrain { get; set; }
		/// <summary>
		/// Determines whether cablecar is allowed in search results.
		/// </summary>
		public static bool AllowCablecar { get; set; }
		/// <summary>
		/// Coefficient that are transfers multiplied by in searching.
		/// </summary>
		public static double WalkingSpeedCoefficient { get; set; }
		/// <summary>
		/// Loads the settings.
		/// </summary>
		public static void Load()
		{
			XmlDocument settings = new XmlDocument();
			settings.Load(PlatformDependentSettings.GetStream(SettingsFile));

			Localization = Localization.GetTranslation(new Tuple<Stream, string>(
				PlatformDependentSettings.GetStream(new FileInfo("loc/" + settings.GetElementsByTagName("Language")?[0].InnerText + ".xml")),
				settings.GetElementsByTagName("Language")?[0].InnerText));
			
			Client.DataFeedClient.ServerIpAddress = settings.GetElementsByTagName("ServerIp")[0].InnerText == string.Empty ? null : IPAddress.Parse(settings.GetElementsByTagName("ServerIp")[0].InnerText);

			Client.DataFeedClient.RouterPortNumber = settings.GetElementsByTagName("RouterPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("RouterPort")[0].InnerText);

			Client.DataFeedClient.DepartureBoardPortNumber = settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText);

			Client.DataFeedClient.BasicDataPortNumber = settings.GetElementsByTagName("BasicDataPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("BasicDataPort")[0].InnerText);

			Lockouts = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("LockoutsUri")[0].InnerText);

			ExtraordinaryEvents = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText);

			AllowSubway = bool.Parse(settings.GetElementsByTagName("AllowSubway")[0].InnerText);

			AllowTram = bool.Parse(settings.GetElementsByTagName("AllowTram")[0].InnerText);

			AllowCablecar = bool.Parse(settings.GetElementsByTagName("AllowCablecar")[0].InnerText);

			AllowBus = bool.Parse(settings.GetElementsByTagName("AllowBus")[0].InnerText);

			AllowTrain = bool.Parse(settings.GetElementsByTagName("AllowTrain")[0].InnerText);

			AllowShip = bool.Parse(settings.GetElementsByTagName("AllowShip")[0].InnerText);

			WalkingSpeedCoefficient = double.Parse(settings.GetElementsByTagName("WalkingSpeedCoefficient")[0].InnerText);
		}
		/// <summary>
		/// Saves current settings.
		/// </summary>
		public static void Save()
		{
			XmlDocument settings = new XmlDocument();
			settings.Load(PlatformDependentSettings.GetStream(SettingsFile));

			void CreateElementIfNotExist(params string[] names)
			{
				foreach (var name in names)
					if (settings.GetElementsByTagName(name).Count == 0)
						settings.DocumentElement.AppendChild(settings.CreateElement(name));
			}

			CreateElementIfNotExist("Language", "ExtraEventsUri", "LockoutsUri", "ServerIp", "RouterPort", "DepartureBoardPort", "BasicDataPort",
				"AllowSubway", "AllowTram", "AllowCablecar", "AllowBus", "AllowTrain", "AllowShip", "WalkingSpeedCoefficient");

			settings.GetElementsByTagName("Language")[0].InnerText = Localization.ToString();
			
			settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText = ExtraordinaryEvents == null ? string.Empty : ExtraordinaryEvents.AbsoluteUri;

			settings.GetElementsByTagName("LockoutsUri")[0].InnerText = Lockouts == null ? string.Empty : Lockouts.AbsoluteUri;
			
			settings.GetElementsByTagName("ServerIp")[0].InnerText = Client.DataFeedClient.ServerIpAddress.ToString();

			settings.GetElementsByTagName("RouterPort")[0].InnerText = Client.DataFeedClient.RouterPortNumber.ToString();

			settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText = Client.DataFeedClient.DepartureBoardPortNumber.ToString();

			settings.GetElementsByTagName("BasicDataPort")[0].InnerText = Client.DataFeedClient.BasicDataPortNumber.ToString();

			settings.GetElementsByTagName("AllowSubway")[0].InnerText = AllowSubway.ToString();

			settings.GetElementsByTagName("AllowTram")[0].InnerText = AllowTram.ToString();

			settings.GetElementsByTagName("AllowCablecar")[0].InnerText = AllowCablecar.ToString();

			settings.GetElementsByTagName("AllowBus")[0].InnerText = AllowBus.ToString();

			settings.GetElementsByTagName("AllowTrain")[0].InnerText = AllowTrain.ToString();

			settings.GetElementsByTagName("AllowShip")[0].InnerText = AllowShip.ToString();

			settings.GetElementsByTagName("WalkingSpeedCoefficient")[0].InnerText = WalkingSpeedCoefficient.ToString();

			settings.Save(PlatformDependentSettings.GetStream(SettingsFile));
		}
		/// <summary>
		/// Gets predefined means of transport that can be used.
		/// </summary>
		public static MeanOfTransport GetMoT()
		{
			MeanOfTransport mot = 0;

			if (AllowBus) mot |= MeanOfTransport.Bus;
			if (AllowCablecar) mot |= MeanOfTransport.CableCar | MeanOfTransport.Funicular | MeanOfTransport.Gondola;
			if (AllowShip) mot |= MeanOfTransport.Ship;
			if (AllowSubway) mot |= MeanOfTransport.Subway;
			if (AllowTrain) mot |= MeanOfTransport.Rail;
			if (AllowTram) mot |= MeanOfTransport.Tram;

			return mot;
		}
		/// <summary>
		/// Loads data feed.
		/// </summary>
		public static async void LoadDataFeedAsync()
		{
			try
			{
				await DataFeedClient.DownloadAsync(false, TimeoutDuration);
			}
			catch
			{
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnreachableHost);
			}
		}
	}	
}

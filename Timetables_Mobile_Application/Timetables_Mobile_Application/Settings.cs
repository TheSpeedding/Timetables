﻿using Plugin.Geolocator;
using System;
using System.IO;
using System.Net;
using System.Xml;
using Timetables.Client;
using Timetables.Utilities;

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
		/// Gets localizations in assets.
		/// </summary>
		public static Func<string[]> GetLocalizations { get; set; }
		/// <summary>
		/// Gets stream for the file.
		/// </summary>
		public static Func<FileInfo, Stream> GetStream { get; set; }
		/// <summary>
		/// Callback to show a message to the user. 
		/// </summary>
		public static Action<string> ShowMessage { get; set; }
		/// <summary>
		/// Shows dialog. This is a delegate to a method, where the first argument is a text to be shown and the second argument is an action to be performed when the submit button is clicked, where the argument is content of the user input.
		/// </summary>
		public static Action<string, Action<string>> ShowDialog { get; set; }
		/// <summary>
		/// Base path to the data folder.
		/// </summary>
		public static string BasePath
		{
			get
			{
				return DataFeedClient.BasePath;
			}
			set
			{
				if (!Directory.Exists(value))
					Directory.CreateDirectory(value);
				DataFeedClient.BasePath = value;
			}
		}
	}
	/// <summary>
	/// Settings for the application.
	/// </summary>
	internal static class Settings
	{
		/// <summary>
		/// If set to false, cache is updated using Wi-Fi only.
		/// </summary>
		public static bool UseCellularsToUpdateCache { get; set; } = false;
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
		/// Path to lockouts XSLT stylesheet.
		/// </summary>
		public static FileInfo LockoutsXslt => new FileInfo("xslt/LockoutsToHtml.xslt");
		/// <summary>
		/// Path to lockouts CSS stylesheet.
		/// </summary>
		public static FileInfo LockoutsCss => new FileInfo("css/LockoutsToHtml.css");
		/// <summary>
		/// Path to departure board detail XSLT stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardDetailXslt => new FileInfo("xslt/DepartureBoardDetailToHtml.xslt");
		/// <summary>
		/// Path to departure board detail CSS stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardDetailCss => new FileInfo("css/DepartureBoardDetailToHtmlLR.css");
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
		public static int TimeoutDuration { get; } = 5000;
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
		/// Copies the settings file from assets to local directory.
		/// </summary>
		private static void CopyFromAssets()
		{
			using (var sw = new StreamWriter(DataFeedClient.BasePath + SettingsFile, false))
			using (var sr = new StreamReader(PlatformDependentSettings.GetStream(SettingsFile)))
				sw.Write(sr.ReadToEnd());
		}
		/// <summary>
		/// Loads the settings.
		/// </summary>
		public static void Load()
		{
			// First launch = copy settings from assets to temporary path.
			if (!File.Exists(DataFeedClient.BasePath + SettingsFile))
				CopyFromAssets();			

			XmlDocument settings = new XmlDocument();

			try
			{
				settings.Load(DataFeedClient.BasePath + SettingsFile);
			}
			catch
			{
				CopyFromAssets();
				settings.Load(DataFeedClient.BasePath + SettingsFile);
			}

			try
			{
				var locName = settings.GetElementsByTagName("Language")?[0].InnerText;
				Localization = locName == "English" ?
					Localization.GetTranslation("English") :
					Localization.GetTranslation(new Tuple<Stream, string>(
						PlatformDependentSettings.GetStream(new FileInfo("loc/" + locName + ".xml")),
						locName));
			}
			catch
			{
				Localization = new Localization();
			}
			
			void SetIpAddress()
			{
				PlatformDependentSettings.ShowDialog("Enter server IP address.", ip => {
					try {
						Client.DataFeedClient.ServerIpAddress = IPAddress.Parse(ip);
						Save();
						LoadDataFeedAsync();
					} catch { SetIpAddress(); }					
				});
			}

			try
			{
				Client.DataFeedClient.ServerIpAddress = settings.GetElementsByTagName("ServerIp")[0].InnerText == string.Empty ? null : IPAddress.Parse(settings.GetElementsByTagName("ServerIp")[0].InnerText);
			}
			catch
			{
				SetIpAddress();
			}

			Client.DataFeedClient.RouterPortNumber = settings.GetElementsByTagName("RouterPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("RouterPort")[0].InnerText);

			Client.DataFeedClient.DepartureBoardPortNumber = settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText);

			Client.DataFeedClient.BasicDataPortNumber = settings.GetElementsByTagName("BasicDataPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("BasicDataPort")[0].InnerText);

			Lockouts = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("LockoutsUri")[0].InnerText);

			ExtraordinaryEvents = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText);

			try
			{
				AllowSubway = bool.Parse(settings.GetElementsByTagName("AllowSubway")[0].InnerText);

				AllowTram = bool.Parse(settings.GetElementsByTagName("AllowTram")[0].InnerText);

				AllowCablecar = bool.Parse(settings.GetElementsByTagName("AllowCablecar")[0].InnerText);

				AllowBus = bool.Parse(settings.GetElementsByTagName("AllowBus")[0].InnerText);

				AllowTrain = bool.Parse(settings.GetElementsByTagName("AllowTrain")[0].InnerText);

				AllowShip = bool.Parse(settings.GetElementsByTagName("AllowShip")[0].InnerText);

				WalkingSpeedCoefficient = double.Parse(settings.GetElementsByTagName("WalkingSpeedCoefficient")[0].InnerText);

				UseCellularsToUpdateCache = bool.Parse(settings.GetElementsByTagName("UseCellularsToUpdateCache")[0].InnerText);
			}
			catch
			{
				AllowSubway = true; AllowTram = true; AllowCablecar = true; AllowBus = true; AllowTrain = true; AllowShip = true;

				WalkingSpeedCoefficient = 1.0;

				UseCellularsToUpdateCache = false;
			}

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

			DataFeedClient.GeoWatcher = new CPGeolocator(() =>
			{
				var position = AsyncHelpers.RunSync(CrossGeolocator.Current.GetLastKnownLocationAsync);

				return new Position(position.Latitude, position.Longitude);
			});
		}
		/// <summary>
		/// Saves current settings.
		/// </summary>
		public static void Save()
		{
			XmlDocument settings = new XmlDocument();
			var fs = new FileStream(DataFeedClient.BasePath + SettingsFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

			settings.Load(new StreamReader(fs));

			void CreateElementIfNotExist(params string[] names)
			{
				foreach (var name in names)
					if (settings.GetElementsByTagName(name).Count == 0)
						settings.DocumentElement.AppendChild(settings.CreateElement(name));
			}

			CreateElementIfNotExist("Language", "ExtraEventsUri", "LockoutsUri", "ServerIp", "RouterPort", "DepartureBoardPort", "BasicDataPort",
				"AllowSubway", "AllowTram", "AllowCablecar", "AllowBus", "AllowTrain", "AllowShip", "WalkingSpeedCoefficient", "UseCellularsToUpdateCache");

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

			settings.GetElementsByTagName("UseCellularsToUpdateCache")[0].InnerText = UseCellularsToUpdateCache.ToString();

			fs.SetLength(0);

			settings.Save(new StreamWriter(fs));

			fs.Close();
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
				DataFeedClient.Load(); // Load some data first if exists, then try to update them.
			}
			catch
			{

			}

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

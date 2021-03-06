﻿using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using Timetables.Client;
using Timetables.Preprocessor;
using WeifenLuo.WinFormsUI.Docking;

namespace Timetables.Application.Desktop
{
	/// <summary>
	/// Settings for the application.
	/// </summary>
	static class Settings
	{
		/// <summary>
		/// Reference to the settings file location.
		/// </summary>
		public static FileInfo SettingsFile { get; } = new FileInfo("settings.xml");
		/// <summary>
		/// Color theme used in the application.
		/// </summary>
		public static Themes.Theme Theme { get; set; }
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
		public static FileInfo JourneyDetailCss => new FileInfo("css/JourneyDetailToHtmlHR.css");
		/// <summary>
		/// Path to journey detail CSS stylesheet for printing.
		/// </summary>
		public static FileInfo JourneyDetailPrintCss => new FileInfo("css/JourneyDetailToHtmlLR.css");
		/// <summary>
		/// Path to journey detail XSLT stylesheet for printing.
		/// </summary>
		public static FileInfo JourneyDetailPrintXslt => JourneyDetailXslt;
		/// <summary>
		/// Path to journey simple XSLT stylesheet.
		/// </summary>
		public static FileInfo JourneySimpleXslt => new FileInfo("xslt/JourneySimpleToHtml.xslt");
		/// <summary>
		/// Path to journey simple CSS stylesheet.
		/// </summary>
		public static FileInfo JourneySimpleCss => new FileInfo("css/JourneySimpleToHtmlHR.css");
		/// <summary>
		/// Path to departure board detail XSLT stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardDetailXslt => new FileInfo("xslt/DepartureBoardDetailToHtml.xslt");
		/// <summary>
		/// Path to departure board detail CSS stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardDetailCss => new FileInfo("css/DepartureBoardDetailToHtmlHR.css");
		/// <summary>
		/// Path to departure board detail CSS stylesheet for printing.
		/// </summary>
		public static FileInfo DepartureBoardDetailPrintCss => new FileInfo("css/DepartureBoardDetailToHtmlLR.css");
		/// <summary>
		/// Path to departure board detail XSLT stylesheet for printing.
		/// </summary>
		public static FileInfo DepartureBoardDetailPrintXslt => DepartureBoardDetailXslt;
		/// <summary>
		/// Path to departure board simple XSLT stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardSimpleXslt => new FileInfo("xslt/DepartureBoardSimpleToHtml.xslt");
		/// <summary>
		/// Path to departure board simple CSS stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardSimpleCss => new FileInfo("css/DepartureBoardSimpleToHtmlHR.css");
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
		/// Path to departure board list CSS stylesheet for printing.
		/// </summary>
		public static FileInfo DepartureBoardSimplePrintCss => new FileInfo("css/DepartureBoardSimpleToHtmlLR.css");
		/// <summary>
		/// Path to departure board list XSLT stylesheet for printing.
		/// </summary>
		public static FileInfo DepartureBoardSimplePrintXslt => DepartureBoardSimpleXslt;
		/// <summary>
		/// Path to journey list CSS stylesheet for printing.
		/// </summary>
		public static FileInfo JourneySimplePrintCss => new FileInfo("css/JourneySimpleToHtmlLR.css");
		/// <summary>
		/// Path to journey list XSLT stylesheet for printing.
		/// </summary>
		public static FileInfo JourneySimplePrintXslt => JourneySimpleXslt;
		/// <summary>
		/// Path to script file with onLoad actions.
		/// </summary>
		public static FileInfo OnLoadActionsJavaScript => new FileInfo("js/OnLoadActions.js");
		/// <summary>
		/// Timeout duration while waiting for connection with the server.
		/// </summary>
		public static int TimeoutDuration { get; } = 5000;
		/// <summary>
		/// Loads the settings.
		/// </summary>
		public static void Load()
		{
			XmlDocument settings = new XmlDocument();
			settings.Load(SettingsFile.FullName);

			try
			{
				Localization = Localization.GetTranslation(settings.GetElementsByTagName("Language")?[0].InnerText);
			}
			catch
			{
				Localization = new Localization();
			}

			try
			{
				switch (settings.GetElementsByTagName("Theme")[0].InnerText[0])
				{
					case '0':
						Theme = new Themes.BlueTheme();
						break;
					case '2':
						Theme = new Themes.DarkTheme();
						break;
					case '1':
						Theme = new Themes.LightTheme();
						break;
					default:
						throw new ArgumentException();
				}
			}
			catch
			{
				Theme = new Themes.BlueTheme();
			}

			Client.DataFeedDesktop.OfflineMode = bool.Parse(settings.GetElementsByTagName("OfflineMode")?[0].InnerText);

			Client.DataFeedDesktop.FullDataSource = Client.DataFeedDesktop.OfflineMode ? new Uri(settings.GetElementsByTagName("FullDataUri")[0].InnerText) : null;

			Client.DataFeedDesktop.ServerIpAddress = settings.GetElementsByTagName("ServerIp")[0].InnerText == string.Empty ? null : IPAddress.Parse(settings.GetElementsByTagName("ServerIp")[0].InnerText);

			Client.DataFeedDesktop.RouterPortNumber = settings.GetElementsByTagName("RouterPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("RouterPort")[0].InnerText);

			Client.DataFeedDesktop.DepartureBoardPortNumber = settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText);

			Client.DataFeedDesktop.BasicDataPortNumber = settings.GetElementsByTagName("BasicDataPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("BasicDataPort")[0].InnerText);

			Lockouts = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("LockoutsUri")[0].InnerText);

			ExtraordinaryEvents = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText);
		}
		/// <summary>
		/// Saves current settings.
		/// </summary>
		public static void Save(bool afterException = false)
		{
			if (!System.IO.File.Exists(SettingsFile.FullName) || afterException)
				using (var sw = new System.IO.StreamWriter(SettingsFile.FullName))
					sw.Write("<Timetables></Timetables>");

			XmlDocument settings = new XmlDocument();
			settings.Load(SettingsFile.FullName);							

			void CreateElementIfNotExist(params string[] names)
			{
				foreach (var name in names)
					if (settings.GetElementsByTagName(name).Count == 0)
						settings.DocumentElement.AppendChild(settings.CreateElement(name));
			}

			CreateElementIfNotExist("Theme", "Language", "OfflineMode", "ExtraEventsUri", "LockoutsUri", "FullDataUri", "ServerIp", "RouterPort", "DepartureBoardPort", "BasicDataPort");

			if (Theme is  Themes.BlueTheme) settings.GetElementsByTagName("Theme")[0].InnerText = "0";
			if (Theme is  Themes.DarkTheme) settings.GetElementsByTagName("Theme")[0].InnerText = "2";
			if (Theme is Themes.LightTheme) settings.GetElementsByTagName("Theme")[0].InnerText = "1";

			settings.GetElementsByTagName("Language")[0].InnerText = Localization.ToString();

			settings.GetElementsByTagName("OfflineMode")[0].InnerText = Client.DataFeedDesktop.OfflineMode.ToString();

			settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText = ExtraordinaryEvents == null ? string.Empty : ExtraordinaryEvents.AbsoluteUri;

			settings.GetElementsByTagName("LockoutsUri")[0].InnerText = Lockouts == null ? string.Empty : Lockouts.AbsoluteUri;
			
			if (!Client.DataFeedDesktop.OfflineMode)
			{
				settings.GetElementsByTagName("ServerIp")[0].InnerText = Client.DataFeedDesktop.ServerIpAddress.ToString();
				settings.GetElementsByTagName("RouterPort")[0].InnerText = Client.DataFeedDesktop.RouterPortNumber.ToString();
				settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText = Client.DataFeedDesktop.DepartureBoardPortNumber.ToString();
				settings.GetElementsByTagName("BasicDataPort")[0].InnerText = Client.DataFeedDesktop.BasicDataPortNumber.ToString();
			}

			else
			{
				settings.GetElementsByTagName("FullDataUri")[0].InnerText = Client.DataFeedDesktop.FullDataSource == null ? string.Empty : Client.DataFeedDesktop.FullDataSource.AbsoluteUri;
			}

			settings.Save(SettingsFile.FullName);
		}

		/// <summary>
		/// Creates settings file directly from the application.
		/// </summary>
		internal static void CreateSettings()
		{
			if (Settings.Localization == null)
			{
				Settings.Localization = new Localization();
			}

			Settings.Theme = new Themes.BlueTheme();
			
			MessageBox.Show(Settings.Localization.ProblemWhileLoadingSettings);

			if (MessageBox.Show("Offline mode?", Settings.Localization.Settings, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Offline mode.
			{
				DataFeedDesktop.OfflineMode = true;

				DataFeedDesktop.FullDataSource = new Uri(Interaction.InputBox("Data source URL:", Settings.Localization.Settings));
			}
			else // Online mode.
			{
				DataFeedDesktop.OfflineMode = false;

				DataFeedDesktop.ServerIpAddress = System.Net.IPAddress.Parse(Interaction.InputBox("Server IP address:", Settings.Localization.Settings, "127.0.0.1"));

				DataFeedDesktop.RouterPortNumber = int.Parse(Interaction.InputBox("Router server port:", Settings.Localization.Settings, "27000"));

				DataFeedDesktop.DepartureBoardPortNumber = int.Parse(Interaction.InputBox("Departure board server port:", Settings.Localization.Settings, "27001"));

				DataFeedDesktop.BasicDataPortNumber = int.Parse(Interaction.InputBox("Basic data server port:", Settings.Localization.Settings, "27002"));
			}


			Settings.ExtraordinaryEvents = new Uri(Interaction.InputBox("Extraordinary events URL:", Settings.Localization.Settings));

			Settings.Lockouts = new Uri(Interaction.InputBox("Lockouts URL:", Settings.Localization.Settings));

			Settings.Save(true);
		}
	}	
}

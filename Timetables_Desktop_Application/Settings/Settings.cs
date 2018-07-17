using System;
using System.IO;
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
		/// Color theme used in the application.
		/// </summary>
		public static Themes.Theme Theme { get; set; }
		/// <summary>
		/// Language used in the application.
		/// </summary>
		public static Localization Language { get; set; }
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
		/// Path to departure board detail XSLT stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardDetailXslt => new FileInfo("xslt/DepartureBoardDetailToHtml.xslt");
		/// <summary>
		/// Path to departure board detail CSS stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardDetailCss => new FileInfo("css/DepartureBoardDetailToHtml.css");
		/// <summary>
		/// Path to departure board simple XSLT stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardSimpleXslt => new FileInfo("xslt/DepartureBoardSimpleToHtml.xslt");
		/// <summary>
		/// Path to departure board simple CSS stylesheet.
		/// </summary>
		public static FileInfo DepartureBoardSimpleCss => new FileInfo("css/DepartureBoardSimpleToHtml.css");
		static Settings()
		{
			try
			{
				XmlDocument settings = new XmlDocument();
				settings.Load(".settings");

				switch (settings.GetElementsByTagName("Theme")[0].InnerText[0])
				{
					case '0':
						Theme = new Themes.BlueTheme();
						break;
					case '1':
						Theme = new Themes.DarkTheme();
						break;
					case '2':
						Theme = new Themes.LightTheme();
						break;
					default:
						throw new ArgumentException();
				}

				Language = Localization.GetTranslation(settings.GetElementsByTagName("Language")?[0].InnerText);

				Lockouts = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("LockoutsUri")[0].InnerText);

				ExtraordinaryEvents = string.IsNullOrEmpty(settings.GetElementsByTagName("LockoutsUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText);
			}
			catch
			{
				System.Windows.Forms.MessageBox.Show("Problem occured while loading user settings. Using default settings instead.");
				Theme = new Themes.BlueTheme();
				Language = Localization.GetTranslation("English");
				Save(true);
			}
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

			void CreateElementIfNotExist(string name)
			{
				if (settings.GetElementsByTagName(name).Count == 0)
					settings.DocumentElement.AppendChild(settings.CreateElement(name));
			}

			CreateElementIfNotExist("Theme");

			if (Theme is  Themes.BlueTheme) settings.GetElementsByTagName("Theme")[0].InnerText = "0";
			if (Theme is  Themes.DarkTheme) settings.GetElementsByTagName("Theme")[0].InnerText = "1";
			if (Theme is Themes.LightTheme) settings.GetElementsByTagName("Theme")[0].InnerText = "2";

			CreateElementIfNotExist("Language");

			settings.GetElementsByTagName("Language")[0].InnerText = Language.Language;

			CreateElementIfNotExist("ExtraEventsUri");

			settings.GetElementsByTagName("ExtraEventsUri")[0].InnerText = ExtraordinaryEvents == null ? string.Empty : ExtraordinaryEvents.AbsoluteUri;

			CreateElementIfNotExist("LockoutsUri");

			settings.GetElementsByTagName("LockoutsUri")[0].InnerText = Lockouts == null ? string.Empty : Lockouts.AbsoluteUri;

			CreateElementIfNotExist("BasicDataUri");

			settings.GetElementsByTagName("BasicDataUri")[0].InnerText = Client.DataFeed.BasicDataSource == null ? string.Empty : Client.DataFeed.BasicDataSource.AbsoluteUri;

			CreateElementIfNotExist("FullDataUri");

			settings.GetElementsByTagName("FullDataUri")[0].InnerText = Client.DataFeed.FullDataSource == null ? string.Empty : Client.DataFeed.FullDataSource.AbsoluteUri;

			settings.Save(".settings");
		}
	}	
}

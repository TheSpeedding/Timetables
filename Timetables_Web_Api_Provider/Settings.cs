using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Net;
using System.Xml;
using Timetables.Client;

namespace Timetables.Server.Web
{ 
	/// <summary>
	/// Settings for the Web API.
	/// </summary>
	static class Settings
	{
		/// <summary>
		/// Reference to the settings file location.
		/// </summary>
		public static FileInfo SettingsFile { get; } = new FileInfo("settings.xml");
		/// <summary>
		/// Timeout duration while waiting for connection with the server.
		/// </summary>
		public static int TimeoutDuration { get; } = 10000;
		/// <summary>
		/// Loads the settings.
		/// </summary>
		public static void Load()
		{
			DataFeedClient.BasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "timetables") + "/";

			try
			{
				XmlDocument settings = new XmlDocument();
				settings.Load(SettingsFile.FullName);

				DataFeedClient.ServerIpAddress = settings.GetElementsByTagName("ServerIp")[0].InnerText == string.Empty ? null : IPAddress.Parse(settings.GetElementsByTagName("ServerIp")[0].InnerText);

				DataFeedClient.RouterPortNumber = settings.GetElementsByTagName("RouterPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("RouterPort")[0].InnerText);

				DataFeedClient.DepartureBoardPortNumber = settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText);

				DataFeedClient.BasicDataPortNumber = settings.GetElementsByTagName("BasicDataPort")[0].InnerText == string.Empty ? default(int) : int.Parse(settings.GetElementsByTagName("BasicDataPort")[0].InnerText);
			}
			catch
			{
				DataFeedClient.ServerIpAddress = IPAddress.Loopback;
				DataFeedClient.RouterPortNumber = 27000;
				DataFeedClient.DepartureBoardPortNumber = 27001;
				DataFeedClient.BasicDataPortNumber = 27002;
			}
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

			CreateElementIfNotExist("ServerIp", "RouterPort", "DepartureBoardPort", "BasicDataPort");

			settings.GetElementsByTagName("ServerIp")[0].InnerText = DataFeedClient.ServerIpAddress.ToString();
			settings.GetElementsByTagName("RouterPort")[0].InnerText = DataFeedClient.RouterPortNumber.ToString();
			settings.GetElementsByTagName("DepartureBoardPort")[0].InnerText = DataFeedClient.DepartureBoardPortNumber.ToString();
			settings.GetElementsByTagName("BasicDataPort")[0].InnerText = DataFeedClient.BasicDataPortNumber.ToString();

			settings.Save(SettingsFile.FullName);
		}
	}	
}

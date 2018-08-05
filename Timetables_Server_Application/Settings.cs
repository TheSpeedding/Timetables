using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Timetables.Server
{
	/// <summary>
	/// Settings for the server.
	/// </summary>
	class Settings
	{
		/// <summary>
		/// Router port.
		/// </summary>
		public static int RouterPort { get; }
		/// <summary>
		/// Departure board port.
		/// </summary>
		public static int DepartureBoardPort { get; }
		/// <summary>
		/// Links to data feed sources.
		/// </summary>
		public static List<Uri> DataFeedSources { get; } = new List<Uri>();
		static Settings()
		{
			try
			{
				XmlDocument settings = new XmlDocument();
				settings.Load(".settings");

				foreach (XmlNode link in settings.GetElementsByTagName("Link"))
					try { DataFeedSources.Add(new Uri(link.InnerText)); } catch { /* Invalid URI. Do not process this data source. */ }

				RouterPort = int.Parse(settings.GetElementsByTagName("RouterPort")?[0].InnerText);
				DepartureBoardPort = int.Parse(settings.GetElementsByTagName("DepartureBoardPort")?[0].InnerText);
			}

			catch
			{
				RouterPort = 24700;
				DepartureBoardPort = 24701;

				if (DataFeedSources.Count == 0)
				{
					Console.WriteLine("Enter link to the data source: ");
					DataFeedSources.Add(new Uri(Console.ReadLine()));
				}
			}
		}
	}
}

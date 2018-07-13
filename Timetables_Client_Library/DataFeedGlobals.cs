using System;
using System.Xml;
using Timetables.Preprocessor;

namespace Timetables.Client
{
	/// <summary>
	/// Class offering data feed for GUI applications.
	/// </summary>
	public static class DataFeedGlobals
	{
		private static Interop.DataFeedManaged fullData = null;
		/// <summary>
		/// Indicates whether application is working in offline mode (desktop application only).
		/// </summary>
		public static bool OfflineMode { get; private set; } = true;
		/// <summary>
		/// Indicates whether the data were sucessfully loaded.
		/// </summary>
		public static bool Loaded { get; private set; }
		/// <summary>
		/// Indicates whether the data are downloaded.
		/// </summary>
		public static bool Downloaded { get; private set; }
		/// <summary>
		/// Basic data feed.
		/// </summary>
		public static Structures.Basic.DataFeedBasic Basic { get; private set; }
		/// <summary>
		/// Full data feed.
		/// </summary>
		public static Interop.DataFeedManaged Full => fullData ?? throw new NotSupportedException("The application is running in the offline mode. Cannot access data.");
		/// <summary>
		/// Path to the data source.
		/// </summary>
		public static Uri FullDataSource { get; private set; }
		public static Uri BasicDataSource { get; private set; }
		/// <summary>
		/// Loads data while starting the application.
		/// </summary>
		public static void Load()
		{
			Downloaded = false;

			try
			{
				XmlDocument settings = new XmlDocument();
				settings.Load(".settings");

				FullDataSource = OfflineMode ? new Uri(settings.GetElementsByTagName("FullDataUri")[0].InnerText) : null;

				BasicDataSource = string.IsNullOrEmpty(settings.GetElementsByTagName("BasicDataUri")[0].InnerText) ? null : new Uri(settings.GetElementsByTagName("BasicDataUri")[0].InnerText);
			}

			catch (Exception ex)
			{
				throw new ArgumentException("Fatal error. Settings file is corrupted and thus cannot load the data.", ex);
			}


			// TO-DO: THIS IS ONLY TEMPORARY SOLUTION.
			if (!System.IO.Directory.Exists("data"))
			{
				try
				{
					DataFeed.GetAndTransformDataFeed<GtfsDataFeed>(FullDataSource.AbsoluteUri);
				}
				catch
				{
					throw new ArgumentException("Fatal error. Cannot process the data.");
				}
			}

			Downloaded = true;

			// TO-DO: Online mode?

			Update();			
		}
		/// <summary>
		/// Updates data feed. Also used in static constructor.
		/// </summary>
		public static void Update()
		{
			Loaded = false;

			Basic = new Structures.Basic.DataFeedBasic();

			if (OfflineMode)
			{
				fullData = new Interop.DataFeedManaged();
			}

			Loaded = true;
		}
	}
}

using System;
using System.Xml;
using Timetables.Preprocessor;

namespace Timetables.Client
{
	/// <summary>
	/// Class offering data feed for GUI applications.
	/// </summary>
	public static partial class DataFeed
	{
		private static Interop.DataFeedManaged fullData = null;
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
		public static Uri FullDataSource { get; set; }
		/// <summary>
		/// Indicates whether application is working in offline mode.
		/// </summary>
		public static bool OfflineMode { get; set; }
		/// <summary>
		/// Server IP address. Only relevant in online mode.
		/// </summary>
		public static System.Net.IPAddress ServerIpAddress { get; set; }
		/// <summary>
		/// End point of router server. Only relevant in online mode.
		/// </summary>
		public static uint RouterPortNumber { get; set; }
		/// <summary>
		/// End point of departure board server. Only relevant in online mode.
		/// </summary>
		public static uint DepartureBoardPortNumber { get; set; }
		/// <summary>
		/// End point of basic data server. Only relevant in online mode.
		/// </summary>
		public static uint BasicDataPortNumber { get; set; }
		/// <summary>
		/// Loads data while starting the application.
		/// </summary>
		public static void Load(bool forceDownload = false)
		{
			Downloaded = false;
			
			// Offline mode.

			if ((!System.IO.Directory.Exists("data") || forceDownload ) && OfflineMode)
			{
				try
				{
					Preprocessor.DataFeed.GetAndTransformDataFeed<GtfsDataFeed>(FullDataSource);
				}
				catch
				{
					throw new ArgumentException("Fatal error. Cannot process the data.");
				}
			}

			// Online mode.

			else if (!System.IO.Directory.Exists("basic") || forceDownload)
			{
				try
				{
					if (System.IO.File.Exists("basic_data_temp.zip"))
						System.IO.File.Delete("basic_data_temp.zip");

					using (System.Net.WebClient wc = new System.Net.WebClient())
						wc.DownloadFile(BasicDataSource, "basic_data_temp.zip");

					if (!System.IO.Directory.Exists("basic"))
						System.IO.Directory.CreateDirectory("basic");

					System.IO.Compression.ZipFile.ExtractToDirectory("basic_data_temp.zip", "basic");

					if (System.IO.File.Exists("basic_data_temp.zip"))
						System.IO.File.Delete("basic_data_temp.zip");
				}
				catch
				{
					throw new ArgumentException("Fatal error. Cannot process the data.");
				}
			}

			Downloaded = true;

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

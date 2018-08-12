using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Timetables.Preprocessor;

namespace Timetables.Client
{
	/// <summary>
	/// Class offering data feed for GUI applications.
	/// </summary>
	public static class DataFeed
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
		public static async Task LoadAsync(bool forceDownload = false, int timeout = 5000)
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

			else
				try
				{
					Structures.Basic.DataFeedBasicResponse response = null;

					try
					{
						using (var sr = new System.IO.StreamReader("basic/.version"))
							response = await new BasicDataProcessing().ProcessAsync(new Structures.Basic.DataFeedBasicRequest(sr.ReadLine()), timeout);
					}

					catch (Exception ex) 
					{
						if (ex is WebException) // Server offline.
							throw;

						else // Data does not exist or the version file is corrupted.
							response = await new BasicDataProcessing().ProcessAsync(new Structures.Basic.DataFeedBasicRequest(), timeout);
					}


					if (response.ShouldBeUpdated)
						response.Data.Save();

				}

				catch (Exception ex)
				{
					if (ex is WebException) // Server offline.
						throw;

					else
						throw new ArgumentException("Fatal error. Cannot process the data.");
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

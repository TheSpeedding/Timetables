// We need this because of mobile application. Desktop application need C++/CLI wrappers, which cannot be loaded in mobile application.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Timetables.Preprocessor;

namespace Timetables.Client
{
	/// <summary>
	/// Class offering data feed for GUI applications. Extended with offline mode.
	/// </summary>
	public class DataFeedDesktop : DataFeedClient
	{
		private volatile static Interop.DataFeedManaged fullData = null;
		/// <summary>
		/// Full data feed.
		/// </summary>
		public static Interop.DataFeedManaged Full
		{
			get
			{
				if (!OfflineMode) throw new NotSupportedException("The application is running in the offline mode. Cannot access data.");
				return fullData ?? throw new NullReferenceException("Basic data not initialized correctly.");
			}
		}
		/// <summary>
		/// Path to the data source.
		/// </summary>
		public static Uri FullDataSource { get; set; }
		/// <summary>
		/// Indicates whether application is working in offline mode.
		/// </summary>
		public static bool OfflineMode { get; set; }
		/// <summary>
		/// Loads data while starting the application.
		/// </summary>
		public new static async Task DownloadAsync(bool forceDownload = false, int timeout = 5000)
		{
			Downloaded = false;

			// Offline mode.

			if (OfflineMode)
			{
				if ((!System.IO.Directory.Exists("data") || !System.IO.Directory.Exists("basic") || forceDownload || (IsUpdateNeeded && IsConnected)))
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
				await DataFeedClient.DownloadAsync(forceDownload, timeout);

			Downloaded = true;

			Load();
		}
		/// <summary>
		/// Updates data feed.
		/// </summary>
		public new static void Load()
		{
			if (OfflineMode)
				fullData = new Interop.DataFeedManaged();

			DataFeedClient.Load();
		}
	}
}

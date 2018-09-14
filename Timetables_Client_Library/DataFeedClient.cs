using System;
using System.Device.Location;
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
		private volatile static Interop.DataFeedManaged fullData = null;
		private volatile static Structures.Basic.DataFeedBasic basicData = null;
		/// <summary>
		/// Geowatcher to retrieve current location.
		/// </summary>
		public static GeoCoordinateWatcher GeoWatcher { get; } = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
		/// <summary>
		/// Indicates whether the data were sucessfully loaded.
		/// </summary>
		public static bool Loaded { get; private set; } = false;
		/// <summary>
		/// Indicates whether the data are downloaded.
		/// </summary>
		public static bool Downloaded { get; private set; } = false;
		/// <summary>
		/// Basic data feed.
		/// </summary>
		public static Structures.Basic.DataFeedBasic Basic => basicData ?? throw new NullReferenceException("Basic data not initialized correctly.");
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
		/// Decides if the data feed should be updated.
		/// </summary>
		public static bool IsUpdateNeeded
		{
			get
			{
				try
				{
					using (var sr = new System.IO.StreamReader("data/expires.tfd"))
						if (DateTime.ParseExact(sr.ReadLine(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddDays(1) < DateTime.Now)
							return true;
						else
							return false;
				}
				catch
				{
					return true;
				}
			}
		}
		/// <summary>
		/// Decides whether the computer is connected to the Internet.
		/// </summary>
		private static bool IsConnected()
		{
			try
			{
				using (var client = new System.Net.WebClient())
				using (client.OpenRead("http://clients3.google.com/generate_204"))
					return true;
			}
			catch
			{
				return false;
			}
		}
		/// <summary>
		/// Loads data while starting the application.
		/// </summary>
		public static async Task DownloadAsync(bool forceDownload = false, int timeout = 5000)
		{
			Downloaded = false;

			GeoWatcher.TryStart(false, TimeSpan.FromSeconds(5));

			await Task.Delay(10); // This is temporary "bugfix". There is some race condition in Timetables.Application.Desktop.InitLoadingWindow.

			// Offline mode.

			if (OfflineMode)
			{
				if ((!System.IO.Directory.Exists("data") || !System.IO.Directory.Exists("basic") || forceDownload || (IsUpdateNeeded && IsConnected())))
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
			{
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
			}

			Downloaded = true;

			Load();			
		}
		/// <summary>
		/// Updates data feed.
		/// </summary>
		public static void Load()
		{
			Loaded = false;
						
			basicData = new Structures.Basic.DataFeedBasic();

			if (OfflineMode)
			{
				fullData = new Interop.DataFeedManaged();
			}

			Loaded = true;
		}
	}
}

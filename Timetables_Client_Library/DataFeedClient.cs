using System;
using System.Net;
using System.Threading.Tasks;
using Timetables.Utilities;

namespace Timetables.Client
{
	/// <summary>
	/// Class offering data feed for GUI applications.
	/// </summary>
	public class DataFeedClient
	{
		private volatile static Structures.Basic.DataFeedBasic basicData = null;
		/// <summary>
		/// Basepath to the local storage.
		/// </summary>
		public static string BasePath { get; set; } = "./";
		/// <summary>
		/// Geowatcher to retrieve current location.
		/// </summary>
		public static CPGeolocator GeoWatcher { get; set; }
		/// <summary>
		/// Indicates whether the data were sucessfully loaded.
		/// </summary>
		public static bool Loaded { get; protected set; } = false;
		/// <summary>
		/// Indicates whether the data are downloaded.
		/// </summary>
		public static bool Downloaded { get; protected set; } = false;
		/// <summary>
		/// Timespan that the cached data should be cached in advance.
		/// </summary>
		public static TimeSpan TimeToCacheFor { get; } = new TimeSpan(1, 0, 0, 0);
		/// <summary>
		/// Timespan that the cached data should be updated before expiration.
		/// </summary>
		public static TimeSpan TimeToUpdateCachedBeforeExpiration { get; } = new TimeSpan(6, 0, 0);
		/// <summary>
		/// Basic data feed.
		/// </summary>
		public static Structures.Basic.DataFeedBasic Basic => basicData ?? throw new NullReferenceException("Basic data not initialized correctly.");
		/// <summary>
		/// Server IP address. Only relevant in online mode.
		/// </summary>
		public static IPAddress ServerIpAddress { get; set; }
		/// <summary>
		/// End point of router server. Only relevant in online mode.
		/// </summary>
		public static int RouterPortNumber { get; set; }
		/// <summary>
		/// End point of departure board server. Only relevant in online mode.
		/// </summary>
		public static int DepartureBoardPortNumber { get; set; }
		/// <summary>
		/// End point of basic data server. Only relevant in online mode.
		/// </summary>
		public static int BasicDataPortNumber { get; set; }
		/// <summary>
		/// Decides if the data feed should be updated.
		/// </summary>
		public static bool IsUpdateNeeded
		{
			get
			{
				try
				{
					using (var sr = new System.IO.StreamReader(BasePath + "data/expires.tfd"))
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
		protected static bool IsConnected
		{
			get
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
		}
		/// <summary>
		/// Loads data while starting the application.
		/// </summary>
		public static async Task DownloadAsync(bool forceDownload = false, int timeout = 5000)
		{
			Downloaded = false;

			try
			{
				Structures.Basic.DataFeedBasicResponse response = null;

				try
				{
					using (var sr = new System.IO.StreamReader(BasePath + "basic/.version"))
						response = await new BasicDataProcessing().ProcessAsync(new Structures.Basic.DataFeedBasicRequest(sr.ReadLine()), timeout);
				}

				catch (Exception ex)
				{
					if (ex is WebException && (forceDownload || !System.IO.Directory.Exists(BasePath + "basic/"))) // Server offline and data does not exist. Cannot continue.
						throw;

					else if (ex is System.IO.IOException) // Data does not exist or the version file is corrupted.
						try
						{
							response = await new BasicDataProcessing().ProcessAsync(new Structures.Basic.DataFeedBasicRequest(), timeout);
						}
						catch
						{
							// Some data exists, we do not have to do anything.
						}
				}


				if (response != null && response.ShouldBeUpdated)
					response.Data.Save(BasePath);
			}

			catch (Exception ex)
			{
				if (ex is WebException) // Server offline.
					throw;

				else
					throw new ArgumentException("Fatal error. Cannot process the data.");
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
			
			basicData = new Structures.Basic.DataFeedBasic(BasePath);

			Loaded = true;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timetables.Client;
using Timetables.Preprocessor;

namespace Timetables.Server
{
	/// <summary>
	/// Class for data feed.
	/// </summary>
	public static class DataFeed
	{
		/// <summary>
		/// Thread that ensures auto update of the data.
		/// </summary>
		public static Thread AutoUpdateThread { get; } = new Thread(AutoUpdate.AutoUpdateRoutine);
		/// <summary>
		/// Full data feed object.
		/// </summary>
		public static Interop.DataFeedManaged Full { get; private set; }
		/// <summary>
		/// Basic data feed.
		/// </summary>
		public static Structures.Basic.DataFeedBasic Basic { get; private set; }
		/// <summary>
		/// Indicates whether the data were sucessfully loaded.
		/// </summary>
		public static bool Loaded { get; private set; }
		/// <summary>
		/// Indicates whether the data are downloaded.
		/// </summary>
		public static bool Downloaded { get; private set; }
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
				}
				catch
				{
					return true;
				}
				return false;
			}
		}
		static DataFeed()
		{
			Preprocessor.DataFeed.DataProcessing += Logging.LoadingProgressCallback;
			Preprocessor.DataFeed.DataErrors += Logging.ErrorsCallback;

			AutoUpdate.Update += Logging.AutoUpdateCallback;

			if (IsUpdateNeeded)
				Download(IsUpdateNeeded);

			else
			{
				Logging.Log("Data feed is up to date.");
				Load();
			}
		}

		/// <summary>
		/// Loads data while starting the application.
		/// </summary>
		public static void Download(bool forceDownload = false)
		{
			Downloaded = false;

			if (!System.IO.Directory.Exists("data") || forceDownload)
			{
				Preprocessor.DataFeed.GetAndTransformDataFeed<GtfsDataFeed>(Settings.DataFeedSources.ToArray());
			}

			Downloaded = true;

			Load();
		}
		/// <summary>
		/// Updates data feed. Also used in static constructor.
		/// </summary>
		public static void Load()
		{
			Loaded = false;

			Basic = new Structures.Basic.DataFeedBasic();

			Full = new Interop.DataFeedManaged();

			Loaded = true;

			if (AutoUpdateThread.ThreadState == ThreadState.Unstarted)
				AutoUpdateThread.Start();
		}
	}
}

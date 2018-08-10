using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetables.Preprocessor;

namespace Timetables.Server
{
	/// <summary>
	/// Class for data feed.
	/// </summary>
	public static class DataFeed
	{
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
		static DataFeed()
		{
			Preprocessor.DataFeed.LoadingProgress += LoadingProgressCallback;

			Load(true);

			Update();

			Preprocessor.DataFeed.LoadingProgress -= LoadingProgressCallback;
		}
		/// <summary>
		/// Callback to log preprocessor actions.
		/// </summary>
		/// <param name="message">Message.</param>
		private static void LoadingProgressCallback(string message, int progress = 0) => Logging.Log($"Preprocessor: { message }");

		/// <summary>
		/// Loads data while starting the application.
		/// </summary>
		public static void Load(bool forceDownload = false)
		{
			Downloaded = false;

			if (!System.IO.Directory.Exists("data") || forceDownload)
			{
				try
				{
					Preprocessor.DataFeed.GetAndTransformDataFeed<GtfsDataFeed>(Settings.DataFeedSources.ToArray());
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

			Full = new Interop.DataFeedManaged();

			Loaded = true;
		}
	}
}

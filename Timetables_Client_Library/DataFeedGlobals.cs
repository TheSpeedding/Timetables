using System;
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
		public static Interop.DataFeedManaged Full { get { return fullData ?? throw new NotSupportedException("The application is running in the offline mode. Cannot access data."); } }
		/// <summary>
		/// Loads data while starting the application.
		/// </summary>
		public static void Load()
		{
			Downloaded = false;

			// TO-DO: THIS IS ONLY TEMPORARY SOLUTION.
			if (!System.IO.Directory.Exists("basic"))
			{
				DataFeed.GetAndTransformDataFeed<GtfsDataFeed>("http://opendata.iprpraha.cz/DPP/JR/jrdata.zip");
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

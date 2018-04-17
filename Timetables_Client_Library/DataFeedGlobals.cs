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
		public static bool OfflineMode { get; } = true;
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
		static DataFeedGlobals()
		{
			// TO-DO: THIS IS ONLY TEMPORARY SOLUTION.
			if (!System.IO.Directory.Exists("basic"))
			{
				DataFeed.GetAndTransformDataFeed<GtfsDataFeed>("http://opendata.iprpraha.cz/DPP/JR/jrdata.zip");
			}

			// TO-DO: Online mode?

			Update();
		}
		/// <summary>
		/// Updates data feed. Also used in static constructor.
		/// </summary>
		public static void Update()
		{
			Basic = new Structures.Basic.DataFeedBasic();

			if (OfflineMode)
			{
				fullData = new Interop.DataFeedManaged();
			}
		}
	}
}

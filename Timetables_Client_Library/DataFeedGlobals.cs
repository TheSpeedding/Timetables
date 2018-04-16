using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Client
{
	public static class DataFeedGlobals
	{
		private static Interop.DataFeedManaged fullData = null;
		public static bool OfflineMode { get; } = true;
		public static Structures.Basic.DataFeedBasic BasicData { get; }
		public static Interop.DataFeedManaged FullData { get { return fullData ?? throw new NotSupportedException("The application is running in the offline mode. Cannot access data."); } }
		static DataFeedGlobals()
		{
			BasicData = new Structures.Basic.DataFeedBasic();

			if (OfflineMode)
			{
				fullData = new Interop.DataFeedManaged();
			}
		}
	}
}

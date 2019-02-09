using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Timetables.Server
{
	/// <summary>
	/// Class that serves methods for auto update of the data.
	/// </summary>
	public static class AutoUpdate
	{
		/// <summary>
		/// Delegate serving information about auto updating.
		/// </summary>
		/// <param name="message">Message to be shown.</param>
		public delegate void UpdateEventHandler(string message);
		/// <summary>
		/// The event acknowleding the server administrator about work of the auto updates.
		/// </summary>
		public static event UpdateEventHandler Update;
		/// <summary>
		/// Starts the auto update routine. It is expected to start this method at some background thread, since it is an infinite cycle.
		/// </summary>
		public static void AutoUpdateRoutine()
		{
			bool firstLoop = true;

			while (true)
			{
				try
				{
					DateTime autoUpdateDateTime;

					using (var sr = new System.IO.StreamReader("data/expires.tfd"))
						autoUpdateDateTime = DateTime.ParseExact(sr.ReadLine(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None).AddDays(1);

					Update?.Invoke($"Another auto update is scheduled to be at { autoUpdateDateTime }.");

					Thread.Sleep(autoUpdateDateTime - DateTime.Now);

					DataFeed.Download(true);

					DataFeed.Load();
				}

				catch (Exception ex)
				{
					if (ex is System.IO.FileNotFoundException)
						Update?.Invoke("Failed when trying to get an information about expiration. There will be another try in 12 hours.");

					else
						Update?.Invoke("Failed when parsing downloaded data. There will be another try in 6 hours.");


					if (!firstLoop)
						Thread.Sleep(new TimeSpan(6, 0, 0)); // File with expiration date does not exist or there was some other problem. Try to update the data in 6 hours.
				}

				firstLoop = false;
			}
		}
	}
}

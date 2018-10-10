using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Timetables.Server
{
	/// <summary>
	/// Class serving some methods to log server events.
	/// </summary>
	public static class Logging
	{
		/// <summary>
		/// Writer that is the server logging into.
		/// </summary>
		public static TextWriter FileForLogging { get; } = new StreamWriter(".log", true) { AutoFlush = true };
		/// <summary>
		/// Delegate used to log some information.
		/// </summary>
		/// <param name="message">Message of the log.</param>
		public delegate void LoggingEventHandler(string message);
		/// <summary>
		/// Event that is fired to log some information.
		/// </summary>
		public static event LoggingEventHandler LoggingEvent;
		/// <summary>
		/// Adds current timestamp to the message and logs it to the textwriter.
		/// </summary>
		/// <param name="message">Message of the log.</param>
		/// <param name="tw">Textwriter.</param>
		public static void LogWithDateTime(this string message, TextWriter tw) => tw.WriteLine(DateTime.Now.ToString() + " - " + message);
		/// <summary>
		/// Fires the event for logging.
		/// </summary>
		/// <param name="message">Message to be logged.</param>
		public static void Log(this string message) => LoggingEvent?.Invoke(message);

		public static void Dispose() => FileForLogging.Dispose();
	}
}

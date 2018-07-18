using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Server
{
	class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			Logging.LoggingEvent += (string message) => Logging.FileForLogging.WriteLine(message.LogWithDateTime());
			Logging.LoggingEvent += (string message) => Console.WriteLine(message.LogWithDateTime());

			while (true)
			{

			}
		}
	}
}

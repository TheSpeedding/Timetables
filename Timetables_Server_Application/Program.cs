using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Timetables.Server
{
	static class Program
	{
		static void Main()
		{
			Logging.LoggingEvent += (string message) => message.LogWithDateTime(Logging.FileForLogging);
			Logging.LoggingEvent += (string message) => message.LogWithDateTime(Console.Out);
			
			try
			{
				while (!DataFeed.Loaded) ; // Temporary. Actually, this does nothing. Just forces data to be loaded (static class constructor). Without this, the data would be loaded when the first request approaches the server.
			}
			catch (Exception ex)
			{
				Logging.Log("Fatal error. Data could not be processed: " + ex.Message);
				return;
			}

			Server.Start(IPAddress.Any, Settings.RouterPort, Settings.DepartureBoardPort, Settings.BasicDataFeedPort);

			Server.ServerManipulation();

			if (!Server.IsStopped)
				Server.Stop();
		}
	}
}
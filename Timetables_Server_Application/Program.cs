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

			Server.Start(IPAddress.Any, 24700, 24701);

			Server.ServerManipulation();

			if (!Server.IsStopped)
				Server.Stop();
		}
	}
}

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
	/// <summary>
	/// Server class.
	/// </summary>
	public static class Server
	{
		/// <summary>
		/// Signals that can be used to control server.
		/// </summary>
		public enum ServerSignal { Abort }
		/// <summary>
		/// Port that is the server for router listening at.
		/// </summary>
		public static int RouterServerPort { get; } = 24700;
		/// <summary>
		/// Port that is the server for departure board listening at.
		/// </summary>
		public static int DepartureBoardServerPort { get; } = 24701;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			Logging.LoggingEvent += (string message) => message.LogWithDateTime(Logging.FileForLogging);
			Logging.LoggingEvent += (string message) => message.LogWithDateTime(Console.Out);

			TcpListener dbServer = new TcpListener(IPAddress.Loopback, DepartureBoardServerPort);
			TcpListener rServer = new TcpListener(IPAddress.Loopback, RouterServerPort);

			dbServer.Start();
			rServer.Start();
			Logging.Log("The server has started.");
			Logging.Log($"Listening at { IPAddress.Loopback }, ports { RouterServerPort } and { DepartureBoardServerPort }.");

			Thread dbThread = new Thread(() =>
			{
				while (true)
				{
					new DepartureBoardProcessing(dbServer.AcceptTcpClient()).ProcessAsync();
				}
			});

			Thread rThread = new Thread(() =>
			{
				while (true)
				{
					new RouterProcessing(rServer.AcceptTcpClient()).ProcessAsync();
				}
			});

			dbThread.Start();
			rThread.Start();
			

			string cmd;
			while ((cmd = Console.ReadLine()) != null)
				switch ((ServerSignal)int.Parse(cmd))
				{
					case ServerSignal.Abort:
						Logging.Log("The server has been requested to be stopped.");

						dbThread.Abort();
						rThread.Abort();

						dbThread.Join();
						rThread.Join();

						dbServer.Stop();
						rServer.Stop();

						Logging.Log("The server has stopped.");
						break;
				}

		}
	}
}

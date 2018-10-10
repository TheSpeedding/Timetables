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
	public abstract class Server : IDisposable
	{
		/// <summary>
		/// Signals that can be used to control server.
		/// </summary>
		public enum ServerSignal { Abort, ForceUpdate }
		/// <summary>
		/// Thread that is operating given server.
		/// </summary>
		public Thread OperationThread { get; protected set; }
		/// <summary>
		/// Instance of TcpListener class.
		/// </summary>
		public TcpListener ServerListener { get; protected set; }
		/// <summary>
		/// Port number.
		/// </summary>
		public int Port { get; protected set; }
		/// <summary>
		/// IP address.
		/// </summary>
		public IPAddress IpAddress { get; protected set; }
		/// <summary>
		/// Object representing departure board server.
		/// </summary>
		public static DepartureBoardServer DepartureBoard { get; private set; }
		/// <summary>
		/// Object representing router server.
		/// </summary>
		public static RouterServer Router { get; private set; }
		/// <summary>
		/// Object representing basic data server.
		/// </summary>
		public static BasicDataFeedServer BasicData { get; private set; }
		/// <summary>
		/// Indicates whether the server is stopped.
		/// </summary>
		public static bool IsStopped { get; private set; } = true;
		/// <summary>
		/// Starts the server.
		/// </summary>
		/// <param name="address">IP address.</param>
		/// <param name="routerPort">Router server port.</param>
		/// <param name="dbPort">Departure board server port.</param>
		/// <param name="dfPort">Data feed server port.</param>
		public static void Start(IPAddress address, int routerPort, int dbPort, int dfPort)
		{
			Logging.Log("The server has started.");

			Router = new RouterServer(address, routerPort);
			DepartureBoard = new DepartureBoardServer(address, dbPort);
			BasicData = new BasicDataFeedServer(address, dfPort);

			Router.Start();
			DepartureBoard.Start();
			BasicData.Start();

			IsStopped = false;
		}
		/// <summary>
		/// Stops the server.
		/// </summary>
		public static void Stop()
		{
			Router.StopServer();
			DepartureBoard.StopServer();
			BasicData.StopServer();

			IsStopped = true;

			Logging.Log("The server has stopped.");
		}
		/// <summary>
		/// Stops the server.
		/// </summary>
		private void StopServer()
		{
			ServerListener.Stop();
			OperationThread.Abort();
			OperationThread.Join();
		}
		/// <summary>
		/// Method that serves manipulation with the server while running, blocks the current thread.
		/// </summary>
		public static void ServerManipulation()
		{
			string cmd;
			while ((cmd = Console.ReadLine()) != null)
				switch ((ServerSignal)int.Parse(cmd))
				{
					case ServerSignal.Abort:
						Logging.Log("The server has been requested to be stopped.");
						Stop();
						break;
					case ServerSignal.ForceUpdate:
						Logging.Log("Request to force data update received.");
						DataFeed.Download(true);
						DataFeed.Load();
						break;
				}
		}
		/// <summary>
		/// Starts the server.
		/// </summary>
		public void Start()
		{
			ServerListener.Start();
			OperationThread.Start();

			Logging.Log($"Listening at { ((IPEndPoint)ServerListener.Server.LocalEndPoint).Address.ToString() }:{ Port }.");
		}
		/// <summary>
		/// Creates customized server.
		/// </summary>
		/// <param name="processingAction">Action that should be performed when client is accepted.</param>
		/// <param name="address">IP address.</param>
		/// <param name="port">Port number.</param>
		protected void CreateServer(Action<TcpClient> processingAction, IPAddress address, int port)
		{
			ServerListener = new TcpListener(address, port);

			IpAddress = address;
			Port = port;

			OperationThread = new Thread(async () =>
			{
				while (true)
				{
					TcpClient client = await ServerListener.AcceptTcpClientAsync();

					Logging.Log($"Connection request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() }.");

					processingAction(client);
				}
			});
		}
		public void Dispose() => Stop();
	}

	/// <summary>
	/// Router server class.
	/// </summary>
	public sealed class RouterServer : Server
	{
		/// <summary>
		/// Initializes new instance of router server.
		/// </summary>
		/// <param name="address">IP address.</param>
		/// <param name="port">Port number.</param>
		public RouterServer(IPAddress address, int port) => CreateServer((TcpClient client) => new RouterProcessing(client).ProcessAsync(), address, port);
	}

	/// <summary>
	/// Basic data feed server class.
	/// </summary>
	public sealed class BasicDataFeedServer : Server
	{
		/// <summary>
		/// Initializes new instance of basic data feed server.
		/// </summary>
		/// <param name="address">IP address.</param>
		/// <param name="port">Port number.</param>
		public BasicDataFeedServer(IPAddress address, int port) => CreateServer((TcpClient client) => new DataFeedProcessing(client).ProcessAsync(), address, port);
	}

	/// <summary>
	/// Departure board server class.
	/// </summary>
	public sealed class DepartureBoardServer : Server
	{
		/// <summary>
		/// Initializes new instance of departure board server.
		/// </summary>
		/// <param name="address">IP address.</param>
		/// <param name="port">Port number.</param>
		public DepartureBoardServer(IPAddress address, int port) => CreateServer((TcpClient client) => new DepartureBoardProcessing(client).ProcessAsync(), address, port);
	}
}

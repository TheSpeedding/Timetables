﻿using System;
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
		public enum ServerSignal { Abort }
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
		/// Indicates whether the server is stopped.
		/// </summary>
		public static bool IsStopped { get; private set; } = true;
		/// <summary>
		/// Starts the server.
		/// </summary>
		/// <param name="address">IP address.</param>
		/// <param name="routerPort">Router server port.</param>
		/// <param name="dbPort">Departure board server port.</param>
		public static void Start(IPAddress address, int routerPort, int dbPort)
		{
			Router = new RouterServer(address, routerPort);
			DepartureBoard = new DepartureBoardServer(address, dbPort);

			Router.OperationThread.Start();
			DepartureBoard.OperationThread.Start();

			IsStopped = false;

			Logging.Log("The server has started.");
		}
		/// <summary>
		/// Stops the server.
		/// </summary>
		public static void Stop()
		{
			Router.Stop();
			DepartureBoard.Stop();

			IsStopped = true;

			Logging.Log("The server has stopped.");
		}
		/// <summary>
		/// Method that serves manipulation with the server while running, block the current thread.
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
				}
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
		public RouterServer(IPAddress address, int port)
		{
			ServerListener = new TcpListener(address, port);
			ServerListener.Start();

			IpAddress = address;
			Port = port;

			Logging.Log($"Listening at { ((IPEndPoint)ServerListener.Server.LocalEndPoint).Address.ToString() }:{ Port }.");

			OperationThread = new Thread(async () =>
			{
				while (true)
				{
					var client = await ServerListener.AcceptTcpClientAsync();

					Logging.Log($"Connection request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() }.");

					new RouterProcessing(client).ProcessAsync().Start();
				}
			});
		}
		/// <summary>
		/// Stops the server.
		/// </summary>
		public new void Stop()
		{
			ServerListener.Stop();
			OperationThread.Abort();
			OperationThread.Join();
		}
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
		public DepartureBoardServer(IPAddress address, int port)
		{
			ServerListener = new TcpListener(address, port);
			ServerListener.Start();

			IpAddress = address;
			Port = port;

			Logging.Log($"Listening at { ((IPEndPoint)ServerListener.Server.LocalEndPoint).Address.ToString() }:{ Port }.");

			OperationThread = new Thread(async () =>
			{
				while (true)
				{
					var client = await ServerListener.AcceptTcpClientAsync();

					Logging.Log($"Connection request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() }.");

					new DepartureBoardProcessing(client).ProcessAsync().Start();
				}
			});
		}
		/// <summary>
		/// Stops the server.
		/// </summary>
		public new void Stop()
		{
			ServerListener.Stop();
			OperationThread.Abort();
			OperationThread.Join();
		}
	}
}

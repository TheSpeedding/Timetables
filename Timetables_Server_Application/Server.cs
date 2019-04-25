using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		public enum ServerSignal
		{
			[Description("HELP - Shows this help.")]
			Help = 0,
			[Description("ABORT - Aborts server execution.")]
			Abort = 1,
			[Description("FORCEUPDATE - Forces data update.")]
			ForceUpdate = 2,
			[Description("RESTART - Restarts the server.")]
			Restart = 3
		}
		/// <summary>
		/// Returns help for server signals.
		/// </summary>
		public static string GetServerSignalHelp()
		{
			string ToDescriptionString(ServerSignal val)
			{
				DescriptionAttribute[] attributes = (DescriptionAttribute[])val
				   .GetType()
				   .GetField(val.ToString())
				   .GetCustomAttributes(typeof(DescriptionAttribute), false);
				return attributes.Length > 0 ? attributes[0].Description : string.Empty;
			}

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < Enum.GetNames(typeof(ServerSignal)).Length; i++)
				sb.AppendLine(ToDescriptionString((ServerSignal)i));

			return sb.ToString();
		}
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

			if (routerPort >= 0) Router = new RouterServer(address, routerPort);
			if (dbPort >= 0) DepartureBoard = new DepartureBoardServer(address, dbPort);
			if (dfPort >= 0) BasicData = new BasicDataFeedServer(address, dfPort);

			Router?.Start();
			DepartureBoard?.Start();
			BasicData?.Start();

			IsStopped = false;
		}
		/// <summary>
		/// Stops the server.
		/// </summary>
		public static void Stop()
		{
			Router?.StopServer();
			DepartureBoard?.StopServer();
			BasicData?.StopServer();

			IsStopped = true;

			Logging.Log("The server has stopped.");
		}
		/// <summary>
		/// Stops the server.
		/// </summary>
		private void StopServer()
		{
			OperationThread.Abort();
			OperationThread.Join();
			ServerListener.Stop();
		}
		/// <summary>
		/// Method that serves manipulation with the server while running, blocks the current thread.
		/// </summary>
		public static void ServerManipulation()
		{
			string cmd;
			while ((cmd = Console.ReadLine()) != null)
				switch ((ServerSignal)Enum.GetNames(typeof(ServerSignal)).Select(s => s.ToUpperInvariant()).ToList().IndexOf(cmd))
				{
					case ServerSignal.Abort:
						Logging.Log("Request to stop the server received.");
						Environment.Exit(0); // This is not obviously the best solution.
						break;
					case ServerSignal.ForceUpdate:
						Logging.Log("Request to force data update received.");
						DataFeed.DownloadAndLoad(true);
						break;
					case ServerSignal.Restart:
						Logging.Log("Request to restart the server received.");
						Logging.Dispose();
						System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
						Environment.Exit(0);
						break;
					case ServerSignal.Help:
						Logging.Log("You can use these commands: " + Environment.NewLine + GetServerSignalHelp());
						break;
					default:
						Logging.Log("Unknown command.");
						goto case ServerSignal.Help;
				}
		}
		/// <summary>
		/// Starts the server.
		/// </summary>
		public void Start()
		{
			ServerListener.Start();
			OperationThread.Start();

			Logging.Log($"Server { GetType().Name } listening at { ((IPEndPoint)ServerListener.Server.LocalEndPoint).Address.ToString() }:{ Port }.");
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
					try
					{
						TcpClient client = await ServerListener.AcceptTcpClientAsync();

						Logging.Log($"Connection request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() }.");

						processingAction(client);
					}
					catch (Exception ex)
					{
						Logging.Log(Logging.LogException(ex));
					}
				}
			});
		}
		public void Dispose() => StopServer();
	}

	/// <summary>
	/// Router server class.
	/// </summary>
	public sealed class RouterServer : Server
	{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		/// <summary>
		/// Initializes new instance of router server.
		/// </summary>
		/// <param name="address">IP address.</param>
		/// <param name="port">Port number.</param>
		public RouterServer(IPAddress address, int port) => CreateServer((TcpClient client) => new RouterProcessing(client).ProcessAsync(), address, port);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
	}

	/// <summary>
	/// Basic data feed server class.
	/// </summary>
	public sealed class BasicDataFeedServer : Server
	{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		/// <summary>
		/// Initializes new instance of basic data feed server.
		/// </summary>
		/// <param name="address">IP address.</param>
		/// <param name="port">Port number.</param>
		public BasicDataFeedServer(IPAddress address, int port) => CreateServer((TcpClient client) => new DataFeedProcessing(client).ProcessAsync(), address, port);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
	}

	/// <summary>
	/// Departure board server class.
	/// </summary>
	public sealed class DepartureBoardServer : Server
	{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		/// <summary>
		/// Initializes new instance of departure board server.
		/// </summary>
		/// <param name="address">IP address.</param>
		/// <param name="port">Port number.</param>
		public DepartureBoardServer(IPAddress address, int port) => CreateServer((TcpClient client) => new DepartureBoardProcessing(client).ProcessAsync(), address, port);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;

namespace Timetables.Server
{
	public static class DataFeed // TEMPORARY
	{
		public static Interop.DataFeedManaged Full { get; private set; } = new Interop.DataFeedManaged();
	}

	/// <summary>
	/// Class that supplies methods for parsing Tcp streams, their operation and answers to the requests.
	/// </summary>
	abstract class TcpProcessing
	{
		protected TcpClient client;
		/// <summary>
		/// Abstract class for processing incoming connections.
		/// </summary>
		/// <returns></returns>
		public abstract Task ProcessAsync();
		/// <summary>
		/// Deserializes the object from the network stream using binary formetter.
		/// </summary>
		/// <typeparam name="T">Object to deserialize.</typeparam>
		/// <returns>Object.</returns>
		public T Receive<T>()
		{
			using (NetworkStream stream = client.GetStream())
			{
				IFormatter formatter = new BinaryFormatter();
				return (T)formatter.Deserialize(stream);
			}
		}
		/// <summary>
		/// Serializes the object so it can be sent via network stream.
		/// </summary>
		/// <typeparam name="T">Object to serialize.</typeparam>
		/// <param name="item">Object to serialize.</param>
		public void Send<T>(T item)
		{
			using (NetworkStream stream = client.GetStream())
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, item);

			}
		}
	}

	/// <summary>
	/// Specialized class derived from TcpProcessing for router requests.
	/// </summary>
	sealed class RouterProcessing : TcpProcessing
	{
		public RouterProcessing(TcpClient client) => this.client = client;
		public async override Task ProcessAsync()
		{
			try
			{
				var routerReq = Receive<RouterRequest>();

				Logging.Log($"Received router request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() }.");

				RouterResponse routerRes = null;

				await Task.Run(() =>
				{
					using (var routerProcessing = new Interop.RouterManaged(DataFeed.Full, routerReq))
					{
						routerProcessing.ObtainJourneys();
						routerRes = routerProcessing.ShowJourneys();
					}
				});

				Send(routerRes);

				Logging.Log($"Router response to { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() } was successfully sent.");
			}
			catch
			{
				Logging.Log($"Router request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() } could not be processed.");
			}
		}
	}

	/// <summary>
	/// Specialized class derived from TcpProcessing for departure board requests.
	/// </summary>
	sealed class DepartureBoardProcessing : TcpProcessing
	{
		public DepartureBoardProcessing(TcpClient client) => this.client = client;
		public async override Task ProcessAsync()
		{
			try
			{
				var dbRequest = Receive<DepartureBoardRequest>();

				Logging.Log($"Received departure board request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() }.");

				DepartureBoardResponse dbRes = null;

				await Task.Run(() =>
				{
					using (var dbProcessing = new Interop.DepartureBoardManaged(DataFeed.Full, dbRequest))
					{
						dbProcessing.ObtainDepartureBoard();
						dbRes = dbProcessing.ShowDepartureBoard();
					}
				});

				Send(dbRes);

				Logging.Log($"Departure board response to { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() } was successfully send.");
			}
			catch
			{
				Logging.Log($"Departure board request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() } could not be processed.");
			}
		}
	}
}

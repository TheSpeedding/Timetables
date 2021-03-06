﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;
using Timetables.Structures.Basic;

namespace Timetables.Server
{
	/// <summary>
	/// Class that supplies methods for parsing Tcp streams, their operation and answers to the requests.
	/// </summary>
	abstract class Networking : IDisposable
	{
		protected TcpClient client;
		protected NetworkStream stream;
		public void Dispose()
		{
			stream?.Dispose();
			client?.Dispose();
		}
		/// <summary>
		/// Abstract method for processing incoming connections.
		/// </summary>
		public abstract Task ProcessAsync();
		/// <summary>
		/// Deserializes the object from the network stream using binary formatter.
		/// </summary>
		/// <typeparam name="T">Object to deserialize.</typeparam>
		/// <returns>Object.</returns>
		public async Task<T> Receive<T>()
		{
			var request = Task.Run(() => (T)new BinaryFormatter().Deserialize(stream));

			if (await Task.WhenAny(request, Task.Delay(Settings.TimeoutDuration)) == request && request.Status == TaskStatus.RanToCompletion)
				return await request;

			else
				throw new WebException("Connection timed out.");
		}
		/// <summary>
		/// Serializes the object so it can be sent via network stream.
		/// </summary>
		/// <typeparam name="T">Object to serialize.</typeparam>
		/// <param name="item">Object to serialize.</param>
		public void Send<T>(T item) => new BinaryFormatter().Serialize(stream, item);
	}

	/// <summary>
	/// Specialized class derived from Netowrking for router requests.
	/// </summary>
	sealed class RouterProcessing : Networking
	{
		/// <summary>
		/// Initializes the object.
		/// </summary>
		/// <param name="client">TCP client.</param>
		public RouterProcessing(TcpClient client)
		{
			this.client = client;
			stream = client.GetStream();
		}
		/// <summary>
		/// Processes the request async.
		/// </summary>
		public async override Task ProcessAsync()
		{
			try
			{
				RouterRequest routerReq = await Receive<RouterRequest>();

				Logging.Log($"Received router request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() }. Data: { routerReq.ToString() }");

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
			catch (Exception ex)
			{
				Logging.Log($"Router request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() } could not be processed. { Logging.LogException(ex) }");
			}
			Dispose();
		}
	}
	/// <summary>
	/// Specialized class derived from Netowrking for data feed requests.
	/// </summary>
	sealed class DataFeedProcessing : Networking
	{
		/// <summary>
		/// Initializes the object.
		/// </summary>
		/// <param name="client">TCP client.</param>
		public DataFeedProcessing(TcpClient client)
		{
			this.client = client;
			stream = client.GetStream();
		}

		/// <summary>
		/// Processes the request async.
		/// </summary>
		public async override Task ProcessAsync()
		{
			try
			{
				DataFeedBasicRequest dfRequest;

				try
				{
					dfRequest = await Receive<Structures.Basic.DataFeedBasicRequest>();
				}
				catch (WebException)
				{
					dfRequest = new DataFeedBasicRequest("ForceDownload");
				}

				Logging.Log($"Received data feed request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() }. Data: { dfRequest.ToString() }");

				Structures.Basic.DataFeedBasicResponse dfRes = null;

				await Task.Run(() =>
				{
					if (dfRequest.Version == "ForceDownload" || dfRequest.Version != DataFeed.Basic.Version)
					{
						dfRes = new Structures.Basic.DataFeedBasicResponse() { Data = DataFeed.Basic };
					}

					else
					{
						dfRes = new Structures.Basic.DataFeedBasicResponse();
					}
				});

				Send(dfRes);

				Logging.Log($"Data feed response to { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() } was successfully send.");
			}
			catch (Exception ex)
			{
				Logging.Log($"Data feed request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() } could not be processed. { Logging.LogException(ex) }");
			}
			Dispose();

		}
	}
	/// <summary>
	/// Specialized class derived from Netowrking for departure board requests.
	/// </summary>
	sealed class DepartureBoardProcessing : Networking
	{
		/// <summary>
		/// Initializes the object.
		/// </summary>
		/// <param name="client">TCP client.</param>
		public DepartureBoardProcessing(TcpClient client)
		{
			this.client = client;
			stream = client.GetStream();
		}
		/// <summary>
		/// Processes the request async.
		/// </summary>
		public async override Task ProcessAsync()
		{
			try
			{
				DepartureBoardRequest dbRequest = await Receive<DepartureBoardRequest>();

				Logging.Log($"Received departure board request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() }. Data: { dbRequest.ToString() }");

				DepartureBoardResponse dbRes = null;

				await Task.Run(() =>
				{
					if (dbRequest is StationInfoRequest)
						using (var dbProcessing = new Interop.DepartureBoardManaged(DataFeed.Full, (StationInfoRequest)dbRequest))
						{
							dbProcessing.ObtainDepartureBoard();
							dbRes = dbProcessing.ShowDepartureBoard();
						}
					else if (dbRequest is LineInfoRequest)
						using (var dbProcessing = new Interop.DepartureBoardManaged(DataFeed.Full, (LineInfoRequest)dbRequest))
						{
							dbProcessing.ObtainDepartureBoard();
							dbRes = dbProcessing.ShowDepartureBoard();
						}
					else
						throw new NotImplementedException();
				});

				Send(dbRes);

				Logging.Log($"Departure board response to { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() } was successfully send.");
			}
			catch (Exception ex)
			{
				Logging.Log($"Departure board request from { ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() } could not be processed. { Logging.LogException(ex) }");
			}
			Dispose();
		}
	}
}

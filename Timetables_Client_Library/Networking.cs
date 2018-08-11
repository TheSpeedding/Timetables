using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Timetables.Structures.Basic;

namespace Timetables.Client
{
	/// <summary>
	/// Interface that ensures some kind of requests will be processed asynchronously and some kind of response will be returned.
	/// </summary>
	/// <typeparam name="Request">Request type.</typeparam>
	/// <typeparam name="Response">Response type.</typeparam>
	public interface IOnlineProcessible<Request, Response>
	{
		/// <summary>
		/// Processes the request asynchronously.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="timeout">Timeout, when reached the processing will exit immediately with an exception thrown.</param>
		/// <returns>Response.</returns>
		Task<Response> ProcessAsync(Request request, int timeout);
	}

	/// <summary>
	/// Class that supplies methods for parsing Tcp streams, their operation and answers to the requests.
	/// </summary>
	public abstract class Networking : IDisposable
	{
		private TcpClient client = new TcpClient();
		private NetworkStream stream;
		public void Dispose()
		{
			stream?.Dispose();
			client?.Dispose();
		}
		/// <summary>
		/// Connects client to the specified host.
		/// </summary>
		/// <param name="host">Hostname.</param>
		/// <param name="port">Port number.</param>
		public async Task ConnectAsync(IPAddress host, uint port)
		{
			await client.ConnectAsync(host, (int)port);
			stream = client.GetStream();
		}
		/// <summary>
		/// Deserializes the object from the network stream using binary formatter.
		/// </summary>
		/// <typeparam name="T">Object to deserialize.</typeparam>
		/// <returns>Object.</returns>
		protected T Receive<T>() => (T)new BinaryFormatter().Deserialize(stream);
		/// <summary>
		/// Serializes the object so it can be sent via network stream.
		/// </summary>
		/// <typeparam name="T">Object to serialize.</typeparam>
		/// <param name="item">Object to serialize.</param>
		protected void Send<T>(T item) => new BinaryFormatter().Serialize(stream, item);
	}
	/// <summary>
	/// Specialized class for router processing.
	/// </summary>
	public sealed class RouterProcessing : Networking, IOnlineProcessible<RouterRequest, RouterResponse>
	{
		/// <summary>
		/// Connects host to the router server.
		/// </summary>
		/// <returns></returns>
		public async Task ConnectAsync() => await ConnectAsync(DataFeed.ServerIpAddress, DataFeed.RouterPortNumber);
		/// <summary>
		/// Receives router response.
		/// </summary>
		/// <returns></returns>
		public RouterResponse GetResponse() => Receive<RouterResponse>();
		/// <summary>
		/// Sends router request.
		/// </summary>
		/// <param name="request">Request.</param>
		public void SendRequest(RouterRequest request) => Send(request);
		/// <summary>
		/// Processes the request asynchronously.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <returns>Response.</returns>
		public async Task<RouterResponse> ProcessAsync(RouterRequest request, int timeout)
		{
			RouterResponse response = null;

			var connection = ConnectAsync();

			if (await Task.WhenAny(connection, Task.Delay(timeout)) == connection)
			{
				SendRequest(request);

				response = await Task.Run(() => GetResponse());
			}

			else
				throw new WebException();

			return response;
		}
	}
	/// <summary>
	/// Specialized class for departure board processing.
	/// </summary>
	public sealed class DepartureBoardProcessing : Networking, IOnlineProcessible<DepartureBoardRequest, DepartureBoardResponse>
	{
		/// <summary>
		/// Connects host to the departure board server.
		/// </summary>
		/// <returns></returns>
		public async Task ConnectAsync() => await ConnectAsync(DataFeed.ServerIpAddress, DataFeed.DepartureBoardPortNumber);
		/// <summary>
		/// Receives departure board response.
		/// </summary>
		/// <returns></returns>
		public DepartureBoardResponse GetResponse() => Receive<DepartureBoardResponse>();
		/// <summary>
		/// Sends departure board request.
		/// </summary>
		/// <param name="request">Request.</param>
		public void SendRequest(DepartureBoardRequest request) => Send(request);
		/// <summary>
		/// Processes the request asynchronously.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <returns>Response.</returns>
		public async Task<DepartureBoardResponse> ProcessAsync(DepartureBoardRequest request, int timeout)
		{
			DepartureBoardResponse response = null;

			var connection = ConnectAsync();

			if (await Task.WhenAny(connection, Task.Delay(timeout)) == connection)
			{
				SendRequest(request);

				response = await Task.Run(() => GetResponse());
			}

			else
				throw new WebException();

			return response;
		}
	}
	/// <summary>
	/// Specialized class for basic data downloading.
	/// </summary>
	public sealed class BasicDataProcessing : Networking, IOnlineProcessible<DataFeedBasicRequest, DataFeedBasicResponse>
	{
		/// <summary>
		/// Connects host to the basic data feed server.
		/// </summary>
		/// <returns></returns>
		public async Task ConnectAsync() => await ConnectAsync(DataFeed.ServerIpAddress, DataFeed.BasicDataPortNumber);
		/// <summary>
		/// Processes the request asynchronously.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <returns>Response.</returns>
		public async Task<DataFeedBasicResponse> ProcessAsync(DataFeedBasicRequest request, int timeout)
		{

			DataFeedBasicResponse response = null;

			var connection = ConnectAsync();

			if (await Task.WhenAny(connection, Task.Delay(timeout)) == connection)
			{
				Send(request);

				response = await Task.Run(() => Receive<Structures.Basic.DataFeedBasicResponse>());
			}

			else
				throw new WebException();

			return response;	
		}
	}
}

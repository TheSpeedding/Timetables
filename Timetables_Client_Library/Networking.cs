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
	/// Class that supplies methods for parsing Tcp streams, their operation and answers to the requests.
	/// </summary>
	public abstract class Networking<Req, Res> : IDisposable
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
		protected async Task ConnectAsync(IPAddress host, int port)
		{
			await client.ConnectAsync(host, port);
			stream = client.GetStream();
		}
		/// <summary>
		/// Connects client to the host.
		/// </summary>
		protected abstract Task ConnectAsync();
		/// <summary>
		/// Deserializes the object from the network stream using binary formatter.
		/// </summary>
		/// <typeparam name="T">Object to deserialize.</typeparam>
		/// <returns>Object.</returns>
		private Res Receive() => (Res)new BinaryFormatter().Deserialize(stream);
		/// <summary>
		/// Serializes the object so it can be sent via network stream.
		/// </summary>
		/// <param name="item">Object to serialize.</param>
		private void Send(Req item) => new BinaryFormatter().Serialize(stream, item);
		/// <summary>
		/// Processes the request asynchronously.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <returns>Response.</returns>
		public async Task<Res> ProcessAsync(Req request, int timeout)
		{
			Res response = default(Res);

			bool suppressErrorsOnTimeout = (request as RequestBase).Count == -1;

			var connection = ConnectAsync();
			
			if (await Task.WhenAny(connection, Task.Delay(timeout)) == connection && connection.Status == TaskStatus.RanToCompletion)
			{
				Send(request);

				response = await Task.Run(() => Receive());
			}

			else
				throw new WebException("Server offline.");

			return response;
		}
	}
	/// <summary>
	/// Specialized class for router processing.
	/// </summary>
	public sealed class RouterProcessing : Networking<RouterRequest, RouterResponse>
	{
		/// <summary>
		/// Connects host to the router server.
		/// </summary>
		protected override async Task ConnectAsync() => await ConnectAsync(DataFeedClient.ServerIpAddress, DataFeedClient.RouterPortNumber);
	}
	/// <summary>
	/// Specialized class for departure board processing.
	/// </summary>
	public sealed class DepartureBoardProcessing : Networking<DepartureBoardRequest, DepartureBoardResponse>
	{
		/// <summary>
		/// Connects host to the departure board server.
		/// </summary>
		protected override async Task ConnectAsync() => await ConnectAsync(DataFeedClient.ServerIpAddress, DataFeedClient.DepartureBoardPortNumber);
	}
	/// <summary>
	/// Specialized class for basic data downloading.
	/// </summary>
	public sealed class BasicDataProcessing : Networking<DataFeedBasicRequest, DataFeedBasicResponse>
	{
		/// <summary>
		/// Connects host to the basic data feed server.
		/// </summary>
		protected override async Task ConnectAsync() => await ConnectAsync(DataFeedClient.ServerIpAddress, DataFeedClient.BasicDataPortNumber);
	}
}

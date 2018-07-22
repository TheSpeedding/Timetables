using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Timetables.Client
{
	/// <summary>
	/// Class that supplies methods for parsing Tcp streams, their operation and answers to the requests.
	/// </summary>
	public class TcpProcessing : IDisposable
	{
		private TcpClient client = new TcpClient();
		private NetworkStream stream;
		public void Dispose()
		{
			stream.Dispose();
			client.Dispose();
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
		public T Receive<T>()
		{
			IFormatter formatter = new BinaryFormatter();
			return (T)formatter.Deserialize(stream);
		}
		/// <summary>
		/// Serializes the object so it can be sent via network stream.
		/// </summary>
		/// <typeparam name="T">Object to serialize.</typeparam>
		/// <param name="item">Object to serialize.</param>
		public void Send<T>(T item)
		{
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, item);
		}
	}
	public class RouterProcessing : TcpProcessing
	{
		public async Task ConnectAsync() => await ConnectAsync(DataFeed.ServerIpAddress, DataFeed.RouterPortNumber);
	}
	public class DepartureBoardProcessing : TcpProcessing
	{
		public async Task ConnectAsync() => await ConnectAsync(DataFeed.ServerIpAddress, DataFeed.DepartureBoardPortNumber);
	}
}

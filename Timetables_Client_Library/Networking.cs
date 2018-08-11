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
	public class Networking : IDisposable
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
	public sealed class RouterProcessing : Networking
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
		/// <param name="rr">Request.</param>
		public void SendRequest(RouterRequest rr) => Send(rr);
	}
	/// <summary>
	/// Specialized class for departure board processing.
	/// </summary>
	public sealed class DepartureBoardProcessing : Networking
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
		/// <param name="rr">Request.</param>
		public void SendRequest(DepartureBoardRequest dbr) => Send(dbr);
	}
	/// <summary>
	/// Specialized class for basic data downloading.
	/// </summary>
	public sealed class BasicDataProcessing : Networking
	{
		/// <summary>
		/// Connects host to the basic data feed server.
		/// </summary>
		/// <returns></returns>
		public async Task ConnectAsync() => await ConnectAsync(DataFeed.ServerIpAddress, DataFeed.BasicDataPortNumber);
		/// <summary>
		/// Downloads the data from the remote server.
		/// </summary>
		public void DownloadData()
		{
			try
			{
				using (var sr = new System.IO.StreamReader("basic/.version"))
					Send(new Structures.Basic.DataFeedBasicRequest(sr.ReadLine()));
			}
			catch // Forces data to be downloaded.
			{
				Send(new Structures.Basic.DataFeedBasicRequest());
			}

			var response = Receive<Structures.Basic.DataFeedBasicResponse>();

			if (response.ShouldBeUpdated)
			{
				response.Data.Save();
				using (var sw = new System.IO.StreamWriter("basic/.version"))
					sw.WriteLine(response.Version);
			}
		}
	}
}

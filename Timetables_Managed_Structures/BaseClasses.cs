using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Client
{
	[Serializable]
	public abstract class RequestBase
	{
		/// <summary>
		/// Number of items to find.
		/// </summary>
		public int Count { get; protected set; } = -1;
		/// <summary>
		/// Maximal arrival datetime as Unix timestamp.
		/// </summary>
		public ulong MaximalArrivalDateTime { get; protected set; }
		/// <summary>
		/// Decides if the algorithm should search by maximal arrival datetime.
		/// </summary>
		public bool SearchByMaximalArrivalDateTime => Count == -1;
		/// <summary>
		/// Decides if the algorithm should search by count.
		/// </summary>
		public bool SearchByCount => !SearchByMaximalArrivalDateTime;

	}
	[Serializable]
	public abstract class ResponseBase
	{
		/// <summary>
		/// Specifies when was the response created. Used in caching.
		/// </summary>
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}

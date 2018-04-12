using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Structures.Basic
{
	public class StopsBasic
	{
		public class StopBasic
		{
			/// <summary>
			/// Stop ID.
			/// </summary>
			public uint ID { get; }
			/// <summary>
			/// Parent station.
			/// </summary>
			public StationsBasic.StationBasic ParentStation { get; }
			/// <summary>
			/// Name of the stop.
			/// </summary>
			public string Name { get { return ParentStation.Name; } }
			/// <summary>
			/// Latitude of the stop.
			/// </summary>
			public double Latitude { get; }
			/// <summary>
			/// Longitude of the stop.
			/// </summary>
			public double Longitude { get; }
			/// <summary>
			/// List of all routes that goes through this stop.
			/// </summary>
			public List<object> ThroughgoingRoutes { get; }
		}
	}
}

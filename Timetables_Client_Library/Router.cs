using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetables.Client
{
	/// <summary>
	/// Class serving router requests.
	/// </summary>
	[Serializable]
	public class RouterRequest
	{
		/// <summary>
		/// Source station identificator.
		/// </summary>
		public uint SourceStationID { get; }
		/// <summary>
		/// Target station identificator.
		/// </summary>
		public uint TargetStationID { get; }
		/// <summary>
		/// Earliest departure date time.
		/// </summary>
		public ulong EarliestDepartureDateTime { get; }
		/// <summary>
		/// Max transfers in the journey.
		/// </summary>
		public uint MaxTransfers { get; }
		/// <summary>
		/// Number of journeys to find.
		/// </summary>
		public uint Count { get; }
		/// <summary>
		/// Coefficient that is transfer duration multiplied with.
		/// </summary>
		public double TransfersDurationCoefficient { get; }
		public RouterRequest(uint sourceStationID, uint targetStationID, DateTime departureDateTime, uint transfers, uint count, double coefficient)
		{
			SourceStationID = sourceStationID;
			TargetStationID = targetStationID;
			EarliestDepartureDateTime = (ulong)(departureDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			MaxTransfers = transfers;
			Count = count;
			TransfersDurationCoefficient = coefficient;
		}
	}

	/// <summary>
	/// Class serving router responses.
	/// </summary>
	[Serializable]
	public class RouterResponse
	{
		/// <summary>
		/// List of journeys found by the algorithms.
		/// </summary>
		public List<Journey> Journeys { get; }
		public RouterResponse(List<Journey> journeys) => Journeys = journeys;
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
		public int SourceStationID { get; }
		/// <summary>
		/// Target station identificator.
		/// </summary>
		public int TargetStationID { get; }
		/// <summary>
		/// Earliest departure date time.
		/// </summary>
		public ulong EarliestDepartureDateTime { get; }
		/// <summary>
		/// Max transfers in the journey.
		/// </summary>
		public int MaxTransfers { get; }
		/// <summary>
		/// Number of journeys to find.
		/// </summary>
		public int Count { get; }
		/// <summary>
		/// Coefficient that is transfer duration multiplied with.
		/// </summary>
		public double TransfersDurationCoefficient { get; }
		/// <summary>
		/// Means of transport that can be used in journey.
		/// </summary>
		public MeanOfTransport MeansOfTransport { get; }
		public RouterRequest(int sourceStationID, int targetStationID, DateTime departureDateTime, int transfers, int count, double coefficient, MeanOfTransport mot)
		{
			SourceStationID = sourceStationID;
			TargetStationID = targetStationID;
			EarliestDepartureDateTime = (ulong)(departureDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			MaxTransfers = transfers;
			Count = count;
			TransfersDurationCoefficient = coefficient;
			MeansOfTransport = mot;
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
		public List<Journey> Journeys { get; set; }
		public RouterResponse() => Journeys = new List<Journey>();
		public RouterResponse(List<Journey> journeys) => Journeys = journeys;
		/// <summary>
		/// Serializes object into the text writer.
		/// </summary>
		/// <param name="writer">Text writer.</param>
		public void Serialize(TextWriter writer) => new XmlSerializer(typeof(RouterResponse)).Serialize(writer, this);
	}
}
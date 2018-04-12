using System;
using System.Collections.Generic;
using System.Drawing;

namespace Timetables.Client
{
	/// <summary>
	/// Class serving departure board requests.
	/// </summary>
	[Serializable]
    public class DepartureBoardRequest
    {
		/// <summary>
		/// Stop identificator.
		/// </summary>
		public uint StopID { get; }
		/// <summary>
		/// Departure time as Unix timestamp.
		/// </summary>
		public ulong EarliestDepartureDateTime { get; }
		/// <summary>
		/// Number of departures wanted.
		/// </summary>
		public uint Count { get; }
		/// <summary>
		/// True if station, false state is used in mobile application.
		/// </summary>
		public bool IsStation { get; }
		public DepartureBoardRequest(uint stopID, DateTime departureTime, uint count, bool station)
		{
			StopID = stopID;
			EarliestDepartureDateTime = (ulong)(departureTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			Count = count;
			IsStation = station;
		}
    }

	/// <summary>
	/// Class serving departure board responses.
	/// </summary>
	[Serializable]
	public class DepartureBoardResponse
	{
		/// <summary>
		/// List of departures found by the algorithms.
		/// </summary>
		public List<Departure> Departures { get; }
		public DepartureBoardResponse(List<Departure> departures) => Departures = departures;
	}
	/// <summary>
	/// Class serving information about one departure.
	/// </summary>
	[Serializable]
	public class Departure
	{
		/// <summary>
		/// Stop identificator.
		/// </summary>
		public uint StopID { get; }
		/// <summary>
		/// Indicated whether departure uses outdated timetables.
		/// </summary>
		public bool Outdated { get; }
		/// <summary>
		/// Headsign of the trip.
		/// </summary>
		public string Headsign { get; }
		/// <summary>
		/// Short name of the line.
		/// </summary>
		public string LineLabel { get; }
		/// <summary>
		/// Long name of the line.
		/// </summary>
		public string LineName { get; }
		/// <summary>
		/// Color of the line used in GUI.
		/// </summary>
		public Color LineColor { get; }
		/// <summary>
		/// Mean of transportation.
		/// </summary>
		public MeanOfTransport MeanOfTransport { get; }
		/// <summary>
		/// Date time of departure.
		/// </summary>
		public DateTime DepartureDateTime { get; }
		/// <summary>
		/// Following stops in the trip. Departure date time and their identificators.
		/// </summary>
		public List<KeyValuePair<DateTime, uint>> IntermediateStops { get; }
		public Departure(uint stopID, bool outdated, string headsign, string lineLabel, string lineName, ulong lineColor, uint meanOfTransport, ulong departureDateTime, List<KeyValuePair<ulong, uint>> intStops)
		{
			StopID = stopID;
			Outdated = outdated;
			Headsign = headsign;
			LineLabel = lineLabel;
			LineName = lineName;
			LineColor = ColorTranslator.FromHtml("#" + lineColor.ToString("X"));
			MeanOfTransport = (MeanOfTransport)meanOfTransport;
			DepartureDateTime = new DateTime(1970, 1, 1).AddSeconds(departureDateTime);

			IntermediateStops = new List<KeyValuePair<DateTime, uint>>();
			foreach (var x in intStops)
				IntermediateStops.Add(new KeyValuePair<DateTime, uint>(new DateTime(1970, 1, 1).AddSeconds(x.Key), x.Value));
		}
	}
}

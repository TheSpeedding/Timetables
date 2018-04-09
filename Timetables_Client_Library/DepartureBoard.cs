using System;
using System.Collections.Generic;
using System.Drawing;

namespace Timetables.Client
{
	[Serializable]
    public class DepartureBoardRequest
    {
		public uint StopID { get; }
		public ulong DepartureTime { get; }
		public uint Count { get; }
		public bool TrueIfStation { get; }
		public DepartureBoardRequest(uint stopID, DateTime departureTime, uint count, bool station)
		{
			StopID = stopID;
			DepartureTime = (ulong)(departureTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			Count = count;
			TrueIfStation = station;
		}
    }

	[Serializable]
	public class DepartureBoardReply
	{
		public List<Departure> Departures { get; }
		public DepartureBoardReply(List<Departure> departures) => Departures = departures;
	}

	[Serializable]
	public class Departure
	{
		public uint StopID { get; }
		public bool Outdated { get; }
		public string Headsign { get; }
		public string LineLabel { get; }
		public string LineName { get; }
		public Color LineColor { get; }
		public MeanOfTransport MeanOfTransport { get; }
		public List<KeyValuePair<DateTime, uint>> IntermediateStops { get; } // Time of arrival, stop ID.
		public Departure(uint stopID, bool outdated, string headsign, string lineLabel, string lineName, ulong lineColor, uint meanOfTransport, List<KeyValuePair<ulong, uint>> intStops)
		{
			StopID = stopID;
			Outdated = outdated;
			Headsign = headsign;
			LineLabel = lineLabel;
			LineName = lineName;
			LineColor = ColorTranslator.FromHtml("#" + lineColor.ToString("X"));
			MeanOfTransport = (MeanOfTransport)meanOfTransport;
			
			IntermediateStops = new List<KeyValuePair<DateTime, uint>>();
			foreach (var x in intStops)
				IntermediateStops.Add(new KeyValuePair<DateTime, uint>(new DateTime(1970, 1, 1).AddSeconds(x.Key), x.Value));
		}
	}
}

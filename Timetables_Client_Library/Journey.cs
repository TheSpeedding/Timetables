using System;
using System.Collections.Generic;
using System.Drawing;

namespace Timetables.Client
{
	/// <summary>
	/// Class serving information about one journey.
	/// </summary>
	[Serializable]
	public class Journey
	{
		/// <summary>
		/// Duration of the journey.
		/// </summary>
		public TimeSpan Duration { get { return ArrivalDateTime - DepartureDateTime; } }
		/// <summary>
		/// Departure date time from the source stop.
		/// </summary>
		public DateTime DepartureDateTime { get { if (JourneySegments.Count == 0) return default(DateTime); return JourneySegments[0].DepartureDateTime; } }
		/// <summary>
		/// Arrival date time at the target stop.
		/// </summary>
		public DateTime ArrivalDateTime { get { if (JourneySegments.Count == 0) return default(DateTime); return JourneySegments[JourneySegments.Count - 1].ArrivalDateTime; } }
		/// <summary>
		/// Indicates whether journey uses outdated timetables.
		/// </summary>
		public bool Outdated { get { foreach (var js in JourneySegments) if (js.Outdated) return true; return false; } }
		/// <summary>
		/// List of journey segments.
		/// </summary>
		public List<JourneySegment> JourneySegments { get; }
		public Journey(List<JourneySegment> list) => JourneySegments = list;
	}

	/// <summary>
	/// Class serving information about one journey segment.
	/// </summary>
	[Serializable]
	public abstract class JourneySegment
	{
		/// <summary>
		/// Source stop identificator.
		/// </summary>
		public uint SourceStopID { get; protected set; }
		/// <summary>
		/// Target stop identificator.
		/// </summary>
		public uint TargetStopID { get; protected set; }
		/// <summary>
		/// Indicates whether segment uses outdated timetables.
		/// </summary>
		public bool Outdated { get; protected set; }
		/// <summary>
		/// Departure date time from the source stop.
		/// </summary>
		public DateTime DepartureDateTime { get; protected set; }
		/// <summary>
		/// Arrival date time at the target top.
		/// </summary>
		public DateTime ArrivalDateTime { get; protected set; }
		/// <summary>
		/// Duration of the segment.
		/// </summary>
		public TimeSpan Duration { get { return ArrivalDateTime - DepartureDateTime; } }
	}

	/// <summary>
	/// Class serving information about one footpath segment.
	/// </summary>
	[Serializable]
	public sealed class FootpathSegment : JourneySegment
	{
		public FootpathSegment(uint sourceStopID, uint targetStopID, ulong departureFromSource, ulong arrivalAtTarget)
		{
			SourceStopID = sourceStopID;
			TargetStopID = targetStopID;
			Outdated = false;
			DepartureDateTime = new DateTime(1970, 1, 1).AddSeconds(departureFromSource);
			ArrivalDateTime = new DateTime(1970, 1, 1).AddSeconds(arrivalAtTarget);
		}
	}

	/// <summary>
	/// Class serving information about one trip segment.
	/// </summary>
	[Serializable]
	public sealed class TripSegment : JourneySegment
	{
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
		/// Intermediate stops in the trip.
		/// </summary>
		public List<KeyValuePair<DateTime, uint>> IntermediateStops { get; }
		public TripSegment(uint sourceStopID, uint targetStopID, bool outdated, string headsign, string lineLabel, string lineName, ulong lineColor, uint meanOfTransport, ulong departureFromSource, ulong arrivalAtTarget, List<KeyValuePair<ulong, uint>> intStops)
		{
			SourceStopID = sourceStopID;
			TargetStopID = targetStopID;
			Outdated = outdated;
			DepartureDateTime = new DateTime(1970, 1, 1).AddSeconds(departureFromSource);
			ArrivalDateTime = new DateTime(1970, 1, 1).AddSeconds(arrivalAtTarget);
			Headsign = headsign;
			LineLabel = lineLabel;
			MeanOfTransport = (MeanOfTransport)meanOfTransport;
			LineName = lineName;
			LineColor = ColorTranslator.FromHtml("#" + lineColor.ToString("X"));

			IntermediateStops = new List<KeyValuePair<DateTime, uint>>();
			foreach (var x in intStops)
				IntermediateStops.Add(new KeyValuePair<DateTime, uint>(new DateTime(1970, 1, 1).AddSeconds(x.Key), x.Value));
		}
	}
}
﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Timetables.Client
{
	/// <summary>
	/// Class serving departure board requests.
	/// </summary>
	[Serializable]
    public abstract class DepartureBoardRequest
	{
		/// <summary>
		/// Departure time as Unix timestamp.
		/// </summary>
		public ulong EarliestDepartureDateTime { get; protected set; }
		/// <summary>
		/// Route identificator. If not set, -1 is a default value.
		/// </summary>
		public int RouteInfoID { get; protected set; } = -1;
		/// <summary>
		/// Number of departures wanted.
		/// </summary>
		public int Count { get; protected set; } = -1;
		/// <summary>
		/// Maximal arrival datetime as Unix timestamp.
		/// </summary>
		public ulong MaximalArrivalDateTime { get; protected set; }
		public bool SearchByMaximalArrivalDateTime => Count == -1;
		public bool SearchByCount => !SearchByMaximalArrivalDateTime;
	}

	/// <summary>
	/// Requests for station info.
	/// </summary>
	[Serializable]
	public sealed class StationInfoRequest : DepartureBoardRequest
	{
		/// <summary>
		/// Stop identificator.
		/// </summary>
		public int StopID { get; }
		/// <summary>
		/// True if station, false state is used in mobile application.
		/// </summary>
		public bool IsStation { get; }
		public StationInfoRequest(int stopID, DateTime departureTime, int count, bool station, int routeID = -1)
		{
			StopID = stopID;
			EarliestDepartureDateTime = (ulong)(departureTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			Count = count;
			IsStation = station;
			RouteInfoID = routeID;
		}
		public StationInfoRequest(int stopID, DateTime departureTime, DateTime arrivalTime, bool station, int routeID = -1)
		{
			StopID = stopID;
			EarliestDepartureDateTime = (ulong)(departureTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			MaximalArrivalDateTime = (ulong)(arrivalTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			IsStation = station;
			RouteInfoID = routeID;
		}
		public override string ToString()
		{
			var idName = IsStation ? "Station" : "Stop";
			var routeInfo = RouteInfoID == -1 ? string.Empty : $"Route Info ID: { RouteInfoID }. ";
			return $"{ idName } ID: { StopID }. { routeInfo }Count: { Count }."; 
		}
	} 

	/// <summary>
	/// Requests for line info.
	/// </summary>
	[Serializable]
	public sealed class LineInfoRequest : DepartureBoardRequest
	{
		public LineInfoRequest(DateTime departureTime, int count, int routeID)
		{
			EarliestDepartureDateTime = (ulong)(departureTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			Count = count;
			RouteInfoID = routeID;
		}
		public LineInfoRequest(DateTime departureTime, DateTime arrivalTime, int routeID)
		{
			EarliestDepartureDateTime = (ulong)(departureTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			MaximalArrivalDateTime = (ulong)(arrivalTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			RouteInfoID = routeID;
		}
		public override string ToString() => $"Line ID: { RouteInfoID }. Count: { Count }.";
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
		public List<Departure> Departures { get; set; }
		public DepartureBoardResponse() => Departures = new List<Departure>();
		public DepartureBoardResponse(List<Departure> departures) => Departures = departures;
		/// <summary>
		/// Serializes object into the text writer.
		/// </summary>
		/// <param name="writer">Text writer.</param>
		public void Serialize(TextWriter writer) => new XmlSerializer(typeof(DepartureBoardResponse)).Serialize(writer, this);
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
		public int StopID { get; set; }
		/// <summary>
		/// Indicated whether departure uses outdated timetables.
		/// </summary>
		public bool Outdated { get; set; }
		/// <summary>
		/// Headsign of the trip.
		/// </summary>
		public string Headsign { get; set; }
		/// <summary>
		/// Short name of the line.
		/// </summary>
		public string LineLabel { get; set; }
		/// <summary>
		/// Long name of the line.
		/// </summary>
		public string LineName { get; set; }
		/// <summary>
		/// Color of the line used in GUI.
		/// </summary>
		[XmlElement(Type = typeof(XmlColor))]
		public Color LineColor { get; set; }
		/// <summary>
		/// Mean of transportation.
		/// </summary>
		public MeanOfTransport MeanOfTransport { get; set; }
		/// <summary>
		/// Date time of departure.
		/// </summary>
		public DateTime DepartureDateTime { get; set; }
		/// <summary>
		/// Following stops in the trip. Departure date time and their identificators.
		/// </summary>
		public List<IntermediateStop> IntermediateStops { get; set; }
		internal Departure() { }
		public Departure(int stopID, bool outdated, string headsign, string lineLabel, string lineName, ulong lineColor, int meanOfTransport, ulong departureDateTime, List<KeyValuePair<ulong, int>> intStops)
		{
			StopID = stopID;
			Outdated = outdated;
			Headsign = headsign;
			LineLabel = lineLabel;
			LineName = lineName;
			LineColor = ColorTranslator.FromHtml("#" + lineColor.ToString("X"));
			MeanOfTransport = (MeanOfTransport)meanOfTransport;
			DepartureDateTime = new DateTime(1970, 1, 1).AddSeconds(departureDateTime);

			IntermediateStops = new List<IntermediateStop>();
			foreach (var x in intStops)
				IntermediateStops.Add(new IntermediateStop(new DateTime(1970, 1, 1).AddSeconds(x.Key), x.Value));
		}

		/// <summary>
		/// Serializes object into the text writer.
		/// </summary>
		/// <param name="writer">Text writer.</param>
		public void Serialize(TextWriter writer) => new XmlSerializer(typeof(Departure)).Serialize(writer, this);
	}
}

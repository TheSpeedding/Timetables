using System;
using System.Collections;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Abstract class for stop times collecting information about stop times.
	/// </summary>
	public abstract class StopTimes : IEnumerable<StopTimes.StopTime>
    {
        public class StopTime
        {
            /// <summary>
            /// Trip that belongs to the stop time.
            /// </summary>
            public Trips.Trip Trip { get; }
            /// <summary>
            /// Arrival time to given stop. Seconds since departure time of the trip.
            /// </summary>
            public int ArrivalTime { get; }
			/// <summary>
			/// Departure time to given stop. Seconds since departure time of the trip.
			/// </summary>
			public int DepartureTime { get; }
            /// <summary>
            /// Stop that belong to the stop time.
            /// </summary>
            public Stops.Stop Stop { get; }
			/// <summary>
			/// Trip ID, Stop ID, Arrival Time, Departure Time.
			/// </summary>
            public override string ToString() => Trip.ID + ";" + Stop.ID + ";" + ArrivalTime + ";" + DepartureTime + ";";
			/// <summary>
			/// Initializes object.
			/// </summary>
			/// <param name="trip">Trip.</param>
			/// <param name="arrival">Arrival.</param>
			/// <param name="departure">Departure.</param>
			/// <param name="stop">Stop.</param>
            public StopTime(Trips.Trip trip, int arrival, int departure, Stops.Stop stop)
            {
                Trip = trip;
				ArrivalTime = arrival;
				DepartureTime = departure;
                Stop = stop;
            }
        }
        protected List<StopTime> list = new List<StopTime>();
        /// <summary>
        /// Gets the total number of stop times.
        /// </summary>
        public int Count => list.Count;
		/// <summary>
		/// Writes the data into given stream.
		/// </summary>
		/// <param name="stopTimes">Stream that the data should be written in.</param>  
		public void Write(System.IO.StreamWriter stopTimes)
        {
			list.Sort((StopTime x, StopTime y) => { return (x.DepartureTime + (x.Trip.DepartureTime % 86400)).CompareTo(y.DepartureTime + (y.Trip.DepartureTime % 86400)); });
            stopTimes.WriteLine(Count); 
            foreach (var item in list)
                stopTimes.Write(item);
            stopTimes.Close();
            stopTimes.Dispose();
        }
		/// <summary>
		/// Converts time in string format to seconds since midnight.
		/// </summary>
		/// <param name="time">String representation of time.</param>
		/// <returns>Seconds since midnight.</returns>
		protected int ConvertTimeToSecondsSinceMidnight(string time)
		{
			// Default format: HH:MM:SS or H:MM:SS

			if (time.Length > 0 && time[0] == ':')
				return 0;
			else if (time.Length != 7 && time.Length != 8)
				throw new FormatException("Invalid time format.");

			int hours = int.Parse(time.Length == 7 ? time.Substring(0, 1) : time.Substring(0, 2));
			int minutes = int.Parse(time.Length == 7 ? time.Substring(2, 2) : time.Substring(3, 2));
			int seconds = int.Parse(time.Length == 7 ? time.Substring(5, 2) : time.Substring(6, 2));

			return hours * 3600 + minutes * 60 + seconds;
		}
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<StopTime> GetEnumerator() => ((IEnumerable<StopTime>)list).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<StopTime>)list).GetEnumerator();
		/// <summary>
		/// Merges two collections into one.
		/// </summary>
		/// <param name="other">The other collection that should be merged.</param>
		public void MergeCollections(StopTimes other)
		{
			foreach (var item in other)
				list.Add(item);
			other = null;
		}
	}
	/// <summary>
	/// Class for stop times with a specific parsing from GTFS format.
	/// </summary>
	public sealed class GtfsStopTimes : StopTimes
	{
		/// <summary>
		/// Initializes object using GTFS data feed.
		/// </summary>
		/// <param name="stops">Stops.</param>
		/// <param name="stopTimes">Stop Times.</param>
		/// <param name="trips">Trip.</param>
		public GtfsStopTimes(System.IO.StreamReader stopTimes, Trips trips, Stops stops)
        {
            // Get order of field names.
            string[] fieldNames = stopTimes.ReadLine().Split(',');
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < fieldNames.Length; i++)
                dic.Add(fieldNames[i].Replace("\"", ""), i);

            // These fields are required for our purpose.
            if (!dic.ContainsKey("arrival_time")) throw new FormatException("Arrival time field name missing.");
            if (!dic.ContainsKey("departure_time")) throw new FormatException("Departure time field name missing.");
            if (!dic.ContainsKey("trip_id")) throw new FormatException("Trip ID field name missing.");
            if (!dic.ContainsKey("stop_id")) throw new FormatException("Stop ID field name missing.");

            while (!stopTimes.EndOfStream)
			{
				IList<string> tokens = GtfsDataFeed.SplitGtfs(stopTimes.ReadLine());

				Trips.Trip trip = null;

				try
				{
					trip = trips[tokens[dic["trip_id"]]];
				}

				catch 
				{
					DataFeed.LogError($"Preprocessor tried to parse a stop-time, but the trip with ID { tokens[dic["trip_id"]] } does not exist. Skipping this item to recover the parsing process.");
					continue;
				}

				if (trip.StopTimes.Count == 0) // Set departure time of the trip.
					trip.DepartureTime = ConvertTimeToSecondsSinceMidnight(tokens[dic["departure_time"]]);

				Stops.Stop stop = null; 

				try
				{
					stop = stops[tokens[dic["stop_id"]]];
				}

				catch 
				{
					DataFeed.LogError($"Preprocessor tried to parse a stop-time to the trip { trip.ID }, line { trip.RouteInfo.ShortName } in direction to { trip.Headsign }, but the stop with ID { tokens[dic["stop_id"]] } does not exist. Skipping this item to recover the parsing process.");
					continue;
				}

				StopTime st = new StopTime(trip, ConvertTimeToSecondsSinceMidnight(tokens[dic["arrival_time"]]) - trip.DepartureTime,
					ConvertTimeToSecondsSinceMidnight(tokens[dic["departure_time"]]) - trip.DepartureTime, stop);
				
				st.Trip.StopTimes.Add(st);

				if (!st.Stop.ThroughgoingRoutes.Contains(st.Trip.RouteInfo))
					st.Stop.ThroughgoingRoutes.Add(st.Trip.RouteInfo);

				list.Add(st);
            }
            stopTimes.Dispose();
        }
    }
}

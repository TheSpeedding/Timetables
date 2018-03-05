using System;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
    public abstract class StopTimes
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
            public override string ToString() => Trip.ID + ";" + Stop.ID + ";" + ArrivalTime + ";" + DepartureTime + ";";
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
        public void Write(System.IO.StreamWriter stopTimes)
        {
            stopTimes.WriteLine(Count);
            foreach (var item in list)
                stopTimes.Write(item);
            stopTimes.Close();
            stopTimes.Dispose();
        }
		protected int ConvertTimeToSecondsSinceMidnight(string time)
		{
			// Default format: HH:MM:SS or H:MM:SS

			if (time.Length != 7 && time.Length != 8)
				throw new FormatException("Invalid time format.");

			int hours = int.Parse(time.Length == 7 ? time.Substring(0, 1) : time.Substring(0, 2));
			int minutes = int.Parse(time.Length == 7 ? time.Substring(2, 2) : time.Substring(3, 2));
			int seconds = int.Parse(time.Length == 7 ? time.Substring(5, 2) : time.Substring(6, 2));

			return hours * 3600 + minutes * 60 + seconds;
		}
    }
    public sealed class GtfsStopTimes : StopTimes
    {
        public GtfsStopTimes(System.IO.StreamReader stopTimes, Trips trips, Stops stops)
        {
            // Get order of field names.
            string[] fieldNames = stopTimes.ReadLine().Split(',');
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < fieldNames.Length; i++)
                dic.Add(fieldNames[i], i);

            // These fields are required for our purpose.
            if (!dic.ContainsKey("arrival_time")) throw new FormatException("Arrival time field name missing.");
            if (!dic.ContainsKey("departure_time")) throw new FormatException("Departure time field name missing.");
            if (!dic.ContainsKey("trip_id")) throw new FormatException("Trip ID field name missing.");
            if (!dic.ContainsKey("stop_id")) throw new FormatException("Stop ID field name missing.");

            while (!stopTimes.EndOfStream)
            {
                Queue<string> q = new Queue<string>(stopTimes.ReadLine().Split(','));

                // Check if there was a comma within the quotes.

                List<string> tokens = new List<string>();

                bool quotes = false;

                
                while (q.Count > 0)
                {
                    string entry = q.Dequeue();

                    if (quotes)
                        tokens[tokens.Count - 1] += ',' + entry;
                    else
                        tokens.Add(entry);

                    if (entry.Length > 0 && entry[0] == '"') quotes = true; // Start of the quotes.
                    if (entry.Length > 0 && entry[entry.Length - 1] == '"') quotes = false; // End of the quotes.
                }
                
				Trips.Trip trip = trips[tokens[dic["trip_id"]]];

				if (trip.StopTimes.Count == 0) // Set departure time of the trip.
					trip.DepartureTime = ConvertTimeToSecondsSinceMidnight(tokens[dic["departure_time"]]);

				StopTime st = new StopTime(trip, ConvertTimeToSecondsSinceMidnight(tokens[dic["arrival_time"]]) - trip.DepartureTime,
					ConvertTimeToSecondsSinceMidnight(tokens[dic["departure_time"]]) - trip.DepartureTime, stops[tokens[dic["stop_id"]]]);
				
				st.Trip.StopTimes.Add(st);

				list.Add(st);
            }
            stopTimes.Dispose();
        }
    }
}

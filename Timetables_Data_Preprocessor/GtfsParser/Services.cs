﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
    public abstract class Calendar : IEnumerable<KeyValuePair<string, Calendar.Service>>
    {
        public class Service
        {
            /// <summary>
            /// ID of the service.
            /// </summary>
            public int ID { get; }
            /// <summary>
            /// The date in string format that the data are valid since.
            /// </summary>
            public int ValidSince { get; }
            /// <summary>
            /// The date in string format that the data are valid until.
            /// </summary>
            public int ValidUntil { get; }
            /// <summary>
            /// Value at i-th position represents operating state on this weekday.
            /// </summary>
            public bool[] OperatingDays { get; }
			/// <summary>
			/// List of extraordinary events.
			/// </summary>
			public List<Tuple<string, bool>> ExtraordinaryEvents { get; }
            public override string ToString()
            {
                System.Text.StringBuilder result = new System.Text.StringBuilder();
                result.Append(ID);
                result.Append(";");
                foreach (bool day in OperatingDays)
                {
                    result.Append(day ? "1" : "0");
                    result.Append(";");
                }
                result.Append(ValidSince);
                result.Append(";");
                result.Append(ValidUntil);
                result.Append(";");
                return result.ToString();
            }
            public Service(int id, bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun, string start, string end)
            {
				ExtraordinaryEvents = new List<Tuple<string, bool>>();
                ID = id;
                OperatingDays = new bool[] { mon, tue, wed, thu, fri, sat, sun };
                ValidSince = int.Parse(start);
                ValidUntil = int.Parse(end);
            }
        }
        protected Dictionary<string, Service> list = new Dictionary<string, Service>();
        /// <summary>
        /// Gets required service.
        /// </summary>
        /// <param name="index">Identificator of the service.</param>
        /// <returns>Obtained service.</returns>
        public Service this[string index] => list[index];
        /// <summary>
        /// Gets the total number of services.
        /// </summary>
        public int Count => list.Count;
        public void Write(System.IO.StreamWriter calendar)
        {
            calendar.WriteLine(Count);
            foreach (var item in list)
                calendar.Write(item.Value);
            calendar.Close();
            calendar.Dispose();
        }
        /// <summary>
        /// Gets the date that the timetables expires in.
        /// </summary>
        /// <returns>Expiration date of the timetables.</returns>
        public int GetExpirationDate()
        {
            int min = int.MaxValue;
            foreach (var item in list)
                if (item.Value.ValidUntil < min)
                    min = item.Value.ValidUntil;
            return min;                        
        }
		public IEnumerator<KeyValuePair<string, Service>> GetEnumerator() => ((IEnumerable<KeyValuePair<string, Service>>)list).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<string, Service>>)list).GetEnumerator();
	}
    public sealed class GtfsCalendar : Calendar
    {
        public GtfsCalendar(System.IO.StreamReader calendar)
        {
            // Get order of field names.
            string[] fieldNames = calendar.ReadLine().Split(',');
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < fieldNames.Length; i++)
                dic.Add(fieldNames[i], i);

            // These fields are required for our purpose.
            if (!dic.ContainsKey("service_id")) throw new FormatException("Service ID field name missing.");
            if (!dic.ContainsKey("monday")) throw new FormatException("Monday field name missing.");
            if (!dic.ContainsKey("tuesday")) throw new FormatException("Tuesday field name missing.");
            if (!dic.ContainsKey("wednesday")) throw new FormatException("Wednesday field name missing.");
            if (!dic.ContainsKey("thursday")) throw new FormatException("Thursday field name missing.");
            if (!dic.ContainsKey("friday")) throw new FormatException("Friday field name missing.");
            if (!dic.ContainsKey("saturday")) throw new FormatException("Saturday field name missing.");
            if (!dic.ContainsKey("sunday")) throw new FormatException("Sunday field name missing.");
            if (!dic.ContainsKey("start_date")) throw new FormatException("Start date field name missing.");
            if (!dic.ContainsKey("end_date")) throw new FormatException("End date field name missing.");

            while (!calendar.EndOfStream)
            {
                string[] tokens = calendar.ReadLine().Split(',');

                bool mon = tokens[dic["monday"]] == "1" ? true : false;
                bool tue = tokens[dic["tuesday"]] == "1" ? true : false;
                bool wed = tokens[dic["wednesday"]] == "1" ? true : false;
                bool thu = tokens[dic["thursday"]] == "1" ? true : false;
                bool fri = tokens[dic["friday"]] == "1" ? true : false;
                bool sat = tokens[dic["saturday"]] == "1" ? true : false;
                bool sun = tokens[dic["sunday"]] == "1" ? true : false;

                Service service = new Service(Count, mon, tue, wed, thu, fri, sat, sun, tokens[dic["start_date"]], tokens[dic["end_date"]]);

                list.Add(tokens[dic["service_id"]], service);
            }
            calendar.Dispose();
        }
    }
    public abstract class CalendarDates
    {
        public class ExtraordinaryEvent
        {
            /// <summary>
            /// Reference to the service that the extraordinary event belongs to.
            /// </summary>
            public Calendar.Service Service { get; }
            /// <summary>
            /// The date in string format that the extraordinary event is present.
            /// </summary>
            public string Date { get; }
            /// <summary>
            /// The type of exception format. True means the service operation was added in this date. False symbolizes removal.
            /// </summary>
            public bool Type { get; }
            public override string ToString() => Service.ID + ";" + Date + ";" + (Type ? "1" : "0") + ";";
            public ExtraordinaryEvent(Calendar.Service service, string date, bool type)
            {
                Service = service;
                Date = date;
                Type = type;
            }
        }
        protected List<ExtraordinaryEvent> list = new List<ExtraordinaryEvent>();
        /// <summary>
        /// Gets the total number of extraordinary events.
        /// </summary>
        public int Count => list.Count;
        public void Write(System.IO.StreamWriter calendarDates)
        {
            calendarDates.WriteLine(Count);
            foreach (var item in list)
                calendarDates.Write(item);
            calendarDates.Close();
            calendarDates.Dispose();
        }
    }
    public sealed class GtfsCalendarDates : CalendarDates
    {
        public GtfsCalendarDates(System.IO.StreamReader calendarDates, Calendar calendar)
        {
            // Get order of field names.
            string[] fieldNames = calendarDates.ReadLine().Split(',');
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < fieldNames.Length; i++)
                dic.Add(fieldNames[i], i);

            // These fields are required for our purpose.
            if (!dic.ContainsKey("service_id")) throw new FormatException("Service ID field name missing.");
            if (!dic.ContainsKey("date")) throw new FormatException("Date field name missing.");
            if (!dic.ContainsKey("exception_type")) throw new FormatException("Exception type field name missing.");

            while (!calendarDates.EndOfStream)
            {
                string[] tokens = calendarDates.ReadLine().Split(',');

                Calendar.Service service;

                try
                {
                    service = calendar[tokens[dic["service_id"]]];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new FormatException("Service with this ID does not exist.");
                }

                bool type = tokens[dic["exception_type"]] == "1" ? true : false;

				service.ExtraordinaryEvents.Add(new Tuple<string, bool>(tokens[dic["date"]], type));

                ExtraordinaryEvent ev = new ExtraordinaryEvent(service, tokens[dic["date"]], type);

                list.Add(ev);
            }
            calendarDates.Dispose();
        }
    }
}
using System;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
    public class Stations
    {
        public class Station
        {
            /// <summary>
            /// ID of the station.
            /// </summary>
            public int ID { get; }
            /// <summary>
            /// Name of the station.
            /// </summary>
            public string Name { get; }
            public override string ToString() => ID + ";" + Name + ";";
            public Station(int id, string name)
            {
                ID = id;
                Name = name;
            }
        }
        private List<Station> list = new List<Station>();
        /// <summary>
        /// Gets required station.
        /// </summary>
        /// <param name="index">Identificator of the station.</param>
        /// <returns>Obtained station.</returns>
        public Station this[int index] => list[index];
        /// <summary>
        /// Gets the total number of station.
        /// </summary>
        public int Count => list.Count;
        public Stations(Stops stops)
        {
            var flags = new Dictionary<string, Station>();

            foreach (var stop in stops)
            {
                if (!flags.ContainsKey(stop.Value.Name))
                {
                    var station = new Station(Count, stop.Value.Name);
                    list.Add(station);
                    flags.Add(stop.Value.Name, station);
                }
                stop.Value.ParentStation = flags[stop.Value.Name];
            }
        }
        public void Write(System.IO.StreamWriter stations)
        {
            stations.WriteLine(Count);
            foreach (var item in list)
                stations.Write(item);
            stations.Close();
        }
    }
}

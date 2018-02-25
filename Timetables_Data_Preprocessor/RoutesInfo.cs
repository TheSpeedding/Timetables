using System;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{    
    public class RoutesInfo
    {
        public class RouteInfo
        {
            public enum RouteType { Tram = 0, Subway = 1, Rail = 2, Bus = 3, Ship = 4, CableCar = 5, Gondola = 6, Funicular = 7 }
            /// <summary>
            /// ID of the route info.
            /// </summary>
            public int ID { get; }
            /// <summary>
            /// Short name of the route.
            /// </summary>
            public string ShortName { get; }
            /// <summary>
            /// Long name of the route.
            /// </summary>
            public string LongName { get; }
            /// <summary>
            /// Type of a vehicle serving the route.
            /// </summary>
            public RouteType Type { get; }
            /// <summary>
            /// Color in HEX format that represents the route.
            /// </summary>
            public string Color { get; }
            public override string ToString() => ID + ";" + ShortName + ";" + LongName + ";" + (int)Type + ";" + Color + ";";
            public RouteInfo(int id, string shortName, string longName, RouteType type, string color)
            {
                ID = id;
                ShortName = shortName;
                LongName = longName;
                Type = type;

                if (color == "")
                    switch (type)
                    {
                        case RouteType.Bus:
                            Color = "00FFFF";
                            break;
                        case RouteType.CableCar:
                        case RouteType.Funicular:
                        case RouteType.Gondola:
                            Color = "FFFFFF";
                            break;
                        case RouteType.Rail:
                            Color = "006600";
                            break;
                        case RouteType.Ship:
                            Color = "0033CC";  
                            break;
                        case RouteType.Subway:
                            Color = "FFFF00";
                            break;
                        case RouteType.Tram:
                            Color = "CC0000";
                            break;
                    }

                else
                    Color = color;
            }
        }
        private Dictionary<string, RouteInfo> list = new Dictionary<string, RouteInfo>();
        /// <summary>
        /// Gets required route info.
        /// </summary>
        /// <param name="index">Identificator of the route info.</param>
        /// <returns>Obtained route info.</returns>
        public RouteInfo this[string index] => list[index];
        /// <summary>
        /// Gets the total number of routes info.
        /// </summary>
        public int Count => list.Count;
        public RoutesInfo(System.IO.StreamReader routesInfo)
        {
            // Get order of field names.
            string[] fieldNames = routesInfo.ReadLine().Split(',');
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < fieldNames.Length; i++)
                dic.Add(fieldNames[i], i);

            // These fields are required for our purpose.
            if (!dic.ContainsKey("route_id")) throw new FormatException("Route ID field name missing.");
            if (!dic.ContainsKey("route_short_name")) throw new FormatException("Route short name field name missing.");
            if (!dic.ContainsKey("route_long_name")) throw new FormatException("Route long name field name missing.");
            if (!dic.ContainsKey("route_type")) throw new FormatException("Route type field name missing.");
            if (!dic.ContainsKey("route_color")) throw new FormatException("Route color field name missing.");

            while (!routesInfo.EndOfStream)
            {
                Queue<string> q = new Queue<string>(routesInfo.ReadLine().Split(','));

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


                string shortName = tokens[dic["route_short_name"]];

                if (shortName[0] == '"') shortName = shortName.Substring(1, shortName.Length - 1);

                if (shortName[shortName.Length - 1] == '"') shortName = shortName.Substring(0, shortName.Length - 1);

                string longName = tokens[dic["route_long_name"]];

                if (longName[0] == '"') longName = longName.Substring(1, longName.Length - 1);

                if (longName[longName.Length - 1] == '"') longName = longName.Substring(0, longName.Length - 1);

                RouteInfo.RouteType type = (RouteInfo.RouteType)int.Parse(tokens[dic["route_type"]]);

                RouteInfo routeInfo = new RouteInfo(Count, shortName, longName, type, tokens[dic["route_color"]]);

                list.Add(tokens[dic["route_id"]], routeInfo);
            }
        }
        public void Write(System.IO.StreamWriter routesInfo)
        {
            routesInfo.WriteLine(Count);
            foreach (var item in list)
                routesInfo.Write(item.Value);
            routesInfo.Close();
        }
    }
}

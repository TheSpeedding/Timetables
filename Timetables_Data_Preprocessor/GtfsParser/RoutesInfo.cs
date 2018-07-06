using System;
using System.Collections;
using System.Collections.Generic;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Static class that supplies extensions needed for routes info.
	/// </summary>
	public static class RoutesInfoExtension
	{
		/// <summary>
		/// Converts Color object to HEX string representation
		/// </summary>
		/// <param name="c">Color.</param>
		/// <returns>Color in HEX format.</returns>
		public static string ToHex(this System.Drawing.Color c) => c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
	}
	/// <summary>
	/// Abstract class for routes info collecting information about routes info.
	/// </summary>
	public abstract class RoutesInfo : IEnumerable<KeyValuePair<string, RoutesInfo.RouteInfo>>
	{
		/// <summary>
		/// Collects information about one route info.
		/// </summary>
		public class RouteInfo
        {
			/// <summary>
			/// Type of the route.
			/// </summary>
            public enum RouteType { Tram = 0, Subway = 1, Rail = 2, Bus = 3, Ship = 4, CableCar = 5, Gondola = 6, Funicular = 7 }
            /// <summary>
            /// ID of the route info.
            /// </summary>
            public int ID { internal set; get; }
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
            /// Color that represents the route.
            /// </summary>
            public System.Drawing.Color Color { get; }
			/// <summary>
			/// Route Info ID, Short Name, Long Name, Mean Of The Transport, Color.
			/// </summary>
			public override string ToString() => ID + ";" + ShortName + ";" + LongName + ";" + (int)Type + ";" + Color.ToHex() + ";";
			/// <summary>
			/// Route Info ID, Short Name, Mean Of The Transport, Color.
			/// </summary>
			public string ToStringBasic() => ID + ";" + ShortName + ";" + (int)Type + ";" + Color.ToHex() + ";";
			/// <summary>
			/// Initializes object.
			/// </summary>
			/// <param name="id">Route Info ID.</param>
			/// <param name="shortName">Short Name.</param>
			/// <param name="longName">Long Name.</param>
			/// <param name="type">Mean Of The Transport.</param>
			/// <param name="color">Color.</param>
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
                            Color = GlobalData.DefaultBusColor;
                            break;
                        case RouteType.CableCar:
                        case RouteType.Funicular:
                        case RouteType.Gondola:
							Color = GlobalData.DefaultCableCarColor;
                            break;
                        case RouteType.Rail:
                            Color = GlobalData.DefaultRailColor;
                            break;
                        case RouteType.Ship:
                            Color = GlobalData.DefaultShipColor;  
                            break;
                        case RouteType.Subway:
							Color = GlobalData.DefaultSubwayColor;
                            break;
                        case RouteType.Tram:
                            Color = GlobalData.DefaultTramColor;
                            break;
						default:
							Color = System.Drawing.ColorTranslator.FromHtml("#000000");
							break;
                    }

                else
                    Color = System.Drawing.ColorTranslator.FromHtml(color[0] == '#' ? color : "#" + color);
            }
        }
		protected Dictionary<string, RouteInfo> list = new Dictionary<string, RouteInfo>();
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
		/// <summary>
		/// Writes the data into given stream.
		/// </summary>
		/// <param name="routesInfo">Stream that the data should be written in.</param>
		public void Write(System.IO.StreamWriter routesInfo)
		{
			routesInfo.WriteLine(Count);
			foreach (var item in list)
				routesInfo.Write(item.Value);
			routesInfo.Close();
			routesInfo.Dispose();
		}
		/// <summary>
		/// Writes basic data into given stream.
		/// </summary>
		/// <param name="routesInfo">Stream that the data should be written in.</param>
		public void WriteBasic(System.IO.StreamWriter routesInfo)
		{
			routesInfo.WriteLine(Count);
			foreach (var item in list)
				routesInfo.Write(item.Value.ToStringBasic());
			routesInfo.Close();
			routesInfo.Dispose();
		}
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<KeyValuePair<string, RouteInfo>> GetEnumerator() => ((IEnumerable<KeyValuePair<string, RouteInfo>>)list).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<KeyValuePair<string, RouteInfo>>)list).GetEnumerator();
		/// <summary>
		/// Merges two collections into one.
		/// </summary>
		/// <param name="other">The other collection that should be merged.</param>
		public void MergeCollections(RoutesInfo other)
		{ 
			foreach (var item in other)
			{
				var info = item.Value;
				info.ID = Count; // Reindex the item.
				string key;
				while (list.ContainsKey(key = DataFeed.RandomString())); // We can index the item using some random string, since this identificator is only used while initialization. Both data are already initialized.
				list.Add(key, info);
			}
			other = null;
		}
	}
	/// <summary>
	/// Class for routes info with a specific parsing from GTFS format.
	/// </summary>
	public sealed class GtfsRoutesInfo : RoutesInfo
	{
		/// <summary>
		/// Initializes object using GTFS data feed.
		/// </summary>
		/// <param name="routesInfo">Routes Info.</param>
		public GtfsRoutesInfo(System.IO.StreamReader routesInfo)
        {
            // Get order of field names.
            string[] fieldNames = routesInfo.ReadLine().Split(',');
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < fieldNames.Length; i++)
                dic.Add(fieldNames[i].Replace("\"", ""), i);

            // These fields are required for our purpose.
            if (!dic.ContainsKey("route_id")) throw new FormatException("Route ID field name missing.");
            if (!dic.ContainsKey("route_short_name")) throw new FormatException("Route short name field name missing.");
            if (!dic.ContainsKey("route_long_name")) throw new FormatException("Route long name field name missing.");
            if (!dic.ContainsKey("route_type")) throw new FormatException("Route type field name missing.");
            if (!dic.ContainsKey("route_color")) throw new FormatException("Route color field name missing.");

            while (!routesInfo.EndOfStream)
            {
				IList<string> tokens = GtfsDataFeed.SplitGtfs(routesInfo.ReadLine());

				int intType = int.Parse(tokens[dic["route_type"]]);

				if (intType == 800) intType = (int)RouteInfo.RouteType.Bus; // Trolleybus converted to bus (Palmovka - Letňany). Temporary solution.

				if (!(intType >= 0 && intType <= 7))
					throw new FormatException("Invalid mean of transport.");

				RouteInfo.RouteType type = (RouteInfo.RouteType)intType;

                RouteInfo routeInfo = new RouteInfo(Count, tokens[dic["route_short_name"]], tokens[dic["route_long_name"]], type, tokens[dic["route_color"]]);

                list.Add(tokens[dic["route_id"]], routeInfo);
            }
            routesInfo.Dispose();
        }
    }
}

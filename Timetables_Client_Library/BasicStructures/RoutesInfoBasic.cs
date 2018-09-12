using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Timetables.Client;

namespace Timetables.Structures.Basic
{
	/// <summary>
	/// Static class that supplies extensions needed for routes info.
	/// </summary>
	public static partial class RoutesInfoExtension
	{
		/// <summary>
		/// Converts Color object to HEX string representation
		/// </summary>
		/// <param name="c">Color.</param>
		/// <returns>Color in HEX format.</returns>
		public static string ToHex(this System.Drawing.Color c) => c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
	}
	/// <summary>
	/// Collection of routes info.
	/// </summary>
	[Serializable]
	public class RoutesInfoBasic : IEnumerable<RoutesInfoBasic.RouteInfoBasic>
	{
		/// <summary>
		/// Class collecting basic information about route info.
		/// </summary>
		[Serializable]
		public class RouteInfoBasic
		{
			/// <summary>
			/// ID of the route info.
			/// </summary>
			public int ID { get; set; }
			/// <summary>
			/// Label of the route.
			/// </summary>
			public string Label { get; set; }
			/// <summary>
			/// Mean of transportation.
			/// </summary>
			public MeanOfTransport MeanOfTransport { get; set; }
			/// <summary>
			/// Color of route used in GUI.
			/// </summary>
			[XmlElement(Type = typeof(XmlColor))]
			public Color Color { get; set; }
			public RouteInfoBasic(int id, string label, MeanOfTransport type, Color c)
			{
				Label = label;
				MeanOfTransport = type;
				Color = c;
			}
			/// <summary>
			/// Route Info ID, Short Name, Mean Of The Transport, Color.
			/// </summary>
			public override string ToString() => ID + ";" + Label + ";" + (int)MeanOfTransport + ";" + Color.ToHex() + ";";
		}
		public static implicit operator RouteInfoBasic[] (RoutesInfoBasic routes) => routes.Items;
		public static explicit operator string[] (RoutesInfoBasic routes) => routes.Select(r => r.Label).ToArray();
		private RouteInfoBasic[] Items { get; }
		/// <summary>
		/// Gets required route info.
		/// </summary>
		/// <param name="index">Identificator of the route info.</param>
		/// <returns>Obtained route info.</returns>
		public RouteInfoBasic this[int index] => Items[index];
		/// <summary>
		/// Gets the total number of routes info.
		/// </summary>
		public int Count => Items.Length;
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<RouteInfoBasic> GetEnumerator() => ((IEnumerable<RouteInfoBasic>)Items).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<RouteInfoBasic>)Items).GetEnumerator();
		public RoutesInfoBasic(System.IO.StreamReader sr)
		{
			var count = int.Parse(sr.ReadLine());
			Items = new RouteInfoBasic[count];
			var tokens = sr.ReadLine().Split(';'); // This could take some time but files are usually small.
			for (int i = 0; i < count; i++)
				Items[i] = (new RouteInfoBasic(int.Parse(tokens[4 * i]), tokens[4 * i + 1], (MeanOfTransport)int.Parse(tokens[4 * i + 2]), ColorTranslator.FromHtml(tokens[4 * i + 3][0] == '#' ? tokens[4 * i + 3] : "#" + tokens[4 * i + 3])));
			sr.Dispose();
		}
		/// <summary>
		/// Writes basic data into given stream.
		/// </summary>
		/// <param name="routesInfo">Stream that the data should be written in.</param>
		public void WriteBasic(System.IO.StreamWriter routesInfo)
		{
			routesInfo.WriteLine(Count);
			foreach (var item in Items)
				routesInfo.Write(item);
			routesInfo.Close();
			routesInfo.Dispose();
		}
		/// <summary>
		/// Returns route info represented by the label.
		/// </summary>
		/// <param name="label">Label of the route info.</param>
		public RouteInfoBasic FindByLabel(string label) => Array.Find(Items, (RouteInfoBasic route) => StringComparer.CurrentCultureIgnoreCase.Compare(route.Label, label) == 0);
		/// <summary>
		/// Returns route info represented by the index.
		/// </summary>
		/// <param name="index">Index of the route info.</param>
		public RouteInfoBasic FindByIndex(int index) => this[index];
	}
}

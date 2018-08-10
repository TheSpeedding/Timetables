using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Timetables.Client;

namespace Timetables.Structures.Basic
{
	/// <summary>
	/// Collection of routes info.
	/// </summary>
	public class RoutesInfoBasic : Preprocessor.RoutesInfo, IEnumerable<RoutesInfoBasic.RouteInfoBasic>
	{
		/// <summary>
		/// Class collecting basic information about route info.
		/// </summary>
		public class RouteInfoBasic
		{
			/// <summary>
			/// Label of the route.
			/// </summary>
			public string Label { get; }
			/// <summary>
			/// Mean of transportation.
			/// </summary>
			public MeanOfTransport MeanOfTransport { get; }
			/// <summary>
			/// Color of route used in GUI.
			/// </summary>
			public Color Color { get; }
			public RouteInfoBasic(string label, MeanOfTransport type, Color c)
			{
				Label = label;
				MeanOfTransport = type;
				Color = c;
			}
		}
		private new List<RouteInfoBasic> list = new List<RouteInfoBasic>();
		/// <summary>
		/// Gets required route info.
		/// </summary>
		/// <param name="index">Identificator of the route info.</param>
		/// <returns>Obtained route info.</returns>
		public RouteInfoBasic this[int index] => list[index];
		/// <summary>
		/// Gets the total number of routes info.
		/// </summary>
		public new int Count => list.Count;
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public new IEnumerator<RouteInfoBasic> GetEnumerator() => ((IEnumerable<RouteInfoBasic>)list).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<RouteInfoBasic>)list).GetEnumerator();
		public RoutesInfoBasic(System.IO.StreamReader sr)
		{
			var count = int.Parse(sr.ReadLine());
			var tokens = sr.ReadLine().Split(';'); // This could take some time but files are usually small.
			for (int i = 0; i < count; i++)
				list.Add(new RouteInfoBasic(tokens[4 * i + 1], (MeanOfTransport)int.Parse(tokens[4 * i + 2]), ColorTranslator.FromHtml(tokens[4 * i + 3][0] == '#' ? tokens[4 * i + 3] : "#" + tokens[4 * i + 3])));
		}
	}
}

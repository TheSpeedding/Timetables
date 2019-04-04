using System;
using System.Xml.Serialization;

// In this file, basic structure representing color is implemented. 
// It is needed in both Xamarin and Windows application and System.Drawing.Color is inaccessible from Xamarin, thus this class is needed.
// What's more, System.Drawing.Color is not serializable (without any wrapper). This one is.

namespace Timetables.Utilities
{
	/// <summary>
	/// Crossplatform color structure.
	/// </summary>
	[Serializable]
	public class CPColor
	{
		public static readonly CPColor Gray = FromHtml("#808080");
		public static readonly CPColor Black = FromHtml("#000000");
		public static readonly CPColor White = FromHtml("#FFFFFF");
		/// <summary>
		/// Red.
		/// </summary>
		public byte R { get; set; }
		/// <summary>
		/// Green.
		/// </summary>
		public byte G { get; set; }
		/// <summary>
		/// Blue.
		/// </summary>
		public byte B { get; set; }
		/// <summary>
		/// Hex format of the color.
		/// </summary>
		[XmlAttribute]
		public string Hex
		{
			get
			{
				return "#" + ToHex();
			}
			set
			{
				var c = FromHtml(value);
				R = c.R;
				G = c.G;
				B = c.B;
			}
		}
		/// <summary>
		/// Converts Color object to HEX string representation
		/// </summary>
		/// <returns>Color in HEX format.</returns>
		public string ToHex() => R.ToString("X2") + G.ToString("X2") + B.ToString("X2");
		/// <summary>
		/// Converts HTML hexcode to CPColor structure.
		/// </summary>
		/// <param name="hex">Hexcode of the color.</param>
		/// <returns>CPColor structure.</returns>
		public static CPColor FromHtml(string hex)
		{
			int offset = hex[0] == '#' ? 1 : 0;

			var r = hex.Length >= 3 ? byte.Parse(hex.Substring(offset, 2), System.Globalization.NumberStyles.HexNumber) : byte.MinValue;
			var g = hex.Length >= 5 ? byte.Parse(hex.Substring(offset + 2, 2), System.Globalization.NumberStyles.HexNumber) : byte.MinValue;
			var b = hex.Length >= 7 ? byte.Parse(hex.Substring(offset + 4, 2), System.Globalization.NumberStyles.HexNumber) : byte.MinValue;

			return new CPColor(r, g, b);
		}
		private CPColor(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}
		internal CPColor() { }
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Timetables.Client
{
	// TO-DO: Better place for this class.

	/// <summary>
	/// Color structure is not serializable by default. We need to serialize the color as well.
	/// </summary>
	public class XmlColor
	{
		private Color c;
		public XmlColor() => c = Color.Black;
		public XmlColor(Color c) => this.c = c;
		public Color ToColor() => c;
		public void FromColor(Color c) => this.c = c;
		public static implicit operator Color(XmlColor x) => x.ToColor();
		public static implicit operator XmlColor(Color c) => new XmlColor(c);

		[XmlAttribute]
		public string Hex
		{
			get { return ColorTranslator.ToHtml(c); }
			set
			{
				try
				{
					c = ColorTranslator.FromHtml(value);
				}
				catch
				{
					c = Color.Black;
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Timetables.Client
{ 
	/// <summary>
	/// Class supplying utilities for XML serialization.
	/// </summary>
	public static class XmlSerialization
	{
		/// <summary>
		/// Replaces all stop IDs in the document with their names.
		/// </summary>
		/// <param name="doc">Document with stop IDs.</param>
		/// <returns>Document with stop names.</returns>
		public static XmlDocument ReplaceStopIdsWithNames(this XmlDocument doc)
		{
			XDocument xDoc = XDocument.Load(new XmlNodeReader(doc));

			IEnumerable<XNode> elementsToChange = from element in xDoc.DescendantNodes()
												  where element.NodeType == XmlNodeType.Element && ((XElement)element).Name.LocalName.Contains("StopID")
												  select element;

			foreach (XElement element in elementsToChange)
			{
				element.Value = DataFeedGlobals.Basic.Stops.FindByIndex(int.Parse(element.Value)).Name;
				element.Name = element.Name.LocalName.Replace("ID", "Name");
			}
			
			XmlDocument newDoc = new XmlDocument();

			using (var xmlReader = xDoc.CreateReader())
			{
				newDoc.Load(xmlReader);
			}

			return newDoc;
		}
	}
}

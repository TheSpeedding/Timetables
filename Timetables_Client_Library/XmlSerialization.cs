using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

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
		private static XmlDocument ReplaceStopIdsWithNames(this XmlDocument doc)
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

		/// <summary>
		/// Converts serializable object into the HTML string using given XSLT script.
		/// </summary>
		/// <param name="o">Object to serialize.</param>
		/// <param name="xsltPath">Path to XSLT stylesheet.</param>
		/// <param name="replaceIdsWithNames">Indicates whether the IDs should be replaced with corresponding names.</param>
		/// <returns>String representation of transformed XML, usually in HTML.</returns>
		public static string ConvertObjectToTransformedString(this object o, string xsltPath, bool replaceIdsWithNames = false)
		{
			System.IO.StringWriter sw = new System.IO.StringWriter();
			if (!o.GetType().IsSerializable) throw new MissingMethodException("Given object is not serializable.");

			o.GetType().GetMethod("Serialize").Invoke(o, new object[] { sw });

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(sw.ToString());

			if (replaceIdsWithNames)
			{
				sw = new System.IO.StringWriter();
				doc.ReplaceStopIdsWithNames().Save(sw);
				doc = new XmlDocument();
				doc.LoadXml(sw.ToString());
			}

			XPathDocument xPathDoc = new XPathDocument(new XmlNodeReader(doc));
			XslCompiledTransform xsltTrans = new XslCompiledTransform();

			xsltTrans.Load(xsltPath);
			sw = new System.IO.StringWriter();

			xsltTrans.Transform(xPathDoc, null, sw);

			return sw.ToString();
		}
	}
}

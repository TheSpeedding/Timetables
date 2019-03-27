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
using System.IO;

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
		[Obsolete]
		private static XmlDocument ReplaceStopIdsWithNames(this XmlDocument doc)
		{
			XDocument xDoc = XDocument.Load(new XmlNodeReader(doc));

			IEnumerable<XNode> elementsToChange = from element in xDoc.DescendantNodes()
												  where element.NodeType == XmlNodeType.Element && ((XElement)element).Name.LocalName.Contains("StopID")
												  select element;

			foreach (XElement element in elementsToChange)
			{
				element.Value = DataFeedClient.Basic.Stops.FindByIndex(int.Parse(element.Value)).Name;
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
		/// <param name="cssPath">Path to CSS stylesheet.</param>
		/// <param name="jsPath">Path to JS script.</param>
		/// <returns>String representation of transformed XML, usually in HTML.</returns>
		public static string TransformToHtml(this object o, string xsltPath, string cssPath = null, string jsPath = null)
		{
			if (o == null) return string.Empty;

			System.IO.StringWriter sw = new System.IO.StringWriter();
			if (!o.GetType().IsSerializable) throw new MissingMethodException("Given object is not serializable.");

			o.GetType().GetMethod("Serialize").Invoke(o, new object[] { sw });

			return TransformToHtml(sw.ToString(), xsltPath, cssPath, jsPath);
		}
		public static string TransformToHtml(this object o, Stream xslt, Stream css = null, Stream js = null)
		{
			if (o == null) return string.Empty;

			System.IO.StringWriter sw = new System.IO.StringWriter();
			if (!o.GetType().IsSerializable) throw new MissingMethodException("Given object is not serializable.");

			o.GetType().GetMethod("Serialize").Invoke(o, new object[] { sw });

			return TransformToHtml(sw.ToString(), xslt, css, js);
		}
		/// <summary>
		/// Converts XML string into the HTML string using given XSLT script.
		/// </summary>
		/// <param name="content">XML in string.</param>
		/// <param name="xsltPath">Path to XSLT stylesheet.</param>
		/// <param name="cssPath">Path to CSS stylesheet.</param>
		/// <param name="jsPath">Path to JS script.</param>
		/// <returns>String representation of transformed XML, usually in HTML.</returns>
		public static string TransformToHtml(this string content, string xsltPath, string cssPath = null, string jsPath = null)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(content);

			XPathDocument xPathDoc = new XPathDocument(new XmlNodeReader(doc));
			XslCompiledTransform xsltTrans = new XslCompiledTransform();

			xsltTrans.Load(xsltPath);
			var sw = new System.IO.StringWriter();

			xsltTrans.Transform(xPathDoc, null, sw);

			return
				(cssPath == null ? "" : ("<style>" + new System.IO.StreamReader(cssPath).ReadToEnd() + "</style>" + Environment.NewLine)) +
				sw.ToString() +
				(jsPath == null ? "" : ("<script>" + new System.IO.StreamReader(jsPath).ReadToEnd() + "</script>" + Environment.NewLine));
		}
		public static string TransformToHtml(this string content, Stream xslt, Stream css = null, Stream js = null)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(content);

			XPathDocument xPathDoc = new XPathDocument(new XmlNodeReader(doc));
			XslCompiledTransform xsltTrans = new XslCompiledTransform();

			xsltTrans.Load(XmlReader.Create(xslt));
			var sw = new System.IO.StringWriter();

			xsltTrans.Transform(xPathDoc, null, sw);

			return
				(css == null ? "" : ("<style>" + new StreamReader(css).ReadToEnd() + "</style>" + Environment.NewLine)) +
				sw.ToString() +
				(js == null ? "" : ("<script>" + new StreamReader(js).ReadToEnd() + "</script>" + Environment.NewLine));
		}
	}
}

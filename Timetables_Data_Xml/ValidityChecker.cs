using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace Timetables.Xml
{
	/// <summary>
	/// Checks XML well-formedness and validity against supplied DTD.
	/// </summary>
	public class ValidityChecker
	{
		private string file;
		/// <summary>
		/// List of the errors produced by the checker.
		/// </summary>
		public List<string> Errors { get; }
		/// <summary>
		/// Initializes object.
		/// </summary>
		/// <param name="file">Path to the XML file.</param>
		public ValidityChecker(string file)
		{
			this.file = file;
			Errors = new List<string>();
		}
		/// <summary>
		/// Checks XML well-formedness and validity against supplied DTD.
		/// </summary>
		/// <returns>True if succeeded.</returns>
		public bool CheckXmlWellFormednessAgainstDtd()
		{
			XmlReaderSettings settings = new XmlReaderSettings { ValidationType = ValidationType.DTD };
			settings.XmlResolver = new XmlUrlResolver();
			settings.DtdProcessing = DtdProcessing.Parse;
			settings.ValidationType = ValidationType.DTD;
			settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

			XmlReader reader = XmlReader.Create(file, settings);

			while (reader.Read()) ;

			return Errors.Count == 0;
		}
		/// <summary>
		/// Event handler for failures in validation process.
		/// </summary>
		private void ValidationCallBack(object sender, ValidationEventArgs e) => Errors.Add(e.Message);
	}
}

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace Timetables.Xml
{
	public class ValidityChecker
	{
		private string file;
		public List<string> Errors { get; }
		public ValidityChecker(string file)
		{
			this.file = file;
			Errors = new List<string>();
		}
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
		private void ValidationCallBack(object sender, ValidationEventArgs e) => Errors.Add(e.Message);
	}
}

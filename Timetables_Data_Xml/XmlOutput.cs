using System;
using System.IO;
using Timetables.Preprocessor;

namespace Timetables.Xml
{
	/// <summary>
	/// Everything necessary to create XML database.
	/// </summary>
    public static class XmlOutput
    {
		/// <summary>
		/// Create XML file from data according to given DTD.
		/// </summary>
		/// <param name="data">Data from which the XML file should be created.</param>
		/// <param name="file">Name of the new XML file.</param>
        public static void CreateXml(this IDataFeed data, string file)
        {
            StreamWriter sw = new StreamWriter(file);

            sw.WriteLine($@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>

<!DOCTYPE timetables SYSTEM ""Data.dtd"">

<timetables>
<today><?php echo date('Ymd'); ?></today>
<expires>{ data.ExpirationDate }</expires>");

			data.Calendar.ToXml(sw);

			data.Stops.ToXml(sw);

			data.Footpaths.ToXml(sw);

			data.RoutesInfo.ToXml(sw);

			data.Routes.ToXml(sw);

			data.Trips.ToXml(sw);

            sw.Write("</timetables>");

            sw.Close();
            sw.Dispose();
        }
		/// <summary>
		/// Creates XML file and checks its valadity against supplied DTD.
		/// </summary>
        public static void GetAndTransformDataFeedToXml<T>() where T : IDataFeed
        {
            Downloader.GetDataFeed("temp_data/");
			
            IDataFeed data = (T)Activator.CreateInstance(typeof(T), (string)"temp_data/");

            data.CreateXml("Data.xml");

			var checker = new ValidityChecker("Data.xml");

			if (!checker.CheckXmlWellFormednessAgainstDtd())
				throw new FormatException("XML not well-formed.");
            
            Downloader.DeleteTrash("temp_data/");
        }
    }
}

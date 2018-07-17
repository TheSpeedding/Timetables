using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Timetables.Client
{
	/// <summary>
	/// Class offering localized string constants.
	/// </summary>
	[XmlRoot("Localization")]
	public class Localization
	{
		public string Language { get; set; } = "English";
		/// <summary>
		/// Creates a new instance of Localization class offering string constants in given language.
		/// </summary>
		/// <param name="language">Language.</param>
		/// <returns>Localization object.</returns>
		public static Localization GetTranslation(string language = "English")
		{
			if (language == "English") return new Localization();

			if (!File.Exists("loc/" + language + ".xml")) throw new ArgumentException($"Translation sheet with language \" { language } \" was not found.");
			
			using (FileStream fileStream = new FileStream("loc/" + language + ".xml", FileMode.Open))
			{
				return (Localization)new XmlSerializer(typeof(Localization)).Deserialize(fileStream);
			}
		}
	}
}

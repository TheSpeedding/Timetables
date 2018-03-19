using Timetables.Preprocessor;
using System.Collections.Generic;
using System;

using static System.Console;

namespace Timetables.SampleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			WriteLine("You can enter as many URLs to data sources as you wish. Seperate them with a new line");
			WriteLine("Type END command to end the input.");

			List<string> urls = new List<string> { "http://opendata.iprpraha.cz/DPP/JR/jrdata.zip" }; // Link to Prague data source added implicitly.
			WriteLine("http://opendata.iprpraha.cz/DPP/JR/jrdata.zip");			

			string line;
			while ((line = ReadLine()) != "END")
				urls.Add(line);

			WriteLine($"{ DateTime.Now }: Starting data preprocessing.");

			DataFeed.DataProcessing += new DataFeed.DataProcessingEventHandler((string message) => WriteLine(DateTime.Now + ": " + message));

			DataFeed.GetAndTransformDataFeed<GtfsDataFeed>(urls.ToArray());

			WriteLine($"{ DateTime.Now }: Ending data preprocessing.");

			WriteLine("Press any key to continue...");
			ReadLine();
		}
	}
}
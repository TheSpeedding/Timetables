using Timetables.Preprocessor;
using System.Collections.Generic;
using System;

namespace Timetables.SampleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("You can enter as many URLs to data sources as you wish. Seperate them with a new line");
			Console.WriteLine("Type END command to end the input.");

			List<string> urls = new List<string> { "http://opendata.iprpraha.cz/DPP/JR/jrdata.zip" }; // Link to Prague data source added implicitly.

			string line;
			while ((line = Console.ReadLine()) != "END")
				urls.Add(line);

			Console.WriteLine($"{ DateTime.Now }: Starting data processing.");

			DataFeed.GetAndTransformDataFeed<GtfsDataFeed>(urls.ToArray());

			Console.WriteLine($"{ DateTime.Now }: Ending data processing.");
		}
	}
}
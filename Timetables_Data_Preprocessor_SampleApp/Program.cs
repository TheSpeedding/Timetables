using Timetables.Preprocessor;

namespace Timetables.SampleApp
{
	class Program
	{
		// Prague only.
		// static void Main(string[] args) => DataFeed.GetAndTransformDataFeed<GtfsDataFeed>("http://opendata.iprpraha.cz/DPP/JR/jrdata.zip");

		// Prague and Vienna data feeds merged.
		static void Main(string[] args) => DataFeed.GetAndTransformDataFeed<GtfsDataFeed>("http://opendata.iprpraha.cz/DPP/JR/jrdata.zip", "https://go.gv.at/l9gtfs");
	}
}
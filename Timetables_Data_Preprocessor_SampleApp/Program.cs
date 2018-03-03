using Timetables.Preprocessor;

namespace Timetables.SampleApp
{
	class Program
	{
		static void Main(string[] args) => DataFeed.GetAndTransformDataFeed<GtfsDataFeed>();
	}
}
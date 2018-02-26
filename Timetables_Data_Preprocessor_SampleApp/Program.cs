using Timetables.Preprocessor;

class Program
{
    static void Main(string[] args) => DataFeed.GetAndTransformDataFeed<GtfsDataFeed>();
}
using Timetables.Preprocessor;

namespace Timetables.Xml
{
    class Program
    {
        static void Main(string[] args) => XmlOutput.GetAndTransformDataFeedToXml<GtfsDataFeed>();
    }
}

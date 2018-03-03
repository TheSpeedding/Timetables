using Timetables.Preprocessor;
using Timetables.Xml;

namespace Timetables.SampleApp
{
    class Program
    {
        static void Main(string[] args) => XmlOutput.GetAndTransformDataFeedToXml<GtfsDataFeed>();
    }
}

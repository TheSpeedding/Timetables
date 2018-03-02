using Timetables.Preprocessor;

// Application is designed to parse timetables data to an XML document using my own DTD, which I was supposed to design as a homework for another subject.

namespace Timetables.Xml
{
    class Program
    {
        static void Main(string[] args) => XmlOutput.GetAndTransformDataFeedToXml<GtfsDataFeed>();
    }
}

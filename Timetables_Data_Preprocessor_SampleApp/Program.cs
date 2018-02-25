using System;
using Timetables.Preprocessor;

class Program
{
    static void Main(string[] args)
    {
        string pathToGtfs = "gtfs_data";
        string pathToData = "data";

        // DataFeed.GetAndTransformDataFeed<GtfsDataFeed>();

        // Downloader.GetDataFeed(pathToGtfs);

        IDataFeed data = new GtfsDataFeed(pathToGtfs);

        data.CreateDataFeed(pathToData);

        // Downloader.DeleteTrash(pathToGtfs);
    }
}
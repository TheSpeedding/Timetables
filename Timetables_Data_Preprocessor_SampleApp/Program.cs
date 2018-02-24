using System;
using Timetables.Preprocessor;

class Program
{
    static void Main(string[] args)
    {
        Downloader.GetDataFeed();

        Downloader.DeleteTrash();
    }
}
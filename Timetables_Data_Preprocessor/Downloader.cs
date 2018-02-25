using System;
using System.IO;
using System.Net;

namespace Timetables.Preprocessor
{
    public static class Downloader
    {
        public static void GetDataFeed(string path)
        {
            Uri url = null;
            StreamReader sr = null;

            try
            {
                sr = new StreamReader("resource.txt");
                url = new Uri(sr.ReadLine()); // Can throw Uri format exception.
            }
            catch (FileNotFoundException)
            {
                url = new Uri(@"http://opendata.iprpraha.cz/DPP/JR/jrdata.zip");
            }       
            catch (UriFormatException)
            {
                url = new Uri(@"http://opendata.iprpraha.cz/DPP/JR/jrdata.zip");
            }
            
                
            using (var client = new WebClient())
            {
                client.DownloadFile(url, "data.zip");
            }

            Directory.CreateDirectory(path);
            
            System.IO.Compression.ZipFile.ExtractToDirectory("data.zip", path);

            File.Delete("data.zip");
        }

        public static void DeleteTrash(string path) => Directory.Delete(path);

    }
}

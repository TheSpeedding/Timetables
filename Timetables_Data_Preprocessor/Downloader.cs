using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Timetables
{
    namespace Preprocessor
    {
        public static class Downloader
        {
            public static void GetDataFeed()
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
                
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, "data.zip");
                }

                Directory.CreateDirectory("temp_data");

                System.IO.Compression.ZipFile.ExtractToDirectory("data.zip", "temp_data");

                File.Delete("data.zip");
            }

            public static void DeleteTrash() => Directory.Delete("temp_data");

        }
    }
}

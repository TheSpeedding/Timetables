using System;
using System.IO;
using System.Net;

namespace Timetables.Preprocessor
{
    public static class Downloader
    {
        private readonly static Uri defaultUrl = new Uri(@"http://opendata.iprpraha.cz/DPP/JR/jrdata.zip");

        [Flags]
        public enum DownloadState { OK = 0, UrlInvalidFormat = 2, ResourceFileNotFound = 4, NoInternetConnection = 8, DefaultDataDownloaded = 16, UnknownError = 32  }
        public static DownloadState GetDataFeed(string path)
        {
            DownloadState state = DownloadState.OK;

            try
            {
                Uri url = null;
                StreamReader sr = null;

                try
                {
                    sr = new StreamReader("resource.txt");
                    url = new Uri(sr.ReadLine());
                }
                catch (FileNotFoundException)
                {
                    url = defaultUrl;
                    state = DownloadState.ResourceFileNotFound | DownloadState.DefaultDataDownloaded;
                }
                catch (UriFormatException)
                {
                    url = defaultUrl;
                    state = DownloadState.UrlInvalidFormat | DownloadState.DefaultDataDownloaded;
                }
                finally
                {
                    if (sr != null) sr.Dispose();
                }


                using (var client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile(url, "data.zip");
                    }
                    catch (WebException)
                    {
                        state = DownloadState.NoInternetConnection;
                    }
                }

                if (Directory.Exists(path))
                    Directory.Delete(path, true);

                Directory.CreateDirectory(path);

                System.IO.Compression.ZipFile.ExtractToDirectory("data.zip", path);

                File.Delete("data.zip");
            }
            catch (Exception)
            {
                state = DownloadState.UnknownError;
            }

            return state;
        }

        public static void DeleteTrash(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

    }
}

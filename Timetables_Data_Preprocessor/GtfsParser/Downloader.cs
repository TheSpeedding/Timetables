using System;
using System.IO;
using System.Net;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Static class used for dealing with data stored in the internet.
	/// </summary>
    public static class Downloader
    {
        private readonly static Uri defaultUrl = new Uri(@"http://opendata.iprpraha.cz/DPP/JR/jrdata.zip");

        [Flags]
        public enum DownloadState { OK = 0, UrlInvalidFormat = 2, ResourceFileNotFound = 4, NoInternetConnection = 8, DefaultDataDownloaded = 16, UnknownError = 32  }
		/// <summary>
		/// Downloads and unzips data feed.
		/// </summary>
		/// <param name="path">Folder where the data should be extracted in.</param>
		/// <returns>Return code.</returns>
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
                catch (FileNotFoundException) // File with URL to the data source not found. Choosing default data source.
                {
                    url = defaultUrl;
                    state = DownloadState.ResourceFileNotFound | DownloadState.DefaultDataDownloaded;
                }
                catch (UriFormatException) // URL supplied in the file is invalid. Choosing default data source.
                {
                    url = defaultUrl;
                    state = DownloadState.UrlInvalidFormat | DownloadState.DefaultDataDownloaded;
                }
                finally
                {
                    if (sr != null)
						sr.Dispose();
                }


                using (var client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile(url, "data.zip");
                    }
                    catch (WebException) // Downloading of data unsuccessful. Missing internet connection?
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
            catch (Exception) // We did our best but data were not successfully downloaded.
            {
                state = DownloadState.UnknownError;
            }

            return state;
        }

		/// <summary>
		/// Deletes given folder.
		/// </summary>
		/// <param name="path">Folder to delete.</param>
        public static void DeleteTrash(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

    }
}

﻿using System;
using System.IO;
using System.Net;

namespace Timetables.Preprocessor
{
	/// <summary>
	/// Static class used for dealing with data stored in the internet.
	/// </summary>
    public static class Downloader
    {		
		/// <summary>
		/// Downloads and unzips data feed.
		/// </summary>
		/// <param name="path">Folder where the data should be extracted in.</param>
		/// <param name="url">Url to the data.</param>
		public static void GetDataFeed(string path, string url)
		{
			Uri uri = new Uri(url);

			using (var client = new WebClient())
			{
				client.DownloadFile(uri, "data.zip");
			}

			if (Directory.Exists(path))
				Directory.Delete(path, true);

			Directory.CreateDirectory(path);

			System.IO.Compression.ZipFile.ExtractToDirectory("data.zip", path);

			File.Delete("data.zip");
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Timetables.Server;
using static System.Console;

namespace Timetables.Benchmarks
{
	class Program
	{
		static void Main(string[] args)
		{
			Directory.Delete("cached", true);

			while (!DataFeed.Loaded) ;

			Server.Server.Start(IPAddress.Any, Server.Settings.RouterPort, Server.Settings.DepartureBoardPort, Server.Settings.BasicDataFeedPort);

			Application.Desktop.Settings.Load();
			Client.DataFeedDesktop.Load();

			using (ManagementObjectSearcher win32Proc = new ManagementObjectSearcher("select * from Win32_Processor"),
					win32CompSys = new ManagementObjectSearcher("select * from Win32_ComputerSystem"),
					win32Memory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory"))
			{
				foreach (ManagementObject obj in win32Proc.Get())
				{
					WriteLine("Clock speed: " + obj["CurrentClockSpeed"].ToString() + " MHz");
					WriteLine("Name: " + obj["Name"].ToString());
					WriteLine("Manufacturer: " + obj["Manufacturer"].ToString());
				}
			}
				WriteLine();

			Client.AsyncHelpers.RunSync(() => RunComplexJourneyBenchmark("", ""));
		}

		async static Task RunComplexJourneyBenchmark(string source, string target)
		{
			async Task<TimeSpan> RunBasicBenchmark(bool offlineMode, Structures.Basic.StationsBasic.StationBasic sourceStation, Structures.Basic.StationsBasic.StationBasic targetStation, int count, DateTime dt)
			{
				Stopwatch sw = new Stopwatch();

				Client.DataFeedDesktop.OfflineMode = offlineMode;

				sw.Start();

				await Application.Desktop.Request.SendRouterRequestAsync(new Client.RouterRequest(sourceStation.ID, targetStation.ID, dt, 10, count, 1, (Client.MeanOfTransport)255));

				sw.Stop();

				return sw.Elapsed;
			}

			async Task RunOnlineAndOfflineBenchmark(Structures.Basic.StationsBasic.StationBasic sourceStation, Structures.Basic.StationsBasic.StationBasic targetStation, int count, DateTime dt)
			{
				var online = await RunBasicBenchmark(false, sourceStation, targetStation, count, dt);
				var offline = await RunBasicBenchmark(true, sourceStation, targetStation, count, dt);

				WriteLine($"Online mode: { online.TotalMilliseconds } ms (avg. { online.TotalMilliseconds / count } ms). ");
				WriteLine($"Offline mode: { offline.TotalMilliseconds } ms (avg. { offline.TotalMilliseconds / count } ms). ");
			}

			async Task RunOneIterationBenchmark(Structures.Basic.StationsBasic.StationBasic sourceStation, Structures.Basic.StationsBasic.StationBasic targetStation, int count)
			{
				WriteLine($"Trying to obtain journeys in the morning rush (very high traffic).");
				await RunOnlineAndOfflineBenchmark(sourceStation, targetStation, count, DateTime.Now.Date.AddHours(9));
				WriteLine();
				WriteLine($"Trying to obtain journeys at the noon time (moderate traffic).");
				await RunOnlineAndOfflineBenchmark(sourceStation, targetStation, count, DateTime.Now.Date.AddHours(13));
				WriteLine();
				WriteLine($"Trying to obtain journeys in the afternoon rush (high traffic).");
				await RunOnlineAndOfflineBenchmark(sourceStation, targetStation, count, DateTime.Now.Date.AddHours(17));
				WriteLine();
				WriteLine($"Trying to obtain journeys at night (very low traffic).");
				await RunOnlineAndOfflineBenchmark(sourceStation, targetStation, count, DateTime.Now.Date.AddHours(2));
				WriteLine();
			}
			
			var src = Application.Desktop.Request.GetStationFromString(source);
			var trg = Application.Desktop.Request.GetStationFromString(target);

			WriteLine($"Running complex journey benchmark for stations { source } and { target }...");

			WriteLine("Number of journeys: 10");
			await RunOneIterationBenchmark(src, trg, 10);
			WriteLine();
			WriteLine("Number of journeys: 25");
			await RunOneIterationBenchmark(src, trg, 25);
			WriteLine();
			WriteLine("Number of journeys: 100");
			await RunOneIterationBenchmark(src, trg, 100);
			WriteLine();

			Write("Caching: ");
			Stopwatch sw1 = new Stopwatch();

			Client.DataFeedDesktop.OfflineMode = false;

			sw1.Start();

			await Application.Desktop.Request.CacheJourneyAsync(new Client.RouterRequest(src.ID, trg.ID, DateTime.Now, 10, DateTime.Now.AddDays(1), 1, (Client.MeanOfTransport)255));

			sw1.Stop();

			WriteLine(sw1.Elapsed.TotalSeconds + " s.");

			WriteLine();
			WriteLine();
		}		
	}
}

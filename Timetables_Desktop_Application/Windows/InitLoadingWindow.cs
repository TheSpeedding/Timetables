using System;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Timetables.Client;
using System.Device.Location;

namespace Timetables.Application.Desktop
{
	public partial class InitLoadingWindow : Form
	{
		/// <summary>
		/// Indicates whether the init window has faulted.
		/// </summary>
		public bool IsFaulted { get; private set; } = false;

		public InitLoadingWindow() => InitializeComponent();

		/// <summary>
		/// Callback for loading progress event.
		/// </summary>
		/// <param name="message">Message to be shown.</param>
		/// <param name="percentage">Precentage.</param>
		private void LoadingProgressCallback(string message, int percentage)
		{
			loadingProgressBar.Invoke((MethodInvoker)delegate { loadingProgressBar.Value += percentage; });
			loadingLabel.Invoke((MethodInvoker)delegate { loadingLabel.Text = message; });
		}
		
		private async void InitLoadingWindow_Shown(object sender, EventArgs e)
		{
			var gw = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
			gw.TryStart(false, TimeSpan.FromSeconds(5));

			DataFeedClient.GeoWatcher = new Timetables.Utilities.CPGeolocator(() =>
			{
				var loc = gw.Position.Location;
				return new Timetables.Utilities.Position(loc.Latitude, loc.Longitude);
			});

			topBarTimer.Start();

			System.Windows.Forms.Application.DoEvents();

			Preprocessor.DataFeed.LoadingProgress += LoadingProgressCallback;
			
			await Task.Run(async () =>
			{
				var loadingThread = DataFeedDesktop.DownloadAsync(false, Settings.TimeoutDuration);
				bool timerStarted = false;

				try
				{
					do
					{
						if (loadingThread.IsFaulted)
							throw loadingThread.Exception;

						if (!timerStarted && (DataFeedDesktop.Downloaded || !DataFeedDesktop.OfflineMode))
							timerStarted = true;

						else if (timerStarted && loadingProgressBar.Value > 95)
							break;

						else if (timerStarted && loadingProgressBar.Value < 100)
						{
							Thread.Sleep(30);
							LoadingProgressCallback("Loading data.", 1);
						}
					}
					while (!DataFeedDesktop.Loaded);
				}

				catch (AggregateException ex)
				{
					foreach (var innerEx in ex.InnerExceptions)
					{
						if (innerEx is System.Net.WebException)
							MessageBox.Show(Settings.Localization.UnreachableHost, Settings.Localization.Offline, MessageBoxButtons.OK, MessageBoxIcon.Error);

						else
						{
							Environment.Exit(0);
						}

						IsFaulted = true;
					}
				}

				finally
				{
					await loadingThread;
					
					// Just for an effect :-)

					while (loadingProgressBar.Value < 100)
					{
						Thread.Sleep(30);
						LoadingProgressCallback("Loading data.", 1);
					}
				}
			});
			

			Preprocessor.DataFeed.LoadingProgress -= LoadingProgressCallback;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			if (!DataFeedDesktop.OfflineMode)
				Request.UpdateCachedResultsAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

			Close();
		}

		private void topBarTimer_Tick(object sender, EventArgs e)
		{
			if (Text.Substring(Text.Length - 3, 3) == "...")
				Text = "Loading ";
			else
				Text += '.';
		}
	}
}

using System;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Timetables.Client;

namespace Timetables.Application.Desktop
{
	public partial class InitLoadingWindow : Form
	{
		public bool Faulted { get; private set; } = false;

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
			topBarTimer.Start();

			System.Windows.Forms.Application.DoEvents();

			Preprocessor.DataFeed.LoadingProgress += LoadingProgressCallback;
			
			await Task.Run(() =>
			{
				try
				{
					var loadingThread = DataFeed.LoadAsync(false, Settings.TimeoutDuration);
					bool timerStarted = false;

					while (!DataFeed.Loaded)
					{
						if (loadingThread.IsFaulted)
							throw loadingThread.Exception;

						if ((DataFeed.Downloaded || !DataFeed.OfflineMode) && !timerStarted)
							timerStarted = true;
						else if (timerStarted && loadingProgressBar.Value < 100)
						{
							Thread.Sleep(30);
							LoadingProgressCallback("The data are being loaded.", 1);
						}
					}
				}

				catch (AggregateException ex)
				{
					foreach (var innerEx in ex.InnerExceptions)
					{
						if (innerEx is System.Net.WebException)
							MessageBox.Show(Settings.Localization.UnreachableHost, Settings.Localization.Offline, MessageBoxButtons.OK, MessageBoxIcon.Error);

						else
							Program.UnhandledExceptionCallback(this, new UnhandledExceptionEventArgs(innerEx, true));

						Faulted = true;
					}
				}
			});
			

			Preprocessor.DataFeed.LoadingProgress -= LoadingProgressCallback;

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

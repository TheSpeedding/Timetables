using System;
using System.Net;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Timetables.Application.Mobile
{
	public partial class App : Xamarin.Forms.Application
	{
		public App()
		{
			bool firstCall = false; // ConnectivityTypeChanged event is broken, it's called twice everytime the type is changed.

			Plugin.Connectivity.CrossConnectivity.Current.ConnectivityTypeChanged += async (s, e) =>
			{
				firstCall = !firstCall;

				if (!firstCall) return;

				bool isWifi = false;

				foreach (var connectionType in e.ConnectionTypes)
					if (connectionType == Plugin.Connectivity.Abstractions.ConnectionType.WiFi)
						isWifi = true;

				if (isWifi)
					try
					{
						await Request.UpdateCachedResultsAsync(true);
					}
					catch { }
			};

			InitializeComponent();

			Settings.Load();

			try
			{
				Settings.LoadDataFeedAsync();
			}
			catch (WebException)
			{
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnreachableHost);
			}

			if (Settings.UseCellularsToUpdateCache)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				Request.UpdateCachedResultsAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			
			MainPage = new MainPage();
		}
				
		protected override void OnSleep() => Settings.Save();
	}
}

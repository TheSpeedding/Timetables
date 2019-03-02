using System;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Timetables.Application.Mobile
{
	public partial class App : Xamarin.Forms.Application
	{
		public App()
		{
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

			MainPage = new MainPage();
		}

		protected override void OnStart()
		{			
			if (Request.IsConnectedToWiFi)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				Request.UpdateCachedResultsAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		protected override void OnResume()
		{
			if (Request.IsConnectedToWiFi)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				Request.UpdateCachedResultsAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}
	}
}

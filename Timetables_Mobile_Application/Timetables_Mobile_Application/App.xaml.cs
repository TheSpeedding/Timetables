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

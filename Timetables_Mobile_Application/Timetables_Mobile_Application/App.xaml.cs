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
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timetables.Application.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindStationInfoPage : ContentPage
	{
		public FindStationInfoPage ()
		{
			InitializeComponent ();

			BindingContext = new FindLineInfoPageViewModel();
		}
		private void OnCountSliderValueChanged(object sender, ValueChangedEventArgs e)
		{
			countSlider.Value = Math.Round(e.NewValue / 1.0);
			countLabel.Text = countSlider.Value.ToString();
		}
		class FindLineInfoPageViewModel : INotifyPropertyChanged
		{
			public Localization Localization { get; } = Settings.Localization;

			public DateTime CurrentDateTime { get; } = DateTime.Now;

			#region INotifyPropertyChanged Implementation
			public event PropertyChangedEventHandler PropertyChanged;
			void OnPropertyChanged([CallerMemberName] string propertyName = "")
			{
				if (PropertyChanged == null)
					return;

				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
			#endregion
		}
		private async void FindButtonClicked(object sender, EventArgs e)
		{
			Structures.Basic.StationsBasic.StationBasic station = Request.GetStationFromString(stopEntry.Text);

			if (station == null)
			{
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnableToFindStation + ": " + stopEntry.Text);
				return;
			}
			/*
			var routerRequest = new RouterRequest(source.ID, target.ID, leavingTimeDatePicker.Date.Add(leavingTimeTimePicker.Time),
				(int)transfersSlider.Value, (int)countSlider.Value, Settings.WalkingSpeedCoefficient, Settings.GetMoT());

			var routerResponse = await Request.SendRouterRequestAsync(routerRequest);

			await Navigation.PushAsync(new FindJourneyResults(routerResponse), true);*/
		}

		private void StopEntryTextChanged(object sender, TextChangedEventArgs e)
		{
			linePicker.Items.Clear();
			var station = DataFeedClient.Basic.Stations.FindByName(stopEntry.Text);
			linePicker.IsEnabled = station != null;
			if (station != null)
				foreach (var item in station.GetThroughgoingRoutes().Select(r => r.Label).Distinct())
					linePicker.Items.Add(item);
		}
	}
}
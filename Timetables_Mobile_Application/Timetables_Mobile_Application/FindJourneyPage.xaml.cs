using dotMorten.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Timetables.Client;
using Timetables.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timetables.Application.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindJourneyPage : ContentPage
    {
		public FindJourneyPage()
        {
            InitializeComponent();

			BindingContext = new FindJourneyPageViewModel();
		}
		private void OnCountSliderValueChanged(object sender, ValueChangedEventArgs e)
		{
			countSlider.Value = Math.Round(e.NewValue / 1.0);
			countLabel.Text = countSlider.Value.ToString();
		}
		private void OnTransfersSliderValueChanged(object sender, ValueChangedEventArgs e)
		{
			transfersSlider.Value = Math.Round(e.NewValue / 1.0);
			transfersLabel.Text = transfersSlider.Value.ToString();
		}

		class FindJourneyPageViewModel : INotifyPropertyChanged
		{
			public Localization Localization { get; } = Settings.Localization;

			public DateTime CurrentDateTime { get; } = DateTime.Now;

			public ObservableCollection<Structures.Basic.StationsBasic.StationBasic> StationCollection { get; set; }

			public FindJourneyPageViewModel()
			{
				StationCollection = new ObservableCollection<Structures.Basic.StationsBasic.StationBasic>();
				foreach (var station in DataFeedClient.Basic.Stations)
					StationCollection.Add(station);
			}

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
			Structures.Basic.StationsBasic.StationBasic source = Request.GetStationFromString(sourceStopEntry.Text);
			Structures.Basic.StationsBasic.StationBasic target = Request.GetStationFromString(targetStopEntry.Text);

			if (source == null)
			{
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnableToFindStation + ": " + sourceStopEntry.Text);
				return;
			}
			if (target == null)
			{
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnableToFindStation + ": " + targetStopEntry.Text);
				return;
			}

			var routerRequest = new RouterRequest(source.ID, target.ID, leavingTimeDatePicker.Date.Add(leavingTimeTimePicker.Time), 
				(int)transfersSlider.Value, (int)countSlider.Value, Settings.WalkingSpeedCoefficient, Settings.GetMoT());

			var routerResponse = await Request.SendRouterRequestAsync(routerRequest);

			await Navigation.PushAsync(new FindJourneyResultsPage(routerResponse, source.Name, target.Name), true);
		}

		private void StopEntryTextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
		{
			if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
			{
				if (((AutoSuggestBox)sender).Text.Length > 2)
				{
					((AutoSuggestBox)sender).ItemsSource = DataFeedClient.Basic.Stations.Where(x => x.Name.StartsWith(((AutoSuggestBox)sender).Text, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name).ToList();
				}
			}

		}

		private void StopEntrySuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
		{
			((AutoSuggestBox)sender).Text = e.SelectedItem.ToString();
		}

		private void FindClosestStation(object sender, EventArgs e)
		{
			var station = DataFeedClient.Basic.Stations.FindClosestStation(AsyncHelpers.RunSync<Position>(DataFeedClient.GeoWatcher.GetCurrentPosition));
			if (station != null)
			{
				sourceStopEntry.Text = station.Name;
			}
		}
	}
}
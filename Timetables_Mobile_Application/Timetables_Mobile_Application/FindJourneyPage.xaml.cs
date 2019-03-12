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
			
			foreach (var cached in JourneyCached.FetchJourneyData())
			{
				favoritesStackLayout.Children.Add(new FavoriteItemContentView(favoritesStackLayout, scrollView, cached, sourceStopEntry, targetStopEntry));
			}
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
			try
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

				findButton.IsEnabled = false;
				var routerResponse = await Request.SendRouterRequestAsync(routerRequest);
				findButton.IsEnabled = true;

				if (routerResponse != null)
					await Navigation.PushAsync(new FindJourneyResultsPage(routerResponse, source.Name, target.Name), true);
			}
			catch
			{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				Request.CheckBasicDataValidity();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			}
		}

		private void StopEntryTextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
		{
			if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
			{
				try
				{
					((AutoSuggestBox)sender).ItemsSource = DataFeedClient.Basic.Stations.Where(x => x.Name.StartsWith(((AutoSuggestBox)sender).Text, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name).ToList();		
				}
				catch
				{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
					Request.CheckBasicDataValidity();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				}
			}

		}

		private void StopEntrySuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
		{
			((AutoSuggestBox)sender).Text = e.SelectedItem.ToString();
		}

		private void FindClosestStation(object sender, EventArgs e)
		{
			try
			{
				Utilities.Position pos = new Position();

				try
				{
					pos = AsyncHelpers.RunSync<Position>(DataFeedClient.GeoWatcher.GetCurrentPosition);
				}
				catch
				{
					PlatformDependentSettings.ShowMessage(Settings.Localization.PositioningNeeded);
				}

				var station = DataFeedClient.Basic.Stations.FindClosestStation(pos);
				if (station != null)
				{
					sourceStopEntry.Text = station.Name;
				}
			}
			catch
			{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				Request.CheckBasicDataValidity();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			}
		}

		private void FavoritesButtonClicked(object sender, EventArgs e)
		{
			try
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

				if (JourneyCached.Select(source.ID, target.ID) != null) return;

				var fav = new JourneyCached(source.ID, target.ID);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				Request.CacheJourneyAsync(fav.ConstructNewRequest());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

				favoritesStackLayout.Children.Add(new FavoriteItemContentView(favoritesStackLayout, scrollView, fav, sourceStopEntry, targetStopEntry));
			}
			catch
			{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				Request.CheckBasicDataValidity();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			}
		}
	}
}
using dotMorten.Xamarin.Forms;
using System;
using System.Collections.Generic;
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
	public partial class FindStationInfoPage : ContentPage
	{
		public FindStationInfoPage ()
		{
			InitializeComponent ();

			BindingContext = new FindStationInfoPageViewModel();

			foreach (var cached in StationInfoCached.FetchStationInfoData())
			{
				favoritesStackLayout.Children.Add(new FavoriteItemContentView(favoritesStackLayout, scrollView, cached, stopEntry));
			}
		}
		private void OnCountSliderValueChanged(object sender, ValueChangedEventArgs e)
		{
			countSlider.Value = Math.Round(e.NewValue / 1.0);
			countLabel.Text = countSlider.Value.ToString();
		}
		class FindStationInfoPageViewModel : INotifyPropertyChanged
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

			if (station == null) return;

			Structures.Basic.RoutesInfoBasic.RouteInfoBasic route = linePicker.SelectedItem == null ? null : Request.GetRouteInfoFromLabel(linePicker.SelectedItem.ToString());

			if (route == null && linePicker.SelectedItem != null) return;

			var dbRequest = new StationInfoRequest(station.ID, leavingTimeDatePicker.Date.Add(leavingTimeTimePicker.Time),
				(int)countSlider.Value, true, route == null ? -1 : route.ID);

			var dbResponse = await Request.SendDepartureBoardRequestAsync(dbRequest);

			await Navigation.PushAsync(new DepartureBoardResultsPage(dbResponse, true, station.Name), true);
		}
		
		private void StopEntryTextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
		{
			linePicker.Items.Clear();
			var station = DataFeedClient.Basic.Stations.FindByName(stopEntry.Text);
			linePicker.IsEnabled = station != null;
			if (station != null)
				foreach (var item in station.GetThroughgoingRoutes().Select(r => r.Label).Distinct())
					linePicker.Items.Add(item);

			if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
			{
				((AutoSuggestBox)sender).ItemsSource = DataFeedClient.Basic.Stations.Where(x => x.Name.StartsWith(((AutoSuggestBox)sender).Text, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name).ToList();
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
				stopEntry.Text = station.Name;
			}
		}

		private void FavoritesButtonClicked(object sender, EventArgs e)
		{
			Structures.Basic.StationsBasic.StationBasic station = Request.GetStationFromString(stopEntry.Text);

			if (station == null)
			{
				PlatformDependentSettings.ShowMessage(Settings.Localization.UnableToFindStation + ": " + stopEntry.Text);
				return;
			}

			if (StationInfoCached.Select(station.ID) != null) return;

			var fav = new StationInfoCached(station.ID);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			Request.CacheDepartureBoardAsync(fav.ConstructNewRequest());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

			favoritesStackLayout.Children.Add(new FavoriteItemContentView(favoritesStackLayout, scrollView, fav, stopEntry));

		}
	}
}
using dotMorten.Xamarin.Forms;
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
	public partial class FindLineInfoPage : ContentPage
	{
		public FindLineInfoPage ()
		{
			InitializeComponent ();

			BindingContext = new FindStationInfoPageViewModel();
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

			Structures.Basic.RoutesInfoBasic.RouteInfoBasic route = Request.GetRouteInfoFromLabel(lineEntry.Text);

			if (route == null) return;

			var dbRequest = new LineInfoRequest(leavingTimeDatePicker.Date.Add(leavingTimeTimePicker.Time),
				(int)countSlider.Value, route.ID);

			var dbResponse = await Request.SendDepartureBoardRequestAsync(dbRequest);

			await Navigation.PushAsync(new DepartureBoardResultsPage(dbResponse, false, route.Label), true);
		}

		private void LineEntryTextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
		{
			if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
			{
				((AutoSuggestBox)sender).ItemsSource = DataFeedClient.Basic.RoutesInfo.Where(x => x.Label.StartsWith(((AutoSuggestBox)sender).Text, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Label).ToList();
			}

		}

		private void LineEntrySuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
		{
			((AutoSuggestBox)sender).Text = e.SelectedItem.ToString();
		}
	}
}
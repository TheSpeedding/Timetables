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
		
		private void FindButtonClicked(object sender, EventArgs e)
		{
			return;
		}
	}
}
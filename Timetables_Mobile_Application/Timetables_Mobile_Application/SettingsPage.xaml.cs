using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();

			BindingContext = new SettingsPageViewModel();

			languagePicker.Items.Add("English");
			
			foreach (var file in PlatformDependentSettings.GetLocalizations().Select(x => new FileInfo(x)))
				if (file.Extension == ".xml")
					languagePicker.Items.Add(file.Name.Split('.')[0]);

			languagePicker.SelectedItem = (from object item in languagePicker.Items where item.ToString() == Settings.Localization.ToString() select item).First();

			languagePicker.SelectedIndexChanged += LanguagePicker_SelectedIndexChanged;

			speedSlider.Value = Settings.WalkingSpeedCoefficient * 100;

			subwaySwitch.IsToggled = Settings.AllowSubway;
			tramSwitch.IsToggled = Settings.AllowTram;
			busSwitch.IsToggled = Settings.AllowBus;
			trainSwitch.IsToggled = Settings.AllowTrain;
			cablecarSwitch.IsToggled = Settings.AllowCablecar;
			shipSwitch.IsToggled = Settings.AllowShip;
			wifiSwitch.IsToggled = Settings.UseCellularsToUpdateCache;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			Settings.Save(); // Save settings made on the page.
		}

		class SettingsPageViewModel : INotifyPropertyChanged
		{
			public Localization Localization { get; } = Settings.Localization;
			
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

		private void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var name = (sender as Picker).SelectedItem.ToString();
			Settings.Localization = name == "English" ? 
				Timetables.Client.Localization.GetTranslation("English") :
				Timetables.Client.Localization.GetTranslation(new Tuple<Stream, string>(
				PlatformDependentSettings.GetStream(new FileInfo($"loc/{ name }.xml")), name));
			PlatformDependentSettings.ShowMessage(Settings.Localization.RestartToApplyChanges);
		}

		private void SpeedSlider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			speedSlider.Value = Math.Round(e.NewValue / 1.0);
			Settings.WalkingSpeedCoefficient = speedSlider.Value / 100.0;
		}

		private void SubwaySwitch_Toggled(object sender, ToggledEventArgs e)
		{
			Settings.AllowSubway = subwaySwitch.IsToggled;
		}

		private void TramSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			Settings.AllowTram = tramSwitch.IsToggled;
		}

		private void BusSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			Settings.AllowBus = busSwitch.IsToggled;
		}

		private void TrainSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			Settings.AllowTrain = trainSwitch.IsToggled;
		}

		private void CablecarSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			Settings.AllowCablecar = cablecarSwitch.IsToggled;
		}

		private void ShipSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			Settings.AllowShip = shipSwitch.IsToggled;
		}

		private void WifiSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			Settings.UseCellularsToUpdateCache = wifiSwitch.IsToggled;
		}
	}
}
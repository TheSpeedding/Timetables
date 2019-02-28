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
	}
}
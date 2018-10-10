using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using Timetables.Client;
using WeifenLuo.WinFormsUI.Docking;

namespace Timetables.Application.Desktop
{
	public partial class SettingsWindow : Form
	{
		private bool restartNeeded;
		public SettingsWindow()
		{
			InitializeComponent();

			Settings.Theme.Apply(this);

			Name = Settings.Localization.Settings;

			languageLabel.Text = Settings.Localization.Language;
			themeLabel.Text = Settings.Localization.Theme;

			dataDownloadButton.Text = Settings.Localization.ForceDataDownload;

			themeComboBox.Items.Add(Settings.Localization.BlueTheme);
			themeComboBox.Items.Add(Settings.Localization.DarkTheme);
			themeComboBox.Items.Add(Settings.Localization.LightTheme);

			if (Settings.Theme is  Themes.BlueTheme) themeComboBox.SelectedIndex = 0;
			if (Settings.Theme is  Themes.DarkTheme) themeComboBox.SelectedIndex = 1;
			if (Settings.Theme is Themes.LightTheme) themeComboBox.SelectedIndex = 2;

			languageComboBox.Items.Add("English");

			if (Directory.Exists("loc"))
				foreach (var file in new DirectoryInfo("loc/").GetFiles())
					if (file.Extension == ".xml")
						languageComboBox.Items.Add(file.Name.Split('.')[0]);

			languageComboBox.SelectedItem = (from object item in languageComboBox.Items where item.ToString() == Settings.Localization.ToString() select item).First();

			restartNeeded = false;
		}

		private void themeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch((sender as ComboBox).SelectedIndex)
			{
				case 0:
					Settings.Theme = new Themes.BlueTheme();
					break;
				case 1:
					Settings.Theme = new Themes.DarkTheme();
					break;
				case 2:
					Settings.Theme = new Themes.LightTheme();
					break;
				default:
					throw new InvalidOperationException();
			}

			restartNeeded = true;

			Settings.Save();
		}

		private void languageComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			Settings.Localization = Timetables.Client.Localization.GetTranslation((sender as ComboBox).SelectedItem.ToString());

			restartNeeded = true;

			Settings.Save();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			if (restartNeeded) MessageBox.Show(Settings.Localization.RestartToApplyChanges, Settings.Localization.RestartNeeded, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private async void dataDownloadButton_Click(object sender, EventArgs e)
		{
			await DataFeedDesktop.DownloadAsync();
			MessageBox.Show(Settings.Localization.DataDownloadedSuccessfully);
		}
	}
}

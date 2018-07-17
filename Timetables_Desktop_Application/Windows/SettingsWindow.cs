using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace Timetables.Application.Desktop
{
	public partial class SettingsWindow : Form
	{
		private bool restartNeeded = false;
		public SettingsWindow()
		{
			InitializeComponent();

			Settings.Theme.Apply(this);
			
			if (Settings.Theme is  Themes.BlueTheme) themeComboBox.SelectedIndex = 0;
			if (Settings.Theme is  Themes.DarkTheme) themeComboBox.SelectedIndex = 1;
			if (Settings.Theme is Themes.LightTheme) themeComboBox.SelectedIndex = 2;

			languageComboBox.Items.Add("English");

			if (Directory.Exists("loc"))
				foreach (var file in new DirectoryInfo("loc/").GetFiles())
					if (Path.GetExtension(file.FullName) == "xml")
						languageComboBox.Items.Add(file.Name.Split('.')[0]);

			// languageComboBox.SelectedText = from ComboBox.ObjectCollection item in languageComboBox.Items where item.ToString() == languageComboBox.Text select item.ToString();
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
			Settings.Language = Timetables.Client.Localization.GetTranslation((sender as ComboBox).SelectedItem.ToString());

			restartNeeded = true;

			Settings.Save();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			if (restartNeeded) MessageBox.Show("You have to restart the application to apply the changes.", "Restart needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}

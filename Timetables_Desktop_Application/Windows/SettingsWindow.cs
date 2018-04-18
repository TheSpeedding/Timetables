using System;
using System.Windows.Forms;
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

			languageComboBox.SelectedIndex = (int)Settings.Language;

			if (Settings.Theme is  Themes.BlueTheme) themeComboBox.SelectedIndex = 0;
			if (Settings.Theme is  Themes.DarkTheme) themeComboBox.SelectedIndex = 1;
			if (Settings.Theme is Themes.LightTheme) themeComboBox.SelectedIndex = 2;

			languageComboBox.SelectedIndexChanged += new EventHandler(languageComboBox_SelectedIndexChanged);
			themeComboBox.SelectedIndexChanged += new EventHandler(themeComboBox_SelectedIndexChanged);
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
			Settings.Language = (Language)(sender as ComboBox).SelectedIndex;

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

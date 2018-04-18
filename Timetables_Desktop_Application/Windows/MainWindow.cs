using System;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;

namespace Timetables.Application.Desktop
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();

			mainDockPanel.Theme = Settings.Theme;
			if (Settings.Theme is VS2015DarkTheme)
				mainMenuStrip.Renderer = new MenuTheme<MenuColors.DarkTheme>(mainMenuStrip);
			if (Settings.Theme is VS2015BlueTheme)
				mainMenuStrip.Renderer = new MenuTheme<MenuColors.BlueTheme>(mainMenuStrip);
			if (Settings.Theme is VS2015LightTheme)
				mainMenuStrip.Renderer = new MenuTheme<MenuColors.LightTheme>(mainMenuStrip);

		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) => new SettingsWindow().ShowDialog();

		private void findDeparturesToolStripMenuItem_Click(object sender, EventArgs e) => new NewDepartureBoardWindow().Show(mainDockPanel, DockState.Document);
	}
}

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
			mainMenuStrip.Renderer = new ThemeMenu<MenuColors.DarkTheme>(mainMenuStrip);
					
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) => new SettingsWindow().ShowDialog();

		private void findDeparturesToolStripMenuItem_Click(object sender, EventArgs e) => new NewDepartureBoardWindow().Show(mainDockPanel, DockState.Document);
	}
}

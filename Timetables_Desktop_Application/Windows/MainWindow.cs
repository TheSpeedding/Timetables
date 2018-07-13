using System;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using Timetables.Application.Desktop.Themes;

namespace Timetables.Application.Desktop
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();

			Settings.Theme.PanelTheme.Apply(mainDockPanel);
			Settings.Theme.MenuTheme.Apply(mainMenuStrip);
			Settings.Theme.Apply(this);
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) => new SettingsWindow().ShowDialog();

		private void findDeparturesToolStripMenuItem_Click(object sender, EventArgs e) => new NewDepartureBoardWindow().Show(mainDockPanel, DockState.Document);

		private void findjourneyToolStripMenuItem_Click(object sender, EventArgs e) => new NewJourneyWindow().Show(mainDockPanel, DockState.Document);

		private void extraordinaryEventsToolStripMenuItem_Click(object sender, EventArgs e) => new ExtraEventsWindow().Show(mainDockPanel, DockState.Document);

		private void lockoutsToolStripMenuItem_Click(object sender, EventArgs e) => new LockoutsWindow().Show(mainDockPanel, DockState.Document);
	}
}

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

			extraordinaryEventsToolStripMenuItem.Text = Settings.Localization.ExtraordinaryEvents;
			lockoutsToolStripMenuItem.Text = Settings.Localization.Lockouts;
			trafficToolStripMenuItem.Text = Settings.Localization.Traffic;
			settingsToolStripMenuItem.Text = Settings.Localization.Settings;
			journeyToolStripMenuItem.Text = Settings.Localization.Journey;
			departureBoardToolStripMenuItem.Text = Settings.Localization.DepartureBoard;
			findDeparturesToolStripMenuItem.Text = Settings.Localization.FindDepartures;
			findjourneyToolStripMenuItem.Text = Settings.Localization.FindJourney;
			showmapToolStripMenuItem.Text = Settings.Localization.ShowMap;
			favoritesToolStripMenuItem.Text = Settings.Localization.Favorites;
			favoritesToolStripMenuItem1.Text = Settings.Localization.Favorites;

			Settings.Theme.PanelTheme.Apply(mainDockPanel);
			Settings.Theme.MenuTheme.Apply(mainMenuStrip);
			Settings.Theme.Apply(this);
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) => new SettingsWindow().ShowDialog();

		private void findDeparturesToolStripMenuItem_Click(object sender, EventArgs e) => new NewDepartureBoardWindow().Show(mainDockPanel, DockState.Document);

		private void findjourneyToolStripMenuItem_Click(object sender, EventArgs e) => new NewJourneyWindow().Show(mainDockPanel, DockState.Document);

		private void extraordinaryEventsToolStripMenuItem_Click(object sender, EventArgs e) => new ExtraEventsWindow().Show(mainDockPanel, DockState.Document);

		private void lockoutsToolStripMenuItem_Click(object sender, EventArgs e) => new LockoutsWindow().Show(mainDockPanel, DockState.Document);

		private async void favoritesToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			var window = new FavoriteJourneysWindow();
			window.ShowDialog();

			if (window.journeysToFind != null)
				foreach (var journey in window.journeysToFind.Favorites)
					(await new NewJourneyWindow(mainDockPanel).SendRequestAsync(journey.Source, journey.Target, DateTime.Now, int.MaxValue, 5, 1)).Show(mainDockPanel, DockState.Document);
		}
	}
}

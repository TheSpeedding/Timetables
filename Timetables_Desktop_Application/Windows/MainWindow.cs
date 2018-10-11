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
			findDeparturesToolStripMenuItem.Text = Settings.Localization.FindDeparturesFromTheStation;
			findjourneyToolStripMenuItem.Text = Settings.Localization.FindJourney;
			showmapToolStripMenuItem.Text = Settings.Localization.ShowMap;
			favoritesToolStripMenuItem.Text = Settings.Localization.Favorites;
			favoritesToolStripMenuItem1.Text = Settings.Localization.Favorites;

			Settings.Theme.PanelTheme.Apply(mainDockPanel);
			Settings.Theme.MenuTheme.Apply(mainMenuStrip);
			Settings.Theme.Apply(this);
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) => new SettingsWindow().ShowDialog();

		private void findDeparturesToolStripMenuItem_Click(object sender, EventArgs e) => new NewStationInfoWindow().Show(mainDockPanel, DockState.Document);

		private void findjourneyToolStripMenuItem_Click(object sender, EventArgs e) => new NewJourneyWindow().Show(mainDockPanel, DockState.Document);

		private void extraordinaryEventsToolStripMenuItem_Click(object sender, EventArgs e) => new ExtraEventsWindow().Show(mainDockPanel, DockState.Document);

		private void lockoutsToolStripMenuItem_Click(object sender, EventArgs e) => new LockoutsWindow().Show(mainDockPanel, DockState.Document);

		private void favoritesToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			var window = new FavoriteJourneysWindow();
			window.ShowDialog();

			if (window.journeysToFind != null)
				foreach (var journey in window.journeysToFind.Favorites)
					new NewJourneyWindow(journey.Source, journey.Target).Show(mainDockPanel, DockState.Document);
		}

		private void favoritesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var window = new FavoriteDeparturesWindow();
			window.ShowDialog();

			if (window.departuresToFind != null)
				foreach (var departure in window.departuresToFind.Favorites)
					new NewStationInfoWindow(departure.Station).Show(mainDockPanel, DockState.Document);
		}

		private void showmapToolStripMenuItem_Click(object sender, EventArgs e) => new ShowMapWindow().Show(mainDockPanel, DockState.Document);
	}
}

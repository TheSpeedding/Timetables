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
			requestlineInfoToolStripMenuItem.Text = Settings.Localization.GetLineInfo;
			findjourneyToolStripMenuItem.Text = Settings.Localization.FindJourney;
			showmapToolStripMenuItem.Text = Settings.Localization.ShowMap;
			favoriteStationsToolStripMenuItem.Text = Settings.Localization.FavoriteStations;
			favoriteJourneysToolStripMenuItem.Text = Settings.Localization.FavoriteJourneys;
			favoriteLinesToolStripMenuItem.Text = Settings.Localization.FavoriteLines;

			Settings.Theme.PanelTheme.Apply(mainDockPanel);
			Settings.Theme.MenuTheme.Apply(mainMenuStrip);
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

			if (window.ItemsToFind != null)
				foreach (var item in window.ItemsToFind)
					new NewJourneyWindow(item.SourceStation.Name, item.TargetStation.Name).Show(mainDockPanel, DockState.Document);
		}

		private void favoriteStationsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var window = new FavoriteStationsWindow();
			window.ShowDialog();

			if (window.ItemsToFind != null)
				foreach (var item in window.ItemsToFind)
					new NewStationInfoWindow(item.Station.Name).Show(mainDockPanel, DockState.Document);
		}

		private void showmapToolStripMenuItem_Click(object sender, EventArgs e) => new ShowMapWindow().Show(mainDockPanel, DockState.Document);

		private void requestlineInfoToolStripMenuItem_Click(object sender, EventArgs e) => new NewLineInfoWindow().Show(mainDockPanel, DockState.Document);

		private void favoriteLinesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var window = new FavoriteLinesWindow();
			window.ShowDialog();

			if (window.ItemsToFind != null)
				foreach (var item in window.ItemsToFind)
					new NewLineInfoWindow(item.Route.Label).Show(mainDockPanel, DockState.Document);

		}
	}
}

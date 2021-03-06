﻿namespace Timetables.Application.Desktop
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.mainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this.journeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findjourneyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.favoriteJourneysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.departureBoardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findDeparturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.requestlineInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.favoriteStationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.trafficToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.extraordinaryEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lockoutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.favoriteLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainDockPanel
			// 
			this.mainDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainDockPanel.Location = new System.Drawing.Point(0, 24);
			this.mainDockPanel.Name = "mainDockPanel";
			this.mainDockPanel.Size = new System.Drawing.Size(1264, 657);
			this.mainDockPanel.TabIndex = 2;
			// 
			// mainMenuStrip
			// 
			this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.journeyToolStripMenuItem,
            this.departureBoardToolStripMenuItem,
            this.trafficToolStripMenuItem,
            this.settingsToolStripMenuItem});
			this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.Size = new System.Drawing.Size(1264, 24);
			this.mainMenuStrip.TabIndex = 5;
			// 
			// journeyToolStripMenuItem
			// 
			this.journeyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findjourneyToolStripMenuItem,
            this.toolStripSeparator2,
            this.favoriteJourneysToolStripMenuItem});
			this.journeyToolStripMenuItem.Name = "journeyToolStripMenuItem";
			this.journeyToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.journeyToolStripMenuItem.Text = "&Journey";
			// 
			// findjourneyToolStripMenuItem
			// 
			this.findjourneyToolStripMenuItem.Name = "findjourneyToolStripMenuItem";
			this.findjourneyToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
			this.findjourneyToolStripMenuItem.Text = "Find &journeys";
			this.findjourneyToolStripMenuItem.Click += new System.EventHandler(this.findjourneyToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(142, 6);
			// 
			// favoritesToolStripMenuItem1
			// 
			this.favoriteJourneysToolStripMenuItem.Name = "favoritesToolStripMenuItem1";
			this.favoriteJourneysToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
			this.favoriteJourneysToolStripMenuItem.Text = "&Favorites";
			this.favoriteJourneysToolStripMenuItem.Click += new System.EventHandler(this.favoritesToolStripMenuItem1_Click);
			// 
			// departureBoardToolStripMenuItem
			// 
			this.departureBoardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findDeparturesToolStripMenuItem,
            this.requestlineInfoToolStripMenuItem,
            this.showmapToolStripMenuItem,
            this.toolStripSeparator1,
            this.favoriteStationsToolStripMenuItem,
            this.favoriteLinesToolStripMenuItem});
			this.departureBoardToolStripMenuItem.Name = "departureBoardToolStripMenuItem";
			this.departureBoardToolStripMenuItem.Size = new System.Drawing.Size(105, 20);
			this.departureBoardToolStripMenuItem.Text = "&Departure board";
			// 
			// findDeparturesToolStripMenuItem
			// 
			this.findDeparturesToolStripMenuItem.Name = "findDeparturesToolStripMenuItem";
			this.findDeparturesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.findDeparturesToolStripMenuItem.Text = "Find &departures from station";
			this.findDeparturesToolStripMenuItem.Click += new System.EventHandler(this.findDeparturesToolStripMenuItem_Click);
			// 
			// requestlineInfoToolStripMenuItem
			// 
			this.requestlineInfoToolStripMenuItem.Name = "requestlineInfoToolStripMenuItem";
			this.requestlineInfoToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.requestlineInfoToolStripMenuItem.Text = "Request &line info";
			this.requestlineInfoToolStripMenuItem.Click += new System.EventHandler(this.requestlineInfoToolStripMenuItem_Click);
			// 
			// showmapToolStripMenuItem
			// 
			this.showmapToolStripMenuItem.Name = "showmapToolStripMenuItem";
			this.showmapToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.showmapToolStripMenuItem.Text = "Show &map";
			this.showmapToolStripMenuItem.Click += new System.EventHandler(this.showmapToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
			// 
			// favoritesToolStripMenuItem
			// 
			this.favoriteStationsToolStripMenuItem.Name = "favoritesToolStripMenuItem";
			this.favoriteStationsToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.favoriteStationsToolStripMenuItem.Text = "Favorite &stations";
			this.favoriteStationsToolStripMenuItem.Click += new System.EventHandler(this.favoriteStationsToolStripMenuItem_Click);
			// 
			// trafficToolStripMenuItem
			// 
			this.trafficToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extraordinaryEventsToolStripMenuItem,
            this.lockoutsToolStripMenuItem});
			this.trafficToolStripMenuItem.Name = "trafficToolStripMenuItem";
			this.trafficToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.trafficToolStripMenuItem.Text = "&Traffic";
			// 
			// extraordinaryEventsToolStripMenuItem
			// 
			this.extraordinaryEventsToolStripMenuItem.Name = "extraordinaryEventsToolStripMenuItem";
			this.extraordinaryEventsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.extraordinaryEventsToolStripMenuItem.Text = "E&xtraordinary events";
			this.extraordinaryEventsToolStripMenuItem.Click += new System.EventHandler(this.extraordinaryEventsToolStripMenuItem_Click);
			// 
			// lockoutsToolStripMenuItem
			// 
			this.lockoutsToolStripMenuItem.Name = "lockoutsToolStripMenuItem";
			this.lockoutsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.lockoutsToolStripMenuItem.Text = "&Lockouts";
			this.lockoutsToolStripMenuItem.Click += new System.EventHandler(this.lockoutsToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.settingsToolStripMenuItem.Text = "Settings";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// favoriteLinesToolStripMenuItem
			// 
			this.favoriteLinesToolStripMenuItem.Name = "favoriteLinesToolStripMenuItem";
			this.favoriteLinesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.favoriteLinesToolStripMenuItem.Text = "Favorite l&ines";
			this.favoriteLinesToolStripMenuItem.Click += new System.EventHandler(this.favoriteLinesToolStripMenuItem_Click);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(1264, 681);
			this.Controls.Add(this.mainDockPanel);
			this.Controls.Add(this.mainMenuStrip);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.mainMenuStrip;
			this.MinimumSize = new System.Drawing.Size(1280, 720);
			this.Name = "MainWindow";
			this.Text = "Timetables";
			this.mainMenuStrip.ResumeLayout(false);
			this.mainMenuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private WeifenLuo.WinFormsUI.Docking.DockPanel mainDockPanel;
		private System.Windows.Forms.MenuStrip mainMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem departureBoardToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findDeparturesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showmapToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem favoriteStationsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem journeyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findjourneyToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem favoriteJourneysToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem trafficToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem extraordinaryEventsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lockoutsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem requestlineInfoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem favoriteLinesToolStripMenuItem;
	}
}


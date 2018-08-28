namespace Timetables.Application.Desktop
{
	partial class FavoriteDeparturesWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoriteDeparturesWindow));
			this.favoritesListBox = new System.Windows.Forms.CheckedListBox();
			this.addButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.stationComboBox = new System.Windows.Forms.ComboBox();
			this.findButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// favoritesListBox
			// 
			this.favoritesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.favoritesListBox.FormattingEnabled = true;
			this.favoritesListBox.Location = new System.Drawing.Point(12, 12);
			this.favoritesListBox.Name = "favoritesListBox";
			this.favoritesListBox.Size = new System.Drawing.Size(240, 319);
			this.favoritesListBox.TabIndex = 0;
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(12, 374);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 20);
			this.addButton.TabIndex = 3;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point(94, 374);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(75, 20);
			this.removeButton.TabIndex = 4;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// stationComboBox
			// 
			this.stationComboBox.FormattingEnabled = true;
			this.stationComboBox.Location = new System.Drawing.Point(12, 347);
			this.stationComboBox.Name = "stationComboBox";
			this.stationComboBox.Size = new System.Drawing.Size(240, 21);
			this.stationComboBox.TabIndex = 5;
			this.stationComboBox.Text = "Station";
			// 
			// findButton
			// 
			this.findButton.Location = new System.Drawing.Point(177, 374);
			this.findButton.Name = "findButton";
			this.findButton.Size = new System.Drawing.Size(75, 20);
			this.findButton.TabIndex = 7;
			this.findButton.Text = "Find";
			this.findButton.UseVisualStyleBackColor = true;
			this.findButton.Click += new System.EventHandler(this.findButton_Click);
			// 
			// FavoriteDeparturesWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(264, 401);
			this.Controls.Add(this.findButton);
			this.Controls.Add(this.stationComboBox);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.favoritesListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FavoriteDeparturesWindow";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FavoriteDeparturesWindow_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox favoritesListBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.ComboBox stationComboBox;
		private System.Windows.Forms.Button findButton;
	}
}
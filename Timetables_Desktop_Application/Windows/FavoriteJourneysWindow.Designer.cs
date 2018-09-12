namespace Timetables.Application.Desktop
{
	partial class FavoriteJourneysWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoriteJourneysWindow));
			this.favoritesListBox = new System.Windows.Forms.CheckedListBox();
			this.addButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.findButton = new System.Windows.Forms.Button();
			this.targetTextBox = new System.Windows.Forms.TextBox();
			this.sourceTextBox = new System.Windows.Forms.TextBox();
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
			this.addButton.Location = new System.Drawing.Point(12, 401);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 20);
			this.addButton.TabIndex = 3;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point(95, 401);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(75, 20);
			this.removeButton.TabIndex = 4;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// findButton
			// 
			this.findButton.Location = new System.Drawing.Point(177, 401);
			this.findButton.Name = "findButton";
			this.findButton.Size = new System.Drawing.Size(75, 20);
			this.findButton.TabIndex = 7;
			this.findButton.Text = "Find";
			this.findButton.UseVisualStyleBackColor = true;
			this.findButton.Click += new System.EventHandler(this.findButton_Click);
			// 
			// targetTextBox
			// 
			this.targetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.targetTextBox.Font = new System.Drawing.Font("Calibri", 8.25F);
			this.targetTextBox.Location = new System.Drawing.Point(12, 374);
			this.targetTextBox.Name = "targetTextBox";
			this.targetTextBox.Size = new System.Drawing.Size(240, 21);
			this.targetTextBox.TabIndex = 1;
			this.targetTextBox.Text = "Target";
			// 
			// sourceTextBox
			// 
			this.sourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sourceTextBox.Font = new System.Drawing.Font("Calibri", 8.25F);
			this.sourceTextBox.Location = new System.Drawing.Point(12, 347);
			this.sourceTextBox.Name = "sourceTextBox";
			this.sourceTextBox.Size = new System.Drawing.Size(240, 21);
			this.sourceTextBox.TabIndex = 0;
			this.sourceTextBox.Text = "Source";
			// 
			// FavoriteJourneysWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(264, 428);
			this.Controls.Add(this.sourceTextBox);
			this.Controls.Add(this.targetTextBox);
			this.Controls.Add(this.findButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.favoritesListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FavoriteJourneysWindow";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.favoriteJourneysWindow_FormClosing);
			this.Load += new System.EventHandler(this.FavoriteJourneysWindow_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox favoritesListBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button findButton;
		private System.Windows.Forms.TextBox targetTextBox;
		private System.Windows.Forms.TextBox sourceTextBox;
	}
}
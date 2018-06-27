namespace Timetables.Application.Desktop
{
	partial class SettingsWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
			this.themeLabel = new System.Windows.Forms.Label();
			this.languageLabel = new System.Windows.Forms.Label();
			this.themeComboBox = new System.Windows.Forms.ComboBox();
			this.languageComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// themeLabel
			// 
			this.themeLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.themeLabel.Location = new System.Drawing.Point(14, 9);
			this.themeLabel.Margin = new System.Windows.Forms.Padding(5);
			this.themeLabel.Name = "themeLabel";
			this.themeLabel.Size = new System.Drawing.Size(110, 42);
			this.themeLabel.TabIndex = 0;
			this.themeLabel.Text = "Theme:";
			this.themeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// languageLabel
			// 
			this.languageLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.languageLabel.Location = new System.Drawing.Point(14, 61);
			this.languageLabel.Margin = new System.Windows.Forms.Padding(5);
			this.languageLabel.Name = "languageLabel";
			this.languageLabel.Size = new System.Drawing.Size(110, 42);
			this.languageLabel.TabIndex = 1;
			this.languageLabel.Text = "Language:";
			this.languageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// themeComboBox
			// 
			this.themeComboBox.DropDownHeight = 90;
			this.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.themeComboBox.Enabled = false;
			this.themeComboBox.Font = new System.Drawing.Font("Calibri", 10F);
			this.themeComboBox.FormattingEnabled = true;
			this.themeComboBox.IntegralHeight = false;
			this.themeComboBox.ItemHeight = 15;
			this.themeComboBox.Items.AddRange(new object[] {
            "Blue",
            "Dark",
            "Light"});
			this.themeComboBox.Location = new System.Drawing.Point(134, 22);
			this.themeComboBox.Margin = new System.Windows.Forms.Padding(5);
			this.themeComboBox.MaxDropDownItems = 3;
			this.themeComboBox.Name = "themeComboBox";
			this.themeComboBox.Size = new System.Drawing.Size(96, 23);
			this.themeComboBox.Sorted = true;
			this.themeComboBox.TabIndex = 0;
			// 
			// languageComboBox
			// 
			this.languageComboBox.DropDownHeight = 90;
			this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.languageComboBox.Enabled = false;
			this.languageComboBox.Font = new System.Drawing.Font("Calibri", 10F);
			this.languageComboBox.FormattingEnabled = true;
			this.languageComboBox.IntegralHeight = false;
			this.languageComboBox.ItemHeight = 15;
			this.languageComboBox.Items.AddRange(new object[] {
            "English"});
			this.languageComboBox.Location = new System.Drawing.Point(134, 74);
			this.languageComboBox.Margin = new System.Windows.Forms.Padding(5);
			this.languageComboBox.MaxDropDownItems = 3;
			this.languageComboBox.Name = "languageComboBox";
			this.languageComboBox.Size = new System.Drawing.Size(96, 23);
			this.languageComboBox.Sorted = true;
			this.languageComboBox.TabIndex = 1;
			// 
			// SettingsWindow
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(244, 261);
			this.Controls.Add(this.languageComboBox);
			this.Controls.Add(this.themeComboBox);
			this.Controls.Add(this.languageLabel);
			this.Controls.Add(this.themeLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SettingsWindow";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Settings";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label themeLabel;
		private System.Windows.Forms.Label languageLabel;
		private System.Windows.Forms.ComboBox themeComboBox;
		private System.Windows.Forms.ComboBox languageComboBox;
	}
}
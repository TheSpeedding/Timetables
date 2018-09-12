namespace Timetables.Application.Desktop
{
	partial class NewJourneyWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewJourneyWindow));
			this.sourceLabel = new System.Windows.Forms.Label();
			this.departureLabel = new System.Windows.Forms.Label();
			this.departureDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.searchButton = new System.Windows.Forms.Button();
			this.countLabel = new System.Windows.Forms.Label();
			this.countNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.targetLabel = new System.Windows.Forms.Label();
			this.transfersNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.transfersLabel = new System.Windows.Forms.Label();
			this.walkingSpeedLabel = new System.Windows.Forms.Label();
			this.slowButton = new System.Windows.Forms.RadioButton();
			this.mediumButton = new System.Windows.Forms.RadioButton();
			this.fastButton = new System.Windows.Forms.RadioButton();
			this.sourceTextBox = new System.Windows.Forms.TextBox();
			this.targetTextBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.transfersNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// sourceLabel
			// 
			this.sourceLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.sourceLabel.Location = new System.Drawing.Point(12, 9);
			this.sourceLabel.Name = "sourceLabel";
			this.sourceLabel.Size = new System.Drawing.Size(98, 42);
			this.sourceLabel.TabIndex = 1;
			this.sourceLabel.Text = "Source:";
			this.sourceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// departureLabel
			// 
			this.departureLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.departureLabel.Location = new System.Drawing.Point(12, 93);
			this.departureLabel.Name = "departureLabel";
			this.departureLabel.Size = new System.Drawing.Size(98, 42);
			this.departureLabel.TabIndex = 2;
			this.departureLabel.Text = "Departure:";
			this.departureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// departureDateTimePicker
			// 
			this.departureDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.departureDateTimePicker.CalendarFont = new System.Drawing.Font("Calibri", 10F);
			this.departureDateTimePicker.CustomFormat = "HH:mm     dd.MM.yyyy";
			this.departureDateTimePicker.Font = new System.Drawing.Font("Calibri", 10F);
			this.departureDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.departureDateTimePicker.Location = new System.Drawing.Point(116, 102);
			this.departureDateTimePicker.Name = "departureDateTimePicker";
			this.departureDateTimePicker.Size = new System.Drawing.Size(285, 24);
			this.departureDateTimePicker.TabIndex = 2;
			// 
			// searchButton
			// 
			this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.searchButton.Font = new System.Drawing.Font("Calibri", 12F);
			this.searchButton.Location = new System.Drawing.Point(267, 265);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(134, 31);
			this.searchButton.TabIndex = 8;
			this.searchButton.Text = "Search";
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
			// 
			// countLabel
			// 
			this.countLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.countLabel.Location = new System.Drawing.Point(12, 175);
			this.countLabel.Name = "countLabel";
			this.countLabel.Size = new System.Drawing.Size(98, 42);
			this.countLabel.TabIndex = 5;
			this.countLabel.Text = "Count:";
			this.countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// countNumericUpDown
			// 
			this.countNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.countNumericUpDown.Font = new System.Drawing.Font("Calibri", 10F);
			this.countNumericUpDown.Location = new System.Drawing.Point(116, 187);
			this.countNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.countNumericUpDown.Name = "countNumericUpDown";
			this.countNumericUpDown.Size = new System.Drawing.Size(285, 24);
			this.countNumericUpDown.TabIndex = 4;
			this.countNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// targetLabel
			// 
			this.targetLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.targetLabel.Location = new System.Drawing.Point(12, 52);
			this.targetLabel.Name = "targetLabel";
			this.targetLabel.Size = new System.Drawing.Size(98, 42);
			this.targetLabel.TabIndex = 8;
			this.targetLabel.Text = "Target:";
			this.targetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// transfersNumericUpDown
			// 
			this.transfersNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.transfersNumericUpDown.Font = new System.Drawing.Font("Calibri", 10F);
			this.transfersNumericUpDown.Location = new System.Drawing.Point(116, 145);
			this.transfersNumericUpDown.Name = "transfersNumericUpDown";
			this.transfersNumericUpDown.Size = new System.Drawing.Size(285, 24);
			this.transfersNumericUpDown.TabIndex = 3;
			this.transfersNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// transfersLabel
			// 
			this.transfersLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.transfersLabel.Location = new System.Drawing.Point(12, 133);
			this.transfersLabel.Name = "transfersLabel";
			this.transfersLabel.Size = new System.Drawing.Size(98, 42);
			this.transfersLabel.TabIndex = 9;
			this.transfersLabel.Text = "Transfers:";
			this.transfersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// walkingSpeedLabel
			// 
			this.walkingSpeedLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.walkingSpeedLabel.Location = new System.Drawing.Point(12, 217);
			this.walkingSpeedLabel.Name = "walkingSpeedLabel";
			this.walkingSpeedLabel.Size = new System.Drawing.Size(98, 42);
			this.walkingSpeedLabel.TabIndex = 10;
			this.walkingSpeedLabel.Text = "W. speed:";
			this.walkingSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// slowButton
			// 
			this.slowButton.Appearance = System.Windows.Forms.Appearance.Button;
			this.slowButton.FlatAppearance.BorderSize = 0;
			this.slowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.slowButton.Font = new System.Drawing.Font("Calibri", 12F);
			this.slowButton.Location = new System.Drawing.Point(116, 218);
			this.slowButton.Name = "slowButton";
			this.slowButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.slowButton.Size = new System.Drawing.Size(90, 42);
			this.slowButton.TabIndex = 5;
			this.slowButton.Text = "Slow";
			this.slowButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.slowButton.UseVisualStyleBackColor = true;
			// 
			// mediumButton
			// 
			this.mediumButton.Appearance = System.Windows.Forms.Appearance.Button;
			this.mediumButton.Checked = true;
			this.mediumButton.FlatAppearance.BorderSize = 0;
			this.mediumButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.mediumButton.Font = new System.Drawing.Font("Calibri", 12F);
			this.mediumButton.Location = new System.Drawing.Point(212, 218);
			this.mediumButton.Name = "mediumButton";
			this.mediumButton.Size = new System.Drawing.Size(90, 42);
			this.mediumButton.TabIndex = 6;
			this.mediumButton.TabStop = true;
			this.mediumButton.Text = "Medium";
			this.mediumButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.mediumButton.UseVisualStyleBackColor = true;
			// 
			// fastButton
			// 
			this.fastButton.Appearance = System.Windows.Forms.Appearance.Button;
			this.fastButton.FlatAppearance.BorderSize = 0;
			this.fastButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.fastButton.Font = new System.Drawing.Font("Calibri", 12F);
			this.fastButton.Location = new System.Drawing.Point(308, 218);
			this.fastButton.Name = "fastButton";
			this.fastButton.Size = new System.Drawing.Size(90, 42);
			this.fastButton.TabIndex = 7;
			this.fastButton.Text = "Fast";
			this.fastButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.fastButton.UseVisualStyleBackColor = true;
			// 
			// sourceTextBox
			// 
			this.sourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sourceTextBox.Font = new System.Drawing.Font("Calibri", 10F);
			this.sourceTextBox.Location = new System.Drawing.Point(116, 18);
			this.sourceTextBox.Name = "sourceTextBox";
			this.sourceTextBox.Size = new System.Drawing.Size(285, 24);
			this.sourceTextBox.TabIndex = 0;
			// 
			// targetTextBox
			// 
			this.targetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.targetTextBox.Font = new System.Drawing.Font("Calibri", 10F);
			this.targetTextBox.Location = new System.Drawing.Point(116, 61);
			this.targetTextBox.Name = "targetTextBox";
			this.targetTextBox.Size = new System.Drawing.Size(285, 24);
			this.targetTextBox.TabIndex = 1;
			// 
			// NewJourneyWindow
			// 
			this.AcceptButton = this.searchButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(413, 303);
			this.Controls.Add(this.targetTextBox);
			this.Controls.Add(this.sourceTextBox);
			this.Controls.Add(this.fastButton);
			this.Controls.Add(this.mediumButton);
			this.Controls.Add(this.slowButton);
			this.Controls.Add(this.walkingSpeedLabel);
			this.Controls.Add(this.transfersNumericUpDown);
			this.Controls.Add(this.transfersLabel);
			this.Controls.Add(this.targetLabel);
			this.Controls.Add(this.countNumericUpDown);
			this.Controls.Add(this.countLabel);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.departureDateTimePicker);
			this.Controls.Add(this.departureLabel);
			this.Controls.Add(this.sourceLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(276, 296);
			this.Name = "NewJourneyWindow";
			this.Text = "New journey";
			this.Load += new System.EventHandler(this.NewJourneyWindow_Load);
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.transfersNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label sourceLabel;
		private System.Windows.Forms.Label departureLabel;
		private System.Windows.Forms.DateTimePicker departureDateTimePicker;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.Label countLabel;
		private System.Windows.Forms.NumericUpDown countNumericUpDown;
		private System.Windows.Forms.Label targetLabel;
		private System.Windows.Forms.NumericUpDown transfersNumericUpDown;
		private System.Windows.Forms.Label transfersLabel;
		private System.Windows.Forms.Label walkingSpeedLabel;
		private System.Windows.Forms.RadioButton slowButton;
		private System.Windows.Forms.RadioButton mediumButton;
		private System.Windows.Forms.RadioButton fastButton;
		private System.Windows.Forms.TextBox sourceTextBox;
		private System.Windows.Forms.TextBox targetTextBox;
	}
}
namespace Timetables.Application.Desktop
{
	partial class NewStationInfoWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewStationInfoWindow));
			this.stationLabel = new System.Windows.Forms.Label();
			this.departureLabel = new System.Windows.Forms.Label();
			this.departureDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.searchButton = new System.Windows.Forms.Button();
			this.countLabel = new System.Windows.Forms.Label();
			this.countNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.lineLabel = new System.Windows.Forms.Label();
			this.lineComboBox = new System.Windows.Forms.ComboBox();
			this.stationTextBox = new System.Windows.Forms.TextBox();
			this.locationPictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locationPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// stationLabel
			// 
			this.stationLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.stationLabel.Location = new System.Drawing.Point(12, 9);
			this.stationLabel.Name = "stationLabel";
			this.stationLabel.Size = new System.Drawing.Size(98, 42);
			this.stationLabel.TabIndex = 1;
			this.stationLabel.Text = "Station:";
			this.stationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// departureLabel
			// 
			this.departureLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.departureLabel.Location = new System.Drawing.Point(12, 51);
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
			this.departureDateTimePicker.CustomFormat = "H:mm     dd.MM.yyyy";
			this.departureDateTimePicker.Font = new System.Drawing.Font("Calibri", 10F);
			this.departureDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.departureDateTimePicker.Location = new System.Drawing.Point(116, 60);
			this.departureDateTimePicker.Name = "departureDateTimePicker";
			this.departureDateTimePicker.Size = new System.Drawing.Size(146, 24);
			this.departureDateTimePicker.TabIndex = 3;
			// 
			// searchButton
			// 
			this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.searchButton.Font = new System.Drawing.Font("Calibri", 12F);
			this.searchButton.Location = new System.Drawing.Point(128, 179);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(134, 31);
			this.searchButton.TabIndex = 6;
			this.searchButton.Text = "Search";
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
			// 
			// countLabel
			// 
			this.countLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.countLabel.Location = new System.Drawing.Point(12, 135);
			this.countLabel.Name = "countLabel";
			this.countLabel.Size = new System.Drawing.Size(98, 42);
			this.countLabel.TabIndex = 50;
			this.countLabel.Text = "Count:";
			this.countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// countNumericUpDown
			// 
			this.countNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.countNumericUpDown.Font = new System.Drawing.Font("Calibri", 10F);
			this.countNumericUpDown.Location = new System.Drawing.Point(116, 147);
			this.countNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.countNumericUpDown.Name = "countNumericUpDown";
			this.countNumericUpDown.Size = new System.Drawing.Size(146, 24);
			this.countNumericUpDown.TabIndex = 6;
			this.countNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// lineLabel
			// 
			this.lineLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.lineLabel.Location = new System.Drawing.Point(12, 93);
			this.lineLabel.Name = "lineLabel";
			this.lineLabel.Size = new System.Drawing.Size(98, 42);
			this.lineLabel.TabIndex = 70;
			this.lineLabel.Text = "Line:";
			this.lineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lineComboBox
			// 
			this.lineComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.lineComboBox.Enabled = false;
			this.lineComboBox.Font = new System.Drawing.Font("Calibri", 10F);
			this.lineComboBox.FormattingEnabled = true;
			this.lineComboBox.Location = new System.Drawing.Point(116, 103);
			this.lineComboBox.Name = "lineComboBox";
			this.lineComboBox.Size = new System.Drawing.Size(146, 23);
			this.lineComboBox.Sorted = true;
			this.lineComboBox.TabIndex = 4;
			this.lineComboBox.DropDown += new System.EventHandler(this.lineComboBox_DropDown);
			// 
			// stationTextBox
			// 
			this.stationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.stationTextBox.Font = new System.Drawing.Font("Calibri", 10F);
			this.stationTextBox.Location = new System.Drawing.Point(116, 18);
			this.stationTextBox.Name = "stationTextBox";
			this.stationTextBox.Size = new System.Drawing.Size(116, 24);
			this.stationTextBox.TabIndex = 0;
			this.stationTextBox.TextChanged += new System.EventHandler(this.stationTextBox_TextChanged);
			// 
			// locationPictureBox
			// 
			this.locationPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.locationPictureBox.Image = global::Timetables.Application.Desktop.Properties.Resources.loc;
			this.locationPictureBox.InitialImage = global::Timetables.Application.Desktop.Properties.Resources.loc;
			this.locationPictureBox.Location = new System.Drawing.Point(238, 18);
			this.locationPictureBox.Name = "locationPictureBox";
			this.locationPictureBox.Size = new System.Drawing.Size(24, 24);
			this.locationPictureBox.TabIndex = 71;
			this.locationPictureBox.TabStop = false;
			this.locationPictureBox.Click += new System.EventHandler(this.locationPictureBox_Click);
			// 
			// NewStationInfoWindow
			// 
			this.AcceptButton = this.searchButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(274, 220);
			this.Controls.Add(this.locationPictureBox);
			this.Controls.Add(this.stationTextBox);
			this.Controls.Add(this.lineComboBox);
			this.Controls.Add(this.lineLabel);
			this.Controls.Add(this.countNumericUpDown);
			this.Controls.Add(this.countLabel);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.departureDateTimePicker);
			this.Controls.Add(this.departureLabel);
			this.Controls.Add(this.stationLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(276, 219);
			this.Name = "NewStationInfoWindow";
			this.Text = "New departure board";
			this.Load += new System.EventHandler(this.NewDepartureBoardWindow_Load);
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locationPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label stationLabel;
		private System.Windows.Forms.Label departureLabel;
		private System.Windows.Forms.DateTimePicker departureDateTimePicker;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.Label countLabel;
		private System.Windows.Forms.NumericUpDown countNumericUpDown;
		private System.Windows.Forms.Label lineLabel;
		private System.Windows.Forms.ComboBox lineComboBox;
		private System.Windows.Forms.TextBox stationTextBox;
		private System.Windows.Forms.PictureBox locationPictureBox;
	}
}
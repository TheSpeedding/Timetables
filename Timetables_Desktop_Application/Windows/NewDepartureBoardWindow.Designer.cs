namespace Timetables.Application.Desktop
{
	partial class NewDepartureBoardWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDepartureBoardWindow));
			this.stationComboBox = new System.Windows.Forms.ComboBox();
			this.stationLabel = new System.Windows.Forms.Label();
			this.departureLabel = new System.Windows.Forms.Label();
			this.departureDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.searchButton = new System.Windows.Forms.Button();
			this.countLabel = new System.Windows.Forms.Label();
			this.countNumericUpDown = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// stationComboBox
			// 
			this.stationComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.stationComboBox.Font = new System.Drawing.Font("Calibri", 10F);
			this.stationComboBox.FormattingEnabled = true;
			this.stationComboBox.Location = new System.Drawing.Point(116, 42);
			this.stationComboBox.Name = "stationComboBox";
			this.stationComboBox.Size = new System.Drawing.Size(34, 23);
			this.stationComboBox.Sorted = true;
			this.stationComboBox.TabIndex = 0;
			// 
			// stationLabel
			// 
			this.stationLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.stationLabel.Location = new System.Drawing.Point(12, 29);
			this.stationLabel.Name = "stationLabel";
			this.stationLabel.Size = new System.Drawing.Size(98, 42);
			this.stationLabel.TabIndex = 1;
			this.stationLabel.Text = "Station:";
			this.stationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// departureLabel
			// 
			this.departureLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.departureLabel.Location = new System.Drawing.Point(12, 71);
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
			this.departureDateTimePicker.CustomFormat = "H:mm     d.MM.yyyy";
			this.departureDateTimePicker.Font = new System.Drawing.Font("Calibri", 10F);
			this.departureDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.departureDateTimePicker.Location = new System.Drawing.Point(116, 80);
			this.departureDateTimePicker.Name = "departureDateTimePicker";
			this.departureDateTimePicker.Size = new System.Drawing.Size(34, 24);
			this.departureDateTimePicker.TabIndex = 3;
			// 
			// searchButton
			// 
			this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.searchButton.Font = new System.Drawing.Font("Calibri", 12F);
			this.searchButton.Location = new System.Drawing.Point(16, 158);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(134, 31);
			this.searchButton.TabIndex = 4;
			this.searchButton.Text = "Search";
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
			// 
			// countLabel
			// 
			this.countLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.countLabel.Location = new System.Drawing.Point(12, 113);
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
			this.countNumericUpDown.Location = new System.Drawing.Point(116, 125);
			this.countNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.countNumericUpDown.Name = "countNumericUpDown";
			this.countNumericUpDown.Size = new System.Drawing.Size(34, 24);
			this.countNumericUpDown.TabIndex = 6;
			this.countNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// NewDepartureBoardWindow
			// 
			this.AcceptButton = this.searchButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(162, 288);
			this.Controls.Add(this.countNumericUpDown);
			this.Controls.Add(this.countLabel);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.departureDateTimePicker);
			this.Controls.Add(this.departureLabel);
			this.Controls.Add(this.stationLabel);
			this.Controls.Add(this.stationComboBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "NewDepartureBoardWindow";
			this.Text = "New departure board";
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox stationComboBox;
		private System.Windows.Forms.Label stationLabel;
		private System.Windows.Forms.Label departureLabel;
		private System.Windows.Forms.DateTimePicker departureDateTimePicker;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.Label countLabel;
		private System.Windows.Forms.NumericUpDown countNumericUpDown;
	}
}
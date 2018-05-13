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
			this.sourceComboBox = new System.Windows.Forms.ComboBox();
			this.sourceLabel = new System.Windows.Forms.Label();
			this.departureLabel = new System.Windows.Forms.Label();
			this.departureDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.searchButton = new System.Windows.Forms.Button();
			this.countLabel = new System.Windows.Forms.Label();
			this.countNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.targetLabel = new System.Windows.Forms.Label();
			this.targetComboBox = new System.Windows.Forms.ComboBox();
			this.transfersNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.transfersLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.transfersNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// sourceComboBox
			// 
			this.sourceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sourceComboBox.Font = new System.Drawing.Font("Calibri", 10F);
			this.sourceComboBox.FormattingEnabled = true;
			this.sourceComboBox.Location = new System.Drawing.Point(116, 22);
			this.sourceComboBox.Name = "sourceComboBox";
			this.sourceComboBox.Size = new System.Drawing.Size(146, 23);
			this.sourceComboBox.Sorted = true;
			this.sourceComboBox.TabIndex = 0;
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
			this.departureDateTimePicker.CustomFormat = "H:mm     dd.MM.yyyy";
			this.departureDateTimePicker.Font = new System.Drawing.Font("Calibri", 10F);
			this.departureDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.departureDateTimePicker.Location = new System.Drawing.Point(116, 102);
			this.departureDateTimePicker.Name = "departureDateTimePicker";
			this.departureDateTimePicker.Size = new System.Drawing.Size(146, 24);
			this.departureDateTimePicker.TabIndex = 2;
			// 
			// searchButton
			// 
			this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.searchButton.Font = new System.Drawing.Font("Calibri", 12F);
			this.searchButton.Location = new System.Drawing.Point(128, 217);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(134, 31);
			this.searchButton.TabIndex = 5;
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
			this.countNumericUpDown.Size = new System.Drawing.Size(146, 24);
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
			// targetComboBox
			// 
			this.targetComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.targetComboBox.Font = new System.Drawing.Font("Calibri", 10F);
			this.targetComboBox.FormattingEnabled = true;
			this.targetComboBox.Location = new System.Drawing.Point(116, 64);
			this.targetComboBox.Name = "targetComboBox";
			this.targetComboBox.Size = new System.Drawing.Size(146, 23);
			this.targetComboBox.Sorted = true;
			this.targetComboBox.TabIndex = 1;
			// 
			// transfersNumericUpDown
			// 
			this.transfersNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.transfersNumericUpDown.Font = new System.Drawing.Font("Calibri", 10F);
			this.transfersNumericUpDown.Location = new System.Drawing.Point(116, 145);
			this.transfersNumericUpDown.Name = "transfersNumericUpDown";
			this.transfersNumericUpDown.Size = new System.Drawing.Size(146, 24);
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
			// NewJourneyWindow
			// 
			this.AcceptButton = this.searchButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(274, 257);
			this.Controls.Add(this.transfersNumericUpDown);
			this.Controls.Add(this.transfersLabel);
			this.Controls.Add(this.targetLabel);
			this.Controls.Add(this.targetComboBox);
			this.Controls.Add(this.countNumericUpDown);
			this.Controls.Add(this.countLabel);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.departureDateTimePicker);
			this.Controls.Add(this.departureLabel);
			this.Controls.Add(this.sourceLabel);
			this.Controls.Add(this.sourceComboBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(276, 296);
			this.Name = "NewJourneyWindow";
			this.Text = "New journey";
			((System.ComponentModel.ISupportInitialize)(this.countNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.transfersNumericUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox sourceComboBox;
		private System.Windows.Forms.Label sourceLabel;
		private System.Windows.Forms.Label departureLabel;
		private System.Windows.Forms.DateTimePicker departureDateTimePicker;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.Label countLabel;
		private System.Windows.Forms.NumericUpDown countNumericUpDown;
		private System.Windows.Forms.Label targetLabel;
		private System.Windows.Forms.ComboBox targetComboBox;
		private System.Windows.Forms.NumericUpDown transfersNumericUpDown;
		private System.Windows.Forms.Label transfersLabel;
	}
}
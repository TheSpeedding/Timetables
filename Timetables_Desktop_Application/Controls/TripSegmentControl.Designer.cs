namespace Timetables.Application.Desktop
{
	partial class TripSegmentControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.meanOfTransportPictureBox = new System.Windows.Forms.PictureBox();
			this.lineDescriptionLabel = new System.Windows.Forms.Label();
			this.lineColorPictureBox = new System.Windows.Forms.PictureBox();
			this.departureLabel = new System.Windows.Forms.Label();
			this.intStopsLabel = new System.Windows.Forms.Label();
			this.lineDescriptionTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.outdatedTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.arrivalLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.meanOfTransportPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lineColorPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// meanOfTransportPictureBox
			// 
			this.meanOfTransportPictureBox.Location = new System.Drawing.Point(8, 8);
			this.meanOfTransportPictureBox.Name = "meanOfTransportPictureBox";
			this.meanOfTransportPictureBox.Size = new System.Drawing.Size(24, 24);
			this.meanOfTransportPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.meanOfTransportPictureBox.TabIndex = 0;
			this.meanOfTransportPictureBox.TabStop = false;
			// 
			// lineDescriptionLabel
			// 
			this.lineDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lineDescriptionLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.lineDescriptionLabel.Location = new System.Drawing.Point(49, 8);
			this.lineDescriptionLabel.Name = "lineDescriptionLabel";
			this.lineDescriptionLabel.Size = new System.Drawing.Size(346, 24);
			this.lineDescriptionLabel.TabIndex = 1;
			this.lineDescriptionLabel.Text = "Line Description";
			this.lineDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lineColorPictureBox
			// 
			this.lineColorPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lineColorPictureBox.Location = new System.Drawing.Point(1, 1);
			this.lineColorPictureBox.Margin = new System.Windows.Forms.Padding(1);
			this.lineColorPictureBox.Name = "lineColorPictureBox";
			this.lineColorPictureBox.Size = new System.Drawing.Size(406, 40);
			this.lineColorPictureBox.TabIndex = 2;
			this.lineColorPictureBox.TabStop = false;
			// 
			// departureLabel
			// 
			this.departureLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.departureLabel.AutoSize = true;
			this.departureLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.departureLabel.Location = new System.Drawing.Point(4, 42);
			this.departureLabel.Name = "departureLabel";
			this.departureLabel.Size = new System.Drawing.Size(114, 19);
			this.departureLabel.TabIndex = 3;
			this.departureLabel.Text = "Departure data";
			this.departureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// intStopsLabel
			// 
			this.intStopsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.intStopsLabel.AutoSize = true;
			this.intStopsLabel.Font = new System.Drawing.Font("Calibri", 12F);
			this.intStopsLabel.Location = new System.Drawing.Point(31, 66);
			this.intStopsLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
			this.intStopsLabel.Name = "intStopsLabel";
			this.intStopsLabel.Size = new System.Drawing.Size(164, 19);
			this.intStopsLabel.TabIndex = 4;
			this.intStopsLabel.Text = "Intermediate stops data";
			// 
			// lineDescriptionTooltip
			// 
			this.lineDescriptionTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.lineDescriptionTooltip.ToolTipTitle = "Line name.";
			// 
			// outdatedTooltip
			// 
			this.outdatedTooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
			this.outdatedTooltip.ToolTipTitle = "Outdated data.";
			// 
			// arrivalLabel
			// 
			this.arrivalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.arrivalLabel.AutoSize = true;
			this.arrivalLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.arrivalLabel.Location = new System.Drawing.Point(4, 66);
			this.arrivalLabel.Name = "arrivalLabel";
			this.arrivalLabel.Size = new System.Drawing.Size(90, 19);
			this.arrivalLabel.TabIndex = 6;
			this.arrivalLabel.Text = "Arrival data";
			this.arrivalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TripSegmentControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.arrivalLabel);
			this.Controls.Add(this.intStopsLabel);
			this.Controls.Add(this.departureLabel);
			this.Controls.Add(this.lineDescriptionLabel);
			this.Controls.Add(this.meanOfTransportPictureBox);
			this.Controls.Add(this.lineColorPictureBox);
			this.MinimumSize = new System.Drawing.Size(408, 88);
			this.Name = "TripSegmentControl";
			this.Size = new System.Drawing.Size(408, 95);
			((System.ComponentModel.ISupportInitialize)(this.meanOfTransportPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lineColorPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox lineColorPictureBox;
		private System.Windows.Forms.PictureBox meanOfTransportPictureBox;
		private System.Windows.Forms.Label lineDescriptionLabel;
		private System.Windows.Forms.Label departureLabel;
		private System.Windows.Forms.Label intStopsLabel;
		private System.Windows.Forms.ToolTip lineDescriptionTooltip;
		private System.Windows.Forms.ToolTip outdatedTooltip;
		private System.Windows.Forms.Label arrivalLabel;
	}
}

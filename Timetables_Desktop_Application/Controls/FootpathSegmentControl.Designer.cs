namespace Timetables.Application.Desktop
{
	partial class FootpathSegmentControl
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
			this.meanOfTransportPictureBox = new System.Windows.Forms.PictureBox();
			this.durationLabel = new System.Windows.Forms.Label();
			this.lineColorPictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.meanOfTransportPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lineColorPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// meanOfTransportPictureBox
			// 
			this.meanOfTransportPictureBox.Image = global::Timetables.Application.Desktop.Properties.Resources.noun_19727_cc;
			this.meanOfTransportPictureBox.Location = new System.Drawing.Point(8, 8);
			this.meanOfTransportPictureBox.Name = "meanOfTransportPictureBox";
			this.meanOfTransportPictureBox.Size = new System.Drawing.Size(24, 24);
			this.meanOfTransportPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.meanOfTransportPictureBox.TabIndex = 0;
			this.meanOfTransportPictureBox.TabStop = false;
			// 
			// durationLabel
			// 
			this.durationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.durationLabel.Font = new System.Drawing.Font("Calibri", 14F);
			this.durationLabel.Location = new System.Drawing.Point(49, 8);
			this.durationLabel.Name = "durationLabel";
			this.durationLabel.Size = new System.Drawing.Size(346, 24);
			this.durationLabel.TabIndex = 1;
			this.durationLabel.Text = "Transfer - ";
			this.durationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lineColorPictureBox
			// 
			this.lineColorPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lineColorPictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
			this.lineColorPictureBox.Location = new System.Drawing.Point(1, 1);
			this.lineColorPictureBox.Margin = new System.Windows.Forms.Padding(1);
			this.lineColorPictureBox.Name = "lineColorPictureBox";
			this.lineColorPictureBox.Size = new System.Drawing.Size(406, 40);
			this.lineColorPictureBox.TabIndex = 2;
			this.lineColorPictureBox.TabStop = false;
			// 
			// FootpathSegmentControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.durationLabel);
			this.Controls.Add(this.meanOfTransportPictureBox);
			this.Controls.Add(this.lineColorPictureBox);
			this.MinimumSize = new System.Drawing.Size(408, 42);
			this.Name = "FootpathSegmentControl";
			this.Size = new System.Drawing.Size(408, 42);
			((System.ComponentModel.ISupportInitialize)(this.meanOfTransportPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lineColorPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox lineColorPictureBox;
		private System.Windows.Forms.PictureBox meanOfTransportPictureBox;
		private System.Windows.Forms.Label durationLabel;
	}
}

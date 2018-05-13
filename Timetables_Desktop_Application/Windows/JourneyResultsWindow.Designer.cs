namespace Timetables.Application.Desktop
{
	partial class JourneyResultsWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JourneyResultsWindow));
			this.resultsPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// resultsPanel
			// 
			this.resultsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.resultsPanel.AutoScroll = true;
			this.resultsPanel.Location = new System.Drawing.Point(12, 12);
			this.resultsPanel.Name = "resultsPanel";
			this.resultsPanel.Size = new System.Drawing.Size(473, 137);
			this.resultsPanel.TabIndex = 1;
			// 
			// JourneyResultsWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(497, 161);
			this.Controls.Add(this.resultsPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "JourneyResultsWindow";
			this.Text = "JourneyResultsWindow";
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel resultsPanel;
	}
}
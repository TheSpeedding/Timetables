namespace Timetables.Application.Desktop
{
	partial class InitLoadingWindow
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitLoadingWindow));
			this.loadingProgressBar = new System.Windows.Forms.ProgressBar();
			this.loadingLabel = new System.Windows.Forms.Label();
			this.topBarTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// loadingProgressBar
			// 
			this.loadingProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.loadingProgressBar.Location = new System.Drawing.Point(15, 12);
			this.loadingProgressBar.Name = "loadingProgressBar";
			this.loadingProgressBar.Size = new System.Drawing.Size(400, 31);
			this.loadingProgressBar.TabIndex = 1;
			// 
			// loadingLabel
			// 
			this.loadingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.loadingLabel.Font = new System.Drawing.Font("Calibri", 9F);
			this.loadingLabel.Location = new System.Drawing.Point(11, 46);
			this.loadingLabel.Name = "loadingLabel";
			this.loadingLabel.Size = new System.Drawing.Size(400, 16);
			this.loadingLabel.TabIndex = 2;
			this.loadingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// topBarTimer
			// 
			this.topBarTimer.Enabled = true;
			this.topBarTimer.Interval = 500;
			this.topBarTimer.Tick += new System.EventHandler(this.topBarTimer_Tick);
			// 
			// InitLoadingWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(424, 71);
			this.ControlBox = false;
			this.Controls.Add(this.loadingLabel);
			this.Controls.Add(this.loadingProgressBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(440, 110);
			this.MinimumSize = new System.Drawing.Size(440, 110);
			this.Name = "InitLoadingWindow";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Loading ...";
			this.Shown += new System.EventHandler(this.InitLoadingWindow_Shown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ProgressBar loadingProgressBar;
		private System.Windows.Forms.Label loadingLabel;
		private System.Windows.Forms.Timer topBarTimer;
	}
}
namespace Timetables.Application.Desktop
{
	partial class AbstractInfoWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AbstractInfoWindow));
			this.resultsWebBrowser = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// resultsWebBrowser
			// 
			this.resultsWebBrowser.AllowNavigation = false;
			this.resultsWebBrowser.AllowWebBrowserDrop = false;
			this.resultsWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultsWebBrowser.IsWebBrowserContextMenuEnabled = false;
			this.resultsWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.resultsWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.resultsWebBrowser.Name = "resultsWebBrowser";
			this.resultsWebBrowser.ScriptErrorsSuppressed = true;
			this.resultsWebBrowser.Size = new System.Drawing.Size(497, 161);
			this.resultsWebBrowser.TabIndex = 1;
			// 
			// AbstractInfoWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(497, 161);
			this.Controls.Add(this.resultsWebBrowser);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AbstractInfoWindow";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser resultsWebBrowser;
	}
}
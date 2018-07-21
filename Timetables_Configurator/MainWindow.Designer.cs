namespace Timetables.Configurator
{
	partial class MainWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.serverRadioButton = new System.Windows.Forms.RadioButton();
			this.clientRadioButton = new System.Windows.Forms.RadioButton();
			this.clientUserControl = new Timetables.Configurator.ClientUserControl();
			this.SuspendLayout();
			// 
			// serverRadioButton
			// 
			this.serverRadioButton.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
			this.serverRadioButton.Location = new System.Drawing.Point(12, 12);
			this.serverRadioButton.Name = "serverRadioButton";
			this.serverRadioButton.Size = new System.Drawing.Size(115, 43);
			this.serverRadioButton.TabIndex = 0;
			this.serverRadioButton.Text = "Server configuration";
			this.serverRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.serverRadioButton.UseVisualStyleBackColor = true;
			this.serverRadioButton.CheckedChanged += new System.EventHandler(this.serverRadioButton_CheckedChanged);
			// 
			// clientRadioButton
			// 
			this.clientRadioButton.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
			this.clientRadioButton.Location = new System.Drawing.Point(195, 12);
			this.clientRadioButton.Name = "clientRadioButton";
			this.clientRadioButton.Size = new System.Drawing.Size(115, 43);
			this.clientRadioButton.TabIndex = 1;
			this.clientRadioButton.Text = "Client configuration";
			this.clientRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.clientRadioButton.UseVisualStyleBackColor = true;
			this.clientRadioButton.CheckedChanged += new System.EventHandler(this.clientRadioButton_CheckedChanged);
			// 
			// clientUserControl
			// 
			this.clientUserControl.Location = new System.Drawing.Point(12, 62);
			this.clientUserControl.Name = "clientUserControl";
			this.clientUserControl.Size = new System.Drawing.Size(300, 376);
			this.clientUserControl.TabIndex = 2;
			this.clientUserControl.Visible = false;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(322, 450);
			this.Controls.Add(this.clientUserControl);
			this.Controls.Add(this.clientRadioButton);
			this.Controls.Add(this.serverRadioButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(338, 489);
			this.MinimumSize = new System.Drawing.Size(338, 489);
			this.Name = "MainWindow";
			this.Text = "Timetables - Configurator";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RadioButton serverRadioButton;
		private System.Windows.Forms.RadioButton clientRadioButton;
		private ClientUserControl clientUserControl;
	}
}


namespace Timetables.Configurator
{
	partial class ServerUserControl
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
			this.loadFileButton = new System.Windows.Forms.Button();
			this.settingsOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.routerPortTextBox = new System.Windows.Forms.TextBox();
			this.routerPortLabel = new System.Windows.Forms.Label();
			this.dbPortLabel = new System.Windows.Forms.Label();
			this.dbPortTextBox = new System.Windows.Forms.TextBox();
			this.cutwslLabel = new System.Windows.Forms.Label();
			this.cutwslTextBox = new System.Windows.Forms.TextBox();
			this.cutstLabel = new System.Windows.Forms.Label();
			this.cutstTextBox = new System.Windows.Forms.TextBox();
			this.cutwdlLabel = new System.Windows.Forms.Label();
			this.cutwdlTextBox = new System.Windows.Forms.TextBox();
			this.avgSpeedLabel = new System.Windows.Forms.Label();
			this.avgSpeedTextBox = new System.Windows.Forms.TextBox();
			this.maxDurLabel = new System.Windows.Forms.Label();
			this.maxDurTextBox = new System.Windows.Forms.TextBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.settingsSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.sourcesLabel = new System.Windows.Forms.Label();
			this.sourcesButton = new System.Windows.Forms.Button();
			this.dataPortLabel = new System.Windows.Forms.Label();
			this.dataPortTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// loadFileButton
			// 
			this.loadFileButton.Location = new System.Drawing.Point(3, 3);
			this.loadFileButton.Name = "loadFileButton";
			this.loadFileButton.Size = new System.Drawing.Size(294, 40);
			this.loadFileButton.TabIndex = 0;
			this.loadFileButton.Text = "Open settings file...";
			this.loadFileButton.UseVisualStyleBackColor = true;
			this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
			// 
			// settingsOpenFileDialog
			// 
			this.settingsOpenFileDialog.FileName = "settings.xml";
			// 
			// routerPortTextBox
			// 
			this.routerPortTextBox.Location = new System.Drawing.Point(131, 56);
			this.routerPortTextBox.Name = "routerPortTextBox";
			this.routerPortTextBox.Size = new System.Drawing.Size(166, 20);
			this.routerPortTextBox.TabIndex = 1;
			// 
			// routerPortLabel
			// 
			this.routerPortLabel.Location = new System.Drawing.Point(3, 56);
			this.routerPortLabel.Name = "routerPortLabel";
			this.routerPortLabel.Size = new System.Drawing.Size(122, 20);
			this.routerPortLabel.TabIndex = 2;
			this.routerPortLabel.Text = "Router port:";
			this.routerPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dbPortLabel
			// 
			this.dbPortLabel.Location = new System.Drawing.Point(3, 82);
			this.dbPortLabel.Name = "dbPortLabel";
			this.dbPortLabel.Size = new System.Drawing.Size(122, 20);
			this.dbPortLabel.TabIndex = 4;
			this.dbPortLabel.Text = "Departure board port:";
			this.dbPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dbPortTextBox
			// 
			this.dbPortTextBox.Location = new System.Drawing.Point(131, 82);
			this.dbPortTextBox.Name = "dbPortTextBox";
			this.dbPortTextBox.Size = new System.Drawing.Size(166, 20);
			this.dbPortTextBox.TabIndex = 2;
			// 
			// cutwslLabel
			// 
			this.cutwslLabel.Location = new System.Drawing.Point(3, 134);
			this.cutwslLabel.Name = "cutwslLabel";
			this.cutwslLabel.Size = new System.Drawing.Size(122, 20);
			this.cutwslLabel.TabIndex = 6;
			this.cutwslLabel.Text = "CUTWSL:";
			this.cutwslLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cutwslTextBox
			// 
			this.cutwslTextBox.Location = new System.Drawing.Point(131, 134);
			this.cutwslTextBox.Name = "cutwslTextBox";
			this.cutwslTextBox.Size = new System.Drawing.Size(166, 20);
			this.cutwslTextBox.TabIndex = 5;
			// 
			// cutstLabel
			// 
			this.cutstLabel.Location = new System.Drawing.Point(3, 186);
			this.cutstLabel.Name = "cutstLabel";
			this.cutstLabel.Size = new System.Drawing.Size(122, 20);
			this.cutstLabel.TabIndex = 12;
			this.cutstLabel.Text = "CUTST:";
			this.cutstLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cutstTextBox
			// 
			this.cutstTextBox.Location = new System.Drawing.Point(131, 186);
			this.cutstTextBox.Name = "cutstTextBox";
			this.cutstTextBox.Size = new System.Drawing.Size(166, 20);
			this.cutstTextBox.TabIndex = 11;
			// 
			// cutwdlLabel
			// 
			this.cutwdlLabel.Location = new System.Drawing.Point(3, 160);
			this.cutwdlLabel.Name = "cutwdlLabel";
			this.cutwdlLabel.Size = new System.Drawing.Size(122, 20);
			this.cutwdlLabel.TabIndex = 10;
			this.cutwdlLabel.Text = "CUTWDL:";
			this.cutwdlLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cutwdlTextBox
			// 
			this.cutwdlTextBox.Location = new System.Drawing.Point(131, 160);
			this.cutwdlTextBox.Name = "cutwdlTextBox";
			this.cutwdlTextBox.Size = new System.Drawing.Size(166, 20);
			this.cutwdlTextBox.TabIndex = 9;
			// 
			// avgSpeedLabel
			// 
			this.avgSpeedLabel.Location = new System.Drawing.Point(3, 238);
			this.avgSpeedLabel.Name = "avgSpeedLabel";
			this.avgSpeedLabel.Size = new System.Drawing.Size(122, 20);
			this.avgSpeedLabel.TabIndex = 16;
			this.avgSpeedLabel.Text = "Avg. walking speed:";
			this.avgSpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// avgSpeedTextBox
			// 
			this.avgSpeedTextBox.Location = new System.Drawing.Point(131, 238);
			this.avgSpeedTextBox.Name = "avgSpeedTextBox";
			this.avgSpeedTextBox.Size = new System.Drawing.Size(166, 20);
			this.avgSpeedTextBox.TabIndex = 15;
			// 
			// maxDurLabel
			// 
			this.maxDurLabel.Location = new System.Drawing.Point(3, 212);
			this.maxDurLabel.Name = "maxDurLabel";
			this.maxDurLabel.Size = new System.Drawing.Size(122, 20);
			this.maxDurLabel.TabIndex = 14;
			this.maxDurLabel.Text = "Transfer max. duration:";
			this.maxDurLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// maxDurTextBox
			// 
			this.maxDurTextBox.Location = new System.Drawing.Point(131, 212);
			this.maxDurTextBox.Name = "maxDurTextBox";
			this.maxDurTextBox.Size = new System.Drawing.Size(166, 20);
			this.maxDurTextBox.TabIndex = 13;
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(156, 291);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(138, 40);
			this.saveButton.TabIndex = 19;
			this.saveButton.Text = "Save as...";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// clearButton
			// 
			this.clearButton.Location = new System.Drawing.Point(6, 291);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(138, 40);
			this.clearButton.TabIndex = 20;
			this.clearButton.Text = "Clear";
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			// 
			// settingsSaveFileDialog
			// 
			this.settingsSaveFileDialog.AddExtension = false;
			this.settingsSaveFileDialog.DefaultExt = "xml";
			this.settingsSaveFileDialog.FileName = "settings.xml";
			// 
			// sourcesLabel
			// 
			this.sourcesLabel.Location = new System.Drawing.Point(3, 265);
			this.sourcesLabel.Name = "sourcesLabel";
			this.sourcesLabel.Size = new System.Drawing.Size(122, 20);
			this.sourcesLabel.TabIndex = 21;
			this.sourcesLabel.Text = "Data sources:";
			this.sourcesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// sourcesButton
			// 
			this.sourcesButton.Location = new System.Drawing.Point(131, 264);
			this.sourcesButton.Name = "sourcesButton";
			this.sourcesButton.Size = new System.Drawing.Size(166, 21);
			this.sourcesButton.TabIndex = 22;
			this.sourcesButton.Text = "Select...";
			this.sourcesButton.UseVisualStyleBackColor = true;
			this.sourcesButton.Click += new System.EventHandler(this.sourcesButton_Click);
			// 
			// dataPortLabel
			// 
			this.dataPortLabel.Location = new System.Drawing.Point(3, 108);
			this.dataPortLabel.Name = "dataPortLabel";
			this.dataPortLabel.Size = new System.Drawing.Size(122, 20);
			this.dataPortLabel.TabIndex = 24;
			this.dataPortLabel.Text = "Basic data port:";
			this.dataPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dataPortTextBox
			// 
			this.dataPortTextBox.Location = new System.Drawing.Point(131, 108);
			this.dataPortTextBox.Name = "dataPortTextBox";
			this.dataPortTextBox.Size = new System.Drawing.Size(166, 20);
			this.dataPortTextBox.TabIndex = 3;
			// 
			// ServerUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataPortLabel);
			this.Controls.Add(this.dataPortTextBox);
			this.Controls.Add(this.sourcesButton);
			this.Controls.Add(this.sourcesLabel);
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.avgSpeedLabel);
			this.Controls.Add(this.avgSpeedTextBox);
			this.Controls.Add(this.maxDurLabel);
			this.Controls.Add(this.maxDurTextBox);
			this.Controls.Add(this.cutstLabel);
			this.Controls.Add(this.cutstTextBox);
			this.Controls.Add(this.cutwdlLabel);
			this.Controls.Add(this.cutwdlTextBox);
			this.Controls.Add(this.cutwslLabel);
			this.Controls.Add(this.cutwslTextBox);
			this.Controls.Add(this.dbPortLabel);
			this.Controls.Add(this.dbPortTextBox);
			this.Controls.Add(this.routerPortLabel);
			this.Controls.Add(this.routerPortTextBox);
			this.Controls.Add(this.loadFileButton);
			this.Name = "ServerUserControl";
			this.Size = new System.Drawing.Size(300, 337);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button loadFileButton;
		private System.Windows.Forms.OpenFileDialog settingsOpenFileDialog;
		private System.Windows.Forms.TextBox routerPortTextBox;
		private System.Windows.Forms.Label routerPortLabel;
		private System.Windows.Forms.Label dbPortLabel;
		private System.Windows.Forms.TextBox dbPortTextBox;
		private System.Windows.Forms.Label cutwslLabel;
		private System.Windows.Forms.TextBox cutwslTextBox;
		private System.Windows.Forms.Label cutstLabel;
		private System.Windows.Forms.TextBox cutstTextBox;
		private System.Windows.Forms.Label cutwdlLabel;
		private System.Windows.Forms.TextBox cutwdlTextBox;
		private System.Windows.Forms.Label avgSpeedLabel;
		private System.Windows.Forms.TextBox avgSpeedTextBox;
		private System.Windows.Forms.Label maxDurLabel;
		private System.Windows.Forms.TextBox maxDurTextBox;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.SaveFileDialog settingsSaveFileDialog;
		private System.Windows.Forms.Label sourcesLabel;
		private System.Windows.Forms.Button sourcesButton;
		private System.Windows.Forms.Label dataPortLabel;
		private System.Windows.Forms.TextBox dataPortTextBox;
	}
}

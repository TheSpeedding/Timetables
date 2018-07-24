namespace Timetables.Configurator
{
	partial class ClientUserControl
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
			this.languageTextBox = new System.Windows.Forms.TextBox();
			this.languageLabel = new System.Windows.Forms.Label();
			this.extraEventsLabel = new System.Windows.Forms.Label();
			this.extraEventsTextBox = new System.Windows.Forms.TextBox();
			this.lockoutsLabel = new System.Windows.Forms.Label();
			this.lockoutsTextBox = new System.Windows.Forms.TextBox();
			this.offlineRadioButton = new System.Windows.Forms.RadioButton();
			this.onlineRadioButton = new System.Windows.Forms.RadioButton();
			this.fullDataLabel = new System.Windows.Forms.Label();
			this.fullDataTextBox = new System.Windows.Forms.TextBox();
			this.basicDataLabel = new System.Windows.Forms.Label();
			this.basicDataTextBox = new System.Windows.Forms.TextBox();
			this.portRouterLabel = new System.Windows.Forms.Label();
			this.portRouterTextBox = new System.Windows.Forms.TextBox();
			this.serverIpLabel = new System.Windows.Forms.Label();
			this.serverIpTextBox = new System.Windows.Forms.TextBox();
			this.portDepBoardLabel = new System.Windows.Forms.Label();
			this.portDepBoardTextBox = new System.Windows.Forms.TextBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.clearButton = new System.Windows.Forms.Button();
			this.settingsSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
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
			this.settingsOpenFileDialog.FileName = ".settings";
			// 
			// languageTextBox
			// 
			this.languageTextBox.Location = new System.Drawing.Point(128, 89);
			this.languageTextBox.Name = "languageTextBox";
			this.languageTextBox.Size = new System.Drawing.Size(166, 20);
			this.languageTextBox.TabIndex = 1;
			this.languageTextBox.Text = "English";
			// 
			// languageLabel
			// 
			this.languageLabel.Location = new System.Drawing.Point(0, 89);
			this.languageLabel.Name = "languageLabel";
			this.languageLabel.Size = new System.Drawing.Size(122, 20);
			this.languageLabel.TabIndex = 2;
			this.languageLabel.Text = "Language:";
			this.languageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// extraEventsLabel
			// 
			this.extraEventsLabel.Location = new System.Drawing.Point(0, 115);
			this.extraEventsLabel.Name = "extraEventsLabel";
			this.extraEventsLabel.Size = new System.Drawing.Size(122, 20);
			this.extraEventsLabel.TabIndex = 4;
			this.extraEventsLabel.Text = "Extra events URI:";
			this.extraEventsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// extraEventsTextBox
			// 
			this.extraEventsTextBox.Location = new System.Drawing.Point(128, 115);
			this.extraEventsTextBox.Name = "extraEventsTextBox";
			this.extraEventsTextBox.Size = new System.Drawing.Size(166, 20);
			this.extraEventsTextBox.TabIndex = 3;
			// 
			// lockoutsLabel
			// 
			this.lockoutsLabel.Location = new System.Drawing.Point(0, 141);
			this.lockoutsLabel.Name = "lockoutsLabel";
			this.lockoutsLabel.Size = new System.Drawing.Size(122, 20);
			this.lockoutsLabel.TabIndex = 6;
			this.lockoutsLabel.Text = "Lockouts URI:";
			this.lockoutsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lockoutsTextBox
			// 
			this.lockoutsTextBox.Location = new System.Drawing.Point(128, 141);
			this.lockoutsTextBox.Name = "lockoutsTextBox";
			this.lockoutsTextBox.Size = new System.Drawing.Size(166, 20);
			this.lockoutsTextBox.TabIndex = 5;
			// 
			// offlineRadioButton
			// 
			this.offlineRadioButton.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
			this.offlineRadioButton.Checked = true;
			this.offlineRadioButton.Location = new System.Drawing.Point(3, 49);
			this.offlineRadioButton.Name = "offlineRadioButton";
			this.offlineRadioButton.Size = new System.Drawing.Size(141, 37);
			this.offlineRadioButton.TabIndex = 7;
			this.offlineRadioButton.TabStop = true;
			this.offlineRadioButton.Text = "Offline mode";
			this.offlineRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.offlineRadioButton.UseVisualStyleBackColor = true;
			this.offlineRadioButton.CheckedChanged += new System.EventHandler(this.offlineRadioButton_CheckedChanged);
			// 
			// onlineRadioButton
			// 
			this.onlineRadioButton.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
			this.onlineRadioButton.Location = new System.Drawing.Point(156, 49);
			this.onlineRadioButton.Name = "onlineRadioButton";
			this.onlineRadioButton.Size = new System.Drawing.Size(141, 37);
			this.onlineRadioButton.TabIndex = 8;
			this.onlineRadioButton.Text = "Online mode";
			this.onlineRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.onlineRadioButton.UseVisualStyleBackColor = true;
			this.onlineRadioButton.CheckedChanged += new System.EventHandler(this.offlineRadioButton_CheckedChanged);
			// 
			// fullDataLabel
			// 
			this.fullDataLabel.Location = new System.Drawing.Point(0, 193);
			this.fullDataLabel.Name = "fullDataLabel";
			this.fullDataLabel.Size = new System.Drawing.Size(122, 20);
			this.fullDataLabel.TabIndex = 12;
			this.fullDataLabel.Text = "Full data URI:";
			this.fullDataLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// fullDataTextBox
			// 
			this.fullDataTextBox.Location = new System.Drawing.Point(128, 193);
			this.fullDataTextBox.Name = "fullDataTextBox";
			this.fullDataTextBox.Size = new System.Drawing.Size(166, 20);
			this.fullDataTextBox.TabIndex = 11;
			// 
			// basicDataLabel
			// 
			this.basicDataLabel.Location = new System.Drawing.Point(0, 167);
			this.basicDataLabel.Name = "basicDataLabel";
			this.basicDataLabel.Size = new System.Drawing.Size(122, 20);
			this.basicDataLabel.TabIndex = 10;
			this.basicDataLabel.Text = "Basic data URI:";
			this.basicDataLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// basicDataTextBox
			// 
			this.basicDataTextBox.Enabled = false;
			this.basicDataTextBox.Location = new System.Drawing.Point(128, 167);
			this.basicDataTextBox.Name = "basicDataTextBox";
			this.basicDataTextBox.Size = new System.Drawing.Size(166, 20);
			this.basicDataTextBox.TabIndex = 9;
			// 
			// portRouterLabel
			// 
			this.portRouterLabel.Location = new System.Drawing.Point(0, 245);
			this.portRouterLabel.Name = "portRouterLabel";
			this.portRouterLabel.Size = new System.Drawing.Size(122, 20);
			this.portRouterLabel.TabIndex = 16;
			this.portRouterLabel.Text = "Router server port:";
			this.portRouterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// portRouterTextBox
			// 
			this.portRouterTextBox.Enabled = false;
			this.portRouterTextBox.Location = new System.Drawing.Point(128, 245);
			this.portRouterTextBox.Name = "portRouterTextBox";
			this.portRouterTextBox.Size = new System.Drawing.Size(166, 20);
			this.portRouterTextBox.TabIndex = 15;
			// 
			// serverIpLabel
			// 
			this.serverIpLabel.Location = new System.Drawing.Point(0, 219);
			this.serverIpLabel.Name = "serverIpLabel";
			this.serverIpLabel.Size = new System.Drawing.Size(122, 20);
			this.serverIpLabel.TabIndex = 14;
			this.serverIpLabel.Text = "Server IP address:";
			this.serverIpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// serverIpTextBox
			// 
			this.serverIpTextBox.Enabled = false;
			this.serverIpTextBox.Location = new System.Drawing.Point(128, 219);
			this.serverIpTextBox.Name = "serverIpTextBox";
			this.serverIpTextBox.Size = new System.Drawing.Size(166, 20);
			this.serverIpTextBox.TabIndex = 13;
			// 
			// portDepBoardLabel
			// 
			this.portDepBoardLabel.Location = new System.Drawing.Point(0, 271);
			this.portDepBoardLabel.Name = "portDepBoardLabel";
			this.portDepBoardLabel.Size = new System.Drawing.Size(122, 20);
			this.portDepBoardLabel.TabIndex = 18;
			this.portDepBoardLabel.Text = "Dep. board server port:";
			this.portDepBoardLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// portDepBoardTextBox
			// 
			this.portDepBoardTextBox.Enabled = false;
			this.portDepBoardTextBox.Location = new System.Drawing.Point(128, 271);
			this.portDepBoardTextBox.Name = "portDepBoardTextBox";
			this.portDepBoardTextBox.Size = new System.Drawing.Size(166, 20);
			this.portDepBoardTextBox.TabIndex = 17;
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(156, 297);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(138, 40);
			this.saveButton.TabIndex = 19;
			this.saveButton.Text = "Save as...";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// clearButton
			// 
			this.clearButton.Location = new System.Drawing.Point(6, 297);
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
			this.settingsSaveFileDialog.DefaultExt = "settings";
			this.settingsSaveFileDialog.FileName = ".settings";
			// 
			// ClientUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.portDepBoardLabel);
			this.Controls.Add(this.portDepBoardTextBox);
			this.Controls.Add(this.portRouterLabel);
			this.Controls.Add(this.portRouterTextBox);
			this.Controls.Add(this.serverIpLabel);
			this.Controls.Add(this.serverIpTextBox);
			this.Controls.Add(this.fullDataLabel);
			this.Controls.Add(this.fullDataTextBox);
			this.Controls.Add(this.basicDataLabel);
			this.Controls.Add(this.basicDataTextBox);
			this.Controls.Add(this.onlineRadioButton);
			this.Controls.Add(this.offlineRadioButton);
			this.Controls.Add(this.lockoutsLabel);
			this.Controls.Add(this.lockoutsTextBox);
			this.Controls.Add(this.extraEventsLabel);
			this.Controls.Add(this.extraEventsTextBox);
			this.Controls.Add(this.languageLabel);
			this.Controls.Add(this.languageTextBox);
			this.Controls.Add(this.loadFileButton);
			this.Name = "ClientUserControl";
			this.Size = new System.Drawing.Size(300, 398);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button loadFileButton;
		private System.Windows.Forms.OpenFileDialog settingsOpenFileDialog;
		private System.Windows.Forms.TextBox languageTextBox;
		private System.Windows.Forms.Label languageLabel;
		private System.Windows.Forms.Label extraEventsLabel;
		private System.Windows.Forms.TextBox extraEventsTextBox;
		private System.Windows.Forms.Label lockoutsLabel;
		private System.Windows.Forms.TextBox lockoutsTextBox;
		private System.Windows.Forms.RadioButton offlineRadioButton;
		private System.Windows.Forms.RadioButton onlineRadioButton;
		private System.Windows.Forms.Label fullDataLabel;
		private System.Windows.Forms.TextBox fullDataTextBox;
		private System.Windows.Forms.Label basicDataLabel;
		private System.Windows.Forms.TextBox basicDataTextBox;
		private System.Windows.Forms.Label portRouterLabel;
		private System.Windows.Forms.TextBox portRouterTextBox;
		private System.Windows.Forms.Label serverIpLabel;
		private System.Windows.Forms.TextBox serverIpTextBox;
		private System.Windows.Forms.Label portDepBoardLabel;
		private System.Windows.Forms.TextBox portDepBoardTextBox;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.SaveFileDialog settingsSaveFileDialog;
	}
}

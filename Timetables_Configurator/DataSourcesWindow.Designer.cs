namespace Timetables.Configurator
{
	partial class DataSourcesWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSourcesWindow));
			this.dataFeedsListBox = new System.Windows.Forms.CheckedListBox();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.addButton = new System.Windows.Forms.Button();
			this.linkTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// dataFeedsListBox
			// 
			this.dataFeedsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataFeedsListBox.FormattingEnabled = true;
			this.dataFeedsListBox.Location = new System.Drawing.Point(12, 12);
			this.dataFeedsListBox.Name = "dataFeedsListBox";
			this.dataFeedsListBox.Size = new System.Drawing.Size(248, 274);
			this.dataFeedsListBox.TabIndex = 0;
			// 
			// nameTextBox
			// 
			this.nameTextBox.Location = new System.Drawing.Point(12, 299);
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(77, 20);
			this.nameTextBox.TabIndex = 1;
			this.nameTextBox.Text = "Name";
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(218, 299);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(42, 20);
			this.addButton.TabIndex = 2;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// linkTextBox
			// 
			this.linkTextBox.Location = new System.Drawing.Point(95, 299);
			this.linkTextBox.Name = "linkTextBox";
			this.linkTextBox.Size = new System.Drawing.Size(117, 20);
			this.linkTextBox.TabIndex = 3;
			this.linkTextBox.Text = "Link";
			// 
			// DataSourcesWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(264, 330);
			this.Controls.Add(this.linkTextBox);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.nameTextBox);
			this.Controls.Add(this.dataFeedsListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DataSourcesWindow";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DataSourcesWindow_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox dataFeedsListBox;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.TextBox linkTextBox;
	}
}
namespace WvsBeta.Shop {
	partial class frmMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtLoad = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLog = new WvsBeta.Common.RollingTextBox();
            this.packetInput = new System.Windows.Forms.TextBox();
            this.packetLabel = new System.Windows.Forms.Label();
            this.packetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLoad
            // 
            this.txtLoad.Location = new System.Drawing.Point(223, 12);
            this.txtLoad.Name = "txtLoad";
            this.txtLoad.ReadOnly = true;
            this.txtLoad.Size = new System.Drawing.Size(100, 20);
            this.txtLoad.TabIndex = 4;
            this.txtLoad.Text = "-1";
            this.txtLoad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Amount of connections:";
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 94);
            this.txtLog.MaxLines = 15;
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(396, 185);
            this.txtLog.TabIndex = 6;
            this.txtLog.Text = "Running in DEBUG mode.";
            // 
            // packetInput
            // 
            this.packetInput.Location = new System.Drawing.Point(206, 43);
            this.packetInput.Name = "packetInput";
            this.packetInput.Size = new System.Drawing.Size(117, 20);
            this.packetInput.TabIndex = 7;
            this.packetInput.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // packetLabel
            // 
            this.packetLabel.AutoSize = true;
            this.packetLabel.Location = new System.Drawing.Point(156, 46);
            this.packetLabel.Name = "packetLabel";
            this.packetLabel.Size = new System.Drawing.Size(44, 13);
            this.packetLabel.TabIndex = 8;
            this.packetLabel.Text = "Packet:";
            // 
            // packetButton
            // 
            this.packetButton.Location = new System.Drawing.Point(333, 41);
            this.packetButton.Name = "packetButton";
            this.packetButton.Size = new System.Drawing.Size(75, 23);
            this.packetButton.TabIndex = 9;
            this.packetButton.Text = "Send";
            this.packetButton.UseVisualStyleBackColor = true;
            this.packetButton.Click += new System.EventHandler(this.packetButton_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 291);
            this.Controls.Add(this.packetButton);
            this.Controls.Add(this.packetLabel);
            this.Controls.Add(this.packetInput);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtLoad);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "WvsBeta.Shop";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox txtLoad;
		private System.Windows.Forms.Label label1;
        private Common.RollingTextBox txtLog;
        private System.Windows.Forms.TextBox packetInput;
        private System.Windows.Forms.Label packetLabel;
        private System.Windows.Forms.Button packetButton;
    }
}


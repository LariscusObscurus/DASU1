namespace Client
{
	partial class Form1
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
		private void InitializeComponent()
		{
			this.lstBoxChat = new System.Windows.Forms.ListBox();
			this.txtInput = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lstBoxChat
			// 
			this.lstBoxChat.Dock = System.Windows.Forms.DockStyle.Top;
			this.lstBoxChat.FormattingEnabled = true;
			this.lstBoxChat.Location = new System.Drawing.Point(0, 0);
			this.lstBoxChat.Name = "lstBoxChat";
			this.lstBoxChat.Size = new System.Drawing.Size(621, 277);
			this.lstBoxChat.TabIndex = 1;
			// 
			// txtInput
			// 
			this.txtInput.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.txtInput.Location = new System.Drawing.Point(0, 283);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(621, 20);
			this.txtInput.TabIndex = 2;
			this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(621, 303);
			this.Controls.Add(this.txtInput);
			this.Controls.Add(this.lstBoxChat);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Instant Messenger Client";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstBoxChat;
		private System.Windows.Forms.TextBox txtInput;
	}
}
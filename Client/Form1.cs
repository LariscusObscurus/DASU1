using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
	public partial class Form1 : Form
	{
		private Client mClient = null;
		private Thread mThread = null;
		private delegate void OnUpdateChat(string message);

		public Form1()
		{
			InitializeComponent();
			mClient = Client.Instance;
		}

		private void ReadThreadStart()
		{
			while (true) {
				string message = Client.Instance.Read();

				if (message != null && message != "") {
					Invoke(new OnUpdateChat(UpdateChat), new object[] { message });
				}
			}
		}

		private void UpdateChat(string message)
		{
			this.lstBoxChat.Items.Add(message);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			mThread = new Thread(new ThreadStart(ReadThreadStart));
			mThread.Start();
		}

		private void txtInput_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return) {
				mClient.Write(this.txtInput.Text);
				this.txtInput.Text = "";
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			mThread.Abort();
			mClient.Dispose();
		}
	}
}

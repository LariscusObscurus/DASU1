using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	class Client
	{
		private Client() 
		{
			mClient = new TcpClient(mHostname, mPort);
			mStream = mClient.GetStream();
			mSsl = new SslStream(mStream);
		}

		public static Client Instance
		{
			get 
			{
				return sClient;
			}
		}

		public Stream GetStream() 
		{
			return mStream;
		}

		private TcpClient mClient = null;
		private string mHostname = "";
		private int mPort = 12345;
		private NetworkStream mStream = null;
		private SslStream mSsl = null;
		private X509Certificate2 _cert = new X509Certificate2("Cert/selfSigned.pfx", "password");
		private static readonly Client sClient = new Client();
	}
}

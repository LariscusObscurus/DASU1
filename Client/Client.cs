using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
	class Client : IDisposable
	{
		private Client() 
		{
			try {
				mClient = new TcpClient(mHostname, mPort);
				mStream = mClient.GetStream();
				mSsl = new SslStream(mStream, true, new RemoteCertificateValidationCallback(ValidateCertificate));
				mSsl.AuthenticateAsClient("InstantMessengerServer");
				mReader = new BinaryReader(mSsl, Encoding.UTF8);
				mWriter = new BinaryWriter(mSsl, Encoding.UTF8);
			} catch (AuthenticationException e) {
				throw new Exception(e.Message, e);
			} catch (ArgumentNullException e) {
				throw new Exception(e.Message, e);
			} catch (ArgumentOutOfRangeException e) {
				throw new Exception(e.Message, e);
			} catch (SocketException e) {
				throw new Exception(e.Message, e);
			} catch (ObjectDisposedException e) {
				throw new Exception(e.Message, e);
			} catch (InvalidOperationException e) {
				throw new Exception(e.Message, e);
			} catch (ArgumentException e) {
				throw new Exception(e.Message, e);
			}
		}

		public static Client Instance
		{
			get
			{
				if (sClient == null) {
					lock (mLock) {
						if (sClient == null) {
							sClient = new Client();
						}
					}
				}

				return sClient;
			}
		}

		public void Dispose()
		{
			mWriter.Close();
			mReader.Close();
			mSsl.Close();
			mStream.Close();
			mClient.Close();
		}

		public void Write(string message)
		{
			mWriter.Write(message);
		}

		public string Read()
		{
			return mReader.ReadString();
		}

		private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		private readonly string mHostname = "127.0.0.1";
		private readonly int mPort = 12345;
		private TcpClient mClient = null;
		private NetworkStream mStream = null;
		private SslStream mSsl = null;
		private BinaryReader mReader = null;
		private BinaryWriter mWriter = null;
		private static volatile Client sClient = null;
		private static object mLock = new Object();
	}
}

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

namespace Client
{
	class Client : IDisposable
	{
		public Client() 
		{
			try {
				mClient = new TcpClient(mHostname, mPort);
				mStream = mClient.GetStream();
				mSsl = new SslStream(mStream, true, new RemoteCertificateValidationCallback(ValidateCertificate));
				mSsl.AuthenticateAsClient("InstantMessengerServer");
				mReader = new BinaryReader(mSsl, Encoding.UTF8);
				mWriter = new BinaryWriter(mSsl, Encoding.UTF8);
			} catch (AuthenticationException e) {
				Console.WriteLine(String.Format("{0}: {1}", e.ToString(), e.Message));
			} catch (ArgumentNullException e) {
				Console.WriteLine(String.Format("{0}: {1}", e.ToString(), e.Message));
			} catch (ArgumentOutOfRangeException e) {
				Console.WriteLine(String.Format("{0}: {1}", e.ToString(), e.Message));
			} catch (SocketException e) {
				Console.WriteLine(String.Format("{0}: {1}", e.ToString(), e.Message));
			} catch (ObjectDisposedException e) {
				Console.WriteLine(String.Format("{0}: {1}", e.ToString(), e.Message));
			} catch (InvalidOperationException e) {
				Console.WriteLine(String.Format("{0}: {1}", e.ToString(), e.Message));
			} catch (ArgumentException e) {
				Console.WriteLine(String.Format("{0}: {1}", e.ToString(), e.Message));
			} finally {
				Dispose();
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
	}
}

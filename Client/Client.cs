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
	/// <summary>
	/// Simple Client-Klasse für das Chatprogramm
	/// </summary>
	class Client : IDisposable
	{
		/// <summary>
		/// Verbindet den Client mit dem Server
		/// </summary>
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

		/// <summary>
		/// Liest Client Singleton
		/// </summary>
		public static Client Instance
		{
			get
			{
				if (sClient == null) {
					lock (sLock) {
						if (sClient == null) {
							sClient = new Client();
						}
					}
				}

				return sClient;
			}
		}

		/// <summary>
		/// Schließt die Verbindung
		/// </summary>
		public void Dispose()
		{
			mWriter.Close();
			mReader.Close();
			mSsl.Close();
			mStream.Close();
			mClient.Close();
		}

		/// <summary>
		/// Sendet einen String zu dem Server
		/// </summary>
		/// <param name="message">Der zu versendende String</param>
		public void Write(string message)
		{
			lock (mStream) {
				mWriter.Write(message);
			}
		}

		public string Read()
		{
			try {
				lock (mStream) {
					return mReader.ReadString();
				}
			} catch {
				return "";
			}
		}

		/// <summary>
		/// Gibt immer TRUE zurück
		/// </summary>
		/// <param name="sender">Nope</param>
		/// <param name="certificate">Nope</param>
		/// <param name="chain">Nope</param>
		/// <param name="sslPolicyErrors">Nope</param>
		/// <returns></returns>
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
		private static object sLock = new Object();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.IO;

namespace Server
{
	/// <summary>
	/// Kümmert sich um eingehende Clients
	/// </summary>
	class ClientHandler : IDisposable
	{
		Server _server;
		TcpClient _client;
		NetworkStream _netStream;
		SslStream _sslStream;
		X509Certificate2 _cert;
		BinaryReader _binaryReader;
		BinaryWriter _binaryWriter;

		/// <summary>
		/// Initialisiert den ClientHandler
		/// </summary>
		/// <param name="server">Server der den Handler ausführt</param>
		/// <param name="client">TcpClient der für die Datenübertragung zuständig ist</param>
		/// <param name="cert">Zertifikat für die Validierung</param>
		public ClientHandler(Server server, TcpClient client, X509Certificate2 cert)
		{
			_client = client;
			_server = server;
			_cert = cert;
			Thread t = new Thread(SetupConnection);
			t.IsBackground = true;
			t.Start();
		}

		/// <summary>
		/// Verbindet den Server mit dem Client
		/// </summary>
		void SetupConnection()
		{
			Console.WriteLine("New connection.");
			_netStream = _client.GetStream();
			_sslStream = new SslStream(_netStream, false);

			try {
				_sslStream.AuthenticateAsServer(_cert, false, SslProtocols.Tls, true); //kein client zertifikat, tls, ablaufdatum aber überprüfen 
				_binaryReader = new BinaryReader(_sslStream, Encoding.UTF8);
				_binaryWriter = new BinaryWriter(_sslStream, Encoding.UTF8);
			} 
			catch(AuthenticationException ex)
			{
				Console.WriteLine(ex.Message);
				Dispose();
			}
			catch(ArgumentException ex)
			{
				Console.WriteLine(ex.Message);
				Dispose();
			}


			Console.WriteLine("Connection authenticated.");

			_server.ClientList.Add(this);
			Receive();

		}

		/// <summary>
		/// Schließt die Verbindung zum Client
		/// </summary>
		public void Dispose()
		{
			_binaryWriter.Close();
			_binaryReader.Close();
			_sslStream.Close();
			_netStream.Close();
			_client.Close();
			Console.WriteLine("Connection closed.");
		}

		/// <summary>
		/// Ließt Daten vom Client
		/// </summary>
		void Receive()
		{
			try
			{
				while (_client.Connected)
				{
					string msg = _binaryReader.ReadString();
					_server.BroadcastMessage(this, msg);
				}

			} 
			catch(EndOfStreamException)
			{
				Dispose();
			}
		}

		/// <summary>
		/// Sendet daten zum Client
		/// </summary>
		/// <param name="msg"></param>
		public void Send(string msg)
		{
			if(_client.Connected)
			{
				_binaryWriter.Write(msg);
				_binaryWriter.Flush();
			}
		}
	}
}

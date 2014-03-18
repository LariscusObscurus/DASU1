using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

namespace Server
{
	/// <summary>
	/// Server-Klasse
	/// </summary>
	class Server
	{
		TcpListener _server;
		IPAddress _ip = IPAddress.Parse("127.0.0.1");
		int _port = 12345;
		X509Certificate2 _cert;
		List<ClientHandler> _clientList = new List<ClientHandler>();
		bool _running = true;

		/// <summary>
		/// Server wird erstellt und führt den main-loop aus.
		/// </summary>
		public Server()
		{
			try
			{
				// openssl req -x509 -nodes -days 365 -newkey rsa:1024 -keyout -private.key -out cert.crt
				// openssl pkcs12 -export -in cert.crt -inkey private.key -out server.pfx -passout pass:password
				_cert = new X509Certificate2("Cert/selfSigned.pfx", "password");
			} 
			catch(CryptographicException ex)
			{
				Console.WriteLine(ex.Message);
			}
			_server = new TcpListener(_ip, _port);
			_server.Start();
			Console.WriteLine("Server started!");
			Listen();
		}

		/// <summary>
		/// Akzeptiert neue Clients
		/// </summary>
		void Listen()
		{
			Console.WriteLine("Listening.");
			while (_running)
			{
				TcpClient tcpClient = _server.AcceptTcpClient();
				ClientHandler clientHandler = new ClientHandler(this, tcpClient, _cert);
			}
		}

		/// <summary>
		/// Schließt alle Client Verbindungen
		/// </summary>
		public void Close()
		{
			foreach(ClientHandler it in _clientList)
			{
				it.Dispose();
			}
		}

		/// <summary>
		/// Sendet eingekommene Nachricht an alle anderen Clients
		/// </summary>
		/// <param name="sender">Der sendende Client</param>
		/// <param name="msg">Die zu erhaltene Nachricht</param>
		public void BroadcastMessage(ClientHandler sender, string msg)
		{
			foreach(ClientHandler it in _clientList) 
			{
				if (it != sender)
				{
					it.Send(msg);
				}
			}
		}

		/// <summary>
		/// Alle verbundene Clients.
		/// </summary>
		public List<ClientHandler> ClientList
		{
			get { return _clientList; }
		}
	}
}

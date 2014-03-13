using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

namespace Server
{
	class Server
	{
		TcpListener _server;
		IPAddress _ip = IPAddress.Parse("127.0.0.1");
		int _port = 12345;
		X509Certificate2 _cert = new X509Certificate2("Cert/selfSigned.pfx", "password");
		List<ClientHandler> _clientList = new List<ClientHandler>();
		bool _running = true;

		public Server()
		{
			_server = new TcpListener(_ip, _port);
			_server.Start();
			Listen();
		}

		void Listen()
		{
			while (_running)
			{
				TcpClient tcpClient = _server.AcceptTcpClient();
				ClientHandler clientHandler = new ClientHandler(this, tcpClient, _cert);
			}
		}

		public void Close()
		{
			foreach(ClientHandler it in _clientList)
			{
				it.CloseConnection();
			}
		}

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

		public List<ClientHandler> ClientList
		{
			get { return _clientList; }
		}
	}
}

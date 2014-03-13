using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Instant Messenger Client");
			Console.WriteLine("Enter . or press ^C to quit client");

			using (Client client = new Client()) {
				while (true) {
					client.Read();
				}
			}
		}
	}
}

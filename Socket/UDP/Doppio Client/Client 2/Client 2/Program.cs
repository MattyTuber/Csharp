using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Client_2
{
    internal class Client
    {
        static void Main(string[] args)
        {
            int port = 8080;

            Console.Write("Inserire Server --> ");
            var server = Console.ReadLine();

            var client = new UdpClient();
            client.Connect(server, port);

            while (true)
            {
                Console.Write("Inserire testo da inviare --> ");
                var text = Console.ReadLine();

                if (text == "exit")
                    break;

                byte[] sendBytes = Encoding.UTF8.GetBytes(text);
                client.Send(sendBytes, sendBytes.Length);
            }

            client.Close();
        }
    }
}

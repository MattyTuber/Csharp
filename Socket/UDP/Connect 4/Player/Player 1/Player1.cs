using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Client_1
{
    internal class Client
    {
        static void Main(string[] args)
        {
            int port = 8080;
            int scelta;
            string choice;

            Console.Write("Inserire Server --> ");
            var server = Console.ReadLine();

            var client = new UdpClient();
            client.Connect(server, port);

            Console.Write("Inserire Nome Giocatore --> ");
            var name = Console.ReadLine();

            byte[] sendBytes = Encoding.UTF8.GetBytes(name);
            client.Send(sendBytes, sendBytes.Length);

            Console.Write("Inserire Pedina Giocatore --> ");
            var id = Console.ReadLine();

            sendBytes = Encoding.UTF8.GetBytes(id);
            client.Send(sendBytes, sendBytes.Length);

            while (true)
            {
                do
                {
                    Console.Write("Inserire la Colonna (1 - 7) --> ");
                    scelta = Convert.ToInt16(Console.ReadLine());
                } while (scelta < 1 || scelta > 7);

                choice = Convert.ToString(scelta);
                sendBytes = Encoding.UTF8.GetBytes(choice);
                client.Send(sendBytes, sendBytes.Length);
            }
        }
    }
}

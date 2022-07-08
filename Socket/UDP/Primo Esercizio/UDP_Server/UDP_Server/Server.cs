using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace UDP_Sincrono
{
    internal class Server
    {
        static int port = 8080;
        static UdpClient server = new UdpClient(port);
        static void Main(string[] args)
        {
            while (true)
            {
                receiveData();
            }
        }

        static void receiveData()
        {
            var ip = new IPEndPoint(IPAddress.Any, port);

            byte[] receiveByte = server.Receive(ref ip);

            if (receiveByte != null)
            {
                var message = Encoding.UTF8.GetString(receiveByte);
                Console.WriteLine(message);
            }
        }
    }
}

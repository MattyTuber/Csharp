using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ServerSocket
{
    class Program
    {
        // Inverte caratteri di una stringa
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        // Gestisce il dialogo con un client(e)
        public static void handleClient(Socket client)
        {
            String line;
            // Stream Reader and Writer permettono di leggere e scrivere dati dal socket
            // una riga alla volta. Senza di essi bisognerebbe gestire esplicitamente
            // la lettura dal socket, determinare dove finisce la riga, etc.
            StreamReader r = new StreamReader(new NetworkStream(client)); // Stream reader dal socket
            StreamWriter w = new StreamWriter(new NetworkStream(client)); // Stream writer dal socket
            while ((line = r.ReadLine()) != null) // Legge una riga alla volta. Se il client si disconnetted, restituisce null
            {
                Console.WriteLine("<- " + line);
                String reverse = Reverse(line); // Riga al contrario
                Console.WriteLine("-> " + reverse);
                w.WriteLine(reverse); // Invia riga al contratio al client(e)
                w.Flush(); // Non aspettare che il buffer si riempia!!
            }
            Console.WriteLine("Bye"); // Client(e) si e' disconnesso
        }

        public static void run()
        {
            Socket main = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            main.Bind(new IPEndPoint(0, 7788)); // Usa '0.0.0.0' o '::0' (cioe' qualunque interfaccia), porta 7788.
            main.Listen(1); // Mette il socket in ascolto (max 1 client(e) in coda ad aspettare) 
            Console.WriteLine("Listening to " + main.LocalEndPoint.ToString());
            while (true)
            {
                Socket client = main.Accept(); // Aspetta che qualche client si connetta
                Console.WriteLine("Connection from client: " + client.RemoteEndPoint.ToString());
                handleClient(client); // Gestisci client(e)
            }
        }

        static void Main(string[] args)
        {
            run(); // Duh!
        }
    }
}
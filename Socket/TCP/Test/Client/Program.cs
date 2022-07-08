/*  TCP_client_stringa_fine_x_terminare_v1.cs 
    viene richiesto ciclicamente di inserire un testo da inviare al server fino
    a quando viene inserita la stringa "FINE"  
    Suggerimento:
    ad ogni invio della stringa occorre aprire e chiudere la connessione, altrimenti
    visualizza tutte le stringhe inviate al server concatenate  */

using System;
using System.Text;
using System.Net.Sockets;

namespace tcp_client
{
    class tcp_client
    {
        static void Main(string[] args)
        {
            string testo_da_inviare = "";
            // definizione array di byte buffer invio dati
            int BUFSIZE = 256;
            byte[] byteBuffer = new byte[BUFSIZE];
            //  numero di porta del server
            int porta = 7788;
            string data = "";


            Console.Clear();
            Console.WriteLine("TCP_client_stringa_fine_x_terminare_v1.cs - trasmissione TCP\n");
            /*  il server si puo' identificare dal nome computer o dall'indirizzo IP 
                    esempio "x-64", "localhost"   oppure "192.168.30.30", "127.0.0.1"  */
            Console.Write("Inserire server (nome_computer o indirizzo IP): ");
            String server = Console.ReadLine();
            Console.WriteLine("\nConnessione al server: {0} Porta: {1}", server, porta);
            //  invio dati al server
            Console.Write("\n\nInserisci testo da inviare al server: ");
            testo_da_inviare = Console.ReadLine();
            testo_da_inviare += "\n";
            //  inizializza una nuova istanza  della classe TcpClient specificando server e porta 
            TcpClient client = new TcpClient(server, porta);
            //  inizializza flusso comunicazione client/server
            NetworkStream netStream = client.GetStream();


            //            StreamReader r = new StreamReader(new NetworkStream(client)); // Stream reader dal socket
            //            StreamWriter w = new StreamWriter(new NetworkStream(client)); // Stream writer dal socket
            while (testo_da_inviare != "FINE\n")
            {
                byteBuffer = Encoding.ASCII.GetBytes(testo_da_inviare);

                Console.WriteLine("\nDati inviati al server    ---> " + testo_da_inviare);
                //  scrive dati sul flusso
                netStream.Write(byteBuffer, 0, byteBuffer.Length);

                //  attende  e visualizza risposta elaborata dal server 
                int byte_ricevuti = 0;
                while ((byte_ricevuti = netStream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                {
                    data = Encoding.ASCII.GetString(byteBuffer, 0, byte_ricevuti);
                    Console.WriteLine("\nDati elaborati dal server   <--- {0}", data);
                    netStream.Read(byteBuffer, 0, byteBuffer.Length);
                    break;
                }


                Console.Write("\n\nInserisci nuovo testo da inviare al server: ");
                testo_da_inviare = Console.ReadLine();
                testo_da_inviare += "\n";
            }
            //  chiude la connessione con il server
            netStream.Close();
            client.Close();

            Console.Write("\nPremere invio per finire");
            Console.ReadLine();
        }
    }
}
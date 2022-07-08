using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;



namespace Client
{
    internal class Program
    {
        public const int COLONNE = 7;
        public const int RIGHE = 6;

        static void Main()
        {
            byte[] byteBuffer = new byte[512];
            string nome, plnome;
            char pedina, plpedina;
            int receivedBytes;
            char[,] board = new char[RIGHE + 1, COLONNE + 1];
            bool ended = false;

            // Inizializzo la board del client con tutti spazi vuoti
            setBoard(board);

            //Inizio la fase di connessione al server
            Console.Write("Inserire Server --> ");
            string server = Console.ReadLine();

            //mi connetto al server indicato sulla porta 7788
            TcpClient client = new TcpClient(server, 7788);
            NetworkStream netStream = client.GetStream();

            //Ricevo in input nome e pedina del player (se il client è client 1 ricevo "E" e cestino l'input)
            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            plnome = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes).TrimEnd('\n','\r');

            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            plpedina = Convert.ToChar(Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes).TrimEnd('\n', '\r'));

            //Controllo che nome e pedina non siano uguali tra i due player
            do
            {
                Console.Write("Inserire Nickname Giocatore --> ");
                nome = Console.ReadLine();

                Console.Write("Inserire Pedina --> ");
                while (!char.TryParse(Console.ReadLine(), out pedina))
                {
                    Console.WriteLine("Input non valido!!");
                    Console.Write("Inserire Pedina --> ");
                }
            } while (nome == plnome || pedina == plpedina);

            //Inoltro al server nome e pedina del giocatore
            byteBuffer = Encoding.ASCII.GetBytes(nome + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);
            
            byteBuffer = Encoding.ASCII.GetBytes(Convert.ToString(pedina) + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            //Diamo il via alle danze
            while (!ended)
            {
                //Ricevo la mossa del giocatore precedente, se sono al primo turno ricevo 0 e lo cestino
                receiveMoves(ref byteBuffer, ref netStream, ref receivedBytes, board);

                //Mostro la board
                displayBoard(board);

                //Faccio inserire la pedina
                drop(ref byteBuffer, ref netStream, ref receivedBytes, board, pedina);

                //Controllo se mi è arrivato l'input di vincita o da parte mia o da parte dell'avversario
                if(receiveEnd(ref byteBuffer, ref netStream, ref receivedBytes))
                    ended = true;
            }

            //Stampo chi ha vinto
            printWin(ref byteBuffer, ref netStream, ref receivedBytes);
            Console.ReadLine();

            //Chiudo la comunicazione
            //client.Close();
        }

        static void setBoard(char[,] board)
        {
            for (int i = 0; i < RIGHE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }

        static void displayBoard(char[,] board)
        {
            Console.Clear();

            for (int i = 0; i < RIGHE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    Console.Write("[ " + board[i, j] + " ]  ");
                }

                Console.WriteLine("\n");
            }
        }

        static void drop(ref byte[] byteBuffer, ref NetworkStream netStream, ref int receivedBytes, char[,]board, char pedina)
        {
            int choice;
            string err, sync;

            //Mi sincronizzo con il server
            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            sync = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes);

            //Controllo che la mossa inserita sia valida, che rientri nel range
            do
            {
                Console.Write("Inserire la Colonna (1 - 7) --> ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Input non valido!!");
                }
            } while (choice < 1 || choice > 7);

            // Mando la mossa
            byteBuffer = Encoding.ASCII.GetBytes(Convert.ToString(choice) + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            //Ricevo errori sulla colonna piena
            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            err = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes);

            //Se sono in errore chiedo di reinserire
            if (err == "1")
            {
                Console.Write("Inserire Nuova Colonna --> ");
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Input non valido!!");
                    Console.Write("Inserire Nuova Colonna --> ");
                }

                byteBuffer = Encoding.ASCII.GetBytes(Convert.ToString(choice) + "\n");
                netStream.Write(byteBuffer, 0, byteBuffer.Length);
            }

            //Parte molto ricorrente nel programma, sincronizzazione con il server
            byteBuffer = Encoding.ASCII.GetBytes("SYN" + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            for (int i = RIGHE - 1; i >= 0; i--)
            {
                if (board[i, choice - 1] == ' ')
                {
                    board[i, choice - 1] = pedina;
                    break;
                }
            }
        }

        static void receiveMoves(ref byte[] byteBuffer, ref NetworkStream netStream, ref int receivedBytes, char[,] board)
        {
            string strScelta, player;
            int choice;

            //Ricevo in input la mossa dell'avversario
            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            strScelta = Encoding.UTF8.GetString(byteBuffer, 0, receivedBytes).TrimEnd('\n', '\r');
            choice = Convert.ToInt32(strScelta[0])-48;

            //SYNC
            byteBuffer = Encoding.ASCII.GetBytes("SYN" + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            //Ricevo la pedina dell'avversario per poter popolare la board
            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            player = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes).TrimEnd('\n', '\r');

            //SYNC
            byteBuffer = Encoding.ASCII.GetBytes("SYN" + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            //NEXT!!
            if (choice == 0)
                return;

            for (int i = RIGHE - 1; i >= 0; i--)
            {

                if (board[i, choice - 1] == ' ')
                {
                    board[i, choice - 1] = Convert.ToChar(player);
                    break;
                }
            }
        }

        static void printWin(ref byte[] byteBuffer, ref NetworkStream netStream, ref int receivedBytes)
        {
            string win;
            
            //Ricevo il nome del giocatore vincente
            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            win = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes);

            //SYNC
            byteBuffer = Encoding.ASCII.GetBytes("SYN" + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            Console.WriteLine("The Winner Is --> " + win);
        }

        static bool receiveEnd(ref byte[] byteBuffer, ref NetworkStream netStream, ref int receivedBytes)
        {
            string end;

            //Ricevo se qualcuno a vinto o meno
            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            end = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes);

            //SYNC
            byteBuffer = Encoding.ASCII.GetBytes("SYN" + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            if (end != "C")
                return true;
            else
                return false;
        }
    }
}

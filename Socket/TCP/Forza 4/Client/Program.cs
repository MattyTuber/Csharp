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
            string nome, win;
            char pedina, plpedina;
            string plnome;
            int receivedBytes;
            char[,] board = new char[RIGHE + 1, COLONNE + 1];
            setBoard(board);

            Console.Write("Inserire Server --> ");
            string server = Console.ReadLine();

            TcpClient client = new TcpClient(server, 7788);
            NetworkStream netStream = client.GetStream();

            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            plnome = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes).TrimEnd('\n','\r');

            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            plpedina = Convert.ToChar(Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes).TrimEnd('\n', '\r'));

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

            byteBuffer = Encoding.ASCII.GetBytes(nome + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);
            
            byteBuffer = Encoding.ASCII.GetBytes(Convert.ToString(pedina) + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            while (true)
            {
                receiveMoves(ref byteBuffer, ref netStream, ref receivedBytes, board);

                drop(ref byteBuffer, ref netStream, ref receivedBytes, board, pedina);
                displayBoard(board);

                if (printWin(ref byteBuffer, ref netStream, ref receivedBytes))
                    break;
            }

            Console.WriteLine("Partita Terminata!!");
            Console.ReadLine();
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

            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            sync = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes);
            Console.WriteLine("Sync --> " + sync);

            do
            {
                Console.Write("Inserire la Colonna (1 - 7) --> ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Input non valido!!");
                }
            } while (choice < 1 || choice > 7);

            byteBuffer = Encoding.ASCII.GetBytes(Convert.ToString(choice) + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            err = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes);

            Console.WriteLine("Error --> " + err);

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

            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            strScelta = Encoding.UTF8.GetString(byteBuffer, 0, receivedBytes).TrimEnd('\n', '\r');
            choice = Convert.ToInt32(strScelta[0])-48;
            Console.WriteLine("[[" + choice + "]]");

            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            player = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes).TrimEnd('\n', '\r');
            Console.WriteLine("[[->" + player + "<-]]");

            byteBuffer = Encoding.ASCII.GetBytes("SYN" + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

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

        static bool printWin(ref byte[] byteBuffer, ref NetworkStream netStream, ref int receivedBytes)
        {
            string win;
            
            receivedBytes = netStream.Read(byteBuffer, 0, byteBuffer.Length);
            win = Encoding.ASCII.GetString(byteBuffer, 0, receivedBytes);

            byteBuffer = Encoding.ASCII.GetBytes("SYN" + "\n");
            netStream.Write(byteBuffer, 0, byteBuffer.Length);

            if (win != "C")
            {
                Console.WriteLine("Vittoria --> " + win);
                return true;
            }

            Console.WriteLine("Win --> " + win);

            return false;
        }
    }
}

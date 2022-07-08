using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

public struct player
{
    public string name;
    public char id;
};

namespace Server
{
    internal class Server
    {
        static int port = 8080;
        static UdpClient server = new UdpClient(port);

        public const int COLONNE = 7;
        public const int RIGHE = 6;

        static void Main(string[] args)
        {
            player playerOne = new player();
            player playerTwo = new player();

            int choice;

            char[,] board = new char[RIGHE + 1, COLONNE + 1];
            setBoard(board);

            playerOne.name = receiveName();
            playerOne.id = Convert.ToChar(receiveId());

            playerTwo.name = receiveName();
            playerTwo.id = Convert.ToChar(receiveId());

            while (true)
            {
                choice = Convert.ToInt16(receiveChoice());
                drop(board, playerOne, choice);
                displayBoard(board); // far vedere la board ai client

                if (checkWin(board, playerOne)) // invia al client la vittoria e blocca i client
                    break;

                if (IsFullBoard(board))
                {
                    Console.WriteLine("PAREGGIO"); // invia al client il pareggio e blocca i client
                    break;
                }

                choice = Convert.ToInt16(receiveChoice());
                drop(board, playerTwo, choice);
                displayBoard(board);

                if (checkWin(board, playerTwo)) // invia al client la vittoria e blocca i client
                    break;

                if (IsFullBoard(board))
                {
                    Console.WriteLine("PAREGGIO"); // invia al client il pareggio e blocca i client
                    break;
                }
            }

            Console.ReadKey();
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

        static string receiveName()
        {
            var ip = new IPEndPoint(IPAddress.Any, port);

            byte[] receiveByte = server.Receive(ref ip);

            if (receiveByte != null)
            {
                string name = Encoding.UTF8.GetString(receiveByte);
                return name;
            }
            else
                return null;
        }

        static string receiveId()
        {
            var ip = new IPEndPoint(IPAddress.Any, port);

            byte[] receiveByte = server.Receive(ref ip);

            if (receiveByte != null)
            {
                string id = Encoding.UTF8.GetString(receiveByte);
                return id;
            }
            else
                return null;
        }

        static string receiveChoice()
        {
            var ip = new IPEndPoint(IPAddress.Any, port);

            byte[] receiveByte = server.Receive(ref ip);

            if (receiveByte != null)
            {
                string choice = Encoding.UTF8.GetString(receiveByte);
                return choice;
            }
            else
                return null;
        }

        static void displayBoard(char[,] board)
        {
            Console.WriteLine();

            for (int i = 0; i < RIGHE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    Console.Write("[ " + board[i, j] + " ]  ");
                }

                Console.WriteLine("\n");
            }
        }

        static void drop(char[,] board, player currentPlayer, int choice)
        {
            bool isFull = true;

            while (isFull)
            {
                for (int i = 0; i < RIGHE; i++)
                {
                    if (board[i, choice - 1] != ' ')
                        isFull = true;
                    else
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull) // da trasferire su client
                {
                    Console.Write("Inserire Nuova Colonna --> ");
                    choice = Convert.ToInt16(Console.ReadLine());
                }
            }

            for (int i = RIGHE - 1; i >= 0; i--)
            {

                if (board[i, choice - 1] == ' ')
                {
                    board[i, choice - 1] = currentPlayer.id;
                    break;
                }
            }
        }

        static bool IsFullBoard(char[,] board)
        {
            foreach (var item in board)
            {
                if (item == ' ')
                    return false;
            }

            return false;
        }
        static bool checkWin(char[,] board, player currentPlayer)
        {
            for (int i = 0; i < RIGHE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    if ((board[i, j] == currentPlayer.id) && (board[i, j + 1] == currentPlayer.id) && (board[i, j + 2] == currentPlayer.id) && (board[i, j + 3] == currentPlayer.id))
                    {
                        Console.Write("Vittoria Orizzontale --> " + currentPlayer.name);
                        return true;
                    }

                    if ((board[i, j] == currentPlayer.id) && (board[i - 1, j] == currentPlayer.id) && (board[i - 2, j] == currentPlayer.id) && (board[i - 3, j] == currentPlayer.id))
                    {
                        Console.Write("Vittoria Verticale --> " + currentPlayer.name);
                        return true;
                    }

                    if ((board[i, j] == currentPlayer.id) && (board[i + 1, j + 1] == currentPlayer.id) && (board[i + 2, j + 2] == currentPlayer.id) && (board[i + 3, j + 3] == currentPlayer.id)) //Obliquo da dx a sx
                    {
                        Console.Write("Vittoria Obliqua --> " + currentPlayer.name);
                        return true;
                    }

                    if ((board[i, j] == currentPlayer.id) && (board[i - 1, j + 1] == currentPlayer.id) && (board[i - 2, j + 2] == currentPlayer.id) && (board[i - 3, j + 3] == currentPlayer.id)) //Obliquo da sx a dx
                    {
                        Console.Write("Vittoria Obliqua --> " + currentPlayer.name);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

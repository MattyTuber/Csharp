using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;

public struct player
{
    public string name;
    public char id;
};

namespace Server
{
    internal class Program
    {
        public const int COLONNE = 7;
        public const int RIGHE = 6;

        static void Main()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(0, 7788));
            socket.Listen(1);
            Console.WriteLine("Listening to " + socket.LocalEndPoint.ToString());

            Socket client = socket.Accept();
            Socket client2 = socket.Accept();
            Console.WriteLine("Connection from client: " + client.RemoteEndPoint.ToString());
            Console.WriteLine("Connection from client: " + client2.RemoteEndPoint.ToString());
            clientOperations(client, client2);
        }

        public static void clientOperations(Socket client, Socket client2)
        {
            StreamReader r1 = new StreamReader(new NetworkStream(client));
            StreamWriter w1 = new StreamWriter(new NetworkStream(client));

            StreamReader r2 = new StreamReader(new NetworkStream(client2));
            StreamWriter w2 = new StreamWriter(new NetworkStream(client2));
            
            player playerOne = new player();
            player playerTwo = new player();
            setPlayerOne(ref playerOne, ref r1, ref w1);
            setPlayerTwo(ref playerOne, ref playerTwo, ref r2, ref w2);

            char[,] board = new char[RIGHE + 1, COLONNE + 1];
            setBoard(board);

            int choice1 = 0;
            int choice2 = 0;

            while (true)
            {
                //drop, send, checkwin
                sendToOne(ref w1, ref r1, ref choice2, ref playerTwo);
                sendToTwo(ref w2, ref r2, ref choice1, ref playerOne); //Ordine di stampa scomodo

                dropOne(board, playerOne, ref w1, ref r1, ref choice1);
                if(checkWinOne(board, playerOne, ref w1, ref w2))
                {
                    printWinOne(playerOne, ref w1, ref r1);
                    printWinTwo(playerOne, ref w2, ref r2);
                    break;
                }
                else
                {
                    w1.Write("C");
                    w1.Flush();

                    r1.ReadLine();
                }

                dropTwo(board, playerTwo, ref w2, ref r2, ref choice2);
                if (checkWinTwo(board, playerTwo, ref w1, ref w2))
                {
                    printWinOne(playerTwo, ref w1, ref r1);
                    printWinTwo(playerTwo, ref w2, ref r2);
                    break;
                }
                else
                {
                    w2.Write("C");
                    w2.Flush();

                    r2.ReadLine();
                }
            }
        }

        static void setPlayerOne(ref player playerOne, ref StreamReader r1, ref StreamWriter w1)
        {
            w1.Write("E");
            w1.Flush();

            w1.Write("E");
            w1.Flush();

            playerOne.name = r1.ReadLine().TrimEnd('\n');
            playerOne.id = Convert.ToChar(r1.ReadLine().TrimEnd('\n'));
        }

        static void setPlayerTwo(ref player playerOne, ref player playerTwo, ref StreamReader r2, ref StreamWriter w2)
        {
            w2.Write(playerOne.name);
            w2.Flush();

            w2.Write(playerOne.id);
            w2.Flush();

            playerTwo.name = r2.ReadLine().TrimEnd('\n');
            playerTwo.id = Convert.ToChar(r2.ReadLine().TrimEnd('\n'));
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

        static void sendToTwo(ref StreamWriter w2, ref StreamReader r2, ref int choice1, ref player playerOne)
        {
            w2.Write(choice1);
            w2.Flush();

            w2.Write(playerOne.id);
            w2.Flush();

            r2.ReadLine();
        }

        static void sendToOne(ref StreamWriter w1, ref StreamReader r1, ref int choice2, ref player playerTwo)
        {
            w1.Write(choice2);
            w1.Flush();

            w1.Write(playerTwo.id);
            w1.Flush();

            r1.ReadLine();
        }

        static void dropOne(char[,] board, player currentPlayer, ref StreamWriter w1, ref StreamReader r1, ref int choice1)
        {
            bool isFull = true;

            w1.Write("D1");
            w1.Flush();

            choice1 = Convert.ToInt16(r1.ReadLine().TrimEnd('\n'));

            while (isFull)
            {
                for (int i = 0; i < RIGHE; i++)
                {
                    if (board[i, choice1 - 1] != ' ')
                        isFull = true;
                    else
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    w1.Write("1");
                    w1.Flush();

                    choice1 = Convert.ToInt16(r1.ReadLine().TrimEnd('\n'));
                }
                else
                {
                    w1.Write("0");
                    w1.Flush();
                }

                r1.ReadLine();
            }

            for (int i = RIGHE - 1; i >= 0; i--)
            {

                if (board[i, choice1 - 1] == ' ')
                {
                    board[i, choice1 - 1] = currentPlayer.id;
                    break;
                }
            }
        }

        static void dropTwo(char[,] board, player currentPlayer, ref StreamWriter w2, ref StreamReader r2, ref int choice2)
        {
            bool isFull = true;

            w2.Write("D2");
            w2.Flush();

            choice2 = Convert.ToInt16(r2.ReadLine().TrimEnd('\n'));

            while (isFull)
            {
                for (int i = 0; i < RIGHE; i++)
                {
                    if (board[i, choice2 - 1] != ' ')
                        isFull = true;
                    else
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    w2.Write("1");
                    w2.Flush();

                    choice2 = Convert.ToInt16(r2.ReadLine().TrimEnd('\n'));
                }
                else
                {
                    w2.Write("0");
                    w2.Flush();
                }

                r2.ReadLine();
            }

            for (int i = RIGHE - 1; i >= 0; i--)
            {

                if (board[i, choice2 - 1] == ' ')
                {
                    board[i, choice2 - 1] = currentPlayer.id;
                    break;
                }
            }
        }

        static bool checkWinOne(char [,] board, player currentPlayer, ref StreamWriter w1, ref StreamWriter w2)
        {
            for (int i = 0; i < RIGHE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i, j + 1] == currentPlayer.id) &&
                        (board[i, j + 2] == currentPlayer.id) &&
                        (board[i, j + 3] == currentPlayer.id))
                    {
                        return true;
                    }

                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i + 1, j] == currentPlayer.id) &&
                        (board[i + 2, j] == currentPlayer.id) &&
                        (board[i + 3, j] == currentPlayer.id))
                    {
                        return true;
                    }

                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i + 1, j + 1] == currentPlayer.id) &&
                        (board[i + 2, j + 2] == currentPlayer.id) &&
                        (board[i + 3, j + 3] == currentPlayer.id)) //Obliquo da dx a sx
                    {
                        return true;
                    }
                }
            }

            for (int i = RIGHE - 1; i >= 3; i--)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i - 1, j + 1] == currentPlayer.id) &&
                        (board[i - 2, j + 2] == currentPlayer.id) &&
                        (board[i - 3, j + 3] == currentPlayer.id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static bool checkWinTwo(char[,] board, player currentPlayer, ref StreamWriter w1, ref StreamWriter w2)
        {
            for (int i = 0; i < RIGHE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i, j + 1] == currentPlayer.id) &&
                        (board[i, j + 2] == currentPlayer.id) &&
                        (board[i, j + 3] == currentPlayer.id))
                    {
                        return true;
                    }

                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i + 1, j] == currentPlayer.id) &&
                        (board[i + 2, j] == currentPlayer.id) &&
                        (board[i + 3, j] == currentPlayer.id))
                    {
                        return true;
                    }

                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i + 1, j + 1] == currentPlayer.id) &&
                        (board[i + 2, j + 2] == currentPlayer.id) &&
                        (board[i + 3, j + 3] == currentPlayer.id)) //Obliquo da dx a sx
                    {
                        return true;
                    }
                }
            }

            for (int i = RIGHE - 1; i >= 3; i--)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i - 1, j + 1] == currentPlayer.id) &&
                        (board[i - 2, j + 2] == currentPlayer.id) &&
                        (board[i - 3, j + 3] == currentPlayer.id))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static void printWinOne(player currentPlayer, ref StreamWriter w1, ref StreamReader r1)
        {
            w1.Write(currentPlayer.name);
            w1.Flush();

            r1.ReadLine();
        }

        static void printWinTwo(player currentPlayer, ref StreamWriter w2, ref StreamReader r2)
        {
            w2.Write(currentPlayer.name);
            w2.Flush();

            r2.ReadLine();
        }
    }
}

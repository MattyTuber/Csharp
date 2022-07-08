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
            //Rimango in attesa di connessioni proventinti da qualsiasi parte (0) sulla porta 7788
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(0, 7788));
            socket.Listen(1);
            Console.WriteLine("Listening to " + socket.LocalEndPoint.ToString());

            //Apro l'ascolto a due client
            Socket client = socket.Accept();
            Socket client2 = socket.Accept();
            Console.WriteLine("Connection from client: " + client.RemoteEndPoint.ToString());
            Console.WriteLine("Connection from client: " + client2.RemoteEndPoint.ToString());
            
            //DIAMO IL VIA ALLE DANZE
            clientOperations(client, client2);
        }

        public static void clientOperations(Socket client, Socket client2)
        {
            //Inizializzo reader e writer dei due client
            StreamReader r1 = new StreamReader(new NetworkStream(client));
            StreamWriter w1 = new StreamWriter(new NetworkStream(client));

            StreamReader r2 = new StreamReader(new NetworkStream(client2));
            StreamWriter w2 = new StreamWriter(new NetworkStream(client2));
            
            //Creo due entità della struct player
            player playerOne = new player();
            player playerTwo = new player();

            //Popolo le due struct con l'input dei client
            setPlayerOne(ref playerOne, ref r1, ref w1);
            setPlayerTwo(ref playerOne, ref playerTwo, ref r2, ref w2);

            //Inizializzo la matrice
            char[,] board = new char[RIGHE + 1, COLONNE + 1];

            //Imposto la matrice con spazi vuoti
            setBoard(board);

            int choice1 = 0, choice2 = 0;
            bool win = false;
            string winner = "";

            while (!win)
            {
                Console.Clear();

                //Invio la mossa del player 2 al player 1
                sendToOne(ref w1, ref r1, ref choice2, ref playerTwo);

                //Faccio inserire la mossa al player 2
                dropOne(board, playerOne, ref w1, ref r1, ref choice1);

                //Controllo se il player 1 ha vinto con la sua mossa e mando la fine al client
                if (checkWinOne(board, playerOne))
                {
                    win = true;
                    winner = playerOne.name;
                    sendEndOne(ref w1, ref r1);
                }
                else //Invio lo stesso al client caratteri per sincronizzare
                {
                    w1.Write("C");
                    w1.Flush();

                    r1.ReadLine();
                }

                //Invio la del player 1 al player 2
                sendToTwo(ref w2, ref r2, ref choice1, ref playerOne);

                //Faccio inserire al player 2 la mossa
                dropTwo(board, playerTwo, ref w2, ref r2, ref choice2);

                //Controllo che il player 2 abbia vinto e che il player 1 non abbia già vinto
                if (checkWinTwo(board, playerTwo) && !win)
                {
                    win = true;
                    winner = playerTwo.name;
                    sendEndTwo(ref w2, ref r2);
                }
                else if (win) //Se il player 1 avesse già vinto invio il termine al player 1
                {
                    sendEndTwo(ref w2, ref r2);
                }
                else //Caratteri per la sincronizzazione
                {
                    w2.Write("C");
                    w2.Flush();

                    r2.ReadLine();
                }
            }

            //SONO FUORI DAL WHILE

            //Nel caso in cui a vincere fosse stato il player 2 sono stato costretto ad uscire dal while
            //In questo caso invierò una scelta fittizia al client 1 con mossa = 0
            //Faccio terminare la mossa al client 1 e invio la fine
            if(winner == playerTwo.name)
            {
                choice2 = 0;
                sendToOne(ref w1, ref r1, ref choice2, ref playerTwo);

                dropOne(board, playerOne, ref w1, ref r1, ref choice1);

                sendEndOne(ref w1, ref r1);
            }

            //Invio il vincitore ai due client
            printWinOne(winner, ref w1, ref r1);
            printWinTwo(winner, ref w2, ref r2);
        }

        static void setPlayerOne(ref player playerOne, ref StreamReader r1, ref StreamWriter w1)
        {
            //E carattere fittizio solo per sincronizzare
            w1.Write("E");
            w1.Flush();

            w1.Write("E");
            w1.Flush();

            //Popolo la struct
            playerOne.name = r1.ReadLine().TrimEnd('\n');
            playerOne.id = Convert.ToChar(r1.ReadLine().TrimEnd('\n'));
        }

        static void setPlayerTwo(ref player playerOne, ref player playerTwo, ref StreamReader r2, ref StreamWriter w2)
        {
            //Invio al client 2 i dati del client 1 così da evitare duplicati
            w2.Write(playerOne.name);
            w2.Flush();

            w2.Write(playerOne.id);
            w2.Flush();

            //Popolo la struct
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
            //Invio mossa e pedina del client 1 al client 2 leggendo l'input del client in modo da sincronizzare
            w2.Write(choice1);
            w2.Flush();

            r2.ReadLine();

            w2.Write(playerOne.id);
            w2.Flush();

            r2.ReadLine();
        }

        static void sendToOne(ref StreamWriter w1, ref StreamReader r1, ref int choice2, ref player playerTwo)
        {
            //Invio mossa e pedina del client 2 al client 1 leggendo l'input del client in modo da sincronizzare
            w1.Write(choice2);
            w1.Flush();

            r1.ReadLine();

            w1.Write(playerTwo.id);
            w1.Flush();

            r1.ReadLine();
        }

        static void sendEndOne(ref StreamWriter w1, ref StreamReader r1)
        {
            //Mando la fine e aspetto l'input per sincronizzare
            w1.Write("End");
            w1.Flush();

            r1.ReadLine();
        }

        static void sendEndTwo(ref StreamWriter w2, ref StreamReader r2)
        {
            //Mando la fine e aspetto l'input per sincronizzare
            w2.Write("End");
            w2.Flush();

            r2.ReadLine();
        }

        static void dropOne(char[,] board, player currentPlayer, ref StreamWriter w1, ref StreamReader r1, ref int choice1)
        {
            bool isFull = true;

            //Sincronizzo
            w1.Write("D1");
            w1.Flush();

            //Ricevo l'input
            choice1 = Convert.ToInt16(r1.ReadLine().TrimEnd('\n'));

            //Contro che la colonna non sia piena
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

                if (isFull) //Invio l'errore 1 al client
                {
                    w1.Write("1");
                    w1.Flush();

                    choice1 = Convert.ToInt16(r1.ReadLine().TrimEnd('\n'));
                }
                else //Comunico al client che non ci sono errori
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

        static void dropTwo(char[,] board, player currentPlayer, ref StreamWriter w2, ref StreamReader r2, ref int choice2) //Vedi dropOne
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

        static bool checkWinOne(char [,] board, player currentPlayer) //Controllo la vittoria
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

        static bool checkWinTwo(char[,] board, player currentPlayer) //Controllo la vittoria
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

        static void printWinOne(string winner, ref StreamWriter w1, ref StreamReader r1)
        {
            //Invio al client 1 il vincitore
            w1.Write(winner);
            w1.Flush();

            r1.ReadLine();
        }

        static void printWinTwo(string winner, ref StreamWriter w2, ref StreamReader r2)
        {
            //Invio al client 2 il vincitore
            w2.Write(winner);
            w2.Flush();

            r2.ReadLine();
        }
    }
}

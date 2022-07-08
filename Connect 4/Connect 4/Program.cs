using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct player
{
    public string name;
    public char id;
};

namespace Connect_4
{
    class Program
    {
        public const int COLONNE = 7;
        public const int RIGHE = 6;
        static void Main()
        {
            player playerOne = new player();
            player playerTwo = new player();
            setPlayer(ref playerOne, ref playerTwo);

            char[,] board = new char[RIGHE+1, COLONNE+1];
            setBoard(board);

            while (true)
            {
                drop(board, playerOne);
                displayBoard(board);

                if (checkWin(board, playerOne))
                    break;
                
                if (IsFullBoard(board)){
                    Console.WriteLine("PAREGGIO");
                    break;
                }
                
                drop(board, playerTwo);
                displayBoard(board);

                if (checkWin(board, playerTwo))
                    break;

                if (IsFullBoard(board)){
                    Console.WriteLine("PAREGGIO");
                    break;
                }
            }

            Console.ReadKey();
        }

        static void setPlayer(ref player playerOne, ref player playerTwo)
        {
            Console.Write("Inserire Nickname Giocatore 1 --> ");
            playerOne.name = Console.ReadLine();

            Console.Write("Inserire Pedina --> ");
            while (!char.TryParse(Console.ReadLine(), out playerOne.id))
            {
                Console.WriteLine("Input non valido!!");
                Console.Write("Inserire Pedina --> ");
            }
                
            Console.WriteLine();

            do
            {
                Console.Write("Inserire Nickname Giocatore 2 --> ");
                playerTwo.name = Console.ReadLine();

                Console.Write("Inserire Pedina --> ");
                while (!char.TryParse(Console.ReadLine(), out playerTwo.id))
                {
                    Console.WriteLine("Input non valido!!");
                    Console.Write("Inserire Pedina --> ");
                }
            } while (playerTwo.name == playerOne.name || playerTwo.id == playerOne.id);

            Console.WriteLine();
        }

        static void setBoard(char[,] board)
        {
            for (int i = 0; i < RIGHE; i++)
            {
                for(int j = 0; j < COLONNE; j++)
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

        static void drop(char[,] board, player currentPlayer)
        {
            int choice;
            bool isFull = true;
            
            Console.WriteLine("Turno di --> " + currentPlayer.name);

            do {
                Console.Write("Inserire la Colonna (1 - 7) --> ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Input non valido!!");
                }
            } while (choice < 1 || choice > 7);

            while (isFull)
            {
                for(int i = 0;i < RIGHE; i++)
                {
                    if (board[i, choice-1] != ' ')
                        isFull = true;
                    else
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    Console.Write("Inserire Nuova Colonna --> ");
                    while (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Input non valido!!");
                        Console.Write("Inserire Nuova Colonna --> ");
                    }
                }
            }

            for (int i = RIGHE-1; i >= 0; i--)
            {
                
                if (board[i, choice - 1] == ' ')
                {
                    board[i, choice - 1] = currentPlayer.id;
                    break;
                }
            }
        }
        
        static bool IsFullBoard (char [,] board)
        {
            foreach (var item in board)
            {
                if (item == ' ')
                    return false;
            }
            
            return false;
        }

        static bool checkWin (char [,] board, player currentPlayer)
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
                        Console.Write("Vittoria Orizzontale --> " + currentPlayer.name);
                        return true;
                    }

                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i + 1, j] == currentPlayer.id) &&
                        (board[i + 2, j] == currentPlayer.id) &&
                        (board[i + 3, j] == currentPlayer.id))
                    {
                        Console.Write("Vittoria Verticale --> " + currentPlayer.name);
                        return true;
                    }

                    if ((board[i, j] == currentPlayer.id) &&
                        (board[i + 1, j + 1] == currentPlayer.id) &&
                        (board[i + 2, j + 2] == currentPlayer.id) &&
                        (board[i + 3, j + 3] == currentPlayer.id)) //Obliquo da dx a sx
                    {
                        Console.Write("Vittoria Obliqua --> " + currentPlayer.name);
                        return true;
                    }
                }
            }

            for(int i = RIGHE-1; i >= 3; i--)
            {
                for(int j = 0; j < COLONNE; j++)
                {
                    if((board[i, j] == currentPlayer.id) &&
                        (board[i - 1, j + 1] == currentPlayer.id) &&
                        (board[i - 2, j + 2] == currentPlayer.id) &&
                        (board[i - 3, j + 3] == currentPlayer.id))
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

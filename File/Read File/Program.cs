using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("studenti.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            string line, classe;
            int i = 0, j, line_n = 0;

            //Conto linee
            while (sr.ReadLine() != null)
            {
                line_n++;
            }

            string [,] val = new string [line_n,7];

            fs.Position = 0;

            line = sr.ReadLine();

            while (line != null)
            {
                var values = line.Split(';');
                
                j = 0;
                
                foreach (var v in values)
                {
                    val [i,j] = v;
                    j++;
                }

                line = sr.ReadLine();

                i++;
            }

            //Stampo tutti gli studenti
            for(i = 0; i < line_n; i++)
            {
                for(j = 0; j < 7; j++)
                {
                    Console.WriteLine(val[i,j]);
                }
                Console.WriteLine();
            }

            //Stampo studenti provincia NO
            Console.WriteLine("Studenti in provincia di Novara\n");
            for (i = 0; i < line_n; i++)
            {
                if (val[i,2] == "NO")
                {
                    for (j = 0; j < 7; j++)
                    {
                        Console.WriteLine(val[i, j]);
                    }
                    Console.WriteLine();
                }
            }

            //Stampo studenti di una classe
            Console.Write("Inserire una classe --> ");
            classe = Console.ReadLine();

            Console.WriteLine();

            for (i = 0; i < line_n; i++)
            {
                if (val[i, 6] == classe)
                {
                    for (j = 0; j < 7; j++)
                    {
                        Console.WriteLine(val[i, j]);
                    }
                    Console.WriteLine();
                }
            }

            sr.Close();

            Console.ReadKey();
        }
    }
}

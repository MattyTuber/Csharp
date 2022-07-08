using System;

namespace Calcoli
{
    class Program
    {
        static void Main(string[] args)
        {
            int c;

            Console.Write("Inserire un numero --> ");
            int a = Int32.Parse(Console.ReadLine());

            Console.Write("Inserire un altro numero --> ");
            int b = Int32.Parse(Console.ReadLine());

            c = a + b;

            Console.Write("La somma da: ");
            Console.WriteLine(c);
        }
    }
}

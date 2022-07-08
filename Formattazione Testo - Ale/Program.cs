/* Alessia Andreis 4 BIN 21/09/2021 */

using System;
using System.IO;
namespace formattazione_testo
{
    class Program
    {
        static void Main(string[] args)
        {
            int conta;

            string[]righe=File.ReadAllLines(@"C:\Users\aless\Desktop\Fauser\Informatica\File\formattazione testo\input.txt");
            
            foreach(string riga in righe){
                conta = riga.Length;
                File.AppendAllText(@"C:\Users\aless\Desktop\Fauser\Informatica\File\formattazione testo\output.txt", riga+conta+'\n');
            }
        }
    }
}

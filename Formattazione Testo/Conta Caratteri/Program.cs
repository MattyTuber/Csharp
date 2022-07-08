using System;
using System.IO;

int conta;

string[] righe = File.ReadAllLines("C:\\Users\\MattyTuber\\OneDrive - ISTITUTO FAUSER NOVARA\\Desktop\\Teo\\Software Developing\\C#\\Formattazine Testo\\Conta Caratteri\\input.txt");

foreach(string riga in righe)
{
    conta = riga.Length;

    File.AppendAllText("C:\\Users\\MattyTuber\\OneDrive - ISTITUTO FAUSER NOVARA\\Desktop\\Teo\\Software Developing\\C#\\Formattazine Testo\\Conta Caratteri\\output.txt", riga + " " + conta + "\n");
}
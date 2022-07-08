using System;
using static System.Console;

internal class Uno_Calcolatrice
{
    public static void Main()
    {

        WriteLine("Calcolatrice console in C #\r");
        WriteLine("--------------------------\n");

        Write("Digitare un numero e quindi premere INVIO\t");
 
        int num1 = Convert.ToInt32(ReadLine());
 
        Write("Digitare un altro numero e quindi premere INVIO\t");
        int num2 = Convert.ToInt32(ReadLine());

        WriteLine("Scegliere un'opzione dall'elenco seguente:");
        WriteLine("\t1 - Somma");
        WriteLine("\t2 - Sottrazione");
        WriteLine("\t3 - Moltiplicazione");
        WriteLine("\t4 - Divisione");
        Write("La tua opzione? ");

        switch (ReadLine())
        {
            case "1":
                WriteLine($"Il tuo risultato: {num1} + {num2} = " + (num1 + num2));
                break;

            case "2":
                WriteLine($"Il tuo risultato: {num1} - {num2} = " + (num1 - num2));
                break;

            case "3":
                WriteLine($"Il tuo risultato: {num1} * {num2} = " + (num1 * num2));
                break;

            case "4":
                WriteLine($"Il tuo risultato: {num1} / {num2} = " + (num1 / num2));
                break;

        }

        ReadLine();
    }
}
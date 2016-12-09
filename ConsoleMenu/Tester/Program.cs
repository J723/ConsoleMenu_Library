using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menu;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fields = new string[]
            {
                "Nome",
                "Cognome",
                "Classe",
            };

            Console.WriteLine(ConsoleMenu.Selection(fields, ConsoleColor.Black, ConsoleColor.DarkRed, ">", "<"));
            Console.ReadLine();
        }
    }
}

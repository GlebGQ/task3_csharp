using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace task3_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game;
            try
            {
                game = new Game(args);
                game.StartGame();
                Console.Read();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
            }

        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassEffectTheGhurstRebellion
{
    class Program
    {
        static void Main(string[] args)
        {
            // set the console to dark blue and the text to white
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            
            // create a new Game object
            Game game = new Game();
        }
    }
}

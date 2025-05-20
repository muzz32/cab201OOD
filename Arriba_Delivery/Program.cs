using System.Net;

namespace Arriba_Delivery
{
    /// <summary>
    /// The main entry point of the class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Will start the program by running menu.StartProgram
        /// </summary>
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.StartProgram();
        }
    }
}


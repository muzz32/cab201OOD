using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Arriba_Delivery
{
    class CMD
    {
        public static void Display(string msg){
            Console.WriteLine(msg);
        }
        public static string StrIn(string errMsg, string retryMsg)
        {
            string? input = Console.ReadLine();
            while (input == null)
            {
                Display(errMsg);
                Display(retryMsg);
                input = Console.ReadLine();
            }
            return input;
        }
        public static string EmptyStrIn()
        {
            return Console.ReadLine() ?? "";
        }
        public static int IntIn(string errMsg, string retryMsg)
        {
            string? input = Console.ReadLine();
            while (!int.TryParse(input, out int output) || input == null)
            {
                Display(errMsg);
                Display(retryMsg);
                input = Console.ReadLine();
            }
            return int.Parse(input);
        }
        public static float FloatIn(string errMsg, string retryMsg)
        {
            string? input = Console.ReadLine();
            while (!float.TryParse(input, out float output) || input == null)
            {
                Display(errMsg);
                Display(retryMsg);
                input = Console.ReadLine();
            }
            return float.Parse(input);
        }

        public static int Choice(string title ,params string[] options) 
        {

            if (options.Length <= 0)
            {
                return 0;
            }

            Display(title);

            for (int i = 0; i < options.Length; i++)
            {
                Display($"{i+1}: {options[i]}");
            }
            Display($"Please enter a choice between 1 and {options.Length}:");
            int choice = IntIn("Invalid choice.", $"Please enter a choice between 1 and {options.Length}:");
            while (choice > options.Length || choice < 1) 
            {
                Display("Invalid choice.");
                Display($"Please enter a choice between 1 and {options.Length}:");
                choice = IntIn("Invalid choice.", $"Please enter a choice between 1 and {options.Length}:");
            }
            return choice;
        }


        
    }
}

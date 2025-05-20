using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Arriba_Delivery
{
    class CMD
    {
        public static void Display(string msg){
            if (msg != "")
            {
                Console.WriteLine(msg);
            }
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

        public static string ValidateInput(string regularExpression, string prompt, string errorMsg, bool notempty = true)
        {
            string input;
            do
            {
                Display(prompt);
                input = notempty ? StrIn(errorMsg, prompt) :  EmptyStrIn();

                if (!notempty && input == "")
                {
                    return input;
                }
                
                if (!Regex.IsMatch(input, regularExpression))
                {
                    Display(errorMsg);
                }
                else
                {
                    return input;
                }
            } while (true);
        }

        public static int ValidateInput(int min, int max, string prompt, string errorMsg)
        {
            do
            {
                Display(prompt);
                int input = IntIn(errorMsg, prompt);
                if (input < min || input > max)
                {
                    Display(errorMsg);
                }
                else
                {
                    return input;
                }
            } while (true);
        }
        public static float ValidateInput(float min, float max, string prompt, string errorMsg)
        {
            do
            {
                Display(prompt);
                float input = FloatIn(errorMsg, prompt);
                if (input < min || input > max)
                {
                    Display(errorMsg);
                }
                else
                {
                    return input;
                }
            } while (true);
        }
        public static string ValidateInput(int min, string regex,string prompt, string errorMsg, bool mustbemin)
        {
            string input;
            do
            {
                Display(prompt);
                input = StrIn(errorMsg, prompt);
                bool firstparam = mustbemin ? input.Length != min: input.Length < min;
                if (firstparam || !Regex.IsMatch(input, regex))
                {
                    Display(errorMsg);
                }
                else
                {
                    return input;
                }
            } while (true);
        }

        public static string ValidateEmail(List<User> users)
        {
            string email;
            do
            {
                email = ValidateInput(@"^.+@.+$", "Please enter your email address:", "Invalid email address.");
                if (users.Any(i => i.email == email))
                {
                    Display("This email address is already in use.");
                }
                else
                {
                    return email;
                }
            } while (true);
        }

        public static string ValidatePassword()
        {
            do
            {
                string password = ValidateInput(8,@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]+$", Consts.passwdprompt, "Invalid password.", false);
                Display("Please confirm your password:");
                if (password != StrIn("Invalid password.", "Please confirm your password:"))
                {
                    Display("Passwords do not match.");
                }
                else
                {
                    return password;
                }
            } while (true);
        }

        public static string[] FormatedList<T>(List<T> inputlist, Func<T, string> format)
        {
            List<string> stringitems = new List<string>();
            foreach (var item in inputlist)
            {
                stringitems.Add(format(item));
            }
            return stringitems.ToArray();
        }
        
        public static void DisplayObjects<T>(List<T> objects, Func<T, string> format, string emptymsg)
        {
            string output = "";
            if (objects.Count == 0)
            {
                Display(emptymsg);
            }
            foreach (var item in objects)
            {
                output += format(item);
            }
            Display(output);
        }
        
        public static (string[], List<T>) GetOptionsAndList<T>(Func<T, bool> condition, List<T> objects, Func<T, string> format, string finaloption)
        {
            List<string> stringitems = new List<string>();
            List<T> filteredlist = new List<T>();
            foreach (var item in objects)
            {
                if (item is T genericitem && condition(genericitem))
                {
                    filteredlist.Add(item);
                    stringitems.Add(format(genericitem));
                }
            }
            stringitems.Add(finaloption);
            return (stringitems.ToArray(), filteredlist);
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

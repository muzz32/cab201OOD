namespace Arriba_Delivery
{
    /// <summary>
    /// A class for handling command line inputs and outputs.
    /// As this project is a prototype, this class would
    /// likely be replaced with a GUI class. All methods
    /// are static so a Cmd class cant be initialised. 
    /// </summary>
    static class Cmd
    {
        /// <summary>
        /// Writes out a message to the line if the message isn't empty.
        /// </summary>
        /// <param name="msg">The message to be displayed</param>
        public static void Display(string msg){
            if (msg != "")
            {
                Console.WriteLine(msg);
            }
        }
        
        /// <summary>
        /// Returns a string only if the input isn't null, otherwise the
        /// user will be reprompted.
        /// </summary>
        /// <param name="errMsg">The message displayed if the string is null</param>
        /// <param name="retryMsg">The message displayed to prompt the user to enter a new string</param>
        /// <returns>A non null string</returns>
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
        
        /// <summary>
        /// Allows for an empty string to be entered
        /// </summary>
        /// <returns>A non null empty string</returns>
        public static string EmptyStrIn()
        {
            return Console.ReadLine() ?? "";
        }
        
        /// <summary>
        /// Returns an integer only if the input can be parsed as an int
        /// </summary>
        /// <param name="errMsg">The message displayed if the integer is null</param>
        /// <param name="retryMsg">The message displayed to prompt the user to enter a new integer</param>
        /// <returns>A non null integer</returns>
        public static int IntIn(string errMsg, string retryMsg)
        {
            string? input = Console.ReadLine();
            int output;
            while (!int.TryParse(input, out output))
            {
                Display(errMsg);
                Display(retryMsg);
                input = Console.ReadLine();
            }
            return output;
        }
        /// <summary>
        /// Same process as IntIn only for floats
        /// </summary>
        /// <returns>A non null float</returns>
        public static float FloatIn(string errMsg, string retryMsg)
        {
            string? input = Console.ReadLine();
            float output;
            while (!float.TryParse(input, out output))
            {
                Display(errMsg);
                Display(retryMsg);
                input = Console.ReadLine();
            }
            return output;
        }
        
        /// <summary>
        /// Displays a menu of options starting from one, and takes a user input as a
        /// selection if it is within the bound of the options.
        /// </summary>
        /// <param name="title">The menu's title</param>
        /// <param name="options">An array of options</param>
        /// <returns>An int that can be used to determine the users choice</returns>
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

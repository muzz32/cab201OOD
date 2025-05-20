using System.Text.RegularExpressions;

namespace Arriba_Delivery;

/// <summary>
/// A class to validate inputs given certain parameters
/// </summary>
class Validate
{
    /// <summary>
    /// Validates a string input of any length to a regular expression
    /// </summary>
    /// <param name="regularExpression">A regex pattern used to determine what characters are and aren't allowed</param>
    /// <param name="prompt">The initial prompt for an input</param>
    /// <param name="errorMsg">A message if the prompt does not match the parameter</param>
    /// <param name="notempty">Defines if the string can be empty or not</param>
    /// <returns>A validated string</returns>
    public static string Input(string regularExpression, string prompt, string errorMsg, bool notempty = true)
    {
        string input;
        do
        {
            Cmd.Display(prompt);
            input = notempty ? Cmd.StrIn(errorMsg, prompt) : Cmd.EmptyStrIn(); //Determines what input method to use depending on if notempty is true or not

            if (!notempty && input == "") 
            {
                return input;
            }
                
            if (!Regex.IsMatch(input, regularExpression)) //If the input doesn't match the regular expresion it will be invalid
            {
                Cmd.Display(errorMsg);
            }
            else
            {
                return input;
            }
        } while (true);
    }

    /// <summary>
    /// Validates an int input if it's within the set size
    /// </summary>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <param name="prompt">The initial prompt</param>
    /// <param name="errorMsg">The error message if the input is invalid</param>
    /// <returns>A validated int</returns>
    public static int Input(int min, int max, string prompt, string errorMsg)
    {
        do
        {
            Cmd.Display(prompt);
            int input = Cmd.IntIn(errorMsg, prompt);
            if (input < min || input > max) //Checks if the input is out of bounds
            {
                Cmd.Display(errorMsg);
            }
            else
            {
                return input;
            }
        } while (true);
    }

    /// <summary>
    /// Validates a float input if its within a set range
    /// </summary>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <param name="prompt">The initial prompt</param>
    /// <param name="errorMsg">The error message if the input is invalid</param>
    /// <returns>A validated float</returns>
    public static float Input(float min, float max, string prompt, string errorMsg)
    {
        do
        {
            Cmd.Display(prompt);
            float input = Cmd.FloatIn(errorMsg, prompt);
            if (input < min || input > max)
            {
                Cmd.Display(errorMsg);
            }
            else
            {
                return input;
            }
        } while (true);
    }

    /// <summary>
    /// Validates an input if it has an @ in between characters and if it is
    /// isn't already used by another user.
    /// </summary>
    /// <param name="users">A list of all users</param>
    /// <returns>A valid email as a string</returns>
    public static string Email(List<User> users)
    {
        string email;
        do
        {
            email = Input(@"^.+@.+$", "Please enter your email address:", "Invalid email address.");
            //regex checks if there's characters before and after one @ 
            if (users.Any(i => i.Email == email)) //Checks if the email is in the list of users
            {
                Cmd.Display("This email address is already in use.");
            }
            else
            {
                return email;
            }
        } while (true);
    }

    /// <summary>
    /// Validates a password if it matches a regular expression and is typed exactly the same twice
    /// </summary>
    /// <returns>A valid password</returns>
    public static string Password()
    {
        do
        {
            string password = Input(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]{8,}$", Consts.Passwdprompt, "Invalid password.");
            //Regex checks if the password contains atleast one capital, lowercase, and numeber and only contains letters and numbers. It also has to be at least 8 digits
            Cmd.Display("Please confirm your password:");
            if (password != Cmd.StrIn("Invalid password.", "Please confirm your password:"))
            {
                Cmd.Display("Passwords do not match.");
            }
            else
            {
                return password;
            }
        } while (true);
    }
}
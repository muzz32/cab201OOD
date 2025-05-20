namespace Arriba_Delivery;

/// <summary>
/// A class for formatting and filtering lists and
/// arrays. Useful for getting options for Cmd.Choice 
/// </summary>
class Format
{
    /// <summary>
    /// Formats a list of any type into an array of strings when given a function
    /// for the strings structure.
    /// </summary>
    /// <param name="inputlist">The raw list to be formatted</param>
    /// <param name="format">A function that turns an arbitrary type into a string</param>
    /// <param name="endmsgs">Any strings to be added to the array that aren't in the list</param>
    /// <typeparam name="T">The type of item to be formatted. Often an object such as Order or Review</typeparam>
    /// <returns></returns>
    public static string[] List<T>(List<T> inputlist, Func<T, string> format, params string[] endmsgs)
    {
        List<string> stringitems = new List<string>();
        foreach (var item in inputlist)
        {
            stringitems.Add(format(item));
        }
        foreach (string msgs in endmsgs)
        {
            stringitems.Add(msgs);
        }
        return stringitems.ToArray();
    }
    
    /// <summary>
    /// Builds a string from a list of objects to be used by Cmd.Display.
    /// </summary>
    /// <param name="objects">A list of objects</param>
    /// <param name="format">A function that defines how each object will be converted into a string</param>
    /// <param name="msg">A title for the string. Can be "" or null if not needed.</param>
    /// <param name="emptymsg">A message for if the list is empty</param>
    /// <typeparam name="T">The type of object to be formatted. Often an object such as Order or Review</typeparam>
    /// <returns>A formated string if the list isn't empty, or the empty message if it is</returns>
    public static string DisplayObjects<T>(List<T> objects, Func<T, string> format, string msg, string emptymsg)
    {
        string output = msg;
        if (objects.Count == 0)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return emptymsg;
            }

            return msg + "\n" + emptymsg;
        }
        foreach (var item in objects)
        {
            output += format(item);
        }

        return output;
    }

    /// <summary>
    /// Filters a list of any type and returns the list and an array of strings.
    /// Useful when getting a choice from the user that will select an option from the list.
    /// </summary>
    /// <param name="condition">A bool function that is used to filter the list. i.e. the object will be added if the function is true</param>
    /// <param name="objects">A list of objects to be filtered</param>
    /// <param name="format">The format of how the objects will be converted into strings for the array</param>
    /// <param name="finaloption">Any extra strings that need to be added to the array that aren't in the list</param>
    /// <typeparam name="T">An arbitrary type</typeparam>
    /// <returns>A tuple of a string array and an arbitrary list</returns>
    public static (string[], List<T>) GetOptionsAndList<T>(Func<T, bool> condition, List<T> objects, Func<T, string> format, params string[] finaloption)
    {
        List<string> stringitems = new List<string>();
        List<T> filteredlist = new List<T>();
        foreach (var item in objects)
        {
            if (item is { } genericitem && condition(genericitem))
            {
                filteredlist.Add(item);
                stringitems.Add(format(genericitem));
            }
        }

        foreach (string option in finaloption)
        {
            stringitems.Add(option);
        }
            
        return (stringitems.ToArray(), filteredlist);
    }
}
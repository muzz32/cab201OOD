namespace Arriba_Delivery;

class Register
{
    private static (string name, int age, string mobile, string email, string password) General(List<User> users)
    {
        string name = CMD.ValidateInput(@"^(?=.*[a-zA-Z])[a-zA-Z\s'-]+$", "Please enter your name:", "Invalid name.");
        int age = CMD.ValidateInput(18, 100, "Please enter your age (18-100):", "Invalid age.");
        string email = CMD.ValidateEmail(users);
        string mobile = CMD.ValidateInput(10, @"^0[0-9]+$", "Please enter your mobile phone number:", "Invalid phone number.", true);
        string password = CMD.ValidatePassword();
        return (name, age, mobile, email, password);
    }

    public static Customer Customer(List<User> users)
    {
       var (name, age, mobile, email, password) = General(users);
       string location = CMD.ValidateInput(@"^[0-9]+,[0-9]+$", "Please enter your location(in the form of X, Y):", "Invalid location.");
       return new Customer(name, age, mobile, email, password, location);
    }
    
    public static Deliverer Deliverer(List<User> users)
    {
        var (name, age, mobile, email, password) = General(users);
        string licence = CMD.ValidateInput(@"^(?=.*[A-Z0-9])[A-Z0-9 ]{1,8}$", "Please enter your licence plate:", "Invalid licence plate.");
        return new Deliverer(name, age, mobile, email, password, licence);
    }
    
    public static Client Client(List<User> users)
    {
        var (name, age, mobile, email, password) = General(users);
        string restaurant = CMD.ValidateInput(@"^(?=.*\S).*$", "Please enter your restaurant's name:", "Invalid restaurant name.");
        int style = CMD.Choice("Please select your restaurant's style:", Consts.styles);
        string location = CMD.ValidateInput(@"^[0-9]+,[0-9]+$",  "Please enter your location(in the form of X, Y):", "Invalid location.");
        return new Client(name, age, mobile, email, password, restaurant, location, style);
    }
    
}
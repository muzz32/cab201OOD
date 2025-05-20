namespace Arriba_Delivery;
/// <summary>
/// Used to handle inputs relevant to class initiation.
/// Mainly used for registering Users but also handles making orders and
/// reviews
/// </summary>
class Register
{
    public static Order Temporder = new (); //A temporary order that is used if the customer makes a new order
    
    /// <summary>
    /// Universal registration prompts for users
    /// </summary>
    /// <param name="users">A list of all users to ensure no double up emails</param>
    /// <returns>A tuple of the users name, age, mobile, email, and password, all of which are validated</returns>
    private static (string name, int age, string mobile, string email, string password) General(List<User> users)
    {
        string name = Validate.Input(@"^(?=.*[a-zA-Z])[a-zA-Z\s'-]+$", "Please enter your name:", "Invalid name.");
        //Regex checks if string contains at least one letter and then only contains letters, whitespace, apostrophes and dashes.
        int age = Validate.Input(18, 100, "Please enter your age (18-100):", "Invalid age.");
        string email = Validate.Email(users);
        string mobile = Validate.Input( @"^0[0-9]{9}$", "Please enter your mobile phone number:", "Invalid phone number.");
        //regex ensures the string contains only numbers, starts with a zero and is 10 characters long
        string password = Validate.Password();
        return (name, age, mobile, email, password);
    }

    /// <summary>
    /// Registers a new customer. Continues off General.
    /// </summary>
    /// <param name="users">A list of all users</param>
    /// <returns>A new Customer</returns>
    public static Customer Customer(List<User> users)
    {
       var (name, age, mobile, email, password) = General(users);
       string location = Validate.Input(@"^[0-9]+,[0-9]+$", "Please enter your location(in the form of X, Y):", "Invalid location.");
       return new Customer(name, age, mobile, email, password, location);
    }
    
    /// <summary>
    /// Registers a new deliverer. Continues off General.
    /// </summary>
    /// <param name="users">A list of all users</param>
    /// <returns>A new deliverer</returns>
    public static Deliverer Deliverer(List<User> users)
    {
        var (name, age, mobile, email, password) = General(users);
        string licence = Validate.Input(@"^(?=.*[A-Z0-9])[A-Z0-9 ]{1,8}$", "Please enter your licence plate:", "Invalid licence plate.");
        //Regex checks that the string contains atleast one of each capital letters and numbers, only contains capital letters and numbers, and is between 1 and 8 digits long
        return new Deliverer(name, age, mobile, email, password, licence);
    }
    
    /// <summary>
    /// Registers a new client. Continues off General.
    /// </summary>
    /// <param name="users">A list of all users</param>
    /// <returns>A new Client</returns>
    public static Client Client(List<User> users)
    {
        var (name, age, mobile, email, password) = General(users);
        string restaurant = Validate.Input(@"^(?=.*\S).*$", "Please enter your restaurant's name:", "Invalid restaurant name.");
        int style = Cmd.Choice("Please select your restaurant's style:", Consts.Styles);
        string location = Validate.Input(@"^[0-9]+,[0-9]+$",  "Please enter your location(in the form of X, Y):", "Invalid location.");
        return new Client(name, age, mobile, email, password, restaurant, location, style);
    }

    /// <summary>
    /// Used for when creating a new menu item and handles the price prompt
    /// </summary>
    /// <param name="foodname">The already defined food name. This method won't be called if this was blank</param>
    /// <returns>A new Food</returns>
    public static Food MenuItem(string foodname)
    {
        float price = Validate.Input(0f,999.99f,"Please enter the price of the new item(without the $):", "Invalid price.");
        return new Food(foodname, price);
        
    }

    /// <summary>
    /// Used to handle prompts and inputs for a customer making a new order. 
    /// </summary>
    /// <param name="client">The selected client that the order will be made by</param>
    /// <param name="allorders">A list of all orders to get the id</param>
    /// <param name="customer">The customer making the order</param>
    /// <returns>True if the order was actually made, so the program knows to refer to the Temporder in this class as the new order. False if the Customer cancels.</returns>
    public static bool MakeOrder(Client client, List<Order> allorders, Customer customer)
    {
        float totalprice = 0;
        int length = client.MenuItems.Count;
        List<Food> items = new List<Food>();
        do
        {
            int choice = Cmd.Choice(
                "Current order total: $" + totalprice.ToString("F2"), 
                Format.List(client.MenuItems, food =>$"${food.Price:F2}  {food.Name}", "Complete order", "Cancel order"));
            if (choice == length + 1) //If the customer chooses to complete the order, save the order as Temporder and return true
            {
                Temporder = new Order(client, totalprice, items, allorders.Count + 1, customer);
                return true;
            }
            if (choice == length + 2) //If the customer cancels the order, return false so the program doesn't save the temporder
            {
                return false;
            }
            //Otherwise, add the selected food item and price to the order
            Food selecteditem = client.MenuItems[choice - 1];
            Cmd.Display($"Adding {selecteditem.Name} to order.");
            int quantity = Validate.Input(0, Int32.MaxValue, "Please enter quantity (0 to cancel):", "Invalid quantity.");
            if (quantity > 0)
            {
                Food? existingitem = items.Find(food => food.Name == selecteditem.Name);
                if (existingitem != null)
                {
                    existingitem.ChangeQuantity(existingitem.Quantity + quantity); //If the food is already in the order, just raise its quantity accordingly
                }
                else
                {
                    Food ordereditem = new Food(selecteditem.Name, selecteditem.Price, quantity);
                    items.Add(ordereditem);
                }
                Cmd.Display($"Added {quantity} x {selecteditem.Name} to order.");
                totalprice += selecteditem.Price * quantity;
            }
        } while (true);
    }

    /// <summary>
    /// Used to handle prompts and inputs when a customer is making a review
    /// </summary>
    /// <param name="customer">The customer making the review</param>
    /// <param name="rating">The predefined rating. This method would not be called if it was zero</param>
    /// <returns>A new review</returns>
    public static Review Review(Customer customer, int rating)
    {
        Cmd.Display("Please enter a comment to accompany this rating:");
        string comment = Cmd.EmptyStrIn();
        return new Review(customer.Name, rating, comment);
    }
    
}
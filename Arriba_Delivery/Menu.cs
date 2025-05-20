using System.Text.RegularExpressions;

namespace Arriba_Delivery
{
    /// <summary>
    /// Handles a large portion of user inputs and outputs in the form of outputs.
    /// Most of these are in the form of command line menu's, which are built by Cmd.Choice
    /// and then the output of those are used in this class to decide what that choice does.
    /// </summary>
    class Menu
    {
        private List<User> AllUsers= new (); //A list of all users in the instance of the program
        private List<Order> CurrOrders= new (); //All current orders that have not been delivered
        private List<Order> AllOrders= new (); //All orders that have been made in the current instance

        /// <summary>
        /// Starts the program. Will only break when the user clicks exit in the
        /// start menu.
        /// </summary>
        public void StartProgram()
        {
            bool running = true;
            Cmd.Display("Welcome to Arriba Eats!");
            while (running){
                running = StartMenu(); 
            }
            Cmd.Display("Thank you for using Arriba Eats!");
        }
        
        /// <summary>
        /// The first menu in the program. Gives the use an option to
        /// log in, register, or exit. 
        /// </summary>
        /// <returns>True to continue the program unless the user chooses exit, in which it'll return false and end the StartProgram loop.</returns>
        private bool StartMenu() 
        {
            switch (Cmd.Choice("Please make a choice from the menu below:", Consts.StartMenu))
            {
                case 1:
                    LoginMenu();
                    break;
                case 2:
                    RegisterMenu();
                    break;
                case 3:
                    return false;
            }
            return true;
        }
        /// <summary>
        /// The login menu. Takes an email and a password and if those match a registered user they will be signed in as
        /// the according user, otherwise they will be taken back to the start menu.
        /// </summary>
        private void LoginMenu() 
        {
            Cmd.Display("Email:");
            string loginemail = Cmd.StrIn("Invalid email.", "Email:");
            Cmd.Display("Password:");
            string loginpasswd = Cmd.StrIn("Invalid password.", "Password:");
            User? registereduser = null;
            foreach(User user in AllUsers)
            {
                if (user.Email == loginemail && user.Password == loginpasswd) //Checks if the entered email and password matches a registered user
                {
                    registereduser = user;
                    break;
                }
            }
            if (registereduser == null)
            {
                Cmd.Display("Invalid email or password.");
                return; //Takes the user back to the main menu if the email and password doesn't match a registered user
            }
            switch (registereduser) //Takes the user to their according menu if the email and password matches a user
            {
                case Customer customer:
                    CustomerMenu(customer);
                    break;
                case Deliverer deliverer:
                    DelivererMenu(deliverer);
                    break;
                case Client client:
                    ClientMenu(client);
                    break;
            }
            Cmd.Display("You are now logged out."); //When the user leaves their menu (Or logs out) this message will be displayed before going back to the start menu
        }
        
        /// <summary>
        /// The Registration menu. Takes the user to register as a customer, deliverer or client, or to go back to the start menu
        /// </summary>
        private void RegisterMenu() 
        {
            switch (Cmd.Choice("Which type of user would you like to register as?", Consts.RegMenu))
            {
                case 1:
                    UserReg(Consts.UserType.Customer);
                    break;
                case 2:
                    UserReg(Consts.UserType.Deliverer);
                    break;
                case 3:
                    UserReg(Consts.UserType.Client);
                    break;
                case 4:
                    return;
            }
        }

        /// <summary>
        /// Registers the user as their chosen user type with the Register class and adds that user to
        /// the list of all users.
        /// </summary>
        /// <param name="userType">The usertype chosen in the register menu. Uses the Const's,UserType filed to do this</param>
        private void UserReg(Consts.UserType userType)
        {
            User? registereduser = null;
            switch (userType)
            {
                case Consts.UserType.Customer: 
                    registereduser = Register.Customer(AllUsers);
                    break;
                case Consts.UserType.Deliverer: 
                    registereduser = Register.Deliverer(AllUsers);
                    break;
                case Consts.UserType.Client:
                    registereduser = Register.Client(AllUsers);
                    break;
            }
            if (registereduser != null)
            {
                Cmd.Display($"You have been successfully registered as a {userType.ToString().ToLower()},  {registereduser.Name}!");
                AllUsers.Add(registereduser);
            }
        }
        
        /// <summary>
        /// The menu for the customer. includes the ability to get info, select a restaurant to order from,
        /// see their active orders, review a delivered order and logout.
        /// </summary>
        /// <param name="customer">The customer that will be using this menu</param>
        private void CustomerMenu(Customer customer)
        {
            Cmd.Display(customer.WelcomeMsg());
            while (true)
            {
                switch (Cmd.Choice("Please make a choice from the menu below:", Consts.CustomerMenu))
                {
                    case 1:
                        Cmd.Display(customer.GetInfo());
                        break;
                    case 2:
                        PickResturantSort(customer);
                        break;
                    case 3:
                        Cmd.Display(Format.DisplayObjects(customer.Orders, order => order.GetInfoCustomer(),"","You have not placed any orders."));
                        break;
                    case 4:
                        ReviewMenu(customer);
                        break;
                    case 5:
                        return;
                }
            }
        }


        /// <summary>
        /// A menu to select how the customer want the restaurants sorted. This includes by restaurant name,
        /// by distance from the user, by the style and by the average rating. If the user then selects a restaurant this will
        /// take them to the order menu for that specific restaurant.
        /// </summary>
        /// <param name="customer">The customer that will be selecting a restaurant to order from</param>
        private void PickResturantSort(Customer customer)
        {
            List<Client> clients = new List<Client>();
            switch (Cmd.Choice("How would you like the list of restaurants ordered?", Consts.RestaurantSortMenu)) //Will sort all the clients in the user list by a selected way
            {
                case 1:
                    clients = Client.SortedRestaurant(AllUsers, client => client.Restaurant);
                    break;
                case 2:
                    clients = Client.SortedRestaurant(AllUsers, client => Gps.GetDistance(customer.Location, client.Location));
                    break;
                case 3:
                    clients = Client.SortedRestaurant(AllUsers, client => client.Style);
                    break;
                case 4:
                    clients = Client.SortedRestaurant(AllUsers, client => client.Rating, true);
                    break;
                case 5:
                    return; 
            }
            string[] displayclients = Format.List(
                clients, 
                client => 
                $"" + 
                $"{client.Restaurant,-20}" + 
                $"{client.Location,-7}" +
                $"{Gps.GetDistance(customer.Location, client.Location),-7}" +
                $"{Consts.Styles[client.Style - 1],-12}" +
                $"{client.Rating,-6}", 
                "Return to the previous menu"
                ); //Converts the list of ordered clients into a table like format array
            
            int index = Cmd.Choice(Consts.RestaurantOptionsTitle, displayclients);
            if (index == clients.Count + 1) return; //If the user picks"Return to the previous menu", they will be taken back to the customer menu
            Cmd.Display($"Placing order from {clients[index - 1].Restaurant}.");
            OrderMenu(customer, clients[index-1]); //Otherwise they will go to the selected restaurants order menu
        }
        
        /// <summary>
        /// A menu for the customer to either order from the selected restaurant or see their reviews.
        /// </summary>
        /// <param name="customer">The customer that is ordering</param>
        /// <param name="client">The client/restaurant that will be ordered from</param>
        private void OrderMenu (Customer customer, Client client)
        {
            do
            {
                switch (Cmd.Choice("", Consts.RestaurantOptionsMenu))
                {
                    case 1:
                        bool ordered = Register.MakeOrder(client, AllOrders, customer); 
                        //Starts making an Order with the registering class. If the customer doesn't cancel and makes the order it will return true
                        if (ordered)
                        {
                            Order neworder = Register.Temporder;
                            customer.AddOrder(neworder);
                            client.AddOrder(neworder);
                            AllOrders.Add(neworder);
                            CurrOrders.Add(neworder);
                            Cmd.Display("Your order has been placed. Your order number is #" + neworder.Id + ".");
                            //Handles the newly created order by adding it to all relevant order lists
                        }
                        break;
                    case 2:
                        Cmd.Display(Format.DisplayObjects(client.Reviews, review => review.GetInfo()+"\n", "","No reviews have been left for this restaurant."));
                        break;
                    case 3:
                        return;
                }
            } while(true);
        }

        /// <summary>
        /// A menu for the customer to create a new review for a delivered, unreviewed order
        /// </summary>
        /// <param name="customer">The customer that will be reviewing</param>
        private void ReviewMenu(Customer customer)
        {
            (string[], List<Order>) options = customer.GetUnreviewedOrders();
            int choice = Cmd.Choice("Select a previous order to rate the restaurant it came from:", options.Item1);
            if (choice < options.Item1.Length)
            {
                Order selectedorder = options.Item2[choice - 1];
                Cmd.Display($"You are rating order #{selectedorder.Id} from {selectedorder.Client.Restaurant}:");
                Cmd.Display(selectedorder.GetContents());
                int rating = Validate.Input(0, 5, "Please enter a rating for this restaurant (1-5, 0 to cancel):","Invalid rating");
                if (rating > 0)
                {
                    Review newreview = Register.Review(customer, rating);
                    selectedorder.Client.AddReview(newreview);
                    selectedorder.Reviewed();
                    Cmd.Display($"Thank you for rating {selectedorder.Client.Restaurant}.");
                }
            }
        }
        
        /// <summary>
        /// The Deliverers menu. Has options for the deliverer to get info about themselves, find a new order to assign to,
        /// notify the relevant client and customer that they have arrived, and logout.
        /// </summary>
        /// <param name="deliverer">The current deliverer</param>
        private void DelivererMenu(Deliverer deliverer)
        {
            Cmd.Display(deliverer.WelcomeMsg());
            while (true)
            {
                switch (Cmd.Choice("Please make a choice from the menu below:", Consts.DelivererMenu))
                {
                    case 1:
                        Cmd.Display(deliverer.GetInfo());
                        break;
                    case 2:
                        OrderSelectMenu(deliverer);
                        if (deliverer.Order != null)
                        {
                            CurrOrders.Remove(deliverer.Order); //Ensures no other deliverers can pick up the same order as this deliverer
                        }
                        break;
                    case 3:
                        Cmd.Display(deliverer.ArriveAtRestaurant());
                        break;
                    case 4:
                        Cmd.Display(deliverer.ArriveAtCustomer());
                        break;
                    case 5:
                        return;
                }
            }
        }

        /// <summary>
        /// A menu for the deliverer to select an order to pick up and deliverer. Will not let them
        /// select anything if they already hav an order.
        /// </summary>
        /// <param name="deliverer"></param>
        private void OrderSelectMenu(Deliverer deliverer)
        {
            if (deliverer.Order != null)
            {
                Cmd.Display("You have already selected an order for delivery.");
            }
            else
            {
                deliverer.UpdateLocation(Validate.Input(@"^[0-9]+,[0-9]+$",  "Please enter your location(in the form of X,Y):", "Invalid location.")); //Updates with a valid location
                string[] options = Format.List(
                    CurrOrders, 
                    order =>
                        $"{order.Id, -7}" +
                        $"{order.Client.Restaurant, -17}" +
                        $"{order.Client.Location, -10}" +
                        $"{order.Customer.Name, -17}" +
                        $"{order.Customer.Location, -7}" +
                        $"{Gps.TotalDistance(deliverer.Location, order.Client.Location, order.Customer.Location)}", 
                    "Return to the previous menu");
                int choice = Cmd.Choice(Consts.OrderOptionsTitle, options);
                if (choice < options.Length)
                {
                    deliverer.GetJob(CurrOrders[choice - 1]);
                    CurrOrders.Remove(deliverer.Order);
                    Cmd.Display($"Thanks for accepting the order. Please head to {deliverer.Order.Client.Restaurant} at {deliverer.Order.Client.Location} to pick it up.");
                }
            }
        }
        
        
        /// <summary>
        /// A menu for the client. Includes options to get info about themselves, add a new item to their menu,
        /// Process the order by either cooking it or notifying that it is cooked, and handing out any orders that are
        /// complete.
        /// </summary>
        /// <param name="client">The client that will be using the menu</param>
        private void ClientMenu(Client client)
        {
            Cmd.Display(client.WelcomeMsg());
            while (true)
            {
                switch (Cmd.Choice("Please make a choice from the menu below:", Consts.ClientMenu))
                {
                    case 1:
                        Cmd.Display(client.GetInfo());
                        break;
                    case 2:  
                        NewItemMenu(client);
                        break;
                    case 3: Cmd.Display(Format.DisplayObjects(client.Orders, order => order.GetInfoClient(), "","Your restaurant has no current orders."));
                        break;
                    case 4:
                        ProcessOrderMenu(client ,0); //Changes orders from "Ordered" to "Cooking"
                        break;
                    case 5:
                        ProcessOrderMenu(client ,1); //Changes orders from "Cooking" to "Cooked"
                        break;
                    case 6:
                        HandOutOrderMenu(client);
                        break;
                    case 7:
                        return;
                }
            }
        }

        /// <summary>
        /// A ui for creating a new item
        /// </summary>
        /// <param name="client">The client that will be adding a new menu item</param>
        private static void NewItemMenu(Client client)
        {
            Cmd.Display(Format.DisplayObjects(client.MenuItems,food =>  $"${food.Price:F2}  {food.Name}", "This is your restaurant's current menu:",""));
            string foodname = Validate.Input(@"^[a-zA-Z-,' ]+$", "Please enter the name of the new item (blank to cancel):", "Invalid item name.", false);
            if (!Regex.IsMatch(foodname, @"^\s*$"))
            {
                Food newfood = Register.MenuItem(foodname); //If the name isn't a blank string it will continue with the item creation
                client.AddItem(newfood);
                Cmd.Display($"Successfully added {newfood.Name} (${newfood.Price:F2}) to menu.");
            }
        }
        
        /// <summary>
        /// Used to select an order to process, which depending on its current status,
        /// will either start or finish cooking the order
        /// </summary>
        /// <param name="client">The current client</param>
        /// <param name="processtype">The current status of the order as an int for Const.Status</param>
        private static void ProcessOrderMenu(Client client,int processtype)
        {
            (string[], List<Order>) options = Format.GetOptionsAndList(order => order.Status == Consts.Status[processtype],
                client.Orders, order => $"Order #{order.Id} for {order.Customer.Name}", "Return to the previous menu");
            int choice = Cmd.Choice(Consts.ProcessingMenu[processtype], options.Item1);
            if (choice < options.Item1.Length)
            {
                Cmd.Display(options.Item2[choice - 1].ProcessOrder(processtype + 1)); //Updates the order with the new status if the user deosnt choose to exit this menu
            }
        }

        /// <summary>
        /// Used to select which order to hand off to a deliverer, which will change its status
        /// to being delivered 
        /// </summary>
        /// <param name="client">The current client</param>
        private static void HandOutOrderMenu(Client client)
        {
            Cmd.Display("These deliverers have arrived and are waiting to collect orders.");
            (string[],List<Order>) arrivedorders = Format.GetOptionsAndList(
                order => order.DelivererArrived, //Only shows orders with deliverers who have arrived at the restaurant
                client.Orders, 
                order => $"Order #{order.Id} for {order.Customer.Name} (Deliverer licence plate: {order.Deliverer.Licence}) (Order status: {order.Status})", 
                "Return to the previous menu");
            int choice = Cmd.Choice("Select an order to indicate that the deliverer has collected it:", arrivedorders.Item1);
            if (choice < arrivedorders.Item1.Length)
            {
                Cmd.Display(client.HandOutOrder(arrivedorders.Item2[choice - 1])); //Hands out a seletced order if the user doesnt exit the menu.
            }
        }
    }
}

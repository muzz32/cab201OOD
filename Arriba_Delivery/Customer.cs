namespace Arriba_Delivery
{
    /// <summary>
    /// An inherited class from User for Customers
    /// </summary>
    class Customer : User
    {
        public string Location { get; } //The customers location
        public List<Order> Orders { get; } //All orders the customer has made
        private float MoneySpent; //Total money spent by the customer
        
        /// <summary>
        /// A default constructor that continues from the abstract User
        /// </summary>
        public Customer()  
        {
            Location = "";
            Orders = new List<Order>();
            MoneySpent = 0f; 
        }

        /// <summary>
        ///  A constructor for the customer
        /// </summary>
        public Customer(string name, int age, string mobile, string email, string password, string location) : base(name, age, mobile,
            email, password)
        {
            Location = location;
            Orders = new List<Order>();
            MoneySpent = 0f;
        }
        
        /// <summary>
        /// A continuation of the general GetInfo Method
        /// </summary>
        /// <returns>General info as well as the customers location and the amount of orders made and money spent</returns>
        public override string GetInfo()
        {
            string start = base.GetInfo();
            return start + 
                   "\nLocation: " + Location +
                   $"\nYou've made {Orders.Count} order(s) and spent a total of ${MoneySpent:F2} here.";
        }

        /// <summary>
        /// Adds a new order to the users list of orders, as well
        /// as adding the orders price to the customers total money spent
        /// </summary>
        /// <param name="order">The new order</param>
        public void AddOrder(Order order)
        {
            MoneySpent += order.Price; 
            Orders.Add(order);
        }
        
        /// <summary>
        /// Calls the Format.GetOptionsAndList function for a list that contains
        /// all orders the user has made that are completed and unreviewed
        /// </summary>
        /// <returns>A tuple of a string array and a list of orders</returns>
        public (string[], List<Order>) GetUnreviewedOrders()
        {
            return Format.GetOptionsAndList(
                order => order.Status == Consts.Status[4] && !order.CustomerReviewed, 
                Orders, 
                order => $"Order #{order.Id} from {order.Client.Restaurant}", 
                "Return to the previous menu"
            );
        }
        

    }
}

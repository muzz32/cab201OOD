namespace Arriba_Delivery
{
    /// <summary>
    /// A type of user specifically for Clients. 
    /// </summary>
    class Client : User
    {
        
        public string Restaurant { get; } //The name of the restaurant 
        public string Location { get; } //The location of the restaurant 
        public int Style { get; } //The style of the restaurant 
        public string Rating {  get; private set; }  //The restaurants average rating
        public List<Food> MenuItems { get; } //A list of the restaurants menu items
        public List<Review> Reviews { get; } //A list of the restaurants reviews
        public List<Order> Orders { get; } //A list of the restaurants current orders

        /// <summary>
        /// A default constructor
        /// </summary>
        public Client()
        {
            Restaurant = "";
            Location = "";
            Style = 0;
            Rating = "-";
            MenuItems = [];
            Reviews = [];
            Orders = [];
        }

        /// <summary>
        ///  An inherited constructor from User with specific client information.
        /// </summary>
        public Client(string name, int age, string mobile, string email, string password, string restaurant, string location, int style) : 
            base(name, age, mobile, email, password)
        {
            Restaurant = restaurant;
            Location = location;
            Style = style;
            Rating = "-";
            MenuItems = [];
            Reviews = [];
            Orders = [];
        }

        /// <summary>
        /// A continuation of the generic GetInfo user method
        /// </summary>
        /// <returns>General user info as well as the restaurants name, style and location</returns>
        public override string GetInfo()
        {
            string start = base.GetInfo();
            return start + 
            "\nRestaurant name: " + Restaurant +
            "\nRestaurant style: " + Consts.Styles[Style - 1] +
            "\nRestaurant location: " + Location;
        }

        /// <summary>
        /// A static Client method for sorting restaurants by a specific field
        /// </summary>
        /// <param name="users">A list of users, usually AllUsers list in the menu class</param>
        /// <param name="sorter">A function to define what the list will be sorted by, usually a lambda function</param>
        /// <param name="descending">If true, the list will be sorted by descending. By default, the list will sort by ascending</param>
        /// <returns>A sorted list of clients</returns>
        public static List<Client> SortedRestaurant(List<User> users, Func<Client, object> sorter, bool descending = false) 
        {
            List<Client> options;
            if (!descending)
            {
                options = users.OfType<Client>().OrderBy(sorter).ThenBy(client => client.Restaurant).ToList();
            }
            else
            {
                options = users.OfType<Client>().OrderByDescending(sorter).ThenBy(client => client.Restaurant).ToList();
            }
            return options; 
        }
        
        /// <summary>
        /// Adds a new item to the restaurants menu
        /// </summary>
        /// <param name="food">The new food item</param>
        public void AddItem(Food food)
        {
            MenuItems.Add(food);
        }
        
        /// <summary>
        /// Adds a new order to an existing list of orders
        /// </summary>
        /// <param name="order">The new order</param>
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        /// <summary>
        /// Updates to status of the order to "Being Delivered" (3) and removes the order
        /// from the list of orders if the status is cooked. Otherwise, it informs the client
        /// that the order hasn't been cooked yet.
        /// </summary>
        /// <param name="selectedorder">The order that is being handed out</param>
        /// <returns>
        /// A message in the form of a string, either saying the order is being delivered or that it
        /// hasn't been cooked.
        /// </returns>
        public string HandOutOrder(Order selectedorder)
        {
            if (selectedorder.Status == Consts.Status[2])
            {
                selectedorder.ProcessOrder(3);
                Orders.Remove(selectedorder);
                return $"Order #{selectedorder.Id} is now marked as being delivered.";

            }
            return "This order has not yet been cooked.";
        }

        /// <summary>
        /// Adds a new review to the clients reviews, and then recalculates
        /// their average rating.
        /// </summary>
        /// <param name="review">The new review that is being added</param>
        public void AddReview(Review review)
        {
            float reviewrating = review.Rating;
            Reviews.Add(review);
            if (Rating == "-" && Reviews.Count == 0)
            {
                Rating = reviewrating.ToString("F1");
            }
            else
            {
                float total = Reviews.Sum(r => r.Rating);
                float newrating = total / Reviews.Count  ;
                Rating = newrating.ToString("F1");
            }
            
        }
    }
}

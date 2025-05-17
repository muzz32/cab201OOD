using System.Text.RegularExpressions;

namespace Arriba_Delivery
{
    class Client : User
    {
        public string restaurant { get; private set; } 
        public string location { get; private set; } 
        public int style { get; private set; }
        public string rating {  get; private set; } 
        public List<Food> menuitems { get; } 
        public List<Review> reviews { get; }
        public List<Order> orders { get; } 

        public Client()
        {
            restaurant = "";
            location = "";
            style = 0;
            rating = "-";
            menuitems = [];
            reviews = [];
            orders = [];
        }

        public Client(string name, int age, string mobile, string email, string password, string restaurant, string location, int style) : 
            base(name, age, mobile, email, password)
        {
            this.restaurant = restaurant;
            this.location = location;
            this.style = style;
            rating = "-";
            menuitems = [];
            reviews = [];
            orders = [];
        }

        
        public override string Getinfo()
        {
            string start = base.Getinfo();
            return start + 
            "\nRestaurant name: " + restaurant +
            "\nRestaurant style: " + Consts.styles[style - 1] +
            "\nRestaurant location: " + location;
        }

        public static List<Client> SortedRestaurant(List<User> users, Func<Client, object> sorter, bool descending = false) 
        {
            List<Client> options;
            if (!descending)
            {
                options = users.OfType<Client>().OrderBy(sorter).ThenBy(client => client.restaurant).ToList();
            }
            else
            {
                options = users.OfType<Client>().OrderByDescending(sorter).ThenBy(client => client.restaurant).ToList();
            }
            return options; 
        }
        
        

        public string[] GetMenuStr()
        {
            return CMD.FormatedList(menuitems, food =>$"${food.price:F2}  {food.name}");
        }
        public void DisplayMenu(string msg)
        {
            CMD.Display(msg);
            foreach (Food food in menuitems)
            {
                CMD.Display($"${food.price.ToString("F2")}  {food.name}");
            }
        }
        public string AddItem()
        {
            string foodname = CMD.ValidateInput(@"^[a-zA-Z-,' ]+$", "Please enter the name of the new item (blank to cancel):", "Invalid item name.", false);
            if (!Regex.IsMatch(foodname, @"^\s*$"))
            {
                float price = CMD.ValidateInput(0f,999.99f,"Please enter the price of the new item(without the $):", "Invalid price.");
                Food food = new Food(foodname, price);
                menuitems.Add(food);
                return $"Successfully added {food.name} (${food.price:F2}) to menu.";
            }
            return "";
        }

        
        public void AddOrder(Order order)
        {
            orders.Add(order);
        }

        
        public void ProcessOrder(int processtype, int status)
        {
            (string[], List<Order>) options = GetSpecificOrders(status);
            int choice = CMD.Choice(Consts.processing_menu[processtype], options.Item1);
            if (choice < options.Item1.Length)
            {
                options.Item2[choice-1].ProcessOrder(status+1);
            }
        }

        private (string[], List<Order>) GetSpecificOrders(int status)
        {
            List<string> stringitems = new List<string>();
            List<Order> specorders = new List<Order>();
            foreach (Order order in orders)
            {
                if (order.status == Consts.status[status])
                {
                    specorders.Add(order);
                    stringitems.Add(order.GetSimpleString());
                }
            }
            stringitems.Add("Return to the previous menu");
            return (stringitems.ToArray(), specorders);
        }

        public void HandOutOrder()
        {
            CMD.Display("These deliverers have arrived and are waiting to collect orders.");
            List<Order> arrivedorders = GetArrivedOrders();
            string[] options = DisplayDeliverers(arrivedorders);
            int choice = CMD.Choice("Select an order to indicate that the deliverer has collected it:", options);
            if (choice < options.Length)
            {
                Order selectedorder =  arrivedorders[choice-1];
                if (selectedorder.status == Consts.status[2])
                {
                    selectedorder.ProcessOrder(3);
                    CMD.Display($"Order #{selectedorder.id} is now marked as being delivered.");
                    orders.Remove(selectedorder);
                }
                else
                {
                    CMD.Display("This order has not yet been cooked.");
                }
                
            }
            
        }

        private List<Order> GetArrivedOrders()
        {
            List<Order> listorders = new List<Order>();
            foreach (Order order in orders) 
            {
                if (order.delivererarrived)
                {
                    listorders.Add(order);
                }
            }
            return listorders;
        }
        public string[] DisplayDeliverers(List<Order> orders)
        {
            List<string> listdeliverers = new List<string>();
            foreach (Order order in orders)
            {
                listdeliverers.Add(order.GetDelivererString());
            }
            listdeliverers.Add("Return to the previous menu");
            return listdeliverers.ToArray();
        }

        public void AddReview(Review review)
        {
            
            float reviewrating = review.rating;
            reviews.Add(review);
            if (rating == "-" && reviews.Count == 0)
            {
                rating = reviewrating.ToString("F1");
            }
            else
            {
                float total = reviews.Sum(r => r.rating);
                float newrating = total / reviews.Count  ;
                rating = newrating.ToString("F1");
            }
            
        }
    }
}

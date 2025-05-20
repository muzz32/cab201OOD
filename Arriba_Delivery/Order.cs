namespace Arriba_Delivery
{
    /// <summary>
    /// A class for orders
    /// </summary>
    class Order
    {
        public int Id { get; } //The orders id
        public float Price { get; private set; } //The total price of the order
        public string Status { get; private set; } //The orders status
        public List<Food> Items { get;  } //A list of the food contained in the order
        public bool DelivererAssigned {get; private set;} //Whether a deliverer has been assigned to the order or not
        public bool DelivererArrived { get; private set; } //Whether the deliverer has arrived at the restaurant
        public bool CustomerReviewed { get; private set; } //Whether the order has been reviewed
        public Client Client { get;  } //The client of the order
        public Customer Customer { get;  } //The customer of the order
        public Deliverer Deliverer {get; private set;} //The deliverer of the order

        /// <summary>
        /// A default constructor for the order
        /// uses the users default constructors
        /// </summary>
        public Order()
        {
            Price = 0;
            Items = new List<Food>();
            Id = 0;
            Status = "";
            DelivererAssigned = false;
            DelivererArrived = false;
            CustomerReviewed = false;
            Client = new Client();
            Customer = new Customer();
            Deliverer = new Deliverer();
        }
        
        /// <summary>
        ///  The constructor for the order class
        /// </summary>
        public Order(Client client, float price, List<Food> items, int id, Customer customer)
        {
            Price = price;
            Items = items;
            Id = id;
            Status = Consts.Status[0];
            DelivererAssigned = false;
            DelivererArrived = false;
            CustomerReviewed = false;
            Client = client;
            Customer = customer;
            Deliverer = new Deliverer();
        }

        /// <summary>
        /// Gets the contents of the order (Or the food)
        /// </summary>
        /// <returns>A string of food and their quantities in the order</returns>
        public string GetContents()
        {
            string output = "";
            foreach (Food food in Items)
            {
                output+= food.Quantity + " x " + food.Name +"\n";
            }

            return output;
        }
        
        /// <summary>
        /// Gets info specific to the customer.
        /// </summary>
        /// <returns>A string to be used by Cmd.Display</returns>
        public string GetInfoCustomer()
        {
            string output = $"Order #{Id} from {Client.Restaurant}: {Status}";
            if(Status == "Delivered")
            {
                output +=$"This order was delivered by {Deliverer.Name} (licence plate: {Deliverer.Licence})";
            }
            return output + GetContents();
        }
        
        /// <summary>
        /// Gets info specific to the client
        /// </summary>
        /// <returns>A string to be used by Cmd.Display</returns>
        public string GetInfoClient()
        {
            string output = $"Order #{Id} for {Customer.Name}: {Status}";
            if(Status == "Delivered")
            {
                output += $"\nThis order was delivered by {Deliverer.Name} (licence plate: {Deliverer.Licence})";
            }
            return output +"\n"+ GetContents();
        }
        
        /// <summary>
        /// Processes the order by changing its status, and then sending a relevant message.
        /// </summary>
        /// <param name="newstatus">The new status of the order</param>
        /// <returns></returns>
        public string ProcessOrder(int newstatus)
        {
            Status = Consts.Status[newstatus];
            string output = $"Order #{Id} is now ready for collection.";
            if (newstatus == 1) //If the status is cooking
            {
                return $"Order #{Id} is now marked as cooking. Please prepare the order, then mark it as finished cooking:\n"+GetContents();
                
            }
            if (newstatus == 2) //If the status is cooked
            {

                if (!DelivererAssigned)
                {
                    return output + "\nNo deliverer has been assigned yet."; //If the status is cooked and there is no deliverer assigned
                }

                if (DelivererArrived)
                {
                    return output + $"\nPlease take it to the deliverer with licence plate {Deliverer.Licence}, who is waiting to collect it."; //If the status is cooked and the deliverer has arrived
                }
                
            }
            return output+$"\nThe deliverer with licence plate {Deliverer.Licence} will be arriving soon to collect it."; //If the status is cooked and the deliverer hasnt arriced
        }

        /// <summary>
        /// Assigns a deliverer to the order and sets the value DelivererAssigned to true
        /// </summary>
        /// <param name="assigneddeliverer">The assigned deliverer</param>
        public void AssignDeliverer(Deliverer assigneddeliverer)
        {
            Deliverer = assigneddeliverer;
            DelivererAssigned = true;
        }

        /// <summary>
        /// Alerts the restaurant that the deliverer has arrived
        /// </summary>
        public void AlertRestaurant()
        {
            DelivererArrived = true;
        }

        /// <summary>
        /// Makes sure no new reviews can be made to this order by setting
        /// CustomerReviewed to true
        /// </summary>
        public void Reviewed()
        {
            CustomerReviewed = true;
        }
    }
}

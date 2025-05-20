namespace Arriba_Delivery
{
    /// <summary>
    /// An inherited class from User for deliverers
    /// </summary>
    class Deliverer : User
    {
        public string Licence {get; } //The deliverers licence
        public string Location{get; private set;} //The deliverers current location
        public Order? Order {get; private set;} //The order currently assigned to the deliverer
        
        /// <summary>
        /// A default constructor for the deliverer
        /// </summary>
        public Deliverer()
        {
            Licence = "";
            Location = "";
            Order = null;
        }

        /// <summary>
        /// Initialises the deliverer user class
        /// </summary>

        public Deliverer(string name, int age, string mobile, string email, string password, string licence) : 
            base(name, age, mobile, email, password)
        {
            Licence = licence;
            Location = "";
            Order = null;
        }
        
        /// <summary>
        /// An extension of the GetInfo method 
        /// </summary>
        /// <returns>
        /// General user info as well as their licence plate and the info about the current order if
        /// they have taken one.
        /// </returns>
        public override string GetInfo()
        {
            string output = base.GetInfo();
            output += "\nLicence plate: "+Licence;
            if (Order != null)
            {
             output +=$"\nCurrent delivery:" +
                         $"\nOrder #{Order.Id} from {Order.Client.Restaurant} at {Order.Client.Location}." +
                         $"\nTo be delivered to {Order.Customer.Name} at {Order.Customer.Location}.";   
            }
            return output;
        }

        /// <summary>
        /// Sets the users location
        /// </summary>
        /// <param name="location">The new location</param>
        public void UpdateLocation(string location)
        {
            Location = location;
        }
        
        /// <summary>
        /// Assigns an order to the deliverer, and
        /// assigns the deliverer to the order.
        /// </summary>
        /// <param name="neworder">The order to be assigned</param>
        public void GetJob(Order neworder)
        {
            Order = neworder;
            Order.AssignDeliverer(this);
        }

        /// <summary>
        /// Used to give order status specific info to the deliverer when they arrive to a restaurant.
        /// This includes informing them if they haven't taken an order, if they've already collected it,
        /// if they've and whether the order is ready.
        /// </summary>
        /// <returns>A message in the form of a string</returns>
        public string ArriveAtRestaurant()
        {
            if (Order == null)
            {
                return "You have not yet accepted an order.";
            }
            if (Order.Status == Consts.Status[3])
            {
                return "You have already picked up this order.";
            }
            if (Order.DelivererArrived)
            {
                return "You already indicated that you have arrived at this restaurant.";
            }
            Order.AlertRestaurant();
            string output =$"Thanks. We have informed {Order.Client.Restaurant} that you have arrived and are ready to pick up order #{Order.Id}.\nPlease show the staff this screen as confirmation.";
            if (Order.Status == Consts.Status[0] || Order.Status == Consts.Status[1])
            {   
                output += "\nThe order is still being prepared, so please wait patiently until it is ready.";
            }
            return output+$"\nWhen you have the order, please deliver it to {Order.Customer.Name} at {Order.Customer.Location}.";
        }

        /// <summary>
        /// Used to finalise the deliverer, which will set the orders status to delivererd and unnasign the order.
        /// Includes safeguards if an order hasnt been accepted or if the order hasnt been collected.
        /// </summary>
        /// <returns>A message in the form of a string</returns>
        public string ArriveAtCustomer()
        {
            if (Order == null)
            {
                return "You have not yet accepted an order.";
            }
            else if (Order.Status != Consts.Status[3])
            {
                return "You have not yet picked up an order.";
            }
            
            
            Order.ProcessOrder(4);
            Order = null;
            return "Thank you for making the delivery.";
        
        }
    }
}

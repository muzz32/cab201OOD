using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Delivery
{
    class Order
    {
        public int id { get; private set; }
        public float price { get; private set; }
        public string restaurant { get; private set; }
        public string restaurantlocation { get; private set; }
        public string status { get; private set; }
        public List<Food> items { get; private set; }
        public string deliverer { get; private set; } = "NoDeliverer";
        public string licence { get; private set; } = "NoDeliverer";
        public string customer { get; private set; }
        public string customerlocation { get; private set; }
        public bool delivererarrived { get; private set; }
        public bool customerreviewded { get; private set; }
        public Client client { get; private set; }

        public Order()
        {
            price = 0;
            items = new List<Food>();
            id = 0;
            status = "";
            restaurant = "";
            restaurantlocation = "";
            delivererarrived = false;
            customer = "";
            customerlocation = "";
            customerreviewded = false;
            client = new Client();
        }
        
        public Order(Client client, float price, List<Food> items, int id, Customer customer)
        {
            this.price = price;
            this.items = items;
            this.id = id;
            status = Consts.status[0];
            restaurant = client.restaurant;
            restaurantlocation = client.location;
            this.customer = customer.name;
            customerlocation = customer.location;
            delivererarrived = false;
            customerreviewded = false;
            this.client = client;
            CMD.Display("Your order has been placed. Your order number is #" + id + ".");
        }

        public string GetContents()
        {
            string output = "";
            foreach (Food food in items)
            {
                output+= food.quantity + " x " + food.name +"\n";
            }

            return output;
        }
        public string GetInfoCustomer()
        {
            string output = $"Order #{id} from {restaurant}: {status}";
            if(status == "Delivered")
            {
                output +=$"This order was delivered by {deliverer} (licence plate: {licence})";
            }
            return output + GetContents();
        }
        public string GetInfoClient()
        {
            string output = $"Order #{id} for {customer}: {status}";
            if(status == "Delivered")
            {
                output += $"\nThis order was delivered by {deliverer} (licence plate: {licence})";
            }
            return output +"\n"+ GetContents();
        }
        public string GetStatusString()
        {
            return $"Order #{id} for {customer}: {status}";
        }
        public string GetSimpleString()
        {
            return $"Order #{id} for {customer}";
        }
        public string GetSimpleCusString()
        {
            return $"Order #{id} from {restaurant}";
        }

        public string GetDelivererString()
        {
            return $"Order #{id} for {customer} (Deliverer licence plate: {licence}) (Order status: {status})";
        }

        public void ProcessOrder(int newstatus)
        {
            status = Consts.status[newstatus];
            if (newstatus == 1)
            {
                CMD.Display($"Order #{id} is now marked as cooking. Please prepare the order, then mark it as finished cooking:\n"+GetContents());
                
            }
            else if (newstatus == 2)
            {
                CMD.Display($"Order #{id} is now ready for collection.");
                if (deliverer == "NoDeliverer")
                {
                    CMD.Display("No deliverer has been assigned yet.");
                }
                else if (delivererarrived)
                {
                    CMD.Display($"Please take it to the deliverer with licence plate {licence}, who is waiting to collect it.");
                }
                else
                {
                    CMD.Display($"The deliverer with licence plate {licence} will be arriving soon to collect it.");
                }
            }
        }

        public void AssignDeliverer(Deliverer assigneddeliverer)
        {
            deliverer = assigneddeliverer.name;
            licence = assigneddeliverer.licence;
        }

        public void AlertRestaurant()
        {
            delivererarrived = true;
        }

        public void Reviewed()
        {
            customerreviewded = true;
        }
    }
}

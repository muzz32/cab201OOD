using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Arriba_Delivery
{
    class Customer : User
    {
        public string location { get; private set; }
        public List<Order> orders { get; private set; } 
        public float moneyspent { get; private set; }
        
        public Customer()  
        {
            location = "";
            orders = new List<Order>();
            moneyspent = 0f; 
        }

        public Customer(string name, int age, string mobile, string email, string password, string location) : base(name, age, mobile,
            email, password)
        {
            this.location = location;
            orders = new List<Order>();
            moneyspent = 0f;
        }
        
        public override string Getinfo()
        {
            string start = base.Getinfo();
            return start + 
                   "\nLocation: " + location +
                   $"\nYou've made {orders.Count} order(s) and spent a total of ${moneyspent.ToString("F2")} here.";
        }

        public string[] DisplayClients(List<Client> rawclients)
        {
            List<string> listclients = new List<string>();
            foreach (Client client in rawclients)
            {
                listclients.Add($"" +
                    $"{client.restaurant,-20}" +
                    $"{client.location,-7}" +
                    $"{GetDistance(location, client.location),-7}" +
                    $"{Consts.styles[client.style - 1], -12}" +
                    $"{client.rating,-6}");
            }
            listclients.Add("Return to the previous menu");
            return listclients.ToArray();
        }

        public bool MakeOrder(Client client, List<Order> allorders)
        {
            float totalprice = 0;
            int length = client.menuitems.Count;
            List<Food> items = new List<Food>();
            
            string[] options = client.GetMenuStr().Concat(["Complete order", "Cancel order"]).ToArray();
            do
            {
                int choice = CMD.Choice("Current order total: $" + totalprice.ToString("F2"), options);
                if (choice == length + 1)
                {
                    Order order = new Order(client, totalprice, items, allorders.Count + 1, this);
                    moneyspent += totalprice;
                    orders.Add(order);
                    return true;
                }
                if (choice == length + 2)
                {
                    return false;
                }
                Food selecteditem = client.menuitems[choice - 1];
                CMD.Display($"Adding {selecteditem.name} to order.");
                int quantity = CMD.ValidateInput(0, Int32.MaxValue, "Please enter quantity (0 to cancel):", "Invalid quantity.");
                if (quantity > 0)
                {
                    Food? existingitem = items.Find(food => food.name == selecteditem.name);
                    if (existingitem != null)
                    {
                        existingitem.ChangeQuantity(existingitem.quantity + quantity);
                    }
                    else
                    {
                        Food ordereditem = new Food(selecteditem.name, selecteditem.price, quantity);
                        items.Add(ordereditem);
                    }
                    CMD.Display($"Added {quantity} x {selecteditem.name} to order.");
                    totalprice += selecteditem.price * quantity;
                }
            } while (true);
        }

        public void LeaveReview()
        {
            List<Order> finishedOrders = GetFinishedOrders();
            string[] options = DisplaySimpleOrders(finishedOrders);
            int choice = CMD.Choice("Select a previous order to rate the restaurant it came from:", options);
            if (choice < options.Length)
            {
                Order selectedorder = finishedOrders[choice - 1];
                CMD.Display($"You are rating order #{selectedorder.id} from {selectedorder.restaurant}:");
                CMD.Display(selectedorder.GetContents());
                int rating = CMD.ValidateInput(0, 5, "Please enter a rating for this restaurant (1-5, 0 to cancel):","Invalid rating");
                if (rating > 0)
                {
                    CMD.Display("Please enter a comment to accompany this rating:");
                    string comment = CMD.EmptyStrIn();
                    Review newreview = new Review(name, rating, comment);
                    selectedorder.client.AddReview(newreview);
                    selectedorder.Reviewed();
                    CMD.Display($"Thank you for rating {selectedorder.restaurant}.");
                }
            }
        }
        
        public List<Order> GetFinishedOrders()
        {
            List<Order> listorders = new List<Order>();
            foreach (Order order in orders) 
            {
                if (order.status == Consts.status[4] && !order.customerreviewded)
                {
                    listorders.Add(order);
                }
            }
            return listorders;
        }
        public string[] DisplaySimpleOrders(List<Order> orders)
        {
            List<string> listdeliverers = new List<string>();
            foreach (Order order in orders)
            {
                listdeliverers.Add(order.GetSimpleCusString());
            }
            listdeliverers.Add("Return to the previous menu");
            return listdeliverers.ToArray();
        }
    }
}

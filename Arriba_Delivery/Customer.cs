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
        public List<Order> orders { get; private set; } = new List<Order>();
        public float moneyspent { get; private set; } = 0f;
        public Customer(List<User> users) : base(users)
        {
            bool valid = true;
            string location;
            do
            {
                CMD.Display("Please enter your location(in the form of X, Y):");
                location = CMD.StrIn("Invalid location.", "Please enter your location(in the form of X, Y):");
                if (!Regex.IsMatch(location, @"^[0-9]+,[0-9]+$"))
                {
                    CMD.Display("Invalid location.");
                    valid = false;
                }
                else
                {
                    valid = true;
                }
            } while (!valid);
            this.location = location;
            CMD.Display("You have been successfully registered as a customer, " + name +"!");
        }

        public override void Getinfo()
        {
            base.Getinfo();
            CMD.Display("Location: " + location);
            CMD.Display($"You've made {orders.Count} order(s) and spent a total of ${moneyspent.ToString("F2")} here.");
        }


        public List<Client> SortedRestaurant(List<User> users, Func<Client, object> sorter, bool descending = false) 
        {
            List<Client> options = new List<Client>();
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
            int quantity;
            List<Food> items = new List<Food>();
            do
            {
                int choice = CMD.Choice("Current order total: $" + totalprice.ToString("F2"), client.GetMenuStr().Concat(["Complete order", "Cancel order" ]).ToArray());
                if (choice == length + 1)
                {
                    Order order = new Order(client, totalprice, items, allorders.Count + 1, this);
                    moneyspent += totalprice;
                    orders.Add(order);
                    return true;
                }
                else if (choice == length + 2)
                {
                    return false;
                }
                else
                {
                    Food selecteditem = client.menuitems[choice - 1];
                    CMD.Display($"Adding {selecteditem.name} to order.");
                    do
                    {
                        CMD.Display("Please enter quantity (0 to cancel):");
                        quantity = CMD.IntIn("Invalid quantity.", "Please enter quantity (0 to cancel):");
                        if (quantity <= 0)
                        {
                            if (quantity == 0) break; 
                            CMD.Display("Invalid quantity.");
                        }
                        else break; 
                    } while (true);

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
                }
            } while (true);
        }

        public void DisplayOrders()
        {
            if (orders.Count == 0)
            {
                CMD.Display("You have not placed any orders.");
            }
            else
            {
                foreach (Order order in orders)
                {
                    order.GetInfoCustomer();
                }
            }
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
                selectedorder.GetContents();
                int rating;
                do
                {
                    CMD.Display("Please enter a rating for this restaurant (1-5, 0 to cancel):");
                    rating = CMD.IntIn("Invalid rating", "Please enter a rating for this restaurant (1-5, 0 to cancel):");
                } while (rating is < 0 or > 5);

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
        
        private List<Order> GetFinishedOrders()
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

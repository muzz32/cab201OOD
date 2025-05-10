using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Arriba_Delivery
{
    class Client : User
    {
        public string restaurant { get; private set; } 
        public string location { get; private set; } 
        public int style { get; private set; }
        public string rating {  get; private set; } 
        public List<Food> menuitems { get; private set; } = new List<Food>();
        public List<Review> reviews { get; private set; } = new List<Review>();
        public List<Order> orders { get; private set; } = new List<Order>();
        public Client(List<User> users) : base(users)
        {
            bool valid = true;
            string restaurant;
            string location;
            int style;
            do
            {
                CMD.Display("Please enter your restaurant's name:");
                restaurant = CMD.StrIn("Invalid restaurant name.", "Please enter your restaurant's name:");
                if (!Regex.IsMatch(restaurant, @"^(?=.*\S).*$"))
                {
                    CMD.Display("Invalid restaurant name.");
                    valid = false;
                }
                else
                {
                    valid = true;
                }
            } while (!valid);


            style = CMD.Choice("Please select your restaurant's style:", Consts.styles);
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

            this.restaurant = restaurant;
            this.style = style;
            this.location = location;
            rating = "-";
            CMD.Display("You have been successfully registered as a client, " + name + "!");
        }


        public override void Getinfo()
        {
            base.Getinfo();
            CMD.Display("Restaurant name: " + restaurant);
            CMD.Display("Restaurant style: " + Consts.styles[style - 1]);
            CMD.Display("Restaurant location: " + location);
        }

        public void DisplayReviews()
        {
            if (reviews.Count == 0)
            {
                CMD.Display("No reviews have been left for this restaurant.");
            }
            else
            {
                foreach (Review review in reviews)
                {
                    CMD.Display("Reviewer: " + review.reviewer);
                    CMD.Display("Rating: " + review.starrating);
                    CMD.Display("Comment: " + review.comment);
                }
            }
        }

        public string[] GetMenuStr() 
        {
            List<string> stringitems = new List<string>();
            foreach (Food food in menuitems)
            {
                stringitems.Add($"${food.price.ToString("F2")}  {food.name}");
            }
            return stringitems.ToArray();
        }
        public void DisplayMenu(string msg)
        {
            CMD.Display(msg);
            foreach (Food food in menuitems)
            {
                CMD.Display($"${food.price.ToString("F2")}  {food.name}");
            }
        }
        public void AddItem()
        {
            string name;
            float price;
            do
            {
                CMD.Display("Please enter the name of the new item (blank to cancel):");
                name = CMD.EmptyStrIn();
                if(Regex.IsMatch(name, @"^\s*$"))
                {
                    return;
                }
                else if (!Regex.IsMatch(name, @"^[a-zA-Z-,' ]+$"))
                {
                    CMD.Display("Invalid name.");
                }
                else
                {
                    break;
                }
            } while (true);
            do
            {
                CMD.Display("Please enter the price of the new item(without the $):");
                price = CMD.FloatIn("Invalid price.", "Please enter the price of the new item(without the $):");
                if (price < 0 || price > 999.99)
                {
                    CMD.Display("Invalid price.");
                }
                else
                {
                    break;
                }
            } while (true);
            Food food = new Food(name, price, 1);
            CMD.Display($"Successfully added {food.name} (${food.price.ToString("F2")}) to menu.");
            menuitems.Add(food);
        }

        
        public void AddOrder(Order order)
        {
            orders.Add(order);
        }

        public void DisplayOrders()
        {
            if (orders.Count == 0)
            {
                CMD.Display("Your restaurant has no current orders.");
            }
            else
            {
                foreach (Order order in orders)
                {
                    order.GetInfoClient();
                }
            }
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

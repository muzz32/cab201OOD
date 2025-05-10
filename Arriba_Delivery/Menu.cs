using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Arriba_Delivery
{
    class Menu
    {
        private List<User> users= new List<User>();
        private List<Order> currorders= new List<Order>();
        private List<Order> allorders= new List<Order>();

        public void StartProgram()
        {
            bool running = true;
            CMD.Display("Welcome to Arriba Eats!");
            while (running){
                running = StartMenu(); 
            }
            CMD.Display("Thank you for using Arriba Eats!");
        }

        private bool StartMenu() 
        {
            switch (CMD.Choice("Please make a choice from the menu below:", Consts.start_menu))
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
        private void LoginMenu() 
        {
            CMD.Display("Email:");
            string loginemail = CMD.StrIn("Invalid email.", "Email:");
            CMD.Display("Password:");
            string loginpasswd = CMD.StrIn("Invalid password.", "Password:");
            User? registereduser = null;

            foreach(User user in users)
            {
                if (user.email == loginemail && user.password == loginpasswd)
                {
                    registereduser = user;
                    break;
                }
            }
            if (registereduser == null)
            {
                CMD.Display("Invalid email or password.");
                return;
            }
            switch (registereduser)
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
        }
        private void RegisterMenu() 
        {
            switch (CMD.Choice("Which type of user would you like to register as?", Consts.reg_menu))
            {
                case 1:
                    CustomerReg();
                    break;
                case 2:
                    DelivererReg();
                    break;
                case 3:
                    ClientReg();
                    break;
                case 4:
                    return;
            }
        }
        private void CustomerReg()
        {   
            Customer customer = new Customer(users);
            users.Add(customer);
        }
        private void DelivererReg() 
        {
            Deliverer deliverer = new Deliverer(users);
            users.Add(deliverer);
        }
        private void ClientReg() 
        {
            Client client = new Client(users);
            users.Add(client);
        } 

        private void CustomerMenu(Customer customer)
        {
            customer.WelcomeMsg();
            while (true)
            {
                switch (CMD.Choice("Please make a choice from the menu below:", Consts.customer_menu))
                {
                    case 1:
                        customer.Getinfo();
                        break;
                    case 2:
                        PickResturantSort(customer);
                        break;
                    case 3:
                        customer.DisplayOrders(); 
                        break;
                    case 4:
                        customer.LeaveReview();
                        break;
                    case 5:
                        CMD.Display("You are now logged out.");
                        return;
                }
            }
        }



        private void PickResturantSort(Customer customer)
        {
            int index = 0;
            List<Client> clients = new List<Client>();

            switch (CMD.Choice("How would you like the list of restaurants ordered?", Consts.restaurant_sort_menu))
            {
                case 1:
                    clients = customer.SortedRestaurant(users, client => client.restaurant);
                    break;
                case 2:
                    clients = customer.SortedRestaurant(users, client => customer.GetDistance(customer.location, client.location));
                    break;
                case 3:
                    clients = customer.SortedRestaurant(users, client => client.style);
                    break;
                case 4:
                    clients = customer.SortedRestaurant(users, client => client.rating, true);
                    break;
                case 5:
                    return; 
            }
            index = CMD.Choice(Consts.restaurant_options_title, customer.DisplayClients(clients));
            if (index == clients.Count + 1) return;
            CMD.Display($"Placing order from {clients[index - 1].restaurant}.");
            OrderMenu(customer, clients[index-1]);
        }
        private void OrderMenu (Customer customer, Client client)
        {
            do
            {
                switch (CMD.Choice("", Consts.restaurant_options_menu))
                {
                    case 1:
                        bool ordered =  customer.MakeOrder(client, allorders);
                        if (ordered)
                        {
                            allorders.Add(customer.orders.Last());
                            currorders.Add(customer.orders.Last());
                            client.AddOrder(customer.orders.Last());
                        }
                        break;
                    case 2:
                        client.DisplayReviews();
                        break;
                    case 3:
                        return;
                }
            } while(true);
        }

        private void DelivererMenu(Deliverer deliverer)
        {
            deliverer.WelcomeMsg();
            while (true)
            {
                switch (CMD.Choice("Please make a choice from the menu below:", Consts.deliverer_menu))
                {
                    case 1:
                        deliverer.Getinfo();
                        break;
                    case 2:
                        deliverer.GetJob(currorders);
                        if (deliverer.joborder != null)
                        {
                            currorders.Remove(deliverer.joborder);
                        }
                        break;
                    case 3:
                        deliverer.ArriveAtRestaurant();
                        break;
                    case 4:
                        deliverer.ArriveAtCustomer();
                        break;
                    case 5:
                        CMD.Display("You are now logged out.");
                        return;
                }
            }
        }
        private void ClientMenu(Client client)
        {
            client.WelcomeMsg();
            while (true)
            {
                switch (CMD.Choice("Please make a choice from the menu below:", Consts.client_menu))
                {
                    case 1:
                       client.Getinfo();
                        break;
                    case 2:  
                        client.DisplayMenu("This is your restaurant's current menu:");
                        client.AddItem();
                        break;
                    case 3:
                        client.DisplayOrders();
                        break;
                    case 4:
                        client.ProcessOrder(0,0);
                        break;
                    case 5:
                        client.ProcessOrder(1,1);
                        break;
                    case 6:
                        client.HandOutOrder();
                        break;
                    case 7:
                        CMD.Display("You are now logged out.");
                        return;
                }
            }
        }
    }
}

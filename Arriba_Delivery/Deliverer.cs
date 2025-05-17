using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Arriba_Delivery
{
    class Deliverer : User
    {
        public string licence {get; private set;}
        private string location;
        public Order? joborder {get; private set;}
        public Deliverer()
        {
            licence = "";
            location = "";
            joborder = null;
        }

        public Deliverer(string name, int age, string mobile, string email, string password, string licence) : 
            base(name, age, mobile, email, password)
        {
            this.licence = licence;
            location = "";
            joborder = null;
        }

        public override string Getinfo()
        {
            string output = base.Getinfo();
            output += "\nLicence plate: "+licence;
            if (joborder != null)
            {
             output +=$"\nCurrent delivery:" +
                         $"\nOrder #{joborder.id} from {joborder.restaurant} at {joborder.restaurantlocation}." +
                         $"\nTo be delivered to {joborder.customer} at {joborder.customerlocation}.";   
            }
            return output;
        }

        public void GetJob(List<Order> currorders)
        {
            if (joborder != null)
            {
                CMD.Display("You have already selected an order for delivery.");
            }
            else
            {
                CMD.Display("Please enter your location (in the form of X,Y):");
                location = CMD.StrIn("Please enter your location (in the form of X,Y):", "Invalid location.");
                string[] options = DisplayOrders(currorders);
                int choice= CMD.Choice(Consts.order_options_title, options);
                if (choice < options.Length)
                {
                    joborder = currorders[choice-1];
                    joborder.AssignDeliverer(this);
                    CMD.Display($"Thanks for accepting the order. Please head to {joborder.restaurant} at {joborder.restaurantlocation} to pick it up.");
                }
            }
        }
        
        public string[] DisplayOrders(List<Order> raworders)
        {
            List<string> listorders = new List<string>();
            foreach (Order order in raworders)
            {
                listorders.Add($"" +
                                $"{order.id, -7}" +
                                $"{order.restaurant, -17}" +
                                $"{order.restaurantlocation, -10}" +
                                $"{order.customer, -17}" +
                                $"{order.customerlocation, -7}"+
                                $"{TotalDistance(location, order.restaurantlocation, order.customerlocation)}"); //FORTMAT THIS
            }
            listorders.Add("Return to the previous menu");
            return listorders.ToArray();
        }

        public void ArriveAtRestaurant()
        {
            if (joborder == null)
            {
                CMD.Display("You have not yet accepted an order.");
            }
            else if (joborder.status == Consts.status[3])
            {
                CMD.Display("You have already picked up this order.");
            }
            else if (joborder.delivererarrived == true)
            {
                CMD.Display("You already indicated that you have arrived at this restaurant.");
            }
            else
            {
                joborder.AlertRestaurant();
                CMD.Display($"Thanks. We have informed {joborder.restaurant} that you have arrived and are ready to pick up order #{joborder.id}.");
                CMD.Display("Please show the staff this screen as confirmation.");
                if (joborder.status == Consts.status[0] || joborder.status == Consts.status[1])
                {   
                    CMD.Display("The order is still being prepared, so please wait patiently until it is ready.");
                }
                CMD.Display($"When you have the order, please deliver it to {joborder.customer} at {joborder.customerlocation}.");
            }
        }

        public void ArriveAtCustomer()
        {
            if (joborder == null)
            {
                CMD.Display("You have not yet accepted an order.");
            }
            else if (joborder.status != Consts.status[3])
            {
                CMD.Display("You have not yet picked up an order.");
            }
            else
            {
                joborder.ProcessOrder(4);
                joborder = null;
                CMD.Display("Thank you for making the delivery.");
            }
        }
    }
}

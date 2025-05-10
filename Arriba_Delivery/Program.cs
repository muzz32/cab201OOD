using System.Net;

namespace Arriba_Delivery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.StartProgram();
        }
    }
}


//As a customer, I would like to rate the restaurant with a short comment and a star rating (between 1 and 5 stars) to inform others about my customer experience.
//As a client, I would like the order to be removed from my order list once it has been delivered so that I can focus on other orders.

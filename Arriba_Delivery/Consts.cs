using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Delivery
{
    class Consts
    {
        public enum UserType
        {
            Customer,
            Deliverer,
            Client
        }
        public static readonly string[] start_menu =
        [
            "Login as a registered user",
            "Register as a new user",
            "Exit"
        ];

        public static readonly string[] reg_menu =
        [
            "Customer",
            "Deliverer",
            "Client",
            "Return to the previous menu"
        ];

        public static readonly string[] styles =
        [
            "Italian",
            "French",
            "Chinese",
            "Japanese",
            "American",
            "Australian"
        ];

        public static readonly string passwdprompt =
            "Your password must:\n- be at least 8 characters long\n- contain a number\n- contain a lowercase letter\n- contain an uppercase letter\nPlease enter a password:";


        public static readonly string[] customer_menu =
        [
            "Display your user information",
            "Select a list of restaurants to order from",
            "See the status of your orders",
            "Rate a restaurant you've ordered from",
            "Log out"
        ];

        public static readonly string[] deliverer_menu =
        [
            "Display your user information",
            "List orders available to deliver",
            "Arrived at restaurant to pick up order",
            "Mark this delivery as complete",
            "Log out"
        ];

        public static readonly string[] client_menu =
        [
            "Display your user information",
            "Add item to restaurant menu",
            "See current orders",
            "Start cooking order",
            "Finish cooking order",
            "Handle deliverers who have arrived",
            "Log out"
        ];

        public static readonly string[] restaurant_sort_menu =
        [
            "Sorted alphabetically by name",
            "Sorted by distance",
            "Sorted by style",
            "Sorted by average rating",
            "Return to the previous menu"
        ];

        public static readonly string[] restaurant_options_menu =
        [
            "See this restaurant's menu and place an order",
            "See reviews for this restaurant",
            "Return to main menu"
        ];


        public static readonly string restaurant_options_title = $"You can order from the following restaurants:\n   {"Restaurant Name",-20}{"Loc",-7}{"Dist",-7}{"Style",-12}{"Rating",-6}";

        public static readonly string order_options_title = $"The following orders are available for delivery. Select an order to accept it:\n   {"Order",-7}{"Restaurant Name",-17}{"Loc",-10}{"Customer Name",-17}{"Loc",-7}{"Dist"}"; //Do this 
        
        public static readonly string[]  processing_menu =
        [
            "Select an order once you are ready to start cooking:",
            "Select an order once you have finished preparing it:"
        ];
        
        public static readonly string[] status =
        [
            "Ordered",
            "Cooking",
            "Cooked",
            "Being Delivered",
            "Delivered"
        ];
    }
}

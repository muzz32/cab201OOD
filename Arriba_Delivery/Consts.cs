namespace Arriba_Delivery
{
    /// <summary>
    /// A class to store array's of menu options and strings with lengths
    /// that would affect readability.
    /// </summary>
    static class Consts
    {
        /// <summary>
        /// An enum that contains the user types to be used with registration
        /// </summary>
        public enum UserType
        {
            Customer,
            Deliverer,
            Client
        }
        
        /// <summary>
        /// Menu options for the start menu
        /// </summary>
        public static readonly string[] StartMenu =
        [
            "Login as a registered user",
            "Register as a new user",
            "Exit"
        ];

        /// <summary>
        /// Menu options for the registration menu
        /// </summary>
        public static readonly string[] RegMenu =
        [
            "Customer",
            "Deliverer",
            "Client",
            "Return to the previous menu"
        ];

        /// <summary>
        /// Different styles of restaurants. Called numerically
        /// with Consts.Styles[x]
        /// </summary>
        public static readonly string[] Styles =
        [
            "Italian",
            "French",
            "Chinese",
            "Japanese",
            "American",
            "Australian"
        ];

        /// <summary>
        /// The prompt users get when setting their password in registration
        /// </summary>
        public static readonly string Passwdprompt =
            "Your password must:\n- be at least 8 characters long\n- contain a number\n- contain a lowercase letter\n- contain an uppercase letter\nPlease enter a password:";
        
        /// <summary>
        /// Menu options for the Customer 
        /// </summary>
        public static readonly string[] CustomerMenu =
        [
            "Display your user information",
            "Select a list of restaurants to order from",
            "See the status of your orders",
            "Rate a restaurant you've ordered from",
            "Log out"
        ];

        /// <summary>
        /// menu options for the deliverer
        /// </summary>
        public static readonly string[] DelivererMenu =
        [
            "Display your user information",
            "List orders available to deliver",
            "Arrived at restaurant to pick up order",
            "Mark this delivery as complete",
            "Log out"
        ];

        /// <summary>
        /// menu options for the client
        /// </summary>
        public static readonly string[] ClientMenu =
        [
            "Display your user information",
            "Add item to restaurant menu",
            "See current orders",
            "Start cooking order",
            "Finish cooking order",
            "Handle deliverers who have arrived",
            "Log out"
        ];

        /// <summary>
        /// Menu options for how restaurants should be sorted
        /// </summary>
        public static readonly string[] RestaurantSortMenu =
        [
            "Sorted alphabetically by name",
            "Sorted by distance",
            "Sorted by style",
            "Sorted by average rating",
            "Return to the previous menu"
        ];
        
        /// <summary>
        /// menu options for the customer after selecting a sorted restaurant
        /// </summary>
        public static readonly string[] RestaurantOptionsMenu =
        [
            "See this restaurant's menu and place an order",
            "See reviews for this restaurant",
            "Return to main menu"
        ];

        /// <summary>
        /// Contains string width to display sorted restaurant menu column headers
        /// </summary>
        public static readonly string RestaurantOptionsTitle = $"You can order from the following restaurants:\n   {"Restaurant Name",-20}{"Loc",-7}{"Dist",-7}{"Style",-12}{"Rating",-6}";

        /// <summary>
        /// Contains string width to display avaliable orders for deliverers menu column headers
        /// </summary>
        public static readonly string OrderOptionsTitle = $"The following orders are available for delivery. Select an order to accept it:\n   {"Order",-7}{"Restaurant Name",-17}{"Loc",-10}{"Customer Name",-17}{"Loc",-7}{"Dist"}"; //Do this 
        
        /// <summary>
        /// menu options for the processing menu
        /// </summary>
        public static readonly string[]  ProcessingMenu =
        [
            "Select an order once you are ready to start cooking:",
            "Select an order once you have finished preparing it:"
        ];
        
        /// <summary>
        /// Different order status's. Used the same way as Styles.
        /// </summary>
        public static readonly string[] Status =
        [
            "Ordered",
            "Cooking",
            "Cooked",
            "Being Delivered",
            "Delivered"
        ];
    }
}

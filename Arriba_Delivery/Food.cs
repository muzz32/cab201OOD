namespace Arriba_Delivery
{
    /// <summary>
    /// A class for food. Used in the Client class
    /// as a menu and in the Order class to show its contents
    /// </summary>
    class Food
    {
        public string Name { get; private set; } //The name of the food
        public float Price { get; private set; } //The price of the food

        public int Quantity { get; private set; } //The qantity of the food
       
        /// <summary>
        ///  Initialises the food class
        /// </summary>
        /// <param name="quantity">Default is one</param>
        public Food(string name, float price, int quantity = 1)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        /// <summary>
        /// Changes the quantity of the food, usually
        /// when the same food item is added to an
        /// order.
        /// </summary>
        /// <param name="newquantity">The updated quantity</param>
        public void ChangeQuantity(int newquantity)
        {
            Quantity = newquantity;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Arriba_Delivery
{
    class Food
    {
        public string name { get; private set; }
        public float price { get; private set; }

        public int quantity { get; private set; }
       
        public Food(string name, float price, int quantity)
        {
            this.name = name;
            this.price = price;
            this.quantity = quantity;
        }

        public void ChangeQuantity(int quantity)
        {
            this.quantity = quantity;
        }

    }
}

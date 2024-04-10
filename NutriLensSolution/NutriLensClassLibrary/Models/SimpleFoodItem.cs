using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriLensClassLibrary.Models
{
    public class SimpleFoodItem
    {
        public string Item { get; set; }
        public int Quantity { get; set; }

        public SimpleFoodItem(string item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}

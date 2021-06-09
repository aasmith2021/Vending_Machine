using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public abstract class Item
    {
        public Item (string name, decimal price, string category)
        {
            Name = name;
            Price = price;
            Category = category;
        }

        public string Name { get; protected set; }

        public decimal Price { get; protected set; }

        public string Category { get; protected set; }

        public abstract string Message { get; }
    }
}

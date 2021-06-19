using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachineApplication
{
    public abstract class Item
    {
        public Item (string name, string category)
        {
            Name = name;
            Category = category;
        }

        public string Name { get; protected set; }

        public string Category { get; protected set; }

        public abstract string Message { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Item
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

        public string Message
        {
            get
            {
                string message = "";
                
                switch (Category.ToLower())
                {
                    case "chip":
                        message = "Crunch";
                        break;
                    case "candy":
                        message = "Munch";
                        break;
                    case "drink":
                        message = "Glug";
                        break;
                    case "gum":
                        message = "Chew";
                        break;
                }

                return message;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    class Drink : Item
    {
        public Drink(string name, decimal price, string category) : base(name, price, category)
        {

        }

        public override string Message
        {
            get { return "Gulp"; }
        }
    }
}

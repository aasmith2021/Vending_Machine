using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    class Gum : Item
    {
        public Gum(string name, decimal price, string category) : base(name, price, category)
        {

        }

        public override string Message
        {
            get { return "Chew"; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Candy : Item
    {
        public Candy(string name, decimal price, string category) : base(name, price, category)
        {

        }

        public override string Message
        {
            get { return "Munch"; }
        }
    }
}

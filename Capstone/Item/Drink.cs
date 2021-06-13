using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Drink : Item
    {
        public Drink(string name, string category) : base(name, category)
        {

        }

        public override string Message
        {
            get { return "Gulp"; }
        }
    }
}

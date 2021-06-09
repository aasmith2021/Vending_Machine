using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    class UnknownItem : Item
    {
        public UnknownItem(string name, decimal price, string category) : base(name, price, category)
        {

        }

        public override string Message
        {
            get { return "Unknown"; }
        }
    }
}

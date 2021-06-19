using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachineApplication
{
    class UnknownItem : Item
    {
        public UnknownItem(string name, string category) : base(name, category)
        {

        }

        public override string Message
        {
            get { return "Unknown"; }
        }
    }
}

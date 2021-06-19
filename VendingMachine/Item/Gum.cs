using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Gum : Item
    {
        public Gum(string name, string category) : base(name, category)
        {

        }

        public override string Message
        {
            get { return "Chew"; }
        }
    }
}

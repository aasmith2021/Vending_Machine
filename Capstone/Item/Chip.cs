using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Chip : Item
    {
        public Chip(string name, decimal price, string category) : base(name, price, category)
        {

        }
        
        public override string Message
        {
            get { return "Crunch"; }
        }
    }
}

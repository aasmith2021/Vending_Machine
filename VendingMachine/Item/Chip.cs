using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Chip : Item
    {
        public Chip(string name, string category) : base(name, category)
        {

        }
        
        public override string Message
        {
            get { return "Crunch"; }
        }
    }
}

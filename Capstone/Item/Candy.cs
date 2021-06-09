﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class Candy : Item
    {
        public Candy(string name, string category) : base(name, category)
        {

        }

        public override string Message
        {
            get { return "Munch"; }
        }
    }
}

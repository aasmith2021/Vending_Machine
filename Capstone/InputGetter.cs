using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class InputGetter : IUserInput
    {
        public string GetInput()
        {
            return Console.ReadLine();
        }
    }
}

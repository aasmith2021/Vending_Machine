using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class ConsoleInputGetter : IInput
    {
        public string GetInput()
        {
            return Console.ReadLine();
        }
    }
}

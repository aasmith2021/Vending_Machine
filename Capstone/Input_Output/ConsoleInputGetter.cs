using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class ConsoleInputGetter : IInput
    {
        //This class creates an object that implements the IInput
        //interface to read user input from the console
        public string GetInput()
        {
            return Console.ReadLine();
        }
    }
}

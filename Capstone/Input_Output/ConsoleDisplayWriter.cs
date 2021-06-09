using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class ConsoleDisplayWriter : IDisplay
    {
        public void DisplayData(string dataToDisplay = "")
        {
            Console.WriteLine(dataToDisplay);
        }

        public void ClearDisplay()
        {
            Console.Clear();
        }
    }
}

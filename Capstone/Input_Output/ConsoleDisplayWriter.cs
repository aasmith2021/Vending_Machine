using System;
using System.Text;

namespace Capstone
{
    public class ConsoleDisplayWriter : IDisplay
    {
        //This class creates an object that implements the IDisplay
        //interface to write string data to the console
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

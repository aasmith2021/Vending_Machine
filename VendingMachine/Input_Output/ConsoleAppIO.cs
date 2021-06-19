using System;
using System.Text;

namespace VendingMachineApplication
{
    public class ConsoleAppIO : IUserIO
    {
        //This class creates an object that implements the IInput
        //interface to read user input from the console
        public string GetInput()
        {
            return Console.ReadLine();
        }

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

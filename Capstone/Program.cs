using System;
using System.Text;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main()
        {
            //Creates new class instances of the objects needed to run the
            //vending machine as a console application using the Input_Output interfaces

            //The <input> object captures input from the user
            IInput input = new ConsoleInputGetter();

            //The <display> object displays written output from the program
            IDisplay display = new ConsoleDisplayWriter();

            //The <output> object writes output from the program to a destination, such as a file or database
            IOutput output = new FileOutputWriter();

            //The <dataReader> object reads data from a data source, such as a file or database
            IDataReader dataReader = new FileDataReader();

            
            //Creates a new Vendo-Matic 800 Vending Machine when the program runs
            new VendingMachine(input, output, display, dataReader);
        }
    }
}

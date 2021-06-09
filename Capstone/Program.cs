using System;

namespace Capstone
{
    class Program
    {
        static void Main()
        {
            IInput input = new ConsoleInputGetter();
            IOutput output = new FileOutputWriter();
            IDataReader dataReader = new FileDataReader();
            IDisplay display = new ConsoleDisplayWriter();
            
            //Creates a new Vendo-Matic 800 Vending Machine when the program runs
            new VendingMachine(input, output, display, dataReader);
        }
    }
}

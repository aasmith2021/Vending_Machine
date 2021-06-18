using System;
using System.Text;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main()
        {
            //Creates new instances of the objects needed to run the vending machine
            //as a console application using the IUserIO and IDataIO interfaces

            //The <userIO> object captures input from and displays data to the user
            IUserIO userIO = new ConsoleAppIO();

            //The <dataIO> object reads data from and writes data to a data source,
            //such as a file or database
            //IDataIO dataIO = new FileIO();
            IDataIO dataIO = new DatabaseIO();

            
            //Creates a new Vendo-Matic 800 Vending Machine when the program runs
            new VendingMachine(userIO, dataIO);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VendingMachineApplication
{
    public static class ReadWrite
    {
        //This method reads the lines from an inventory data source to produce the inventory
        //needed to stock the vending machine
        public static List<string[]> ReadInventory(IUserIO userIO, IDataIO dataIO)
        {
            List<string[]> inventory = dataIO.GetInventoryData(userIO);

            return inventory;
        }

        //Writes a vending machine log entry to the data source for a specific transaction
        public static void LogEntry(IUserIO userIO, IDataIO dataIO, string transaction, decimal startBalance, decimal currentBalance)
        {
            List<string[]> outputToWrite = new List<string[]>();
            int destinationNumber = 0;

            string[] entryToLog = new string[] { $"{DateTime.Now:MM/dd/yyyy hh:mm:ss tt}", transaction, $"{startBalance:C2}", $"{currentBalance:C2}" };
            outputToWrite.Add(entryToLog);

            dataIO.WriteOutputToSource(userIO, outputToWrite, destinationNumber);
        }

        //Writes the vending machine sales report to a destination file whose file name is based on the
        //time and date the report was generated.
        public static void WriteSalesReport(IUserIO userIO, IDataIO dataIO, Dictionary<string, int> salesList, decimal salesSum)
        {
            List<string[]> outputToWrite = new List<string[]>();
            int destinationNumber = 1;

            foreach (KeyValuePair<string, int> kvp in salesList)
            {
                outputToWrite.Add(new string[2] { kvp.Key.ToString(), kvp.Value.ToString() });
            }

            outputToWrite.Add(new string[2] { "**TOTAL SALES**", $"{salesSum:C2}" });

            dataIO.WriteOutputToSource(userIO, outputToWrite, destinationNumber);
        }
    }
}

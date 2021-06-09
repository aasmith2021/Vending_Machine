using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public static class ReadWrite
    {
        //This method reads the lines from an inventory file to produce the inventory
        //needed to stock the vending machine
        public static List<string[]> ReadInventoryFile(IDataReader dataReader)
        {
            string inventoryFileName = "Inventory.txt";
            string delimiter = "|";
            List<string[]> inventory = dataReader.GetDataFromSource(inventoryFileName, delimiter);

            return inventory;
        }

        //Writes a vending machine log entry to the Log.txt file for a specific transaction
        public static void LogEntry(IOutput output, string transaction, decimal startBalance, decimal currentBalance)
        {
            string outputString = $"{DateTime.Now:MM/dd/yyyy hh:mm:ss tt} {transaction} {startBalance:C2} {currentBalance:C2}";
            string destinationFile = "Log.txt";
            bool appendFile = true;

            output.ProduceOutput(outputString, destinationFile, appendFile);
        }

        //Writes the vending machine sales report to a destination file whose file name is based on the
        //time and date the report was generated.
        public static void WriteSalesReport(IOutput output, Dictionary<string, int> salesList, decimal salesSum)
        {
            string outputString = "";
            string destinationFile = $"{DateTime.Now:MM-dd-yyyy_hh.mm.ss_tt} Sales Report.txt";
            bool appendFile = true;

            foreach (KeyValuePair<string, int> kvp in salesList)
            {
                outputString = $"{kvp.Key}|{kvp.Value}";

                output.ProduceOutput(outputString, destinationFile, appendFile);
            }

            string[] endOfSalesReport = new string[] { "", $"**TOTAL SALES** {salesSum:C2}" };

            foreach (string element in endOfSalesReport)
            {
                outputString = element;

                output.ProduceOutput(outputString, destinationFile, appendFile);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Capstone
{
    public class VendIO
    {
        //Writes a sales report to a file, using the current date and time in the file name
        public static void WriteSalesReport(VendingMachine vm)
        {
            string fileName = $"{DateTime.Now:MM-dd-yyyy_hh.mm.ss_tt} Sales Report.txt";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (KeyValuePair<string, int> kvp in vm.salesList)
                {
                    sw.WriteLine($"{kvp.Key}|{kvp.Value}");
                }
                sw.WriteLine();
                sw.WriteLine($"**TOTAL SALES** {vm.SalesSum:C2}");
            }
        }

        //Writes a vending machine log entry to the Log.txt file for a specific transaction
        public static void LogEntry(string transaction, decimal startBalance, decimal currentBalance)
        {
            string logLine = $"{DateTime.Now:MM/dd/yyyy hh:mm:ss tt} {transaction} {startBalance:C2} {currentBalance:C2}";
            string fileName = "Log.txt";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(logLine);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid or not found log file.");
            }
        }

        //This method reads the lines from the Inventory.txt file and uses RegEx to separate out the
        //item components (name, price, etc.), then calls ParseAndCreateItem to create each item in the vending machine
        public static void ReadInput(VendingMachine vm)
        {
            string fileName = "Inventory.txt";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string regexPattern = @"^(?<slotid>^[A-Z]\d+)[,|](?<name>.+)[,|](?<price>\d*\.\d+)[,|](?<category>.+)$";
                        GroupCollection itemMatchGroups = Regex.Match(line, regexPattern).Groups;
                        ParseAndCreateItem(itemMatchGroups, vm);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("<Error> Unable to load inventory from file. Please contact your local Vendo-Matic service expert for assistance.");
            }
        }

        //This method is called by ReadInput to create a single type of item in the vending machine
        //based on the item data read from the Inventory.txt file
        public static void ParseAndCreateItem(GroupCollection matchGroups, VendingMachine vm)
        {
            string slotId = matchGroups["slotid"].Value;
            string name = matchGroups["name"].Value;
            string category = matchGroups["category"].Value;
            decimal price = Decimal.Parse(matchGroups["price"].Value);
            Item newItem = new Item(name, price, category);
            vm.InventoryList[slotId]=(new Slot(slotId, newItem));
        }
    }
}

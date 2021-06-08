using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class VendIOTests
    {
        private string GetFileContents(string fileName)
        {
            string fileContents = "";

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        fileContents += sr.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return fileContents;
        }

        private void ClearTestLogFile()
        {
            string blankFileContents = "";

            string fileName = "Log.txt";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine(blankFileContents);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [TestMethod]
        public void WriteSalesReport_produces_correct_report_with_valid_data()
        {
            //Arrange
            VendingMachine testVM = new VendingMachine(false);
            Dictionary<string, int> testSalesList = new Dictionary<string, int>()
            {
                { "Big Red", 5 },
                { "Coca Cola", 7 },
                { "Sprite", 0 },
                { "Lay's Original", 1 },
                { "Cheetos", 100 },
                { "Skittles", 5 },
            };
            testVM.salesList = testSalesList;
            testVM.SalesSum = 27.99m;

            string expected = $"Big Red|5Coca Cola|7Sprite|0Lay's Original|1Cheetos|100Skittles|5**TOTAL SALES** {testVM.SalesSum:C2}";

            //Act
            VendIO.WriteSalesReport(testVM);
            string actual = GetFileContents($"{DateTime.Now:MM-dd-yyyy_hh.mm.ss_tt} Sales Report.txt");

            //Assert
            Assert.AreEqual(expected, actual, "Output from WriteSalesReport did not match expected output");
        }

        [TestMethod]
        public void LogEntry_correctly_writes_entry_to_file()
        {
            //Arrange
            string testTransaction = "SELL ITEM:";
            decimal testStartBalance = 32.00m;
            decimal testCurrentBalance = 27.99m;

            string currentTimeStamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            string expected = $"{currentTimeStamp} {testTransaction} {testStartBalance:C2} {testCurrentBalance:C2}";

            //Act
            VendIO.LogEntry(testTransaction, testStartBalance, testCurrentBalance);
            string actual = GetFileContents($"Log.txt");

            //Assert
            Assert.AreEqual(expected, actual, "LogEntry did not write the expected output to file");
            ClearTestLogFile();
        }
    }
}

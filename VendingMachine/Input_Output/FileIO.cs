using System;
using System.Collections.Generic;
using System.IO;

namespace Capstone
{
    public class FileIO : IDataIO
    {
        //This class creates an object that implements the IDataIO interface to read data from or
        //write data to a file in the current directory
        public List<string[]> GetInventoryData(IUserIO userIO)
        {
            string sourceFileName = "Inventory.txt";
            char[] delimiters = { '|', ',' };

            List<string[]> inventoryFromFile = new List<string[]>();
            bool readSuccessful = false;

            do
            {
                string filePathToReadFrom = Path.Combine(Directory.GetCurrentDirectory(), sourceFileName);

                try
                {
                    using (StreamReader sr = new StreamReader(filePathToReadFrom))
                    {
                        while (!sr.EndOfStream)
                        {
                            string fileLineText = sr.ReadLine();
                            string[] splitFileLineText = fileLineText.Split(delimiters);
                            inventoryFromFile.Add(splitFileLineText);
                        }
                    }

                    readSuccessful = true;
                }
                catch (Exception ex)
                {
                    userIO.DisplayData("Unable to find file to read from. Please enter new file path:");
                    filePathToReadFrom = userIO.GetInput();
                }
            }
            while (!readSuccessful);

            return inventoryFromFile;
        }

        public void WriteOutputToSource(IUserIO userIO, List<string[]> outputToWrite, int destinationNumber)
        {
            //For this Vending Machine application, the following destination numbers correspond to
            //the labeled output files:
            // 0 - Log File
            // 1 - Sales Report File

            bool appendFile = false;
            bool writeSuccessful = false;
            string destinationFileName;
            string textToWriteToFile = "";

            if (destinationNumber == 0)
            {
                appendFile = true;
                destinationFileName = "Log.txt";
                string[] logEntryToWrite = outputToWrite[0];

                for (int i = 0; i < logEntryToWrite.Length; i++)
                {
                    string logEntryComponent = logEntryToWrite[i];
                    
                    if (i != (logEntryToWrite.Length - 1))
                    {
                        textToWriteToFile += logEntryComponent + " ";
                    }
                    else
                    {
                        textToWriteToFile += logEntryComponent;
                    }
                }
            }
            else
            {
                destinationFileName = $"{DateTime.Now:MM-dd-yyyy_hh.mm.ss_tt} Sales Report.txt";

                for (int i = 0; i < outputToWrite.Count - 1; i++)
                {
                    string[] logEntryToWrite = outputToWrite[i];

                    for (int j = 0; j < logEntryToWrite.Length; j++)
                    {
                        string logEntryComponent = logEntryToWrite[j];

                        if (j != (logEntryToWrite.Length - 1))
                        {
                            textToWriteToFile += logEntryComponent + "|";
                        }
                        else
                        {
                            textToWriteToFile += logEntryComponent + Environment.NewLine;
                        }
                    }
                }

                textToWriteToFile += $"{Environment.NewLine}{outputToWrite[outputToWrite.Count - 1][0]} {outputToWrite[outputToWrite.Count - 1][1]}";
            }

            do
            {
                string filePathToWriteTo = Path.Combine(Directory.GetCurrentDirectory(), destinationFileName);

                try
                {
                    using (StreamWriter sw = new StreamWriter(filePathToWriteTo, appendFile))
                    {
                        sw.WriteLine(textToWriteToFile);
                    }

                    writeSuccessful = true;
                }
                catch (Exception ex)
                {
                    userIO.DisplayData("Unable to find destination file to write to. Please enter new file path:");
                    filePathToWriteTo = userIO.GetInput();
                }
            }
            while (!writeSuccessful);
        }
    }
}

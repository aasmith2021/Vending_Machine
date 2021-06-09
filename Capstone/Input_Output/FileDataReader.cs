using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public class FileDataReader : IDataReader
    {
        //This class creates an object that implements the IDataReader
        //interface to read input from a file stored in the current directory
        public List<string[]> GetDataFromSource(IInput input, IDisplay display, string fileToRead, string delimiter)
        {
            List<string[]> fileContents = new List<string[]>();
            bool readSuccessful = false;

            do
            {
                string filePathToReadFrom = Path.Combine(Directory.GetCurrentDirectory(), fileToRead);

                try
                {
                    using (StreamReader sr = new StreamReader(filePathToReadFrom))
                    {
                        while (!sr.EndOfStream)
                        {
                            string fileLineText = sr.ReadLine();
                            string[] splitFileLineText = fileLineText.Split(delimiter);
                            fileContents.Add(splitFileLineText);
                        }
                    }

                    readSuccessful = true;
                }
                catch (Exception ex)
                {
                    display.DisplayData("Unable to find file to read from. Please enter new file path:");
                    filePathToReadFrom = input.GetInput();
                }
            }
            while (!readSuccessful);

            return fileContents;
        }
    }
}

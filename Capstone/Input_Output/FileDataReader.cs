using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public class FileDataReader : IDataReader
    {
        public List<string[]> GetDataFromSource(string fileToRead, string delimiter)
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
                    Console.WriteLine("Unable to find file to read from. Please enter new file path:");
                    filePathToReadFrom = new ConsoleInputGetter().GetInput();
                }
            }
            while (!readSuccessful);

            return fileContents;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone
{
    public class FileOutputWriter : IOutput
    {
        public void ProduceOutput(IInput input, string outputToWrite, string destinationFile, bool appendFile)
        {
            bool writeSuccessful = false;

            do
            {
                string filePathToWriteTo = Path.Combine(Directory.GetCurrentDirectory(), destinationFile);

                try
                {
                    using (StreamWriter sw = new StreamWriter(filePathToWriteTo, appendFile))
                    {
                        sw.WriteLine(outputToWrite);
                    }

                    writeSuccessful = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to find destination file to write to. Please enter new file path:");
                    filePathToWriteTo = input.GetInput();
                }
            }
            while (!writeSuccessful);
        }
    }
}

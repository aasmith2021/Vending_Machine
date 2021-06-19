using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public interface IDataIO
    {
        //This interface is used to read data into the program and output data from the program with
        //an external source such as a file or a database
        public List<string[]> GetInventoryData(IUserIO userIO);

        public void WriteOutputToSource(IUserIO userIO, List<string[]> outputToWrite, int destinationNumber);
    }
}

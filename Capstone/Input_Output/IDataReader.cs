using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public interface IDataReader
    {
        //This interface is used to read data into the program from an external source,
        //such as getting data from a file or a database
        public List<string[]> GetDataFromSource(IInput input, IDisplay display, string source, string delimiter);
    }
}

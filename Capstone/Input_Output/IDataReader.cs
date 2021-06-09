using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public interface IDataReader
    {
        public List<string[]> GetDataFromSource(string source, string delimiter);
    }
}

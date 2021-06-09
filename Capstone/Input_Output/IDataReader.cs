using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public interface IDataReader
    {
        public List<string[]> GetDataFromSource(IInput input, string source, string delimiter);
    }
}

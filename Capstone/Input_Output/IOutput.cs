using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public interface IOutput
    {
        //This interface is used to structure classes that produce output data from the vending
        //machine, such as writing information to a file or a database
        public void ProduceOutput(IInput input, IDisplay display, string output, string destination, bool appendDestination);
    }
}

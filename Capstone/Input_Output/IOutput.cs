using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public interface IOutput
    {
        public void ProduceOutput(IInput input, string output, string destination, bool appendDestination);
    }
}

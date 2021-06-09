using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public interface IDisplay
    {
        public void DisplayData(string dataToDisplay = "");

        public void ClearDisplay();
    }
}

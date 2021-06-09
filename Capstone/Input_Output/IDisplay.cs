using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public interface IDisplay
    {
        //This interface is used to structure classes that display the output from the
        //vending machine. It also provides structure to provide a way to clear the display.
        public void DisplayData(string dataToDisplay = "");

        public void ClearDisplay();
    }
}

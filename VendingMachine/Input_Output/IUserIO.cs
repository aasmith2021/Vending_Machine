using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachineApplication
{
    public interface IUserIO
    {
        //This interface is used to get user input and display data to the user
        public string GetInput();

        public void DisplayData(string dataToDisplay = "");

        public void ClearDisplay();
    }
}

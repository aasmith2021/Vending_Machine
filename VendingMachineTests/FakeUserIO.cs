using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using VendingMachineApplication;

namespace VendingMachineTests
{
    [TestClass]
    public class FakeUserIO : IUserIO
    {
        public FakeUserIO(string testInput = "3")
        {
            this.TestInput = testInput;
        }

        public string TestInput { get; }
        
        public string GetInput()
        {
            return this.TestInput;
        }

        public void ClearDisplay()
        {

        }

        public void DisplayData(string dataToDisplay = "")
        {

        }


    }
}

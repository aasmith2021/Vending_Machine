using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class FakeUserIO3 : IUserIO
    {
        public string GetInput()
        {
            return "3";
        }

        public void ClearDisplay()
        {

        }

        public void DisplayData(string dataToDisplay = "")
        {

        }


    }
}

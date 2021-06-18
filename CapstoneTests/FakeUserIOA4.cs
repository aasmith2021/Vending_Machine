using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class FakeUserIOA4 : IUserIO
    {
        public string GetInput()
        {
            return "A4";
        }

        public void ClearDisplay()
        {

        }

        public void DisplayData(string dataToDisplay = "")
        {

        }
    }
}

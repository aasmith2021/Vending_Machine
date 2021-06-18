using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class FakeUserIOAdmin : IUserIO
    {
        public string GetInput()
        {
            return "admin";
        }

        public void ClearDisplay()
        {

        }

        public void DisplayData(string dataToDisplay = "")
        {

        }
    }
}

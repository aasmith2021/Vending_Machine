using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class FakeDisplay : IDisplay
    {
        public void DisplayData(string dataToDisplay = "")
        {
        }

        public void ClearDisplay()
        {
        }
    }
}

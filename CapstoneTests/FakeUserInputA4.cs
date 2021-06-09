using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class FakeUserInputA4 : IInput
    {
        public string GetInput()
        {
            return "A4";
        }
    }
}

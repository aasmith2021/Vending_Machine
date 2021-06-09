using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class FakeUserInput5 : IInput
    {
        public string GetInput()
        {
            return "5";
        }
    }
}

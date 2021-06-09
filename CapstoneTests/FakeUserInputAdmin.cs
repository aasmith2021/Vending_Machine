using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class FakeUserInputAdmin : IInput
    {
        public string GetInput()
        {
            return "admin";
        }
    }
}

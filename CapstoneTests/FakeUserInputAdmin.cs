using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Capstone;

namespace CapstoneTests
{
    [TestClass]
    public class FakeUserInputAdmin : IUserInput
    {
        public string GetInput()
        {
            return "admin";
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Votr.Models;

namespace Votr.Tests
{
    /// <summary>
    /// Summary description for PollTests
    /// </summary>
    [TestClass]
    public class PollTests
    {
        [TestMethod]
        public void EnsureICanCreateInstance()
        {
            Poll p = new Poll();
            Assert.IsNotNull(p);
        }
    }
}

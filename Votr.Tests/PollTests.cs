using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Votr.Models;
using Votr.DAL;

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

        [TestMethod]
        public void PollEnsureICanSaveAPoll()
        {
            //Arrange
            VotrContext context = new VotrContext();
            Poll p = new Poll();

            //Act
            context.Polls.Add(p);
            context.SaveChanges();

            //Assert
            Assert.AreEqual(1, context.Polls.Find().PollId);
        }
    }
}

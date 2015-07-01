using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABLCloudStaff.Models;
using ABLCloudStaff.Board_Logic;

namespace ABLCloudStaff.Tests.Board_Logic
{
    [TestClass]
    public class ChangeLogUtilitiesTest
    {
        [TestMethod]
        public void TestLogChange()
        {
            // Arrange
            Core os = new Core();
            os.UserID = 1;
            os.StateStart = DateTime.Now;
            Core ns = new Core();
            ns.UserID = 1;

            // Act 
            ChangeLogUtilities.LogChange(os, ns);

            ChangeLog cl = new ChangeLog();

            using (var context = new ABLCloudStaffContext())
            {
                cl = context.ChangeLogs.FirstOrDefault();
            }

            // Assert
            Assert.AreEqual(cl.UserID, os.UserID);
        }
    }
}

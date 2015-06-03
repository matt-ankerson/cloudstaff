using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABLCloudStaff.Models;
using ABLCloudStaff.Board_Logic;

namespace ABLCloudStaff.Tests.Board_Logic
{
    [TestClass]
    public class UserUtilitiesTest
    {
        [TestMethod]
        public void TestGetAllUsers()
        {
            // Arrange
            List<User> actual = new List<User>();

            // Act
            try
            {
                actual = UserUtilities.GetAllUsers();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            // Assert
            CollectionAssert.AllItemsAreUnique(actual);
        }
    }
}

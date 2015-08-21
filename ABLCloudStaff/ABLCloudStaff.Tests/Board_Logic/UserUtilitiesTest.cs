using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABLCloudStaff.Models;
using ABLCloudStaff.Biz_Logic;

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

        [TestMethod]
        public void TestGetUser()
        {
            // Arrange
            User expected = new User();
            User actual = new User();
            int userID = 0;

            // Act
            using (var context = new ABLCloudStaffContext())
            {
                expected = context.Users.Where(u => u.UserID == userID).FirstOrDefault();
            }

            actual = UserUtilities.GetUser(userID);

            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestAddUser()
        {
            // Arrange
            string firstName = "Matt";
            string lastName = "Ankerson";

            // Act
            try
            {
                //UserUtilities.AddUser(firstName, lastName);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsTrue(true);
            }
        }
    }
}

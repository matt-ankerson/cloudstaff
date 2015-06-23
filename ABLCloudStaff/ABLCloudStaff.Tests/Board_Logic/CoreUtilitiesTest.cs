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
    public class CoreUtilitiesTest
    {
        [TestMethod]
        public void TestGetAllCores()
        {
            // Arrange
            List<Core> actual = new List<Core>();

            // Act
            try
            {
                actual = CoreUtilities.GetAllCoreInstances();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            // Assert
            CollectionAssert.AllItemsAreUnique(actual);
            CollectionAssert.AllItemsAreNotNull(actual);
            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(Core));
        }

        [TestMethod]
        public void TestGetCoreByCoreID()
        {
            // Arrange
            int coreID = 1;
            Core actual = new Core();

            // Act
            actual = CoreUtilities.GetCoreInstanceByCoreID(coreID);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(Core));
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void TestGetCoreByUserID()
        {
            // Arrange
            int userID = 1;
            Core actual = new Core();

            // Act
            actual = CoreUtilities.GetCoreInstanceByUserID(userID);

            // Assert
            Assert.IsInstanceOfType(actual, typeof(Core));
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void TestGetArbitraryNumberOfCoresByCoreIDs()
        {
            // Arrange
            List<int> coreIDs = new List<int>();
            for (int i = 1; i < 5; i++)
                coreIDs.Add(i);

            // Act
            List<Core> cores = CoreUtilities.GetArbitraryNumberOfCoresByCoreID(coreIDs);

            // Assert
            CollectionAssert.AllItemsAreInstancesOfType(cores, typeof(Core));
            CollectionAssert.AllItemsAreNotNull(cores);
            CollectionAssert.AllItemsAreUnique(cores);
        }

        [TestMethod]
        public void TestGetArbitraryNumberOfCoresByUserIDs()
        {
            // Arrange
            List<int> userIDs = new List<int>();
            for (int i = 1; i < 5; i++)
                userIDs.Add(i);

            // Act
            List<Core> cores = CoreUtilities.GetArbitraryNumberOfCoresByUserID(userIDs);

            // Assert
            CollectionAssert.AllItemsAreInstancesOfType(cores, typeof(Core));
            CollectionAssert.AllItemsAreNotNull(cores);
            CollectionAssert.AllItemsAreUnique(cores);
        }

        [TestMethod]
        public void TestGetStatusByCoreID()
        {
            // Arrange
            int coreID = 1;
            Status expected;

            // Act
            Status actual = CoreUtilities.GetStatusByCoreID(coreID);

            using(var context = new ABLCloudStaffContext())
            {
                Core thisCore = context.Cores.Include("Status").Where(c => c.CoreID == coreID).FirstOrDefault();
                expected = thisCore.Status;
            }

            // Assert
            Assert.AreEqual(expected.Name, actual.Name);
        }
    }
}

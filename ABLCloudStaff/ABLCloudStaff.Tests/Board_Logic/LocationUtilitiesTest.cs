using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABLCloudStaff.Models;
using ABLCloudStaff.Biz_Logic;

namespace ABLCloudStaff.Tests.Board_Logic
{
    [TestClass]
    public class LocationUtilitiesTest
    {
        [TestMethod]
        public void TestGetAvailableLocations()
        {
            // Arrange
            int userID = 1;

            // Act
            List<Location> actual = LocationUtilities.GetAvailableLocations(userID);

            // Assert
            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(Location));
            CollectionAssert.AllItemsAreNotNull(actual);
            CollectionAssert.AllItemsAreUnique(actual);
        }
    }
}

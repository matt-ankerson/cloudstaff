using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABLCloudStaff.Models;
using ABLCloudStaff.Biz_Logic;

namespace ABLCloudStaff.Tests.Biz_Logic
{
    [TestClass]
    public class StatusUtilitiesTest
    {
        public void TestGetAvailableStatuses()
        {
            // Arrange
            int userID = 1;

            // Act
            List<Status> actual = StatusUtilities.GetAvailableStatuses(userID);
            
            // Assert
            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(Status));
            CollectionAssert.AllItemsAreNotNull(actual);
            CollectionAssert.AllItemsAreUnique(actual);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ABLCloudStaff.Biz_Logic;

namespace ABLCloudStaff.Tests.Biz_Logic
{
    [TestClass]
    public class AuthenticationUtilitiesTest
    {
        [TestMethod]
        public void TestHashPasswordCorrect()
        {
            // Arrange
            string originalPassword = "Password";

            // Act
            string hashOfOriginal = EncryptionUtilities.HashPassword(originalPassword);
            bool match = AuthenticationUtilities.VerifyHashedPassword(originalPassword, hashOfOriginal);

            // Assert
            Assert.IsTrue(match);
        }

        [TestMethod]
        public void TestHashPasswordInCorrect()
        {
            // Arrange
            string originalPassword = "Password";
            string testPassword = "123456789";

            // Act
            string hashOfOriginal = EncryptionUtilities.HashPassword(originalPassword);
            bool match = AuthenticationUtilities.VerifyHashedPassword(testPassword, hashOfOriginal);

            // Assert
            Assert.IsFalse(match);
        }
    }
}

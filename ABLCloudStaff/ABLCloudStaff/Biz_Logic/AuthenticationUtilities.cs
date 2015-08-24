// Author: Matt Ankerson
// Date: 17 August 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Provodes utilities for Authentication
    /// </summary>
    public static class AuthenticationUtilities
    {
        /// <summary>
        /// Add a new Authentication instance to the model.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="token"></param>
        public static void AddAuthentication(int userID, string userName, string password, string token=null)
        {
            // User and Authentication share an optional 1 to 1 relationship.

            // We first set up required fields in the User table:
            // Given a userID, we need to pull up that user and set thier AuthenticationID to thier UserID.

            // Next we add a new Authentication instance with the given information. Password and Token need to be hashed. 
            //  (Token might be null, in which case, leave it null.)
            // The UserID field in the Authentication instance is set to the userID that was passed in.

            // Now the 1 to 1 relationship is set up
        }

        /// <summary>
        /// Verify a given password against a given user's already stored password.
        /// </summary>
        /// <param name="userID">The user in question.</param>
        /// <param name="password">The password given in this request.</param>
        /// <returns>Boolean value indicating success or failure.</returns>
        public static bool VerifyPassword(int userID, string password)
        {
            bool passwordsMatch = false;

            try
            {
                // Get the user in question
                User user = UserUtilities.GetUser(userID);

                if(user != null)
                {
                    // Get the password already stored in the db
                    byte[] userPassword = user.Authentication.Password;
                    // Assess whether or not the passwords match.
                    passwordsMatch = VerifyHashedPassword(password, Convert.ToBase64String(userPassword));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't get password for user " + userID.ToString() + " " + ex.Message);
            }

            return passwordsMatch;
        }

        /// <summary>
        /// Compare two passwords, a new one, and an existing password from the db.
        /// </summary>
        /// <param name="password">The new password</param>
        /// <param name="correctHash">Password from the db.</param>
        /// <returns>Boolean value indicating success or failure.</returns>
        public static bool VerifyHashedPassword(string password, string correctHash)
        {
            // Extract the parameters from the hash.
            char[] delimeter = { ':' };
            string[] split = correctHash.Split(delimeter);

            // The hash precedes the salt.
            byte[] hash = Convert.FromBase64String(split[0]);
            byte[] salt = Convert.FromBase64String(split[1]);

            string testHash = EncryptionUtilities.HashPassword(password, salt);

            return testHash.Equals(correctHash);
        }
    }
}
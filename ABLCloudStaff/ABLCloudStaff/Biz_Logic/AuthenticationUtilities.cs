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
        /// Return the userID that maps to a given username. Return 0 if no user exists.
        /// </summary>
        /// <param name="userName">The username to search on.</param>
        /// <returns>The appropriate userID.</returns>
        public static int GetUserIDOnUserName(string userName)
        {
            // Initialise to 0, this is an invalid userID.
            int userID = 0;

            using (var context = new ABLCloudStaffContext())
            {
                try
                {
                    // Pull up the Authentication instance by the given username. We assume uniqueness.
                    Authentication authInstance = context.Authentications.Where(x => x.UserName == userName).FirstOrDefault();

                    userID = authInstance.UserID;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return userID;
        }

        /// <summary>
        /// Add a new Authentication instance to the model.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="token"></param>
        public static void AddAuthentication(int userID, string userName, string password, string token=null)
        {
            // User and Authentication share an optional 1 to 1 relationship.

            try
            {
                // We first set up required fields in the User table:
                // Given a userID, we need to pull up that user and set thier AuthenticationID to thier UserID.
                using (var context = new ABLCloudStaffContext())
                {
                    // Before we do anything:
                    // Enforce UserName uniqueness. - Check for this.
                    List<string> existingUsernames = context.Authentications.Select(x => x.UserName).ToList();

                    if (existingUsernames.Contains(userName))
                        throw new Exception("Username must be unique.");

                    User u = context.Users.Where(x => x.UserID == userID).FirstOrDefault();
                    u.AuthenticationID = userID;
                    context.SaveChanges();
                }

                // Next we add a new Authentication instance with the given information. Password and Token need to be hashed. 
                //  (Token might be null, in which case, leave it null.)
                // The UserID field in the Authentication instance is set to the userID that was passed in.
                using (var context = new ABLCloudStaffContext())
                {
                    // Hash the password
                    byte[] passwordHash = Convert.FromBase64String(EncryptionUtilities.HashPassword(password));

                    // Hash the token if not null
                    if (token != null)
                    {
                        byte[] tokenHash = Convert.FromBase64String(EncryptionUtilities.HashPassword(token));
                        Authentication auth = new Authentication { UserID = userID, UserName = userName, Password = passwordHash, Token = tokenHash };
                    }
                    else
                    {
                        Authentication auth = new Authentication { UserID = userID, UserName = userName, Password = passwordHash };
                    } 
                }

                // Now the 1 to 1 relationship is set up
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't add authentication: " + ex.Message);
            }
        }

        /// <summary>
        /// Verify given user credentials, returns empty string if successful.
        /// </summary>
        /// <param name="userID">The user to check</param>
        /// <param name="userName">Username to check</param>
        /// <param name="password">Password to check</param>
        /// <returns>Empty string or message containing reason for failure.</returns>
        public static string AuthenticateUsernamePassword(int userID, string userName, string password)
        {
            string response = "";

            try
            {
                // Pull up the user indicated by the given userID
                User u = UserUtilities.GetUser(userID);

                // Check username and password
                if (userName.Equals(u.Authentication.UserName))
                {
                    // Username matches, now check password
                    if (!VerifyPassword(userID, password))
                    {
                        response = "Password is incorrect.";
                    }
                }
                else
                {
                    response = "Username is invalid.";
                }
            }
            catch (Exception ex)
            {
                response = "Authentication Failed.";
                throw new Exception("Authentication Failed: " + ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Authenticate a given api token against one already stored in the db. Return appropriate response.
        /// </summary>
        /// <param name="userID">The user to authenticate against.</param>
        /// <param name="token">The token to try</param>
        /// <returns>Empty string for success, or reason for failure.</returns>
        public static string AuthenticateUserIDToken(int userID, string token)
        {
            string response = "";

            try
            {
                // Pull up the user indicated by the given userID
                User u = UserUtilities.GetUser(userID);
                
                // Check that a token exists already in the db
                if (u.Authentication.Token != null)
                {
                    string existing = Convert.ToBase64String(u.Authentication.Token);

                    if(VerifyHashedPassword(token, existing))
                    {
                        // Success!
                    }
                    else
                    {
                        response = "Invalid token.";
                    }
                }
                else
                {
                    response = "User has no token.";
                }
            }
            catch (Exception ex)
            {
                response = "Authentication failed due to an error.";
                throw new Exception("Authentication Failed: " + ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Issue and store a new api token for a user who doesn't already own one.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>The new api token.</returns>
        public static string IssueApiToken(int userID)
        {
            string response = "";

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Check whether or not this user already has a api token
                    User u = context.Users.Include("Authentication").Where(x => x.UserID == userID).FirstOrDefault();

                    if (u.Authentication.Token == null)
                    {
                        // Generate a new token
                        string newToken = EncryptionUtilities.GenerateApiToken();

                        // Save the new token
                        u.Authentication.Token = Convert.FromBase64String(newToken);

                        // Return the new token
                        response = newToken;
                    }
                    else
                    {
                        // Return the token that already exists
                        response = Convert.ToBase64String(u.Authentication.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't generate token: " + ex.Message);
            }

            return response;
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
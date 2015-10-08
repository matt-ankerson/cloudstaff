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
        /// Return the userID that maps to a given userName. Return 0 if no username exists.
        /// </summary>
        /// <param name="userName">The userName to search on.</param>
        /// <returns>The appropriate userID.</returns>
        public static int GetUserIDOnUserName(string userName)
        {
            // Initialise to 0, this is an invalid userID.
            int userID = 0;

            using (var context = new ABLCloudStaffContext())
            {
                try
                {
                    // Pull up the Authentication instance by the given userName. We assume uniqueness.
                    Authentication authInstance = context.Authentications.Where(x => x.UserName == userName).FirstOrDefault();

                    // Check that the auth instance isn't null.
                    if (authInstance != null)
                        userID = authInstance.UserID;
                }
                catch (Exception ex)
                {
                    ErrorUtilities.LogException(ex.Message, DateTime.Now);
                    throw ex;
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
                // Given a userID, we need to pull up that username and set thier AuthenticationID to thier UserID.
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
                    string passwordHash = EncryptionUtilities.HashPassword(password);

                    // Hash the token if not null
                    if (token != null)
                    {
                        string tokenHash = EncryptionUtilities.HashPassword(token);
                        Authentication auth = new Authentication { UserID = userID, UserName = userName, Password = passwordHash, Token = tokenHash };
                        context.Authentications.Add(auth);
                    }
                    else
                    {
                        Authentication auth = new Authentication { UserID = userID, UserName = userName, Password = passwordHash };
                        context.Authentications.Add(auth);
                    }

                    context.SaveChanges();
                }

                // Now the 1 to 1 relationship is set up
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now, "Couldn't add authentication information.");
                throw ex;
            }
        }

        /// <summary>
        /// Update the authentication instance indicated by the userID.
        /// </summary>
        /// <param name="userID">Indicates the Auth instance to update.</param>
        /// <param name="userName">The new username</param>
        /// <param name="password">The new password.</param>
        public static void UpdateAuthentication(int userID, string userName, string password)
        {
            // User and Authentication share an optional 1 to 1 relationship.

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Before we do anything:
                    // Username must be unique, so check for that. But, don't include this user's existing username.

                    // Get this user's existing username
                    string existingUsernameForThisUser = context.Authentications.Where(a => a.UserID == userID).Select(a => a.UserName).FirstOrDefault();

                    // Get a list of all existing usernames (not inclusive of this user's existing username.)
                    List<string> existingUsernames = context.Authentications.Where(x => x.UserName != existingUsernameForThisUser).Select(x => x.UserName).ToList();

                    if (existingUsernames.Contains(userName))
                        throw new Exception("Username must be unique.");

                    // As an extra step of caution, let's reset the AuthenticationID on the associated User instance
                    User u = context.Users.Where(x => x.UserID == userID).FirstOrDefault();
                    u.AuthenticationID = userID;

                    context.SaveChanges();      // Save changes to User instance.

                    // Hash the password before saving in the database.
                    string passwordHash = EncryptionUtilities.HashPassword(password);

                    // Pull up the Authentication instance.
                    Authentication auth = context.Authentications.Where(a => a.UserID == userID).FirstOrDefault();
                    // Update fields.
                    auth.UserName = userName;
                    auth.Password = passwordHash;

                    context.SaveChanges();      // Save changes to Authentication instance.       
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now, "Couldn't update authentication information.");
                throw ex;
            }
        }

        /// <summary>
        /// Verify given username credentials, returns empty string if successful.
        /// </summary>
        /// <param name="userID">The username to check</param>
        /// <param name="userName">Username to check</param>
        /// <param name="password">Password to check</param>
        /// <returns>Empty string or message containing reason for failure.</returns>
        public static string AuthenticateUsernamePassword(int userID, string userName, string password)
        {
            string response = "";

            try
            {
                // Pull up the username indicated by the given userID
                User u = UserUtilities.GetUser(userID);

                // Check userName and password
                if (userName.Equals(u.Authentication.UserName))
                {
                    // Username matches, start check password
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
                ErrorUtilities.LogException(ex.Message, DateTime.Now,  "Authentication Failed.");
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Authenticate a given api token against one already stored in the db. Return appropriate response.
        /// </summary>
        /// <param name="userID">The username to authenticate against.</param>
        /// <param name="token">The token to try</param>
        /// <returns>Empty string for success, or reason for failure.</returns>
        public static string AuthenticateUserIDToken(int userID, string token)
        {
            string response = "";

            try
            {
                // Pull up the username indicated by the given userID
                User u = UserUtilities.GetUser(userID);
                
                // Check that a token exists already in the db
                if (u.Authentication.Token != null)
                {
                    string existing = u.Authentication.Token;

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
                ErrorUtilities.LogException(ex.Message, DateTime.Now, "Authentication Failed.");
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Issue and store a new api token for a username who doesn't already own one.
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
                    // Get the indicated username.
                    User u = context.Users.Include("Authentication").Where(x => x.UserID == userID).FirstOrDefault();

                    // Generate a new token
                    string newToken = EncryptionUtilities.GenerateApiToken();

                    // Hash the new token
                    string newTokenHashed = EncryptionUtilities.HashPassword(newToken);

                    // Save the new hashed token
                    u.Authentication.Token = newTokenHashed;
                    context.SaveChanges();

                    // Return the unhashed version of the token
                    response = newToken;
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now, "Couldn't generate new api token");
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Verify a given password against a given username's already stored password.
        /// </summary>
        /// <param name="userID">The username in question.</param>
        /// <param name="password">The password given in this request.</param>
        /// <returns>Boolean value indicating success or failure.</returns>
        public static bool VerifyPassword(int userID, string password)
        {
            bool passwordsMatch = false;

            try
            {
                // Get the username in question
                User user = UserUtilities.GetUser(userID);

                if(user != null)
                {
                    // Get the password already stored in the db
                    string userPassword = user.Authentication.Password;
                    // Assess whether or not the passwords match.
                    passwordsMatch = VerifyHashedPassword(password, userPassword);
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now, "Couldn't get password for userID " + userID.ToString());
                throw ex;
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
            // Extract the parameters from the provided correct hash.
            char[] delimeter = { ':' };
            string[] split = correctHash.Split(delimeter);

            // The hash precedes the salt.
            byte[] salt = EncryptionUtilities.GetBytes(split[1]);

            // Hash the new password using the same salt used for the correct hash.
            string testHash = EncryptionUtilities.HashPassword(password, salt);

            // Compare the new hash with the correct hash.
            bool result = testHash.Equals(correctHash);
            return result;
        }

        /// <summary>
        /// Get the username type for the indicated username.
        /// </summary>
        /// <param name="userID">The username to query for.</param>
        /// <returns>String, representing the type of this username.</returns>
        public static string GetUserType(int userID)
        {
            string userType = "";

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    userType = context.Users.Include("UserType").Where(x => x.UserID == userID).Select(x => x.UserType.Type).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return userType;
        }
    }

    /// <summary>
    /// Encapsulates information for an api register response.
    /// </summary>
    public class RegisterInfo
    {
        public int UserID { get; set; }
        public string ApiToken { get; set; }
    }

    /// <summary>
    /// Encapsulates information for a helpful authentication error.
    /// </summary>
    public class AuthErrorInfo
    {
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}
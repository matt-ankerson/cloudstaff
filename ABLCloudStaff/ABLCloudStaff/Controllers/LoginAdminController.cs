using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABLCloudStaff.Biz_Logic;

namespace ABLCloudStaff.Controllers
{
    public class LoginAdminController : Controller
    {
        /// <summary>
        /// Pull up the login view for the administration screen.
        /// </summary>
        /// <returns>ActionResult object</returns>
        public ActionResult LoginAdmin()
        {
            // Return the login interface.
            return View();
        }

        /// <summary>
        /// Redirect to administration screen if login credentials valid.
        /// </summary>
        /// <returns>ActionResult obkect, depends on login result.</returns>
        public ActionResult Login(string username, string password)
        {
            string authResponse = "";

            // Authenticate the given credentials:

            try
            {
                // Get the userID for this user, this will allow us to authenticate correctly.
                int userID = AuthenticationUtilities.GetUserIDOnUserName(username);

                // If userID is 0, throw an exception.
                if (userID == 0)
                {
                    authResponse = "Login Failed. Unvalid Username.";
                    throw new Exception("Supplied username is invalid.");
                }

                // Check the type of this user, throw exception if not admin.
                string userType = AuthenticationUtilities.GetUserType(userID);

                if (userType != Constants.ADMIN_TYPE)
                {
                    authResponse = "Sorry, you don't have sufficient permission to access the administration controls.";
                    throw new Exception("User is unautherized.");
                }

                authResponse = AuthenticationUtilities.AuthenticateUsernamePassword(userID, username, password);

                // A successful auth result will return an empty string.
                if (authResponse != "")
                    throw new Exception("Login falied. " + authResponse);

                // Now we know the auth was successful, put the username into session and redirect to Administration.
                Session["username"] = username;

                // Return the administration view
                return RedirectToAction("Admin", "Admin");
            }
            catch (Exception ex)
            {
                // Flag the error, put description in viewbag.
                ViewBag.Message = authResponse;

                // Return the login interface again, it will indicate the failed result.
                return View("LoginAdmin");
            }
        }
	}
}
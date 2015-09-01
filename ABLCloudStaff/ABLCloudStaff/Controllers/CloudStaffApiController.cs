using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ABLCloudStaff.Biz_Logic;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Controllers
{
    /// <summary>
    /// RPC style api for external applications to interface with.
    /// </summary>
    public class CloudStaffApiController : ApiController
    {
        /// <summary>
        /// Issue and save a new api token for the indicated user, refuse if credentials invalid.
        /// <remarks>
        /// Sample Request:
        /// http://localhost:1169/api/cloudstaffapi/register/?userName=CloudStaff5&password=P@ssw0rd
        /// </remarks>
        /// </summary>
        /// <param name="userName">The userName of this user.</param>
        /// <param name="password">The password for this user.</param>
        /// <returns>A response message containing the token, or a message detailing the error.</returns>
        [HttpGet]
        public HttpResponseMessage Register(string userName, string password)
        {
            HttpResponseMessage response;
            string newApiToken;

            try
            {
                // Pull up the userID for the given user.
                // We're relying on usernames being unique.
                int userID = AuthenticationUtilities.GetUserIDOnUserName(userName);

                // If userID is 0, throw an exception.
                if (userID == 0)
                    throw new Exception("Supplied username is invalid.");

                // Authenticate the userName and password
                string authResponse = AuthenticationUtilities.AuthenticateUsernamePassword(userID, userName, password);

                // If authResponse contains an error message, throw an exception to deal with that.
                if (authResponse != "")
                    throw new Exception(authResponse);

                // Generate a token, save it and return it.
                newApiToken = AuthenticationUtilities.IssueApiToken(userID);

                // Create the succesful response.
                // A successful response contains the api token and the userID for this user.
                RegisterInfo data = new RegisterInfo { UserID = userID, ApiToken = newApiToken };

                response = Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                AuthErrorInfo data = new AuthErrorInfo { Message = "Failed to authenticate", Detail = ex.Message };
                // Create a response to report the problem.
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, data);
            }

            return response;
        }

        /// <summary>
        /// Get current status, location and time allotted for the indicated user
        /// <remarks>
        /// Sample request:
        /// http://localhost:1169/api/cloudstaffapi/getuserinfo/?userID=5&apiToken=XXXXXXXXXXX
        /// </remarks>
        /// </summary>
        /// <param name="userID">The user to get information for.</param>
        /// <returns>Information relavent for this user.</returns>
        [HttpGet]
        public HttpResponseMessage GetUserInfo(int userID, string apiToken)
        {
            HttpResponseMessage response;

            // Authenticate this user
            string authResult = AuthenticationUtilities.AuthenticateUserIDToken(userID, apiToken);

            if (authResult == "")
            {
                // User was successfully authenticated.

                try
                {
                    // Call to the application business logic
                    Core c = CoreUtilities.GetCoreInstanceByUserID(userID);

                    // Use a serializable type to encapsulate the data we want to send via json/xml
                    CoreInfo data = new CoreInfo
                    {
                        userID = c.UserID.ToString(),
                        statusID = c.StatusID.ToString(),
                        status = c.Status.Name,
                        locationID = c.LocationID.ToString(),
                        location = c.Location.Name,
                        returnTime = c.IntendedEndTime.ToString()
                    };

                    // Write the core info data to the response body.
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
                catch (Exception ex)
                {
                    // Report the error
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            else
            {
                // There was an issue.
                AuthErrorInfo data = new AuthErrorInfo { Message = "Failed to authenticate", Detail = authResult };
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, data);
            }

            return response;
        }

        /// <summary>
        /// Get the current date and time according to the server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCurrentTime()
        {
            HttpResponseMessage response;

            try
            {
                DateTime now = DateTime.Now;

                // Use a serialisable type to encapsulate the data we want to send via xml/json
                TimeInfo data = new TimeInfo
                {
                    numeric_repr = now.ToShortTimeString(),
                    dateString = now.ToString(),
                    year = now.Year.ToString(),
                    month = now.Month.ToString(),
                    day = now.Day.ToString(),
                    hour = now.Hour.ToString(),
                    minute = now.Minute.ToString(),
                    second = now.Second.ToString()
                };

                // Write the time info data to the response body.
                response = Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Get all available statuses for the given user
        /// <remarks>
        /// Sample request:
        /// http://localhost:1169/api/cloudstaffapi/getavailablestatuses/?userID=5&apiToken=XXXXXXXXXXX
        /// </remarks>
        /// </summary>
        /// <param name="userID">The user to query for</param>
        /// <returns>List of available statuses</returns>
        [HttpGet]
        public HttpResponseMessage GetAvailableStatuses(int userID, string apiToken)
        {
            HttpResponseMessage response;

            // Authenticate this user
            string authResult = AuthenticationUtilities.AuthenticateUserIDToken(userID, apiToken);

            if (authResult == "")
            {
                // User was successfully authenticated.

                try
                {
                    // Call to the application business logic
                    List<Status> rawStatuses = StatusUtilities.GetAvailableStatuses(userID);
                    List<StatusInfo> data = new List<StatusInfo>();

                    // Build list of statusinfos
                    foreach (var s in rawStatuses)
                    {
                        data.Add(new StatusInfo
                        {
                            name = s.Name,
                            statusID = s.StatusID.ToString(),
                            available = s.Available.ToString()
                        });
                    }

                    // Write the status info data to the response body.
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
                catch (Exception ex)
                {
                    // Report the error
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            else
            {
                // There was an issue.
                AuthErrorInfo data = new AuthErrorInfo { Message = "Failed to authenticate", Detail = authResult };
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, data);
            }

            return response;
        }

        /// <summary>
        /// Get all locations available to the given user
        /// <remarks>
        /// Sample request:
        /// http://localhost:1169/api/cloudstaffapi/getavailablelocations/?userID=5&apiToken=XXXXXXXXXXX
        /// </remarks>
        /// </summary>
        /// <param name="userID">The user to query on</param>
        /// <returns>List of location info objects</returns>
        [HttpGet]
        public HttpResponseMessage GetAvailableLocations(int userID, string apiToken)
        {
            HttpResponseMessage response;

            // Authenticate this user
            string authResult = AuthenticationUtilities.AuthenticateUserIDToken(userID, apiToken);

            if (authResult == "")
            {
                // User was successfully authenticated.

                try
                {
                    // Call to the application business logic
                    List<Location> rawLocations = LocationUtilities.GetAvailableLocations(userID);
                    List<LocationInfo> data = new List<LocationInfo>();

                    // Build a list of locationInfos
                    foreach (var l in rawLocations)
                    {
                        data.Add(new LocationInfo
                        {
                            locationID = l.LocationID.ToString(),
                            name = l.Name
                        });
                    }

                    // Write the location info data to the response body.
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
                catch (Exception ex)
                {
                    // Report the error
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            else
            {
                // There was an issue.
                AuthErrorInfo data = new AuthErrorInfo { Message = "Failed to authenticate", Detail = authResult };
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, data);
            }

            return response;
        }

        /// <summary>
        /// Push new status, location and time allotted to the server
        /// (By using a core info object, populated from json in the request body.)
        /// </summary>
        /// <remarks>
        /// POSTing format is as follows:
        /// Head:
        /// (input appropriate url beginning)
        /// http://localhost:3022/api/cloudstaffapi/poststatusorlocationupdate/?apiToken=XXXXXXXXX
        /// User-Agent: Fiddler
        /// Type: POST
        /// Content-Type: application/json 
        /// Host: localhost:3022
        /// Content-Length: 73
        /// Body:
        /// {'userID': '1', 'statusID': '1', 'locationID': '1', 'returnTime': '26-Aug-15 11:59:54 AM' }
        /// </remarks>
        /// <param name="coreInfo">The new core info for the given user</param>
        [HttpPost]
        public HttpResponseMessage PostStatusOrLocationUpdate([FromBody] CoreInfo coreInfo, [FromUri]string apiToken)
        {
            HttpResponseMessage response;

            // Convert string fields to usable types
            int userID = Convert.ToInt32(coreInfo.userID);
            int statusID = Convert.ToInt32(coreInfo.statusID);
            int locationID = Convert.ToInt32(coreInfo.locationID);

            // Authenticate this user
            string authResult = AuthenticationUtilities.AuthenticateUserIDToken(userID, apiToken);

            if (authResult == "")
            {
                // Authentication was successful. Try to perform the update.

                try
                {
                    // Perform the update. ReturnTime is handled as an optional field. 
                    CoreUtilities.UpdateStatus(userID, statusID, coreInfo.returnTime);
                    CoreUtilities.UpdateLocation(userID, locationID);

                    // Return status code 200.
                    response = Request.CreateResponse(HttpStatusCode.OK, "");
                }
                catch (Exception ex)
                {
                    // Report the error.
                    CoreUpdateErrorInfo data = new CoreUpdateErrorInfo { Message = "Bad request, inspect your input parameters", Detail = ex.Message };
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, data);
                }
            }
            else
            {
                // There was an issue.
                AuthErrorInfo data = new AuthErrorInfo { Message = "Failed to authenticate", Detail = authResult };
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, data);
            }

            return response;
        }
    }
}
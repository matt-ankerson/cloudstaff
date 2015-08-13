using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ABLCloudStaff.Board_Logic;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.API.Controllers
{
    /// <summary>
    /// RPC style api for external applications to interface with.
    /// </summary>
    public class CloudStaffApiController : ApiController
    {
        /// <summary>
        /// Get current status, location and time allotted for the indicated user
        /// </summary>
        /// <param name="userID">The user to get information for.</param>
        /// <returns>List containting information relavent for this user.</returns>
        [HttpGet]
        public Core GetUserInfo(int userID)
        {
            Core thisCore = CoreUtilities.GetCoreInstanceByUserID(userID);

            // Use an anonymous type to encapsulate the data we want to send via json/xml
            return thisCore;
        }


        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
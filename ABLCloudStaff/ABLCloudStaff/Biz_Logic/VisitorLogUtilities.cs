// Author: Matt Ankerson
// Date: 7 October 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Provides utilities for logging visitors.
    /// </summary>
    public static class VisitorLogUtilities
    {
        /// <summary>
        /// Log an entry in the visitor log for a newcoming visitor.
        /// </summary>
        /// <param name="visitorUserID">The userID of the person visiting.</param>
        /// <param name="visitedUserID">The userID of the person being visited.</param>
        /// <param name="company">The company that the visitor is associated with.</param>
        public static void LogVisitorIn(int visitorUserID, int visitedUserID, string company, DateTime intendedDepartTime)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Ensure that the given userIDs actually exist.
                    List<int> allUserIDs = context.Users.Select(x => x.UserID).ToList();

                    if (!allUserIDs.Contains(visitorUserID))
                        throw new Exception("Visitor is not a valid user.");
                    if (!allUserIDs.Contains(visitedUserID))
                        throw new Exception("Person being visited is not a valid user.");

                    // Ensure that the given visitor userID is not already visiting.
                    List<int> visitingUserIDs = context.VisitorLogs.Where(x => x.DepartedTime == null).Select(x => x.VisitorUserID).ToList();

                    if (visitingUserIDs.Contains(visitorUserID))
                        throw new Exception("This user has not yet departed from an existing visit.");

                    // Create a new VisitorLog object
                    VisitorLog vl = new VisitorLog 
                    { 
                        VisitorUserID = visitorUserID,
                        VisitedUserID = visitedUserID,
                        ArrivedTime = DateTime.Now,         // Include arrived time, but not departed time.
                        Company = company,
                        IntendedDepartTime = intendedDepartTime
                    };

                    // Push into the model
                    context.VisitorLogs.Add(vl);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }

        /// <summary>
        /// Fill in the departed time on the most recent visitor log for the given visiting userID
        /// </summary>
        /// <param name="visitorUserID"></param>
        public static void LogVisitorOut(int visitorUserID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the appropriate VisitorLog instance.
                    VisitorLog vl = context.VisitorLogs.OrderByDescending(x => x.VisitorLogID).Where(x => x.VisitorLogID == visitorUserID).FirstOrDefault();

                    // Ensure this is a valid visitor
                    if (vl == null)
                        throw new Exception("Cannot depart visitor. Not a valid visitor.");
                    // Ensure the visitor has not already left.
                    if (vl.DepartedTime != null)
                        throw new Exception("Visitor has already departed.");

                    // Fill in the departed time.
                    vl.DepartedTime = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }
    }
}
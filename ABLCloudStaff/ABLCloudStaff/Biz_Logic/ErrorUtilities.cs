// Author: Matt Ankerson
// Date: 25 September

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Provide a means of logging errors thrown within the app to the database table 'Errors'.
    /// </summary>
    public class ErrorUtilities
    {
        /// <summary>
        /// Log given exception details in the Error table
        /// </summary>
        /// <param name="exception">The main exception text</param>
        /// <param name="timeStamp">The time the error occured</param>
        /// <param name="innerException">Optional field for inner exception.</param>
        /// <param name="detail">Optional further detail about the nature of the exception.</param>
        public static void LogException(string exception, DateTime timeStamp, string detail="")
        {
            using (var context = new ABLCloudStaffContext())
            {
                try
                {
                    // Craft the error instance.
                    Error err = new Error { Exception = exception, TimeStamp = timeStamp, Detail = detail };
                    // Save to error table.
                    context.Errors.Add(err);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
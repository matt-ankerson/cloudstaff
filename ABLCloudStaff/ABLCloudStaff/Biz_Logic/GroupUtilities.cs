// Author: Matt Ankerson
// Date: 15 October 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABLCloudStaff.Models;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Provides utilities for groups.
    /// </summary>
    public static class GroupUtilities
    {
        /// <summary>
        /// Add a group to the database with 'Now' for the timestamp
        /// </summary>
        /// <param name="userIDs">List of UserIDs to include in this group.</param>
        public static void AddGroup(List<int> userIDs)
        {
            try
            {
                // Serialise the list of userIDs to a comma separated string representation.
                string members = "";

                foreach (int userID in userIDs)
                {
                    members += userID.ToString() + ",";
                }

                using (var context = new ABLCloudStaffContext())
                {
                    // Create new group
                    Group grp = new Group { Members = members, Active = true, Initiated = DateTime.Now };
                    // Add and save.
                    context.Groups.Add(grp);
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
        /// Deactivate the indicated group.
        /// </summary>
        /// <param name="groupID">The group to deactivate.</param>
        public static void RemoveGroup(int groupID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Pull up the indicated Group instance
                    Group grp = context.Groups.Where(x => x.GroupID == groupID).FirstOrDefault();
                    // Deactivate the group.
                    grp.Active = false;
                    // Save changes
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
        /// Get all currently actve groups.
        /// </summary>
        /// <returns>List of active groups, with member lists deserialised.</returns>
        public static List<GroupDeserialized> GetActiveGroups()
        {
            List<GroupDeserialized> groups = new List<GroupDeserialized>();

            try
            {
                List<Group> rawGroups = new List<Group>();

                using (var context = new ABLCloudStaffContext())
                {
                    // Get all active groups from database.
                    rawGroups = context.Groups.Where(x => x.Active == true).OrderByDescending(x => x.Initiated).ToList();
                }

                // Build list of deserialized groups.
                foreach (var rawGroup in rawGroups)
                {
                    var stringMembers = rawGroup.Members.Split(',');
                    List<int> intMembers = new List<int>();

                    // Loop over string array of members:
                    foreach (string stringMember in stringMembers)
                    {
                        intMembers.Add(Convert.ToInt32(stringMember));
                    }

                    // Create deserialised group object
                    GroupDeserialized grpDsrlzd = new GroupDeserialized { 
                        Members = intMembers, 
                        Active = rawGroup.Active, 
                        GroupID = rawGroup.GroupID, 
                        Initiated = rawGroup.Initiated 
                    };

                    // Add object to list
                    groups.Add(grpDsrlzd);
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return groups;
        }

        /// <summary>
        /// Get all groups regardless of whether or not they're actve.
        /// </summary>
        /// <returns>List of groups with deserialized members.</returns>
        public static List<GroupDeserialized> GetAllGroups()
        {
            List<GroupDeserialized> groups = new List<GroupDeserialized>();

            try
            {
                List<Group> rawGroups = new List<Group>();

                using (var context = new ABLCloudStaffContext())
                {
                    // Get all active groups from database.
                    rawGroups = context.Groups.OrderByDescending(x => x.Initiated).ToList();
                }

                // Build list of deserialized groups.
                foreach (var rawGroup in rawGroups)
                {
                    var stringMembers = rawGroup.Members.Split(',');
                    List<int> intMembers = new List<int>();

                    // Loop over string array of members:
                    foreach (string stringMember in stringMembers)
                    {
                        intMembers.Add(Convert.ToInt32(stringMember));
                    }

                    // Create deserialised group object
                    GroupDeserialized grpDsrlzd = new GroupDeserialized
                    {
                        Members = intMembers,
                        Active = rawGroup.Active,
                        GroupID = rawGroup.GroupID,
                        Initiated = rawGroup.Initiated
                    };

                    // Add object to list
                    groups.Add(grpDsrlzd);
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return groups;
        }
    }

    /// <summary>
    /// Deserialised version of the group class. (list of integers instead of comma separated string)
    /// </summary>
    public class GroupDeserialized
    {
        public int GroupID { get; set; }
        public List<int> Members { get; set; }
        public DateTime Initiated { get; set; }
        public bool Active { get; set; }
    }
}
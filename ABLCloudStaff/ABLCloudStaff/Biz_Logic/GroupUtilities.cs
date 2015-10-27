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
        /// Add a group to the database, autopopulates the UserInGroup table
        /// </summary>
        /// <param name="userIDs">List of UserIDs to include in this group.</param>
        public static void AddGroup(List<int> userIDs, string name, bool active = false, int priority=0)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    Group g = new Group { Name = name, Active = active, Priority = priority };
                    context.Groups.Add(g);
                    context.SaveChanges();
                }

                // For each userID given, add a UserInGroup instance for this new group.
                
                // Pull the group back out again for the ID.
                int groupID = 0;

                using (var context = new ABLCloudStaffContext())
                {
                    groupID = context.Groups.OrderByDescending(x => x.GroupID).Select(x => x.GroupID).FirstOrDefault();
                }

                if (groupID == 0)
                    throw new Exception("Failed to add group.");
                else
                {
                    // Add UserInGroup instances.
                    foreach (int userID in userIDs)
                    {
                        AddUserToGroup(userID, groupID);
                    }
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
        public static void DeactivateGroup(int groupID)
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
        /// Activate the indicated group.
        /// </summary>
        /// <param name="groupID">The group to activate</param>
        public static void ActivateGroup(int groupID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Pull up the indicated Group instance
                    Group grp = context.Groups.Where(x => x.GroupID == groupID).FirstOrDefault();
                    // Activate the group.
                    grp.Active = true;
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
        public static List<Group> GetActiveGroups()
        {
            List<Group> groups = new List<Group>();

            try
            {

                using (var context = new ABLCloudStaffContext())
                {
                    // Get all active groups from database.
                    groups = context.Groups.Where(x => x.Active == true).ToList();
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
        public static List<Group> GetAllGroups()
        {
            List<Group> groups = new List<Group>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get all active groups from database.
                    groups = context.Groups.OrderBy(x => x.Priority).ToList();
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
        /// Get all users belonging to an indicated group.
        /// </summary>
        /// <param name="groupID">The group to pull members for.</param>
        /// <returns>List of users.</returns>
        public static List<User> GetMembersOfGroup(int groupID)
        {
            List<User> members = new List<User>();

            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    members = context.UserInGroups.Where(x => x.GroupID == groupID).Select(x => x.User).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }

            return members;
        }

        /// <summary>
        /// Add a UserInGroup instance to the database
        /// </summary>
        /// <param name="userID">The user to add to the group</param>
        /// <param name="groupID">The group to add the user to.</param>
        public static void AddUserToGroup(int userID, int groupID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Ensure that both the user and the group actually exist.
                    List<int> existingUserIDs = context.Users.Select(x => x.UserID).ToList();
                    List<int> existingGroupIDs = context.Groups.Select(x => x.GroupID).ToList();

                    if (!existingUserIDs.Contains(userID))
                        throw new Exception("User does not exist.");
                    if (!existingGroupIDs.Contains(groupID))
                        throw new Exception("Group does not exist.");

                    // Create new UserInGroup instance
                    UserInGroup uig = new UserInGroup { GroupID = groupID, UserID = userID };
                    context.UserInGroups.Add(uig);
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
        /// Remove an indicated user from the indicated group.
        /// </summary>
        /// <param name="userID">The user to remove.</param>
        /// <param name="groupID">The group to remove.</param>
        public static void RemoveUserFromGroup(int userID, int groupID)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Ensure that both the user and the group actually exist.
                    List<int> existingUserIDs = context.Users.Select(x => x.UserID).ToList();
                    List<int> existingGroupIDs = context.Groups.Select(x => x.GroupID).ToList();

                    if (!existingUserIDs.Contains(userID))
                        throw new Exception("User does not exist.");
                    if (!existingGroupIDs.Contains(groupID))
                        throw new Exception("Group does not exist.");

                    // Remove the indicated UserInGroup instance from the database.
                    UserInGroup uigToDelete = context.UserInGroups.Where(x => x.UserID == userID).Where(x => x.GroupID == groupID).FirstOrDefault();
                    
                    if (uigToDelete != null)
                    {
                        context.UserInGroups.Remove(uigToDelete);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }

        /// <summary>
        /// Update the indicated group with the given information.
        /// </summary>
        /// <remarks>
        /// Updates useringroup table with newMembers.
        /// </remarks>
        /// <param name="groupID">The group to update</param>
        /// <param name="newName">The new name for the group.</param>
        /// <param name="newPriority">The new priority for the group.</param>
        /// <param name="newMembers">The new list of members for the group.</param>
        public static void UpdateGroup(int groupID, string newName, int newPriority, List<int> newMembers)
        {
            try
            {
                using (var context = new ABLCloudStaffContext())
                {
                    // Get the appropriate group from the database.
                    Group groupToUpdate = context.Groups.FirstOrDefault(x => x.GroupID == groupID);
                    // Update fields:
                    groupToUpdate.Name = newName;
                    groupToUpdate.Priority = newPriority;
                    context.SaveChanges();
                    // Pull up UserInGroup instances for this group
                    List<UserInGroup> uigs = context.UserInGroups.Where(x => x.GroupID == groupID).ToList();
                    // Remove these instances of UserInGroup
                    context.UserInGroups.RemoveRange(uigs);
                    context.SaveChanges();
                }

                // Add new UserInGroup instances for the list of new members
                foreach (int userID in newMembers)
                {
                    AddUserToGroup(userID, groupID);
                }
            }
            catch (Exception ex)
            {
                ErrorUtilities.LogException(ex.Message, DateTime.Now);
                throw ex;
            }
        }
    }

    public class GroupInfo
    {
        public string GroupID;
        public string Name;
        public string Active;
        public string Priority;
    }
}
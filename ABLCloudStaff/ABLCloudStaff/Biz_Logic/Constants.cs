using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Repository for important constant values
    /// </summary>
    public static class Constants
    {
        public const int DEFAULT_IN_STATUS = 1;
        public const int DEFAULT_OUT_STATUS = 2;
        public const int DEFAULT_LOCATION = 1;
        public const int SALT_SIZE = 4;
        public const int TOKEN_LENGTH = 20;
        public const string ADMIN_TYPE = "Admin";
        public const int SESSION_TIMEOUT = 10;
        public const int START_OF_DAY = 7;      // 7am
        public const int WORKDAY_DURATION = 12; // hours
        public static readonly int[] DEFAULT_STATUSES = { 1, 2, 3, 4, 5, 6 };
        public static readonly int[] DEFAULT_LOCATIONS = { 1, 2, 3, 4 };
        public static readonly string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    }
}
﻿using System;
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
        public const int VISITING_STATUS = 7;
        public const int SALT_SIZE = 4;
        public const int TOKEN_LENGTH = 20;
        public const string ADMIN_TYPE = "Admin";
        public const string VISITOR_TYPE = "Visitor";
        public const string GENERAL_TYPE = "General";
        public const int SESSION_TIMEOUT = 10;
        public const int START_OF_DAY = 6;      // 6am
        public const int END_OF_DAY = 18;       // 12am <--- it is imperative that this be changed to 18 when debugging is finished
        public const int WORKDAY_DURATION = 12; // hours
        public static readonly int[] DEFAULT_STATUSES = { 1, 2, 3, 4, 5, 6, 8 };
        public static readonly int[] DEFAULT_LOCATIONS = { 1, 2, 3, 4, 6 , 7 };
        public static readonly string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public const int MAX_PRIORITY = 10;
    }
}
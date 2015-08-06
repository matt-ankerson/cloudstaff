using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABLCloudStaff.Board_Logic
{
    /// <summary>
    /// Repository for important constant values
    /// </summary>
    public static class Constants
    {
        public const int DEFAULT_IN_STATUS = 1;
        public const int DEFAULT_OUT_STATUS = 2;
        public const int DEFAULT_LOCATION = 1;
        public static readonly int[] DEFAULT_STATUSES = { 1, 2, 3, 4, 5, 6 };
        public static readonly int[] DEFAULT_LOCATIONS = { 1, 2, 3, 4 };
    }
}
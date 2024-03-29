﻿namespace StudentsHelper.Common
{
    using System;
    using System.Collections.Concurrent;

    public static class GlobalVariables
    {
        /// <summary>
        /// Key -> Application User Email
        /// Value -> Last time active.
        /// </summary>
        public static readonly ConcurrentDictionary<string, DateTime> UsersActivityDictionary;

        /// <summary>
        /// Key -> Application User Email
        /// Value -> Is Teacher Connected Account Confirmed.
        /// </summary>
        public static readonly ConcurrentDictionary<string, bool> TeachersConnectedAccountStatus;

        /// <summary>
        /// Key -> Application User Email
        /// Value -> Is Teacher Application Account Confirmed.
        /// </summary>
        public static readonly ConcurrentDictionary<string, bool> TeachersAccountStatus;

        static GlobalVariables()
        {
            UsersActivityDictionary = new ConcurrentDictionary<string, DateTime>();
            TeachersConnectedAccountStatus = new ConcurrentDictionary<string, bool>();
            TeachersAccountStatus = new ConcurrentDictionary<string, bool>();
        }
    }
}

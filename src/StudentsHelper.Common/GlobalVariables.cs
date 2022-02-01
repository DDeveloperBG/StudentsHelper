namespace StudentsHelper.Common
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

        static GlobalVariables()
        {
            UsersActivityDictionary = new ConcurrentDictionary<string, DateTime>();
        }
    }
}

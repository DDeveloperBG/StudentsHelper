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
        public static readonly ConcurrentDictionary<string, DateTime> TeachersActivityDictionary;

        static GlobalVariables()
        {
            TeachersActivityDictionary = new ConcurrentDictionary<string, DateTime>();
        }
    }
}

namespace StudentsHelper.Services.Time
{
    using System;

    public class CustomDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}

namespace StudentsHelper.Services.Time
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();
    }
}

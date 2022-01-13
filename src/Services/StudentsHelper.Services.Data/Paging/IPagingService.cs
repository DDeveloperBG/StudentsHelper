namespace StudentsHelper.Services.Data.Paging
{
    using System.Linq;

    public interface IPagingService
    {
        PagedResult<T> GetPaged<T>(IQueryable<T> query, int currentNumber, int pageSize)
             where T : class;
    }
}

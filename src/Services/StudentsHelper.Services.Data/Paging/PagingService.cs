namespace StudentsHelper.Services.Data.Paging
{
    using System.Linq;

    public class PagingService : IPagingService
    {
        public PagedResult<T> GetPaged<T>(IQueryable<T> query, int currentNumber, int pageSize)
            where T : class
        {
            var result = new PagedResult<T>()
            {
                CurrentNumber = currentNumber,
                PageSize = pageSize,
                AllCount = query.Count(),
            };

            result.Results = query.Skip(currentNumber - 1).Take(pageSize).ToList();

            return result;
        }
    }
}

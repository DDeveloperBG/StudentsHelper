namespace StudentsHelper.Services.Data.Paging
{
    using System.Linq;

    using StudentsHelper.Services.Data.Paging.OldPaging.Models;

    public interface IReviewsPagingService
    {
        PagedResult<T> GetPaged<T>(IQueryable<T> query, int currentNumber, int pageSize)
             where T : class;
    }
}

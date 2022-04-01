namespace StudentsHelper.Services.Data.Paging.NewPaging
{
    using System.Linq;

    using StudentsHelper.Web.ViewModels.Paging;

    public interface IPagingService
    {
        PagedResultModel<T> GetPaged<T>(IQueryable<T> query, int currentNumber, int pageSize)
             where T : class;
    }
}

namespace StudentsHelper.Services.Data.Paging.NewPaging
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Web.ViewModels.Paging;

    public interface IPagingService
    {
        PagedResult<T> GetPaged<T>(IQueryable<T> query, int currentNumber, int pageSize)
             where T : class;

        string UpdateQueryString(string fullUrl, string queryString, ICollection<string> queryStringParameters, int newPage);
    }
}

namespace StudentsHelper.Services.Data.Paging.NewPaging
{
    using System;
    using System.Linq;

    using StudentsHelper.Web.ViewModels.Paging;

    public class PagingService : IPagingService
    {
        public PagedResultModel<T> GetPaged<T>(IQueryable<T> query, int currentPage, int pageSize)
            where T : class
        {
            var collection = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var pagingData = new PagingDataModel
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                AllPagesCount = (int)Math.Ceiling((double)query.Count() / pageSize),
                CurrentPageCollectionCount = collection.Count,
            };

            return new PagedResultModel<T>(collection, pagingData);
        }
    }
}

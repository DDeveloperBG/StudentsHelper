namespace StudentsHelper.Web.ViewModels.Paging
{
    using System.Collections.Generic;

    public class PagedResult<T>
        where T : class
    {
        public PagedResult(PagingData pagingData)
        {
            this.Results = new List<T>();
            this.PagingData = pagingData;
        }

        public PagedResult(IList<T> results, PagingData pagingData)
            : this(pagingData)
        {
            this.Results = results;
        }

        public IList<T> Results { get; set; }

        public PagingData PagingData { get; set; }
    }
}

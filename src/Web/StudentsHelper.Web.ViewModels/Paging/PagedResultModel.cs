namespace StudentsHelper.Web.ViewModels.Paging
{
    using System.Collections.Generic;

    public class PagedResultModel<T>
        where T : class
    {
        public PagedResultModel(PagingDataModel pagingData)
        {
            this.Results = new List<T>();
            this.PagingData = pagingData;
        }

        public PagedResultModel(IList<T> results, PagingDataModel pagingData)
            : this(pagingData)
        {
            this.Results = results;
        }

        public IList<T> Results { get; set; }

        public PagingDataModel PagingData { get; set; }
    }
}

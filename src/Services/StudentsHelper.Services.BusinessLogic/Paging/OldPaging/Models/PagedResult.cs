namespace StudentsHelper.Services.Data.Paging.OldPaging.Models
{
    using System.Collections.Generic;

    public class PagedResult<T> : PagedResultBase
        where T : class
    {
        public PagedResult()
        {
            this.Results = new List<T>();
        }

        public IList<T> Results { get; set; }
    }
}

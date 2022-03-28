namespace StudentsHelper.Services.Data.Paging.OldPaging.Models
{
    public abstract class PagedResultBase
    {
        public int CurrentNumber { get; set; }

        public int PageSize { get; set; }

        public int AllCount { get; set; }
    }
}

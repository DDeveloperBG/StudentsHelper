namespace StudentsHelper.Services.Data.Paging
{
    public abstract class PagedResultBase
    {
        public int CurrentNumber { get; set; }

        public int PageSize { get; set; }

        public int AllCount { get; set; }
    }
}

namespace StudentsHelper.Web.ViewModels.Paging
{
    public class PagingDataModel
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int AllPagesCount { get; set; }

        public int CurrentPageCollectionCount { get; set; }
    }
}

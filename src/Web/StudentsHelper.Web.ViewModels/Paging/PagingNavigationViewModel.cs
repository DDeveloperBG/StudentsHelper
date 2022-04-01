namespace StudentsHelper.Web.ViewModels.Paging
{
    using System.Collections.Generic;

    public class PagingNavigationViewModel
    {
        public int CurrentPageNumber { get; set; }

        public int FromItem { get; set; }

        public int ToItem { get; set; }

        public int AllItemsCount { get; set; }

        public string FirstPageUrl { get; set; }

        public string LastPageUrl { get; set; }

        public IEnumerable<PageInfoViewModel> PagesBefore { get; set; }

        public IEnumerable<PageInfoViewModel> PagesAfter { get; set; }
    }
}

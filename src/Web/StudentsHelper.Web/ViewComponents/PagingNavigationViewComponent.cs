namespace StudentsHelper.Web.ViewComponents
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Services.BusinessLogic.QueryStringUpdater;
    using StudentsHelper.Web.ViewModels.Paging;

    public class PagingNavigationViewComponent : ViewComponent
    {
        private readonly IQueryStringUpdatingService queryStringUpdater;

        public PagingNavigationViewComponent(IQueryStringUpdatingService queryStringUpdater)
        {
            this.queryStringUpdater = queryStringUpdater;
        }

        public IViewComponentResult Invoke(PagingDataModel pagingData)
        {
            if (pagingData.CurrentPageCollectionCount < 1)
            {
                return this.Content(string.Empty);
            }

            int fromItem = ((pagingData.CurrentPage - 1) * pagingData.PageSize) + 1;
            int allItemsCount = ((pagingData.AllPagesCount - 1) * pagingData.PageSize) + pagingData.CurrentPageCollectionCount;

            if (allItemsCount < 0)
            {
                allItemsCount = 0;
                fromItem = 0;
            }

            int toItem = fromItem + pagingData.CurrentPageCollectionCount - 1;

            var viewModel = new PagingNavigationViewModel
            {
                CurrentPageNumber = pagingData.CurrentPage,
                AllItemsCount = allItemsCount,
                FromItem = fromItem,
                ToItem = toItem,
                FirstPageUrl = this.GetNewPageUrl(1),
                LastPageUrl = this.GetNewPageUrl(pagingData.AllPagesCount),
                PagesBefore = this.GetPagesBefore(pagingData.CurrentPage),
                PagesAfter = this.GetPagesAfter(pagingData.CurrentPage, pagingData.AllPagesCount),
            };

            return this.View(viewModel);
        }

        private string GetNewPageUrl(int newPageNumber)
        {
            string currentUrl = this.Request.GetDisplayUrl();
            return this.queryStringUpdater.UpdatePageParameter(currentUrl, newPageNumber);
        }

        private IEnumerable<PageInfoViewModel> GetPagesBefore(int currentPage)
        {
            var pagesBefore = new List<PageInfoViewModel>();
            for (int i = 1; i <= 2 && currentPage - i >= 1; i++)
            {
                var page = new PageInfoViewModel
                {
                    PageNumber = currentPage - i,
                    PageUrl = this.GetNewPageUrl(currentPage - i),
                };

                pagesBefore.Add(page);
            }

            return pagesBefore;
        }

        private IEnumerable<PageInfoViewModel> GetPagesAfter(int currentPage, int allPagesCount)
        {
            var pagesAfter = new List<PageInfoViewModel>();
            for (int i = 1; i <= 2 && currentPage + i <= allPagesCount; i++)
            {
                pagesAfter.Add(new PageInfoViewModel
                {
                    PageNumber = currentPage + i,
                    PageUrl = this.GetNewPageUrl(currentPage + i),
                });
            }

            return pagesAfter;
        }
    }
}

namespace StudentsHelper.Services.Data.Paging.NewPaging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Web.ViewModels.Paging;

    public class PagingService : IPagingService
    {
        public PagedResult<T> GetPaged<T>(IQueryable<T> query, int currentPage, int pageSize)
            where T : class
        {
            var collection = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var pagingData = new PagingData
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                AllPagesCount = (int)Math.Ceiling((double)query.Count() / pageSize),
                CurrentPageCollectionCount = collection.Count,
            };

            return new PagedResult<T>(collection, pagingData);
        }

        public string UpdateQueryString(
            string fullUrl,
            string queryString,
            ICollection<string> queryStringParameters,
            int newPage)
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                fullUrl = fullUrl.Replace(queryString, string.Empty);
            }

            const string pageKey = "page";
            const char parametersSeparator = '&';
            const char parameterSeparator = '=';

            var newParameter = $"{pageKey}{parameterSeparator}{newPage}";

            if (!queryString.Contains(pageKey))
            {
                if (queryStringParameters.Count == 0)
                {
                    return $"{fullUrl}?{newParameter}";
                }

                return $"{fullUrl}{queryString}{parametersSeparator}{newParameter}";
            }

            var queryStringParametersClone = queryStringParameters.ToList(); // cloning the original collection

            int pageParameterInd = queryStringParametersClone.FindIndex(x => x.Contains(pageKey));
            queryStringParametersClone[pageParameterInd] = newParameter;
            queryString = string.Join(parametersSeparator, queryStringParametersClone);

            return $"{fullUrl}?{queryString}";
        }
    }
}

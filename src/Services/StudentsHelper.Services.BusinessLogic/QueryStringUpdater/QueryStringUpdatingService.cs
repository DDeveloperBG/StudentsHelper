namespace StudentsHelper.Services.BusinessLogic.QueryStringUpdater
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class QueryStringUpdatingService : IQueryStringUpdatingService
    {
        public string UpdatePageParameter(
                    string fullUrl,
                    int newPage)
        {
            return this.UpdateQueryStringParameter(
                   fullUrl,
                   "page",
                   newPage.ToString());
        }

        public string UpdateSortByParameter(
                    string fullUrl,
                    string newSortBy,
                    bool isAscending)
        {
            var updatedUrl = this.UpdateQueryStringParameter(
                    fullUrl,
                    "sortBy",
                    newSortBy);

            return this.UpdateQueryStringParameter(
                    updatedUrl,
                    "isAscending",
                    isAscending.ToString());
        }

        private string UpdateQueryStringParameter(
                    string fullUrl,
                    string parameterKey,
                    string newValue)
        {
            int queryStringStartIndex = fullUrl.IndexOf('?');
            string queryString = queryStringStartIndex != -1 ? fullUrl.Substring(queryStringStartIndex) : string.Empty;
            if (!string.IsNullOrEmpty(queryString))
            {
                fullUrl = fullUrl.Replace(queryString, string.Empty);
            }

            const char parametersSeparator = '&';
            const char parameterSeparator = '=';

            var newParameter = $"{parameterKey}{parameterSeparator}{newValue}";
            ICollection<string> queryStringParameters = queryString.Split(new char[] { '&', '?' }, StringSplitOptions.RemoveEmptyEntries);

            if (!queryString.Contains(parameterKey))
            {
                if (queryStringParameters.Count == 0)
                {
                    return $"{fullUrl}?{newParameter}";
                }

                return $"{fullUrl}{queryString}{parametersSeparator}{newParameter}";
            }

            var queryStringParametersClone = queryStringParameters.ToList(); // cloning the original collection

            int pageParameterInd = queryStringParametersClone.FindIndex(x => x.Contains(parameterKey));
            queryStringParametersClone[pageParameterInd] = newParameter;
            queryString = string.Join(parametersSeparator, queryStringParametersClone);

            return $"{fullUrl}?{queryString}";
        }
    }
}

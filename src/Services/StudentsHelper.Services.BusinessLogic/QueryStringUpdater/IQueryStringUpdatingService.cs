namespace StudentsHelper.Services.BusinessLogic.QueryStringUpdater
{
    public interface IQueryStringUpdatingService
    {
        public string UpdatePageParameter(
                    string fullUrl,
                    int newPage);

        public string UpdateSortByParameter(
                    string fullUrl,
                    string newSortBy,
                    bool isAscending);
    }
}

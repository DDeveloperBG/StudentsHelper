namespace StudentsHelper.Web.ViewModels.Teachers
{
    using StudentsHelper.Web.ViewModels.Paging;

    public class TeachersOfSubjectType<T>
        where T : class
    {
        public PagedResult<T> Teachers { get; set; }

        public int SubjectId { get; set; }

        public string SubjectName { get; set; }
    }
}

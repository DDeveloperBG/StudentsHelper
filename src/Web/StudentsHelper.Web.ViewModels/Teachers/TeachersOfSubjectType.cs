namespace StudentsHelper.Web.ViewModels.Teachers
{
    using System.Collections.Generic;

    public class TeachersOfSubjectType<T>
    {
        public IEnumerable<T> Teachers { get; set; }

        public int SubjectId { get; set; }

        public string SubjectName { get; set; }
    }
}

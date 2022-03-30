namespace StudentsHelper.Web.ViewModels.StudentFavouriteTeacher
{
    using StudentsHelper.Web.ViewModels.Paging;

    public class AllStudentFavouriteTeachersViewModel
    {
        public PagedResult<StudentFavouriteTeacherViewModel> Teachers { get; set; }
    }
}

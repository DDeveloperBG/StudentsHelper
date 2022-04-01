namespace StudentsHelper.Web.ViewModels.StudentFavouriteTeacher
{
    using StudentsHelper.Web.ViewModels.Paging;

    public class AllStudentFavouriteTeachersViewModel
    {
        public PagedResultModel<StudentFavouriteTeacherViewModel> Teachers { get; set; }
    }
}

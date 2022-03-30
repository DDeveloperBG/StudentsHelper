namespace StudentsHelper.Services.BusinessLogic.StudentFavouriteTeachers
{
    using System.Threading.Tasks;

    using StudentsHelper.Services.Data.Paging.NewPaging;
    using StudentsHelper.Services.Data.StudentFavouriteTeachers;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Web.ViewModels.StudentFavouriteTeacher;

    public class StudentFavouriteTeachersBusinessLogicService : IStudentFavouriteTeachersBusinessLogicService
    {
        private readonly IStudentsService studentsService;
        private readonly ITeachersService teachersService;
        private readonly IStudentFavouriteTeachersService studentFavouriteTeachersService;
        private readonly IPagingService pagingService;

        public StudentFavouriteTeachersBusinessLogicService(
            IStudentFavouriteTeachersService studentFavouriteTeachersService,
            IStudentsService studentsService,
            ITeachersService teachersService,
            IPagingService pagingService)
        {
            this.studentFavouriteTeachersService = studentFavouriteTeachersService;
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.pagingService = pagingService;
        }

        public Task AddOrRemoveTeacherAsync(string studentUserId, string teacherUserId)
        {
            var studentId = this.studentsService.GetId(studentUserId);
            var teacherId = this.teachersService.GetId(teacherUserId);

            return this.studentFavouriteTeachersService.AddOrRemoveAsync(studentId, teacherId);
        }

        public AllStudentFavouriteTeachersViewModel GetIndexPageViewModel(string studentUserId, int page)
        {
            var studentId = this.studentsService.GetId(studentUserId);
            var teachersAsIQueryable = this.studentFavouriteTeachersService
                .GetStudentAllFavouriteTeachers<StudentFavouriteTeacherViewModel>(studentId);
            var teachers = this.pagingService.GetPaged<StudentFavouriteTeacherViewModel>(teachersAsIQueryable, page, 10);

            var viewModel = new AllStudentFavouriteTeachersViewModel
            {
                Teachers = teachers,
            };

            return viewModel;
        }
    }
}
